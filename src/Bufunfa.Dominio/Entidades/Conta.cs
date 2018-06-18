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
        public string Nome { get; set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; set; }

        private Conta()
        {

        }

        public Conta(int idUsuario, string nome)
            : this()
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
