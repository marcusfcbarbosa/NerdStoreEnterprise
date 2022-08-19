using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Application.Events;
using NSE.Cliente.API.Data;
using NSE.Cliente.API.Data.Repositories;
using NSE.Cliente.API.Services;
using NSE.Core.Mediator;

namespace NSE.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
               
            services.AddScoped<IMediatrHandler, MediatrHandler>();
            
            services.AddScoped<IRequestHandler<RegistrarClientCommand,ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            
            services.AddScoped<ClientesContext>();

            //BackgroundService
            services.AddHostedService<RegistroClienteIntegrationHandler>();//singleton so permite injetar dependencias singleton
            
        }
    }
}
