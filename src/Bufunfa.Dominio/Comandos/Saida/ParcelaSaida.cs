using JNogueira.Bufunfa.Dominio.Entidades;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    public class ParcelaSaida
    {
        /// <summary>
        /// Id da parcela
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int? IdAgendamento { get; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; }

        /// <summary>
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool Descartada { get; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; }

        public ParcelaSaida(Parcela parcela)
        {
            if (parcela == null)
                return;

            this.Id             = parcela.Id;
            this.IdAgendamento  = parcela.IdAgendamento;
            this.IdFatura       = parcela.IdFatura;
            this.Data           = parcela.Data;
            this.Valor          = parcela.Valor;
            this.Lancada        = parcela.Lancada;
            this.Descartada     = parcela.Descartada;
            this.MotivoDescarte = parcela.MotivoDescarte;
            this.Observacao     = parcela.Observacao;
        }

        public override string ToString()
        {
            return $"{this.Data.ToString("dd/MM/yyyy")} - {this.Valor.ToString("C2")}";
        }
    }
}
