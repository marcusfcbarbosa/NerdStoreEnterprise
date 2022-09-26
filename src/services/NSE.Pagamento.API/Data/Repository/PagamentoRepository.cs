using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pagamento.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pagamento.API.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentosContext _context;

        public PagamentoRepository(PagamentosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Models.Pagamento entity)
        {
            throw new NotImplementedException();
        }

        public void AdicionarPagamento(Models.Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public Task Atualizar(Models.Pagamento entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Models.Pagamento> ObterPagamentoPorPedidoId(Guid pedidoId)
        {
            return await _context.Pagamentos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }

        public Task<Models.Pagamento> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Models.Pagamento>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transacao>> ObterTransacaoesPorPedidoId(Guid pedidoId)
        {
            return await _context.Transacoes.AsNoTracking()
                .Where(t => t.Pagamento.PedidoId == pedidoId).ToListAsync();
        }
    }
}
