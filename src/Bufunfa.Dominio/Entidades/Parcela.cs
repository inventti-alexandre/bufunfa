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
        public int IdAgendamento { get; private set; }

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
        public bool SeLancada { get; private set; }

        /// <summary>
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool SeDescartada { get; private set; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; private set; }

        /// <summary>
        /// Agendamento que originou a parcela
        /// </summary>
        public Agendamento Agendamento { get; private set; }

        private Parcela()
        {
            this.SeLancada = false;
            this.SeDescartada = false;
        }

        //public Parcela(CadastrarPeriodoEntrada cadastrarEntrada)
        //{
        //    if (!cadastrarEntrada.Valido())
        //        return;

        //    this.IdUsuario  = cadastrarEntrada.IdUsuario;
        //    this.Nome       = cadastrarEntrada.Nome;
        //    this.DataInicio = cadastrarEntrada.DataInicio;
        //    this.DataFim    = cadastrarEntrada.DataFim;
        //}

        //public void Alterar(AlterarPeriodoEntrada alterarEntrada)
        //{
        //    if (!alterarEntrada.Valido() || alterarEntrada.IdPeriodo != this.Id)
        //        return;

        //    this.Nome       = alterarEntrada.Nome;
        //    this.DataInicio = alterarEntrada.DataInicio;
        //    this.DataFim    = alterarEntrada.DataFim;
        //}

        public override string ToString()
        {
            return $"{this.Data.ToString("dd/MM/yyyy")} - {this.Valor.ToString("C2")}";
        }
    }
}