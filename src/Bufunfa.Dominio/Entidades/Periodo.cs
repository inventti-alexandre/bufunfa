using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma período
    /// </summary>
    public class Periodo
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        public DateTime DataFim { get; set; }

        private Periodo()
        {
        }

        public Periodo(int idUsuario, string nome, DateTime dataInicio, DateTime dataFim)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.DataInicio.ToString("dd/MM/yyyy")} até {this.DataFim.ToString("dd/MM/yyyy")}";
        }
    }
}