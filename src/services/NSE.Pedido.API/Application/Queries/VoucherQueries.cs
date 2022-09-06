using NSE.Pedido.API.Application.DTO;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Infra.Data.Repository;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Application.Queries
{
    public interface IVoucherQueries {

        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }

    public class VoucherQueries : IVoucherQueries
    {

        private readonly VoucherRepository _voucherRepository;

        public VoucherQueries(VoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterVoucherPeloCodigo(codigo);
            if(voucher == default(Voucher)) return default(VoucherDTO);
            if(!voucher.EstaValidoParaUtilizacao()) return default(VoucherDTO);

            return new VoucherDTO {
                Codigo = voucher.Codigo,
                Percentual = voucher.Percentual,
                TipoDesconto = voucher.TipoDesconto,
                ValorDesconto = voucher.ValorDesconto
            };
        }
    }
}
