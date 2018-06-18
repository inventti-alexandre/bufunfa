using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um período
    /// </summary>
    public class AlterarPeriodoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID do período
        /// </summary>
        public int IdPeriodo { get; }

        /// <summary>
        /// Nome do período
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

        public AlterarPeriodoEntrada(int idPeriodo, string nome, DateTime dataInicio, DateTime dataFim, int idUsuario)
        {
            this.IdPeriodo = idPeriodo;
            this.Nome = nome;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
            this.IdUsuario = idUsuario;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdPeriodo, 0, $"O ID do período informado ({this.IdPeriodo}) é inválido.")
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, $"O ID do usuário informado ({this.IdUsuario}) é inválido.")
                .NotificarSeNuloOuVazio(this.Nome, "O nome é obrigatório e não foi informado.")
                .NotificarSeMaiorOuIgualA(this.DataInicio, this.DataFim, "A data início do período é maior ou igual a data fim.");

            return !this.Invalido;
        }
    }
}
