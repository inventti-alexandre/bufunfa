namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um usuário
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; internal set; }

        /// <summary>
        /// Permissões de acesso do usuário
        /// </summary>
        public string[] PermissoesAcesso { get; internal set; }

        private Usuario()
        {

        }

        public Usuario(string nome, string email)
        {
            this.Nome = nome;
            this.Email = email;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
