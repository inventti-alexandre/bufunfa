using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Bufunfa.Dominio.Testes
{
    [TestClass]
    public class UnitTest1
    {
        private IUsuarioRepositorio _usuarioRepositorio;
        private IUsuarioServico _usuarioServico;

        public UnitTest1()
        {
            _usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();

            // Usuário encontrado
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns(new Usuario("Jorge Luiz Nogueira", "jlnpinheiro@gmail.com"));

            // Usuário inativo
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro3@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns(new Usuario("Jorge Luiz Nogueira", "jlnpinheiro@gmail.com", false));

            // Usuário e-mail incorreto
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro2@gmail.com", "60602435AE5A49EB3F2800C2A46AF810")
                .Returns((Usuario)null);

            // Usuário com senha incorreta
            _usuarioRepositorio
                .ObterPorEmailSenha("jlnpinheiro@gmail.com", "SenhaInvalida")
                .Returns((Usuario)null);

            _usuarioServico = Substitute.For<UsuarioServico>(_usuarioRepositorio);
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Sem_Email_Senha()
        {
            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada(string.Empty, string.Empty));

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Com_Email_Nao_Cadastrado()
        {
            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro2@gmail.com", "surfista"));

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Com_Senha_Incorreta()
        {
            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "SenhaInvalida"));

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Autenticar_Usuario_Inativo()
        {
            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro3@gmail.com", "surfista"));

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Autenticar_Usuario()
        {
            var saida = _usuarioServico.Autenticar(new AutenticarUsuarioEntrada("jlnpinheiro@gmail.com", "surfista"));

            Assert.IsTrue(saida.Sucesso);
        }

        
    }
}
