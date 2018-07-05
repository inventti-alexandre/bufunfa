namespace JNogueira.Bufunfa.Dominio
{
    /// <summary>
    /// Classe que armazena as permissões de acesso.
    /// </summary>
    public static class PermissaoAcesso
    {
        /// <summary>
        /// Permite realizar o cadastramento de usuários.
        /// </summary>
        public const string Usuarios = "usuarios";

        /// <summary>
        /// Permite realizar o cadastramento de contas.
        /// </summary>
        public const string Contas = "contas";

        /// <summary>
        /// Permite realizar o cadastramento de períodos.
        /// </summary>
        public const string Periodos = "periodos";

        /// <summary>
        /// Permite realizar o cadastramento de pessoas.
        /// </summary>
        public const string Pessoas = "pessoas";

        /// <summary>
        /// Permite realizar o cadastramento de categorias.
        /// </summary>
        public const string Categorias = "categorias";

        /// <summary>
        /// Permite realizar o cadastramento de cartões de crédito.
        /// </summary>
        public const string CartoesCredito = "cartoes";
    }
}
