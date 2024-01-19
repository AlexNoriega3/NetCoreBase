using Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Models.DTOs.commons;
using Models.DTOs.user;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public override ActionResult ValidationProblem()
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        private readonly IUserBll _UserBll;
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public UserController(
            IUserBll UserBll,
            IAuthManager authManager,
            ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
            this._UserBll = UserBll;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> Get()
         => await this._UserBll.Get();

        [HttpGet("Search")]
        public async Task<ActionResult<PagedResult<AppUserDto>>> Get([FromQuery] QueryParameters paramsQuery)
         => await this._UserBll.Get(paramsQuery);

        /// <summary>
        /// Endpoint para el registro de los usuarios
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///    GET api/User/Registrar \n
        ///    [FirstName] Opcional\n
        ///    [LastName] Opcional\n
        ///    \n
        ///    [RoleName] Opcional si se deja vacío sera rol tipo "User"\n
        ///    Para los roles existentes consultar el recurso "/api/Role"
        ///
        /// El user
        ///
        /// </remarks>
        /// <param name="filter">Description here</param>
        /// <returns>Description here</returns>
        /// <response code="200">Description here</response>
        /// <response code="400">Description here</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register([FromBody] UserPostDto apiAppUserDto) =>
            await this._UserBll.Add(apiAppUserDto);

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDto>> Get(string id) =>
            await this._UserBll.Get(id);

        [HttpPut("{id}")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<EditUserResponse>> Put(string id, [FromForm][FromBody] UserPutDto edit) =>
           await this._UserBll.Edit(id, edit);

        [HttpPut("EditUserApp/{id}")]
        public async Task<ActionResult<EditUserResponse>> Edit(string id, [FromBody] UserPutDto edit) =>
           await this._UserBll.Edit(id, edit);

        [HttpPost("UploadImage/{id}")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<EditUserResponse>> Upload(string id, IFormFile image) =>
           await this._UserBll.Upload(id, image);
    }
}