﻿using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova conta
    /// </summary>
    public class CadastrarContaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Tipo da conta
        /// </summary>
        public TipoConta Tipo { get; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get;  }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; }

        public CadastrarContaEntrada(
            int idUsuario,
            string nome,
            TipoConta tipo,
            decimal? valorSaldoInicial = null,
            string nomeInstituicao = null,
            string numeroAgencia = null,
            string numero = null)
        {
            this.IdUsuario         = idUsuario;
            this.Nome              = nome;
            this.Tipo              = tipo;
            this.ValorSaldoInicial = valorSaldoInicial;
            this.NomeInstituicao   = nomeInstituicao;
            this.NumeroAgencia     = numeroAgencia;
            this.Numero            = numero;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0,Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, ContaMensagem.Nome_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, ContaMensagem.Nome_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NomeInstituicao))
                this.NotificarSePossuirTamanhoSuperiorA(this.NomeInstituicao, 500, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NumeroAgencia))
                this.NotificarSePossuirTamanhoSuperiorA(this.NumeroAgencia, 20, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.Numero))
                this.NotificarSePossuirTamanhoSuperiorA(this.Numero, 20, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);
        }
    }
}
