using JNogueira.Bufunfa.Dominio.Entidades;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um período
    /// </summary>
    public class PeriodoSaida
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data início do período
        /// </summary>
        public DateTime DataInicio { get; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        public DateTime DataFim { get; }

        public PeriodoSaida(Periodo periodo)
        {
            if (periodo == null)
                return;

            this.Id = periodo.Id;
            this.Nome = periodo.Nome;
            this.DataInicio = periodo.DataInicio;
            this.DataFim = periodo.DataFim;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.DataInicio.ToString("dd/MM/yyyy")} até {this.DataFim.ToString("dd/MM/yyyy")}";
        }
    }
}
