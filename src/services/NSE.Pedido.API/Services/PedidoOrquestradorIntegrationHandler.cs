using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Pedidos.API.Services
{
    //Implementando Tarefa Agendada
    public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;
        private Timer _timer;
        public PedidoOrquestradorIntegrationHandler(ILogger<PedidoOrquestradorIntegrationHandler> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos iniciado.");
            _timer = new Timer(ProcessarPedidos, state: null,
                        TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        private void ProcessarPedidos(object rate)
        {
            _logger.LogInformation("Processando Pedidos");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos Finalizado.");
            _timer?.Change(dueTime: Timeout.Infinite, period: 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
           _timer?.Dispose();
        }
    }
}