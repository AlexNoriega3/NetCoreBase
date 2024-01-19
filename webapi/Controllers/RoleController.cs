using Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleBll _RoleBll;

        public RoleController(IRoleBll RoleBll)
        => this._RoleBll = RoleBll;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
         => await this._RoleBll.Get();

        [HttpGet("Search")]
        public async Task<ActionResult<PagedResult<RoleDto>>> Get([FromQuery] QueryParameters paramsQuery)
         => await this._RoleBll.Get(paramsQuery);

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<string>> Post([FromBody] RoleDto Role) =>
          await this._RoleBll.Add(Role);

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> Get(string id) =>
            await this._RoleBll.Get(id);

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> Put(string id, [BindRequired] RoleDto edit) =>
           await this._RoleBll.Edit(id, edit);

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> Delete(string id) =>
           await this._RoleBll.Delete(id);
    }
}