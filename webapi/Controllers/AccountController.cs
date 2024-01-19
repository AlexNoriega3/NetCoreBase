using Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTOs;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        //private readonly IUserService _userService;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
        }

        /// <summary>
        /// Endpoint para realizar el login del usuario de los usuarios
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///    GET api/Account/Login
        ///
        /// </remarks>
        /// <param name="filter">Description here</param>
        /// <returns>Description here</returns>
        /// <response code="200">Description here</response>
        /// <response code="400">Description here</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Login Attempt for {loginDto.Email} ");
            var authResponse = await _authManager.Login(loginDto);

            if (authResponse.response == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, authResponse.message);
            }

            return Ok(authResponse.response);
        }

        /// <summary>
        /// Endpoint para realizar el login del usuario de los usuarios
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///    GET api/Account/Login
        ///
        /// </remarks>
        /// <param name="filter">Description here</param>
        /// <returns>Description here</returns>
        /// <response code="200">Description here</response>
        /// <response code="400">Description here</response>
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Logout()
        => await this._authManager.Logout();

        [HttpPost("changepassword/{userEmail},{currentPassword},{newPassword},{confirmPassword}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword(string userEmail, string currentPassword, string newPassword, string confirmPassword)
        {
            if (currentPassword.Equals(newPassword) || !newPassword.Equals(confirmPassword))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var result = await _authManager.ChangePassword(userEmail, currentPassword, newPassword);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.message);
            }

            return Ok(result.message);
        }

        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword(string email)
        => await this._authManager.GeneratePasswordResetTokenAsync(email);

        [HttpPost("resetpassword")]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordModel model)
        => await this._authManager
                    .ResetPasswordAsync(model);
    }
}