using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de saída para as informações de um cartão de crédito
    /// </summary>
    public class CartaoCreditoSaida
    {
        /// <summary>
        /// Id do cartão
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; }

        public CartaoCreditoSaida(CartaoCredito cartao)
        {
            if (cartao == null)
                return;

            this.Id                  = cartao.Id;
            this.Nome                = cartao.Nome;
            this.ValorLimite         = cartao.ValorLimite;
            this.DiaVencimentoFatura = cartao.DiaVencimentoFatura;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
