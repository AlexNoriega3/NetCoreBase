using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.DTOs.commons;
using Models.DTOs.user;

namespace Bll.Interfaces
{
    public interface IUserBll
    {
        Task<ActionResult<string>> Add(UserPostDto User);

        Task<ActionResult<bool>> Delete(string UserId);

        Task<ActionResult<EditUserResponse>> Edit(string id, UserPutDto userDto);

        Task<ActionResult<IEnumerable<AppUserDto>>> Get();

        Task<ActionResult<PagedResult<AppUserDto>>> Get(QueryParameters paramsQuery);

        Task<ActionResult<AppUserDto>> Get(string UserId);

        Task<ActionResult<EditUserResponse>> Upload(string id, IFormFile image);
    }
}