using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NSE.Pagamento.API.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<NSE.Pagamento.API.Models.Pagamento>
    {
        public void Configure(EntityTypeBuilder<NSE.Pagamento.API.Models.Pagamento> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Ignore(c => c.CartaoCredito);

            // 1 : N => Pagamento : Transacao
            builder.HasMany(c => c.Transacoes)
                .WithOne(c => c.Pagamento)
                .HasForeignKey(c => c.PagamentoId);

            builder.ToTable("Pagamentos");
        }
    }
}
