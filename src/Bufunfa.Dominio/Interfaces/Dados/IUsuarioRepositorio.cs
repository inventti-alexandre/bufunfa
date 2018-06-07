using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu e-mail
        /// </summary>
        Usuario ObterPorEmail(string email);

        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Usuario ObterPorEmailSenha(string email, string senha);
        
        /// <summary>
        /// Realiza a alteração da senha de um usuário
        /// </summary>
        void AlterarSenhaUsuario(AlterarSenhaUsuarioEntrada entrada);
    }
}
