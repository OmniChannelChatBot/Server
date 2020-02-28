using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OCCBPackage.Extensions;
using OCCBPackage.Middlewares;
using OCCBPackage.Options;
using OCCBPackage.Swagger.OperationFilters;
using Server.Api.Extensions;
using Server.Api.Hubs;
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
            services.AddAutoMapper(typeof(Startup));
            services.AddCustomHealthChecks();
            services.AddJwtBearerAuthentication(options => Configuration.GetSection(nameof(AccessTokenOptions)).Bind(options));
            services.AddApplicationServices(options => Configuration.GetSection(nameof(DBApiOptions)).Bind(options));
            services.AddApiServices();
            services.AddCorsPolicy(options => Configuration.GetSection(nameof(CorsPolicyOptions)).Bind(options));
            services.AddSignalR()
                .AddHubOptions<ChatHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                    //TODO: вынести в настройки
                    options.KeepAliveInterval = TimeSpan.FromMinutes(1);
                });
            services.AddCustomSwagger(o =>
            {
                o.AddBearerSecurityDefinition();
                o.OperationFilter<OperationApiProblemDetailsFilter>(
                    new int[] { 504, 503, 502, 501, 500, 415, 413, 412, 405, 400 });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseRouting();
            app.UseCors(CorsPolicyOptions.CorsPolicy);
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
