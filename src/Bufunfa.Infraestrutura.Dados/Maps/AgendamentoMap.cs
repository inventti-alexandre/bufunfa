using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class AgendamentoMap : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.ToTable("agendamento");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdAgendamento");
            builder.Property(x => x.IdUsuario);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.IdCategoria);

            builder.HasOne(x => x.Conta)
                .WithMany()
                .HasForeignKey(x => x.IdConta);

            builder.HasOne(x => x.CartaoCredito)
                .WithMany()
                .HasForeignKey(x => x.IdCartaoCredito);

            builder.HasOne(x => x.Pessoa)
                .WithMany()
                .HasForeignKey(x => x.IdPessoa);
            
            builder.HasMany(x => x.Parcelas)
                .WithOne(x => x.Agendamento)
                .HasForeignKey(x => x.IdAgendamento);

            builder.Ignore(x => x.DataPrimeiraParcela);
            builder.Ignore(x => x.DataUltimaParcela);
            builder.Ignore(x => x.QuantidadeParcelas);
            builder.Ignore(x => x.QuantidadeParcelasDescartadas);
            builder.Ignore(x => x.QuantidadeParcelasLancadas);
            builder.Ignore(x => x.SeConcluido);
        }
    }
}