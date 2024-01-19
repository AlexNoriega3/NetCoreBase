using Bll.commons;
using Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bll
{
    public class AuthManagerBll : BaseBll, IAuthManager
    {
        private readonly UserManager<AppUsuario> _userManager;
        private readonly SignInManager<AppUsuario> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManagerBll> _loggerL;
        private AppUsuario _user;
        private readonly IEmailService _emailService;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManagerBll(
            UserManager<AppUsuario> userManager,
            IConfiguration configuration,
            ILogger<AuthManagerBll> logger,
            IEmailService emailService)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._loggerL = logger;
            this._emailService = emailService;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<(string message, AuthResponseDto? response)> Login(LoginDto loginDto)
        {
            _loggerL.LogInformation($"Looking for user with email {loginDto.Email}");
            _user = await _userManager.FindByEmailAsync(loginDto.Email);

            bool isValidUser = _user != null ? await _userManager.CheckPasswordAsync(_user, loginDto.Password) : false;

            bool access = _user is not null && isValidUser;

            if (!access)
            {
                _loggerL.LogWarning($"User with email {loginDto.Email} was not found");
                return ("Correo o contraseña incorrectos", null);
            }

            var roles = _user != null ? await _userManager.GetRolesAsync(_user) : null;

            var token = await GenerateToken();
            _loggerL.LogInformation($"Token generated for user with email {loginDto.Email} | Token: {token}");

            return (string.Empty, new AuthResponseDto
            {
                Token = token,
                UserId = _user?.Id,
                RefreshToken = await CreateRefreshToken(),
                Name = access ? _user.Name ?? _user.UserName : null,
                Image = access ? _user.Image ?? _user.Image : null,
                FirstName = access ? _user.FirstName : null,
                LastName = access ? _user.LastName : null,
                Url = access ? _user.Url : null,
                Active = access ? _user.Active : false,
                Roles = roles?.ToList(),
            });
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByNameAsync(username);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(Convert.ToInt32(_configuration["JwtSettings:DurationInMonths"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ActionResult<bool>> Logout()
        {
            bool success = false;

            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error in logout");
            }

            return Ok(true);
        }

        public async Task<AppUsuario> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ActionResult> GeneratePasswordResetTokenAsync(string email)
        {
            AppUsuario user;

            try
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return StatusCode(404, "User not fount");

                string token = await this._userManager
                    .GeneratePasswordResetTokenAsync(user);

                var message = new Message(new string[] { user.Email },
                    "Restablecer Contraseña",
                    "Clic en el siguiente enlace para cambiar la contraseña: \n" + $"{Environment.GetEnvironmentVariable("WEB_URL")}/reset?token={token}");

                await _emailService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.Message);
                return BadRequest("Server error");
            }

            return Ok(true);
        }

        public async Task<(string message, bool Success)> ChangePassword(string userEmail, string currentPassword, string newPassword)
        {
            AppUsuario user = await _userManager.FindByEmailAsync(userEmail);

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result is null)
                return ("Server Error ", false);
            if (!result.Succeeded)
                return ("Error: " + result.ToString(), false);

            return (result.ToString(), true);
        }

        public async Task<ActionResult<bool>> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                if (model.Password is null || model.Email is null || model.Token is null || !model.Password.Equals(model.ConfirmPassword))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null)
                    return NotFound("User not fount");

                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (!resetPassResult.Succeeded)
                {
                    string errors = string.Empty;

                    foreach (var error in resetPassResult.Errors)
                    {
                        errors += $"{error.Code}, {error.Description}. ";
                    }

                    return BadRequest(errors);
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to reset password");
            }

            return Ok(true);
        }
    }
}