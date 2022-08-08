using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Catalogo.API.Data;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.API.Configuration
{
    public static  class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CatalogoContext>(optionsAction: options =>
            options.UseSqlServer(configuration.GetConnectionString(name: "DefaultConnection")));
            services.AddControllers();
            //Como sera chamada por varias apis, habilitar o CORS

            services.AddCors(options => {
                options.AddPolicy(name:"Acesso_Total",
                    configurePolicy: builder =>
                    builder
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod());
            });
        }


        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Acesso_Total");
            app.UseAuthConfiguration();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
