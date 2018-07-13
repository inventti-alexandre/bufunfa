using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma parcela
    /// </summary>
    public class Parcela
    {
        /// <summary>
        /// ID da parcela
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do agendamento que originou a parcela
        /// </summary>
        public int? IdAgendamento { get; private set; }

        /// <summary>
        /// Id da fatura cuja parcela pertence
        /// </summary>
        public int? IdFatura { get; private set; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data{ get; private set; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; private set; }

        /// <summary>
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool Descartada { get; private set; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; private set; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Agendamento que originou a parcela
        /// </summary>
        public Agendamento Agendamento { get; private set; }

        /// <summary>
        /// Indica a situação da parcela: fechada (quando lançada ou descartada) ou aberta.
        /// </summary>
        public StatusParcela Status
        {
            get
            {
                return !this.Lancada && !this.Descartada
                    ? StatusParcela.Aberta
                    : StatusParcela.Fechada;
            }
        }      

        private Parcela()
        {
            this.Lancada = false;
            this.Descartada = false;
        }

        public Parcela(CadastrarParcelaEntrada cadastrarEntrada)
            : this()
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdAgendamento = cadastrarEntrada.IdAgendamento;
            this.IdFatura      = cadastrarEntrada.IdFatura;
            this.Data          = cadastrarEntrada.Data;
            this.Valor         = cadastrarEntrada.Valor;
            this.Observacao    = cadastrarEntrada.Observacao;
        }

        public void Alterar(AlterarParcelaEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdParcela != this.Id)
                return;

            this.Data       = alterarEntrada.Data;
            this.Valor      = alterarEntrada.Valor;
            this.Observacao = alterarEntrada.Observacao;
        }

        public void Descartar(DescartarParcelaEntrada descartarEntrada)
        {
            if (!descartarEntrada.Valido() || descartarEntrada.IdParcela != this.Id)
                return;

            this.Descartada     = true;
            this.MotivoDescarte = descartarEntrada.MotivoDescarte;
        }

        public void Lancar(LancarParcelaEntrada lancarEntrada)
        {
            if (!lancarEntrada.Valido() || lancarEntrada.IdParcela != this.Id)
                return;

            this.Lancada    = true;
            this.Valor      = lancarEntrada.Valor;
            this.Observacao = lancarEntrada.Observacao;
        }

        public override string ToString()
        {
            return $"{this.Data.ToString("dd/MM/yyyy")} - {this.Valor.ToString("C2")}";
        }
    }
}