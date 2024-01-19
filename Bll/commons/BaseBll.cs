using AutoMapper;
using Bll.FactoryServices.UOW.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Globals;
using System.Security.Claims;

namespace Bll.commons
{
    public class BaseBll : ControllerBase
    {
        public readonly GlobalSettings _global;
        public readonly Iuow _UOW;
        public readonly AppUsuario _userModel;
        public readonly Serilog.ILogger _logger;
        public readonly IMapper _mapper;

        public BaseBll()
        {
            this._global = BaseServices.Global();
            this._logger = BaseServices.Logger();
            this._UOW = new BaseServices()
                .uowInstance();
            this._mapper = BaseServices.Mapper();

            ClaimsPrincipal? user = BaseServices.Accessor().HttpContext?.User;

            if (user is not null)
            {
                var claims = user.Identities.First().Claims.ToList();

                if (claims != null && claims.Count > 0)
                {
                    this._global.UserName = user.FindFirst(ClaimTypes.Email).Value;
                    this._global.UserId = user.Claims.First(i => i.Type == "uid").Value;
                }
            }
        }
    }
}