namespace JNogueira.Bufunfa.Api
{
    public class TokenParametros
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Segundos { get; set; }
        public string SecretKey { get; set; }
    }
}
