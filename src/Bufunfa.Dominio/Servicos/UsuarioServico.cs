using JNogueira.Bufunfa.Dominio.Comandos.Entrada.Usuario;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class UsuarioServico : Notificavel, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IComandoSaida Autenticar(AutenticarUsuarioComando autenticacaoComando)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (!autenticacaoComando.Valido())
                return new ComandoSaida(false, autenticacaoComando.Notificacoes.Select(x => x.Mensagem), null);

            var usuario = _usuarioRepositorio.ObterPorEmailSenha(autenticacaoComando.Email, autenticacaoComando.CriarHashSenha());

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, "Usuário não encontrado. Favor verificar o login e senha informados.");

            if (this.Invalido)
                return new ComandoSaida(false, this.Notificacoes.Select(x => x.Mensagem), null);

            // Verifica se o usuário está ativo
            this.NotificarSeFalso(usuario.Ativo, "Usuário inativo. Não é possível acessar o sistema.");

            if (this.Invalido)
                return new ComandoSaida(false, this.Notificacoes.Select(x => x.Mensagem), null);

            return new ComandoSaida(true, new[] { "Usuário autenticado com sucesso. "}, usuario);
        }
    }
}
