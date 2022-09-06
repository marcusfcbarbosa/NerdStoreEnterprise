using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain.Vouchers
{
    public interface IVoucherRepository : IRepository<Voucher>
    {

        Task<Voucher> ObterVoucherPeloCodigo(string codigo);
    }
}
