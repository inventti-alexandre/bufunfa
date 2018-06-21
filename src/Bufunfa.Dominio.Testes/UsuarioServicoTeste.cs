using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;

namespace Bufunfa.Dominio.Testes
{
    [TestClass]
    [TestCategory("UsuarioServico")]
    public class UsuarioServicoTeste
    {
        private IUsuarioRepositorio _usuarioRepositorio;
        private IUsuarioServico _usuarioServico;

        public UsuarioServicoTeste()
        {
            _usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Com_Parametros_Invalidos()
        {
            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);

            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada(string.Empty, string.Empty)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == UsuarioMensagem.Senha_Obrigatoria_Nao_Informada), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Com_Email_Nao_Cadastrado()
        {
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns((Usuario)null);

            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);

            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "surfista")).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == UsuarioMensagem.Usuario_Nao_Encontrado_Por_Login_Senha), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Com_Senha_Incorreta()
        {
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns(new Usuario("Jorge Luiz Nogueira", "jlnpinheiro@gmail.com"));

            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);

            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "SenhaInvalida")).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == UsuarioMensagem.Usuario_Nao_Encontrado_Por_Login_Senha), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Inativo()
        {
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns(new Usuario("Jorge Luiz Nogueira", "jlnpinheiro@gmail.com", false));

            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);

            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "surfista")).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == UsuarioMensagem.Usuario_Inativo), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Autenticar_Usuario()
        {
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns(new Usuario("Jorge Luiz Nogueira", "jlnpinheiro@gmail.com", true));

            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);

            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "surfista")).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == UsuarioMensagem.Usuario_Autenticado_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}
