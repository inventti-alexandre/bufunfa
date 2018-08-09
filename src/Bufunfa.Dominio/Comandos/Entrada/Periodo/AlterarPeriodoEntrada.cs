using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para alterar um período
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

        public AlterarPeriodoEntrada(
            int idPeriodo,
            string nome,
            DateTime dataInicio,
            DateTime dataFim,
            int idUsuario)
        {
            this.IdPeriodo = idPeriodo;
            this.Nome = nome;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
            this.IdUsuario = idUsuario;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdPeriodo, 0, PeriodoMensagem.Id_Periodo_Invalido)
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, PeriodoMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeMaiorOuIgualA(this.DataInicio, this.DataFim, PeriodoMensagem.Data_Periodo_Invalidas);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 50, PeriodoMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
