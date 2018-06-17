using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
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

        public ISaida Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (!autenticacaoEntrada.Valido())
                return new Saida(false, autenticacaoEntrada.Mensagens, null);

            var usuario = _usuarioRepositorio.ObterPorEmailSenha(autenticacaoEntrada.Email, autenticacaoEntrada.CriarHashSenha());

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, "Usuário não encontrado. Favor verificar o login e senha informados.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário está ativo
            this.NotificarSeFalso(usuario.Ativo, "Usuário inativo. Não é possível acessar o sistema.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Define as permissões de acesso (aqui, de forma estática, porém essas permissões poderiam ser obtidas do banco de dados).
            usuario.PermissoesAcesso = new[]
            {
                PermissaoAcesso.Usuarios,
                PermissaoAcesso.Contas
            };

            return new Saida(true, new[] { "Usuário autenticado com sucesso."}, usuario);
        }

        public ISaida ObterUsuarioPorEmail(string email)
        {
            // Verifica se um e-mail válido foi informado.
            this.NotificarSeEmailInvalido(email, "O e-mail informado é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var usuario = _usuarioRepositorio.ObterPorEmail(email);

            // Verifica se o usuário com o e-mail foi encontrado no banco
            this.NotificarSeNulo(usuario, $"Usuário não encontrado com o e-mail {email}.");

            if (this.Invalido)
                return new Saida(false, this.Notificacoes.Select(x => x.Mensagem), null);

            return new Saida(true, new[] { $"Usuário com o e-mail {email} encontrado." }, new UsuarioSaida(usuario));
        }
    }
}
