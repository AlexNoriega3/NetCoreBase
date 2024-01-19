using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;

namespace Bll.Interfaces
{
    public interface IRoleBll
    {
        Task<ActionResult<string>> Add(RoleDto Role);

        Task<ActionResult<bool>> Delete(string RoleId);

        Task<ActionResult<bool>> Edit(string RoleId, RoleDto edit);

        Task<ActionResult<IEnumerable<RoleDto>>> Get();

        Task<ActionResult<PagedResult<RoleDto>>> Get(QueryParameters paramsQuery);

        Task<ActionResult<RoleDto>> Get(string RoleId);
    }
}