using NSE.Core.Data;
using NSE.Pedidos.Domain.Vouchers;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain
{
    public interface IVoucherRepository : IRepository<Voucher>
    {

        Task<Voucher> ObterVoucherPeloCodigo(string codigo);
    }
}
