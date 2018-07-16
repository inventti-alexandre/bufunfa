using JNogueira.Bufunfa.Dominio.Comandos.Entrada;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma conta (conta corrente, conta poupança, título público, etc)
    /// </summary>
    public class Conta
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Tipo da conta
        /// </summary>
        public TipoConta Tipo { get; private set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; private set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; private set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; private set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; private set; }

        private Conta()
        {

        }

        public Conta(CadastrarContaEntrada cadastrarEntrada)
            : this()
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdUsuario         = cadastrarEntrada.IdUsuario;
            this.Nome              = cadastrarEntrada.Nome;
            this.Tipo              = cadastrarEntrada.Tipo;
            this.ValorSaldoInicial = cadastrarEntrada.ValorSaldoInicial;
            this.NomeInstituicao   = cadastrarEntrada.NomeInstituicao;
            this.NumeroAgencia     = cadastrarEntrada.NumeroAgencia;
            this.Numero            = cadastrarEntrada.Numero;
        }

        public void Alterar(AlterarContaEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdConta != this.Id)
                return;

            this.Nome              = alterarEntrada.Nome;
            this.Tipo              = alterarEntrada.Tipo;
            this.ValorSaldoInicial = alterarEntrada.ValorSaldoInicial;
            this.NomeInstituicao   = alterarEntrada.NomeInstituicao;
            this.NumeroAgencia     = alterarEntrada.NumeroAgencia;
            this.Numero            = alterarEntrada.Numero;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
