using JNogueira.Bufunfa.Dominio.Comandos.Entrada;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um cartão de crédito
    /// </summary>
    public class CartaoCredito
    {
        /// <summary>
        /// Id do cartão
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; private set; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; private set; }

        private CartaoCredito()
        {

        }

        //public CartaoCredito(CadastrarContaEntrada cadastrarEntrada)
        //    : this()
        //{
        //    if (!cadastrarEntrada.Valido())
        //        return;

        //    this.IdUsuario         = cadastrarEntrada.IdUsuario;
        //    this.Nome              = cadastrarEntrada.Nome;
        //    this.ValorSaldoInicial = cadastrarEntrada.ValorSaldoInicial;
        //    this.NomeInstituicao   = cadastrarEntrada.NomeInstituicao;
        //    this.NumeroAgencia     = cadastrarEntrada.NumeroAgencia;
        //    this.Numero            = cadastrarEntrada.Numero;
        //}

        //public void Alterar(AlterarContaEntrada alterarEntrada)
        //{
        //    if (!alterarEntrada.Valido() || alterarEntrada.IdConta != this.Id)
        //        return;

        //    this.Nome              = alterarEntrada.Nome;
        //    this.ValorSaldoInicial = alterarEntrada.ValorSaldoInicial;
        //    this.NomeInstituicao   = alterarEntrada.NomeInstituicao;
        //    this.NumeroAgencia     = alterarEntrada.NumeroAgencia;
        //    this.Numero            = alterarEntrada.Numero;
        //}

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
