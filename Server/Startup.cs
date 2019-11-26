using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Common;
using Server.Settings;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    //TODO: ������� � ���������
                    .WithOrigins("http://localhost:3000");
            }));

            services.AddControllers();

            services.AddHttpClient();

            // configure strongly typed settings objects

            services.Configure<DBApiSettings>(options =>
            {
                options.Url
                    = Configuration.GetSection("DBApiSettings:Url").Value;
            });

            services.Configure<OAuthSettings>(options =>
            {
                options.Url
                    = Configuration.GetSection("OAuthSettings:Url").Value;
            });

            // use SignalR
            services.AddSignalR().AddHubOptions<ChatHub>(options =>
            {
                // ����������� ������ ������ ��� ����
                options.EnableDetailedErrors = true;

                //TODO: ������� � ���������
                options.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHealthChecks("/health");
                endpoint.MapHub<ChatHub>("/chat", options => options.Transports = HttpTransportType.WebSockets);
                endpoint.MapControllers();
            });
        }
    }
}
