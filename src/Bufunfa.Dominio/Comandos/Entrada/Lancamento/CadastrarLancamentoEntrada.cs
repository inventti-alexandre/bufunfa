using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um novo lançamento
    /// </summary>
    public class CadastrarLancamentoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

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
        /// Id da parcela
        /// </summary>
        public int? IdParcela { get; }

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

        public CadastrarLancamentoEntrada(
            int idUsuario,
            int idConta,
            int idCategoria,
            DateTime data,
            decimal valor,
            int? idPessoa = null,
            int? idParcela = null,
            string observacao = null)
        {
            this.IdUsuario   = idUsuario;
            this.IdConta     = idConta;
            this.IdCategoria = idCategoria;
            this.IdParcela   = idParcela;
            this.Data        = data;
            this.Valor       = valor;
            this.IdPessoa    = idPessoa;
            this.Observacao  = observacao;

            Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMenorOuIgualA(this.IdConta, 0, ContaMensagem.Id_Conta_Invalido)
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido)
                .NotificarSeMaiorQue(this.Data, DateTime.Today, LancamentoMensagem.Data_Lancamento_Maior_Data_Corrente);

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorQue(this.IdPessoa.Value, 1, PessoaMensagem.Id_Pessoa_Invalido);

            if (this.IdParcela.HasValue)
                this.NotificarSeMenorQue(this.IdParcela.Value, 1, ParcelaMensagem.Id_Parcela_Invalido);

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, LancamentoMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
