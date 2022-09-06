using NSE.Pedidos.Domain.Vouchers.Enums;

namespace NSE.Pedido.API.Application.DTO
{
    public class VoucherDTO
    {
        public string Codigo { get; set; }
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public TipoDescontoVoucher TipoDesconto { get; set; }
    }
}