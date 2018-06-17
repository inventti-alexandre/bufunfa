using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class ContaMap : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("conta");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdConta");
            builder.Property(x => x.ValorSaldoInicial).HasColumnName("SaldoInicial");
            builder.Property(x => x.NomeInstituicao);
            builder.Property(x => x.NumeroAgencia);
            builder.Property(x => x.Numero);
        }
    }
}
