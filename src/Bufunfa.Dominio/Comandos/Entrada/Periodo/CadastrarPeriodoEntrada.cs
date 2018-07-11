using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
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

        public CadastrarPeriodoEntrada(
            int idUsuario,
            string nome,
            DateTime dataInicio,
            DateTime dataFim)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeNuloOuVazio(this.Nome, PeriodoMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeMaiorOuIgualA(this.DataInicio, this.DataFim, PeriodoMensagem.Data_Periodo_Invalidas);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 50, PeriodoMensagem.Nome_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
