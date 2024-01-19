using Bll;
using Bll.commons;
using Bll.FactoryServices.UOW.Interfaces;
using Bll.FactoryServices.UOW.Repositories;
using Bll.HangFire;
using Bll.Interfaces;
using Bll.Mapper;
using CorePush.Google;
using Dal;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models.Entities;
using Models.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace backend_api_lmi.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration Configuration)
        {
            string conn = Environment.GetEnvironmentVariable("DB_CONNECTION");
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddControllers().AddJsonOptions(opciones =>
              {
                  opciones.JsonSerializerOptions.IgnoreNullValues = true;
                  opciones.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                  opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
              })
                .AddOData(
                options =>
                {
                    options.Select().Filter().OrderBy();
                });

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PROJECT API - " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    Description = "PROJECT .NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Alex",
                        Email = string.Empty,
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example if we have license",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                                    Enter 'Bearer' [space] and then you token in the next input below
                                    Example: 'Bearer ad123dkalsjdlfh'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        },
                            Scheme = "Oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                options.UseMySql(conn, ServerVersion.AutoDetect(conn),
                    mysql =>
                    {
                        mysql.UseNetTopologySuite()
                        .MigrationsAssembly("Dal");
                    });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);                
            }
            );

            // Ensure to create database if not exists
            _ = (services.BuildServiceProvider().GetService<ApplicationDbContext>()?.Database?.EnsureCreated());

            services.AddIdentityCore<AppUsuario>()
                .AddRoles<Role>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUsuario>>("HotelListingApi")
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "Authentication:Google:AppId";
                googleOptions.ClientSecret = "Authentication:Google:AppSecret";
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "Authentication:Facebook:AppId";
                facebookOptions.AppSecret = "Authentication:Facebook:AppSecret";
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
            });

            services.AddScoped<Iuow, UOW>();

            #region Busines Logic (BLLs)

            services.AddTransient<IAuthManager, AuthManagerBll>();
            services.AddTransient<IUserBll, UserBll>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IRoleBll, RoleBll>();
            services.AddTransient<IAzureStorage, AzureStorage>();
            services.AddTransient<IHangFireBll, HangFireBll>();
            services.AddTransient<IPushNotificationService, PushNotificationService>();

            #endregion Busines Logic (BLLs)

            services.AddHttpClient<FcmSender>();
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            return services;
        }
    }
}