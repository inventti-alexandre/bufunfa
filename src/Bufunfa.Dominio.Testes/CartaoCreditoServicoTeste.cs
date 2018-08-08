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
    [TestCategory("CartaoCreditoServico")]
    public class CartaoCreditoServicoTeste
    {
        private ICartaoCreditoRepositorio _cartaoCreditoRepositorio;
        private ICartaoCreditoServico _cartaoCreditoServico;
        private IUow _uow;

        public CartaoCreditoServicoTeste()
        {
            _uow = Substitute.For<IUow>();

            _cartaoCreditoRepositorio = Substitute.For<ICartaoCreditoRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Obter_Cartao_Credito_Por_Id_De_Outro_Usuario()
        {
            var idCartaoCredito = 1;
            var idUsuario = 1;

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito)
                .Returns(new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5)));

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartaoCreditoPorId(idCartaoCredito, 2).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Cartao_Credito_Por_Id_Inexistente()
        {
            var idCartaoCredito = 1;
            var idUsuario = 1;

            _cartaoCreditoRepositorio.ObterPorId(idUsuario)
                .Returns((CartaoCredito)null);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartaoCreditoPorId(idCartaoCredito, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartaoCredito)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Cartao_Credito_Por_Id_Com_Parametros_Invalidos()
        {
            var idCartaoCredito = 0;
            var idUsuario = 0;

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartaoCreditoPorId(idCartaoCredito, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CartaoCreditoMensagem.Id_Cartao_Invalido, idCartaoCredito)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Cartao_Credito_Por_Id()
        {
            var idCartaoCredito = 1;
            var idUsuario = 1;

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito)
                .Returns(new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5)));

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartaoCreditoPorId(1, 1).Result;

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Cartoes_Credito_Por_Usuario_Com_Id_Usuario_Invalido()
        {
            var idUsuario = 0;

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartoesCreditoPorUsuario(idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == Mensagem.Id_Usuario_Invalido), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Cartoes_Credito_Por_Usuario()
        {
            var idUsuario = 1;

            _cartaoCreditoRepositorio.ObterPorUsuario(idUsuario)
                .Returns(new List<CartaoCredito> { new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5)) });

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ObterCartoesCreditoPorUsuario(idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Cartao_Credito_Com_Parametros_Invalidos()
        {
            var cadastroEntrada = new CadastrarCartaoCreditoEntrada(0, string.Empty, -5000, 33);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.CadastrarCartaoCredito(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Nome_Obrigatorio_Nao_Informado), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Cartao_Credito_Com_Mesmo_Nome_De_Outra_CartaoCredito()
        {
            var idUsuario = 1;

            _cartaoCreditoRepositorio.VerificarExistenciaPorNome(idUsuario, "Cartão 1")
               .Returns(true);

            var cadastroEntrada = new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.CadastrarCartaoCredito(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Cartao_Credito_Com_Parametros_Invalidos()
        {
            var idCartaoCredito = 0;
            var idUsuario = 0;

            var alterarEntrada = new AlterarCartaoCreditoEntrada(idCartaoCredito, string.Empty, idUsuario, -5000, 32);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == Mensagem.Id_Usuario_Invalido), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Cartao_Credito_Com_Id_Inexistente()
        {
            var idUsuario = 1;
            var idCartaoCredito = 1;

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito, true)
                .Returns((CartaoCredito)null);

            var alterarEntrada = new AlterarCartaoCreditoEntrada(idCartaoCredito, "Cartão 1", idUsuario, 5000, 5);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartaoCredito)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Cartao_Credito_Com_Mesmo_Nome_De_Outra_CartaoCredito()
        {
            var idUsuario = 1;
            var idCartaoCredito = 1;

            var cartao = new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5));
            typeof(CartaoCredito).GetProperty("Id").SetValue(cartao, idCartaoCredito);

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito, true)
                .Returns(cartao);

            _cartaoCreditoRepositorio.VerificarExistenciaPorNome(idUsuario, "Cartão 1", idCartaoCredito)
                .Returns(true);

            var alterarEntrada = new AlterarCartaoCreditoEntrada(idCartaoCredito, "Cartão 1", idUsuario, 5000, 5);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Cartao_Credito_De_Outro_Usuario()
        {
            var idUsuario = 1;
            var idCartaoCredito = 1;

            var conta = new CartaoCredito(new CadastrarCartaoCreditoEntrada(2, "Cartão 1", 5000, 5));
            typeof(CartaoCredito).GetProperty("Id").SetValue(conta, idCartaoCredito);

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito, true)
                .Returns(conta);

            var alterarEntrada = new AlterarCartaoCreditoEntrada(idCartaoCredito, "Cartão 1 alterado", idUsuario, 5000, 5);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Alterar_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Alterar_Cartao_Credito()
        {
            var idUsuario = 1;
            var idCartaoCredito = 1;

            var cartao = new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5));
            typeof(CartaoCredito).GetProperty("Id").SetValue(cartao, idCartaoCredito);

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito, true)
                .Returns(cartao);

            var alterarEntrada = new AlterarCartaoCreditoEntrada(idCartaoCredito, "Cartão 1 alterado", idUsuario, 5000, 5);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Alterado_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Cartao_Credito_Com_Parametros_Invalidos()
        {
            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ExcluirCartaoCredito(0, 0).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CartaoCreditoMensagem.Id_Cartao_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Cartao_Credito_Com_Id_Inexistente()
        {
            var idCartaoCredito = 1;
            var idUsuario = 1;

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito)
               .Returns((CartaoCredito)null);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ExcluirCartaoCredito(idCartaoCredito, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartaoCredito)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Cartao_Credito_De_Outro_Usuario()
        {
            var idCartaoCredito = 1;
            var idUsuario = 1;

            var cartao = new CartaoCredito(new CadastrarCartaoCreditoEntrada(2, "Cartão 1", 5000, 5));
            typeof(CartaoCredito).GetProperty("Id").SetValue(cartao, idCartaoCredito);

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito)
               .Returns(cartao);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ExcluirCartaoCredito(idCartaoCredito, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Excluir_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Excluir_Cartao_Credito()
        {
            var idUsuario = 1;
            var idCartaoCredito = 1;

            var cartao = new CartaoCredito(new CadastrarCartaoCreditoEntrada(idUsuario, "Cartão 1", 5000, 5));
            typeof(CartaoCredito).GetProperty("Id").SetValue(cartao, idCartaoCredito);

            _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito)
                .Returns(cartao);

            _cartaoCreditoServico = Substitute.For<CartaoCreditoServico>(_cartaoCreditoRepositorio, _uow);

            var saida = _cartaoCreditoServico.ExcluirCartaoCredito(idCartaoCredito, idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CartaoCreditoMensagem.Cartao_Excluido_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}