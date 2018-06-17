using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;
using NETCore.Encrypt.Extensions;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado na autenticação de um usuário
    /// </summary>
    public class AutenticarUsuarioEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; set; }

        internal string CriarHashSenha()
        {
            return this.Senha.MD5();
        }

        public bool Valido()
        {
            this
                .NotificarSeNuloOuVazio(this.Email, "O e-mail é obrigatório e não foi informado.")
                .NotificarSeNuloOuVazio(this.Senha, "A senha é obrigatória e não foi informada.");

            if (this.Invalido)
                return false;


            this.NotificarSeEmailInvalido(this.Email, $"O e-mail informado {this.Email} é inválido.");

            return !this.Invalido;
        }
    }
}
