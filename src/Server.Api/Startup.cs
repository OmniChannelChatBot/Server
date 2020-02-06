using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OCCBPackage.Extensions;
using OCCBPackage.Options;
using OCCBPackage.Swagger.OperationFilters;
using Server.Api.Extensions;
using Server.Api.Hubs;
using Server.Api.Middlewares;
using Server.Core.Options;
using System;

namespace Server.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR();
            services.AddCustomHealthChecks();
            services.AddJwtBearerAuthentication(options => Configuration.GetSection(nameof(AccessTokenOptions)).Bind(options));
            services.AddAutoMapper(typeof(Startup));
            services.AddApplicationServices(options => Configuration.GetSection(nameof(DBApiOptions)).Bind(options));
            services.AddApiServices();
            services.AddCustomSwagger(o =>
            {
                o.AddBearerSecurityDefinition();
                o.OperationFilter<OperationApiProblemDetailsFilter>(
                    new int[] { 504, 503, 502, 501, 500, 415, 413, 412, 405, 400 });
            });
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000")
            ));
            services.AddSignalR()
                .AddHubOptions<ChatHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    //TODO: вынести в настройки
                    options.KeepAliveInterval = TimeSpan.FromMinutes(1);
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCustomHealthChecks();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHub<ChatHub>("/chat", options => options.Transports = HttpTransportType.WebSockets);
                endpoint.MapControllers();
            });
            app.UseCustomSwagger();
        }
    }
}
