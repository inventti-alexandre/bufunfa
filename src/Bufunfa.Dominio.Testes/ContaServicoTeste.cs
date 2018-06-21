using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;

namespace Bufunfa.Dominio.Testes
{
    [TestClass]
    [TestCategory("ContaServico")]
    public class ContaServicoTeste
    {
        private IContaRepositorio _contaRepositorio;
        private IContaServico _contaServico;
        private IUow _uow;

        public ContaServicoTeste()
        {
            _uow = Substitute.For<IUow>();

            _contaRepositorio = Substitute.For<IContaRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Obter_Conta_Por_Id_De_Outro_Usuario()
        {
            var idConta = 1;
            var idUsuario = 1;

            _contaRepositorio.ObterPorId(idConta)
                .Returns(new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1")));

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContaPorId(idConta, 2).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Inexistente()
        {
            var idConta = 1;
            var idUsuario = 1;

            _contaRepositorio.ObterPorId(idUsuario)
                .Returns((Conta)null);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContaPorId(idConta, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(ContaMensagem.Id_Conta_Nao_Existe, idConta)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Com_Parametros_Invalidos()
        {
            var idConta = 0;
            var idUsuario = 0;

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContaPorId(idConta, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(ContaMensagem.Id_Conta_Invalido, idConta)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Conta_Por_Id()
        {
            var idConta = 1;
            var idUsuario = 1;

            _contaRepositorio.ObterPorId(idConta)
                .Returns(new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1")));

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContaPorId(1, 1).Result;

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Contas_Por_Usuario_Com_Id_Usuario_Invalido()
        {
            var idUsuario = 0;

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContasPorUsuario(idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, idUsuario)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Contas_Por_Usuario()
        {
            var idUsuario = 1;

            _contaRepositorio.ObterPorUsuario(idUsuario)
                .Returns(new List<Conta> { new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1")) });

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ObterContasPorUsuario(idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Contas_Encontradas_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Conta_Com_Parametros_Invalidos()
        {
            var cadastroEntrada = new CadastrarContaEntrada(0, string.Empty);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.CadastrarConta(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Nome_Obrigatorio_Nao_Informado), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Conta_Com_Mesmo_Nome_De_Outra_Conta()
        {
            var idUsuario = 1;

            _contaRepositorio.VerificarExistenciaPorNome(idUsuario, "Conta 1")
               .Returns(true);

            var cadastroEntrada = new CadastrarContaEntrada(idUsuario, "Conta 1");

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.CadastrarConta(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Com_Mesmo_Nome), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Conta_Com_Parametros_Invalidos()
        {
            var idConta = 0;
            var idUsuario = 0;

            var alterarEntrada = new AlterarContaEntrada(idConta, string.Empty, idUsuario);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.AlterarConta(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, idUsuario)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Conta_Com_Id_Inexistente()
        {
            var idUsuario = 1;
            var idConta = 1;

            _contaRepositorio.ObterPorId(idConta, true)
                .Returns((Conta)null);

            var alterarEntrada = new AlterarContaEntrada(idConta, "Período 1", idUsuario);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.AlterarConta(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(ContaMensagem.Id_Conta_Nao_Existe, idConta)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Conta_Com_Mesmo_Nome_De_Outra_Conta()
        {
            var idUsuario = 1;
            var idConta = 1;

            var conta = new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1"));
            typeof(Conta).GetProperty("Id").SetValue(conta, idConta);

            _contaRepositorio.ObterPorId(idConta, true)
                .Returns(conta);

            _contaRepositorio.VerificarExistenciaPorNome(idUsuario, "Conta 1", idConta)
                .Returns(true);

            var alterarEntrada = new AlterarContaEntrada(idConta, "Conta 1", idUsuario);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.AlterarConta(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Com_Mesmo_Nome), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Conta_De_Outro_Usuario()
        {
            var idUsuario = 1;
            var idConta = 1;

            var conta = new Conta(new CadastrarContaEntrada(2, "Conta 1"));
            typeof(Conta).GetProperty("Id").SetValue(conta, idConta);

            _contaRepositorio.ObterPorId(idConta, true)
                .Returns(conta);

            var alterarEntrada = new AlterarContaEntrada(idConta, "Conta 1 alterada", idUsuario);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.AlterarConta(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Alterar_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Alterar_Periodo()
        {
            var idUsuario = 1;
            var idConta = 1;

            var periodo = new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1"));
            typeof(Conta).GetProperty("Id").SetValue(periodo, idConta);

            _contaRepositorio.ObterPorId(idConta, true)
                .Returns(periodo);

            var alterarEntrada = new AlterarContaEntrada(idConta, "Conta 1 alterada", idUsuario);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.AlterarConta(alterarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Alterada_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Conta_Com_Parametros_Invalidos()
        {
            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ExcluirConta(0, 0).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(ContaMensagem.Id_Conta_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Conta_Com_Id_Inexistente()
        {
            var idConta = 1;
            var idUsuario = 1;

            _contaRepositorio.ObterPorId(idConta)
               .Returns((Conta)null);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ExcluirConta(idConta, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(ContaMensagem.Id_Conta_Nao_Existe, idConta)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Conta_De_Outro_Usuario()
        {
            var idConta = 1;
            var idUsuario = 1;

            var conta = new Conta(new CadastrarContaEntrada(2, "Conta 1"));
            typeof(Conta).GetProperty("Id").SetValue(conta, idConta);

            _contaRepositorio.ObterPorId(idConta)
               .Returns(conta);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ExcluirConta(idConta, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Excluir_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Excluir_Conta()
        {
            var idUsuario = 1;
            var idConta = 1;

            var conta = new Conta(new CadastrarContaEntrada(idUsuario, "Conta 1"));
            typeof(Conta).GetProperty("Id").SetValue(conta, idConta);

            _contaRepositorio.ObterPorId(idConta)
                .Returns(conta);

            _contaServico = Substitute.For<ContaServico>(_contaRepositorio, _uow);

            var saida = _contaServico.ExcluirConta(idConta, idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == ContaMensagem.Conta_Excluida_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}