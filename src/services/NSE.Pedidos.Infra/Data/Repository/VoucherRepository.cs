using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedidos.Domain;
using NSE.Pedidos.Domain.Vouchers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosContext _context;
        public VoucherRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Voucher entity)
        {
            throw new NotImplementedException();
        }

        public async  Task Atualizar(Voucher entity)
        {
            _context.Vouchers.Update(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<Voucher> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Voucher>> ObterTodos()
        {
            throw new NotImplementedException();
        }
        public async Task<Voucher> ObterVoucherPeloCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }
    }
}
