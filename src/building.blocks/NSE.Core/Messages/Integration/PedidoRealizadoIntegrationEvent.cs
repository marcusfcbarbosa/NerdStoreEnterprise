using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.Messages.Integration
{
    public class PedidoRealizadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; set; }
        //somente para excluir o carrinho do cliente
        public PedidoRealizadoIntegrationEvent(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
