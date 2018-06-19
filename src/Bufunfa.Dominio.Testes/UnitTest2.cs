using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Bufunfa.Dominio.Testes
{
    [TestClass]
    [TestCategory("Periodo")]
    public class UnitTest2
    {
        private IPeriodoRepositorio _periodoRepositorio;
        private IPeriodoServico _periodoServico;
        private IUow _uow;

        public UnitTest2()
        {
            _uow = Substitute.For<IUow>();

            _periodoRepositorio = Substitute.For<IPeriodoRepositorio>();

            // Período usuário 1
            _periodoRepositorio.ObterPorId(1)
                .Returns(new Periodo(new CadastrarPeriodoEntrada(1, "Período 1", DateTime.Now, DateTime.Now.AddDays(5))));

            _periodoRepositorio.ObterPorUsuario(1)
                .Returns(new List<Periodo> { new Periodo(new CadastrarPeriodoEntrada(1, "Período 1", DateTime.Now, DateTime.Now.AddDays(5))) });

            // Período usuário 2
            _periodoRepositorio.ObterPorId(2)
                .Returns(new Periodo(new CadastrarPeriodoEntrada(2, "Período 2", DateTime.Now, DateTime.Now.AddDays(5))));

            // Período inexistente
            _periodoRepositorio.ObterPorId(3)
               .Returns((Periodo)null);

            _periodoRepositorio.ObterPorUsuario(3)
                .Returns((IEnumerable<Periodo>)null);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_De_Outro_Usuario()
        {
            var saida = _periodoServico.ObterPeriodoPorId(2, 1);

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Inexistente()
        {
            var saida = _periodoServico.ObterPeriodoPorId(3, 1);

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Com_Parametros_Invalidos()
        {
            var saida = _periodoServico.ObterPeriodoPorId(0, 0);

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Periodo_Por_Id()
        {
            var saida = _periodoServico.ObterPeriodoPorId(1, 1);

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodos_Por_Usuario_Com_Id_Usuario_Invalido()
        {
            var saida = _periodoServico.ObterPeriodosPorUsuario(0);

            Assert.IsFalse(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Periodos_Por_Usuario()
        {
            var saida = _periodoServico.ObterPeriodosPorUsuario(1);

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }
    }
}
