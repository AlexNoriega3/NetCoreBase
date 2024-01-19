using AutoMapper;
using Bll.FactoryServices.UOW.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models.Globals;
using Serilog;

namespace Bll.commons
{
    public class BaseServices
    {
        public static IHttpContextAccessor _accessor;
        public static IServiceProvider _service;
        public Iuow _UnitOW;
        public static IMapper _mapper;
        public static IOptions<GlobalSettings> _global;
        public static ILogger _logger;

        public static IHttpContextAccessor Accessor() => _accessor;

        public static IServiceProvider ServiceProvider() => _service;

        public static void Register(IServiceProvider service)
        {
            _service = service;
            _accessor = service.GetRequiredService<IHttpContextAccessor>();
        }

        internal Iuow uowInstance()
        {
            this._UnitOW = _accessor
                .HttpContext.RequestServices.
                GetService<Iuow>();

            return this._UnitOW;
        }

        public static ILogger Logger()
        {
            _logger = _accessor.HttpContext.RequestServices
                .GetRequiredService<ILogger>();
            return _logger;
        }

        public static GlobalSettings Global()
        {
            _global = _accessor.HttpContext
                .RequestServices
                .GetRequiredService<IOptions<GlobalSettings>>();

            return (GlobalSettings)_global.Value;
        }

        public static IMapper Mapper()
        {
            _mapper = _accessor.HttpContext.RequestServices.GetRequiredService<IMapper>();
            return _mapper;
        }
    }
}