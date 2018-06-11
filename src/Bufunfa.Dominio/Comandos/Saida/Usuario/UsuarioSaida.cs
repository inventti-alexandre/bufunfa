using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um usuário
    /// </summary>
    public class UsuarioSaida
    {
        public int Id { get; }

        public string Nome { get; }

        public string Email { get; }

        public bool Ativo { get; }

        public UsuarioSaida(Usuario usuario)
        {
            if (usuario == null)
                return;

            this.Id = usuario.Id;
            this.Nome = usuario.Nome.ToUpper();
            this.Email = usuario.Email.ToLower();
            this.Ativo = usuario.Ativo;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
