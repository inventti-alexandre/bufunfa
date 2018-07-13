using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para alterar um lançamento
    /// </summary>
    public class AlterarLancamentoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int IdConta { get; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação do lançamento
        /// </summary>
        public string Observacao { get; }

        public AlterarLancamentoEntrada(
            int idLancamento,
            int idConta,
            int idCategoria,
            DateTime data,
            decimal valor,
            int idUsuario,
            int? idPessoa = null,
            string observacao = null)
        {
            this.IdLancamento = idLancamento;
            this.IdUsuario    = idUsuario;
            this.IdConta      = idConta;
            this.IdCategoria  = idCategoria;
            this.Data         = data;
            this.Valor        = valor;
            this.IdPessoa     = idPessoa;
            this.Observacao   = observacao;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdLancamento, 0, string.Format(LancamentoMensagem.Id_Lancamento_Invalido, this.IdLancamento))
                .NotificarSeMenorOuIgualA(this.IdConta, 0, string.Format(LancamentoMensagem.Id_Conta_Invalido, this.IdConta))
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, string.Format(LancamentoMensagem.Id_Categoria_Invalido, this.IdCategoria))
                .NotificarSeMaiorQue(this.Data, DateTime.Today, LancamentoMensagem.Data_Lancamento_Maior_Data_Corrente);

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorQue(this.IdPessoa.Value, 1, string.Format(LancamentoMensagem.Id_Pessoa_Invalido, this.IdPessoa.Value));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, LancamentoMensagem.Observacao_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
