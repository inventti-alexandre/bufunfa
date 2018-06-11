namespace JNogueira.Bufunfa.Dominio
{
    /// <summary>
    /// Classe que armazena as permissões de acesso.
    /// </summary>
    public static class PermissaoAcesso
    {
        /// <summary>
        /// Permite realizar o cadastramento de outros usuários.
        /// </summary>
        public const string CadastrarUsuario = "cadastrar-usuario";

        /// <summary>
        /// Permite consultar as informaçõe de um usuário.
        /// </summary>
        public const string ConsultarUsuario = "consultar-usuario";
    }
}
