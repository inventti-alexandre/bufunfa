using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de uma conta
    /// </summary>
    public class ContaSaida
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int Id { get; }

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
        public string NumeroAgencia { get; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; }

        public ContaSaida(Conta conta)
        {
            if (conta == null)
                return;

            this.Id                = conta.Id;
            this.Nome              = conta.Nome;
            this.Tipo              = conta.Tipo;
            this.ValorSaldoInicial = conta.ValorSaldoInicial;
            this.NomeInstituicao   = conta.NomeInstituicao;
            this.NumeroAgencia     = conta.NumeroAgencia;
            this.Numero            = conta.Numero;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
