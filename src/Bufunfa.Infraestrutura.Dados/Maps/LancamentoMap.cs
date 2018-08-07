using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class LancamentoMap : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ToTable("lancamento");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdLancamento");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.IdParcela);
            builder.Property(x => x.Data);
            builder.Property(x => x.Valor);
            builder.Property(x => x.Observacao);

            builder.HasOne(x => x.Conta)
                .WithMany()
                .HasForeignKey(x => x.IdConta);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.IdCategoria);

            builder.HasOne(x => x.Pessoa)
                .WithMany()
                .HasForeignKey(x => x.IdPessoa);

            builder.HasMany(x => x.Anexos)
                .WithOne(x => x.Lancamento)
                .HasForeignKey(x => x.IdLancamento)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
