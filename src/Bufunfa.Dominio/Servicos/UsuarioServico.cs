using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class UsuarioServico : Notificavel, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<ISaida> Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (!autenticacaoEntrada.Valido())
                return new Saida(false, autenticacaoEntrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(autenticacaoEntrada.Email, autenticacaoEntrada.Senha);

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, UsuarioMensagem.Usuario_Nao_Encontrado_Por_Login_Senha);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário está ativo
            this.NotificarSeFalso(usuario.Ativo, UsuarioMensagem.Usuario_Inativo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Define as permissões de acesso (aqui, de forma estática, porém essas permissões poderiam ser obtidas do banco de dados).
            usuario.PermissoesAcesso = new[]
            {
                PermissaoAcesso.Usuarios,
                PermissaoAcesso.Contas,
                PermissaoAcesso.Periodos
            };

            return new Saida(true, new[] { UsuarioMensagem.Usuario_Autenticado_Com_Sucesso }, new UsuarioSaida(usuario));
        }
    }
}
