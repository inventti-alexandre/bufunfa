using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um novo período
    /// </summary>
    public class CadastrarPeriodoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data inicial do período
        /// </summary>
        public DateTime DataInicio { get; }

        /// <summary>
        /// Data final do período
        /// </summary>
        public DateTime DataFim { get; }

        public CadastrarPeriodoEntrada(int idUsuario, string nome, DateTime dataInicio, DateTime dataFim)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, $"O ID do usuário informado ({this.IdUsuario}) é inválido.")
                .NotificarSeNuloOuVazio(this.Nome, "O nome é obrigatório e não foi informado.")
                .NotificarSeMaiorOuIgualA(this.DataInicio, this.DataFim, "A data início do período é maior ou igual a data fim.");

            return !this.Invalido;
        }
    }
}
