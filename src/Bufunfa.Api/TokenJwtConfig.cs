namespace JNogueira.Bufunfa.Api
{
    /// <summary>
    /// Classe que armazena as configurações do token JWT
    /// </summary>
    public class TokenJwtConfig
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiracaoEmSegundos { get; set; }
        public string SecretKey { get; set; }
    }
}
