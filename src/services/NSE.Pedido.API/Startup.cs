using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSE.Pedido.API.Configuration;
using NSE.WebApi.Core.Identidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pedido.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);

            services.AddJWTConfiguration(Configuration);

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(Startup));

            services.RegisterServices();

            services.AddMessageBusConfiguration(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);
        }
    }
}
