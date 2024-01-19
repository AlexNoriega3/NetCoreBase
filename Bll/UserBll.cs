using Bll.commons;
using Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTOs;
using Models.DTOs.commons;
using Models.DTOs.user;
using Models.Entities;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bll
{
    public class UserBll : BaseBll, IUserBll
    {
        private readonly UserManager<AppUsuario> _userManager;
        private AppUsuario _user;
        private readonly ILogger<UserBll> _loggerL;
        private readonly IAzureStorage _azureStorage;

        public UserBll(UserManager<AppUsuario> userManager,
        ILogger<UserBll> logger, IAzureStorage azureStorage)
        {
            this._userManager = userManager; this._loggerL = logger;
            this._azureStorage = azureStorage;
        }

        public async Task<ActionResult<IEnumerable<AppUserDto>>> Get()
        {
            IEnumerable<AppUserDto> Users = null;

            try
            {
                IEnumerable<AppUsuario> UsersEntity = await this._UOW.UserRepository
                    .GetAllAsync();

                Users = this._mapper.Map<IEnumerable<AppUserDto>>(UsersEntity);
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to load Users");
            }

            return Ok(Users);
        }

        public async Task<ActionResult<PagedResult<AppUserDto>>> Get(QueryParameters paramsQuery)
        {
            PagedResult<AppUserDto> users = null;

            try
            {
                users = await this._UOW.UserRepository
                    .GetAllAsync<AppUserDto>(paramsQuery,
                    !string.IsNullOrWhiteSpace(paramsQuery.Search) ?
                    (w =>
                       w.Name.Contains(paramsQuery.Search)
                    || w.FirstName.Contains(paramsQuery.Search)
                    || w.LastName.Contains(paramsQuery.Search)
                    || w.Email.Contains(paramsQuery.Search)
                    )
                    : null
                    );
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to load users");
            }

            return Ok(users);
        }

        public async Task<ActionResult<AppUserDto>> Get(string UserId)
        {
            AppUserDto User = null;

            try
            {
                AppUsuario UsersEntity = this._UOW.UserRepository
                    .FindBy(p => p.Id == UserId)?
                    .FirstOrDefault();

                User = this._mapper.Map<AppUserDto>(UsersEntity);
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to get User");
            }

            return Ok(User);
        }

        public async Task<ActionResult<string>> Add(UserPostDto userDto)
        {
            _loggerL.LogInformation($"Registration attemp for {userDto.Email}");

            string userId = string.Empty;
            IdentityResult result = null;
            try
            {
                _user = _mapper.Map<AppUsuario>(userDto);
                _user.UserName = userDto.Email;
                _user.Name = string.IsNullOrEmpty(_user.FirstName) ? "" : $"{_user.FirstName} {_user.LastName}";

                result = await this._userManager.CreateAsync(_user, userDto.Password);

                if (result.Succeeded)
                {
                    userId = _user.Id;
                    userDto.RoleName = "User";
                    await this._userManager.AddToRoleAsync(_user, userDto.RoleName.ToUpper());
                }
                else
                {
                    if (result.Errors.Any())
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                        return BadRequest(new ValidationResult(errorMessage: ModelState.GetErrors()));
                    }
                }
            }
            catch (Exception e)
            {
                if (result is not null && result.Succeeded)
                    await this._userManager.DeleteAsync(_user);

                this._logger.Error(e, e.Message);
                return BadRequest(e.Message);
            }

            return Ok(userId);
        }

        public async Task<ActionResult<EditUserResponse>> Edit(string id, UserPutDto userDto)
        {
            bool success = false;
            string image = string.Empty;

            try
            {
                AppUsuario userEntity = await this._UOW.UserRepository
                    .FindTrackingAsync(p => p.Id == id);

                if (userEntity is null)
                {
                    return NotFound("User not found");
                }

                userEntity = _mapper.Map(userDto, userEntity);

                if (userDto.imageFile != null)
                {
                    userEntity.Image = await this._azureStorage
                    .EditFile(
                        AzureStorageEnum.users.ToString(),
                        userDto.imageFile,
                        userEntity.Image
                        );
                }

                image = userEntity.Image;
                var result = await this._userManager.UpdateAsync(userEntity);
                success = result.Succeeded;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return BadRequest("Server error to edit an user");
            }

            return Ok(new EditUserResponse() { Saved = success, Image = image });
        }

        public async Task<ActionResult<bool>> Delete(string UserId)
        {
            bool success = false;

            try
            {
                AppUsuario UserEntity = this._UOW.UserRepository
                   .Find(p => p.Id == UserId);

                this._UOW.UserRepository.Delete(UserEntity);
                this._UOW.Save();
                success = true;
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to delete Users");
            }

            return Ok(success);
        }

        public async Task<ActionResult<EditUserResponse>> Upload(string id, IFormFile image)
        {
            bool success = false;
            string imageUrl = string.Empty;

            try
            {
                AppUsuario userEntity = await this._UOW.UserRepository
                    .FindTrackingAsync(p => p.Id == id);

                if (userEntity is null)
                {
                    return NotFound("User not found");
                }

                if (image != null)
                {
                    userEntity.Image = await this._azureStorage.EditFile(AzureStorageEnum.users.ToString(), image, userEntity.Image);
                }

                imageUrl = userEntity.Image;

                this._UOW.Save();

                success = true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return BadRequest("Server error to edit an image user");
            }

            return Ok(new EditUserResponse() { Saved = success, Image = imageUrl });
        }
    }
}