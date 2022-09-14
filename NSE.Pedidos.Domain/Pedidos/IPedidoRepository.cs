using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);

        //DbConnection ObterConexao();

        /* Pedido Item */
        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
    }
}
