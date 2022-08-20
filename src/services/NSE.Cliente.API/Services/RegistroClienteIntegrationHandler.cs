using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Services
{
    //BackgroundService = Feature do .Net Core, serviço hospedado Tarefas temporizadas tipo hangfire
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;
        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(responder:async request =>
                        await RegistrarCliente(request));
            return Task.CompletedTask;
        }
        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clientCommand = new RegistrarClientCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatrHandler>();
                sucesso = await mediator.EnviarCommando(clientCommand);
            }
            return new ResponseMessage(sucesso);
        }
    }
}
