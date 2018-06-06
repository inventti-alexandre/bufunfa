namespace JNogueira.Bufunfa.Dominio.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Nome { get; }

        public string Email { get; }

        public bool Ativo { get; internal set; }

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
