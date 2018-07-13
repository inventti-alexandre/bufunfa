using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para lançar uma parcela
    /// </summary>
    public class LancarParcelaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da parcela
        /// </summary>
        public int IdParcela { get; }

        /// <summary>
        /// Id da conta onde o lançamento que será cadastrado
        /// </summary>
        public int IdConta { get; }

        /// <summary>
        /// Id da categoria do lançamento que será cadastrado
        /// </summary>
        public int IdCategoria{ get; }

        /// <summary>
        /// Id da pessoa do lançamento que será cadastrado
        /// </summary>
        public int? IdPessoa { get; }

        /// <summary>
        /// Data do lançamento da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor do lançamento da parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação sobre o lançamento da parcela
        /// </summary>
        public string Observacao { get; }

        public LancarParcelaEntrada(
            int idParcela,
            int idConta,
            int idCategoria,
            DateTime data,
            decimal valor,
            int idUsuario,
            int? idPessoa = null,
            string observacao = null)
        {
            this.IdParcela   = idParcela;
            this.IdConta     = idConta;
            this.IdCategoria = idCategoria;
            this.IdPessoa    = idPessoa;
            this.IdUsuario   = idUsuario;
            this.Data        = data;
            this.Valor       = valor;
            this.Observacao  = observacao;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdParcela, 0, string.Format(ParcelaMensagem.Id_Parcela_Invalido, this.IdParcela))
                .NotificarSeMenorOuIgualA(this.IdConta, 0, string.Format(ParcelaMensagem.Id_Conta_Lancar_Parcela_Invalido, this.IdConta))
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, string.Format(ParcelaMensagem.Id_Categoria_Lancar_Parcela_Invalido, this.IdCategoria))
                .NotificarSeMaiorQue(this.Data, DateTime.Today, ParcelaMensagem.Data_Lancamento_Maior_Data_Corrente);

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorOuIgualA(this.IdPessoa.Value, 0, string.Format(ParcelaMensagem.Id_Pessoa_Lancar_Parcela_Invalido, this.IdPessoa.Value));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ParcelaMensagem.Observacao_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
