using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado na alteração de senha de um usuário
    /// </summary>
    public class AlterarSenhaUsuarioEntrada : Notificavel, IComandoEntrada
    {
        public string Email { get; set; }
        
        public string SenhaAtual { get; set; }

        public string SenhaNova { get; set; }

        public string ConfirmacaoSenhaNova { get; set; }

        public bool Valido()
        {
            this
                .NotificarSeNuloOuVazio(Email, "O e-mail é obrigatório e não foi informado.")
                .NotificarSeNuloOuVazio(SenhaAtual, "A senha atual é obrigatória e não foi informada.")
                .NotificarSeNaoNuloOuVazio(SenhaNova, "A senha nova é obrigatória e não foi informada.");

            if (this.Invalido)
                return false;

            this
                .NotificarSeEmailInvalido(this.Email, $"O e-mail informado {this.Email} é inválido.")
                .NotificarSeDiferentes(this.SenhaNova, this.ConfirmacaoSenhaNova, "A senha e a confirmação da senha são diferentes. Verifique as senhas informadas.");

            return !this.Invalido;
        }
    }
}
