namespace JNogueira.Bufunfa.Dominio
{
    /// <summary>
    /// Tipo de categoria
    /// </summary>
    public static class TipoCategoria
    {
        public static readonly string Credito = "C";
        public static readonly string Debito = "D";
    }

    /// <summary>
    /// Métodos de pagamento
    /// </summary>
    public enum MetodoPagamento
    {
        Cheque        = 1,
        Debito        = 2,
        Deposito      = 3,
        Transferencia = 4,
        Dinheiro      = 5
    }

    /// <summary>
    /// Periodicidade
    /// </summary>
    public enum Periodicidade
    {
        Mensal     = 1,
        Trimestral = 3,
        Semestral  = 6,
        Anual      = 12
    }

    /// <summary>
    /// Status de uma parcela
    /// </summary>
    public enum StatusParcela
    {
        Aberta,
        Fechada
    }

    /// <summary>
    /// Tipo da conta
    /// </summary>
    public enum TipoConta
    {
        ContaCorrente = 1,
        Investimento  = 2
    }
}
