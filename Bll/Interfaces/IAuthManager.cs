using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Entities;

namespace Bll.Interfaces
{
    public interface IAuthManager
    {
        Task<(string message, AuthResponseDto? response)> Login(LoginDto loginDto);

        Task<string> CreateRefreshToken();

        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

        Task<ActionResult<bool>> Logout();

        Task<AppUsuario> FindByEmailAsync(string email);

        Task<ActionResult> GeneratePasswordResetTokenAsync(string email);

        Task<(string message, bool Success)> ChangePassword(string userEmail, string currentPassword, string newPassword);

        Task<ActionResult<bool>> ResetPasswordAsync(ResetPasswordModel model);
    }
}