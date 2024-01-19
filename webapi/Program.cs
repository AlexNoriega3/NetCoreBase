using backend_api_lmi.Services;
using Bll.commons;
using Dal.Http;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Web.Api;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Host.UseSerilog((ctx, lc) => lc
      .WriteTo
      .MySQL(
        connectionString: Environment.GetEnvironmentVariable("DB_CONNECTION"),
        "Log_System",
        restrictedToMinimumLevel: LogEventLevel.Information)
    .Enrich.FromLogContext());

builder.Services.AddInfrastructure(configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddCoreAdmin("Administrator");

// UnComment to use HangFire
//builder.Services.AddHangfire(configuration => configuration
//       .UseSimpleAssemblyNameTypeSerializer()
//       .UseRecommendedSerializerSettings()
//       .UseStorage(
//             new MySqlStorage(
//                 Environment.GetEnvironmentVariable("DB_CONNECTION"),
//                 new MySqlStorageOptions
//                 {
//                     TransactionIsolationLevel = IsolationLevel.ReadCommitted,
//                     QueuePollInterval = TimeSpan.FromSeconds(15),
//                     JobExpirationCheckInterval = TimeSpan.FromHours(1),
//                     CountersAggregateInterval = TimeSpan.FromMinutes(5),
//                     PrepareSchemaIfNecessary = true,
//                     DashboardJobListLimit = 50000,
//                     TransactionTimeout = TimeSpan.FromMinutes(1),
//                     TablesPrefix = "Hangfire",
//                 }
//                 )
//             ));

// hangfire server
//builder.Services.AddHangfireServer(options => options.WorkerCount = 4);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
        c.SwaggerEndpoint("v1/swagger.json", "Base v1 Development");
    });
}
if (app.Environment.IsEnvironment("QA"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
        c.SwaggerEndpoint("v1/swagger.json", "Base v1 QA");
    });
}


BaseServices.Register(app.Services);
HttpServiceContext.Register(app.Services);

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});

app.UseAuthentication();

app.UseAuthorization();

// UnComment to use HangFire dashboard
//app.UseHangfireDashboard();

// HangFire Jobs
// RecurringJob.AddOrUpdate<IHangFireBll>("daily-notifications",
//    service => service.DailyService(), "0 30 8 ? * *");

app.MapControllers();

app.UseCoreAdminCustomTitle("Administrador");

app.MapDefaultControllerRoute();

app.Run();