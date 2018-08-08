using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bufunfa.Dominio.Testes
{
    [TestClass]
    [TestCategory("PeriodoServico")]
    public class PeriodoServicoTeste
    {
        private IPeriodoRepositorio _periodoRepositorio;
        private IPeriodoServico _periodoServico;
        private IUow _uow;

        public PeriodoServicoTeste()
        {
            _uow = Substitute.For<IUow>();

            _periodoRepositorio = Substitute.For<IPeriodoRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_De_Outro_Usuario()
        {
            var idPeriodo = 2;
            var idUsuario = 1;

            _periodoRepositorio.ObterPorId(idPeriodo)
                .Returns(new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now.Date, DateTime.Now.Date.AddDays(5))));

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodoPorId(idPeriodo, 2).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodo_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Inexistente()
        {
            var idPeriodo = 1;
            var idUsuario = 1;

            _periodoRepositorio.ObterPorId(idPeriodo)
                .Returns((Periodo)null);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodoPorId(idPeriodo, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodo_Por_Id_Com_Parametros_Invalidos()
        {
            var idPeriodo = 0;
            var idUsuario = 0;

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodoPorId(idPeriodo, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PeriodoMensagem.Id_Periodo_Invalido, idPeriodo)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Periodo_Por_Id()
        {
            var idPeriodo = 1;
            var idUsuario = 1;

            _periodoRepositorio.ObterPorId(idPeriodo)
                .Returns(new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now.Date, DateTime.Now.Date.AddDays(5))));

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodoPorId(1, 1).Result;

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Periodos_Por_Usuario_Com_Id_Usuario_Invalido()
        {
            var idUsuario = 0;

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodosPorUsuario(idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == Mensagem.Id_Usuario_Invalido), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Periodos_Por_Usuario()
        {
            var idUsuario = 1;

            _periodoRepositorio.ObterPorUsuario(idUsuario)
                .Returns(new List<Periodo> { new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now.Date, DateTime.Now.Date.AddDays(5))) });

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ObterPeriodosPorUsuario(idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodos_Encontrados_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Procurar_Periodos_Com_Parametros_Invalidos()
        {
            var procurarEntrada = new ProcurarPeriodoEntrada(0, "Abc", "ASC", -1, -1);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ProcurarPeriodos(procurarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Paginacao_Pagina_Index_Invalido, -1)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Procurar_Periodos()
        {
            var idUsuario = 1;

            var procurarEntrada = new ProcurarPeriodoEntrada(idUsuario, "Nome", "ASC", 1, 1);

            var periodo1 = new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            var periodo2 = new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 2", DateTime.Now, DateTime.Now.AddDays(5)));

            _periodoRepositorio.Procurar(procurarEntrada)
                .Returns(new ProcurarSaida(new[] { periodo1, periodo2 }, "Nome", "ASC", 2, 2, 1, 1));

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ProcurarPeriodos(procurarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && (int)saida.Retorno.GetType().GetProperty("TotalPaginas").GetValue(saida.Retorno, null) == 2, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Periodo_Com_Parametros_Invalidos()
        {
            var cadastroEntrada = new CadastrarPeriodoEntrada(0, string.Empty, DateTime.Now, DateTime.Now.AddDays(-2));

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.CadastrarPeriodo(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Data_Periodo_Invalidas), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Periodo_Com_Datas_Abrangidas_Por_Outro_Periodo()
        {
            var idUsuario = 1;

            _periodoRepositorio.VerificarExistenciaPorDataInicioFim(idUsuario, DateTime.Now.Date, DateTime.Now.Date.AddDays(5))
               .Returns(true);

            var cadastroEntrada = new CadastrarPeriodoEntrada(idUsuario, "Período X", DateTime.Now.Date, DateTime.Now.Date.AddDays(5));

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.CadastrarPeriodo(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Datas_Abrangidas_Outro_Periodo), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Periodo_Com_Parametros_Invalidos()
        {
            var idUsuario = 0;

            var alterarEntrada = new AlterarPeriodoEntrada(0, string.Empty, DateTime.Now, DateTime.Now.AddDays(-2), idUsuario);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.AlterarPeriodo(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == Mensagem.Id_Usuario_Invalido), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Periodo_Com_Id_Inexistente()
        {
            var idUsuario = 1;
            var idPeriodo = 1;

            _periodoRepositorio.ObterPorId(idPeriodo, true)
                .Returns((Periodo)null);

            var alterarEntrada = new AlterarPeriodoEntrada(idPeriodo, "Período 1", DateTime.Now, DateTime.Now.AddDays(2), idUsuario);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.AlterarPeriodo(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Periodo_Com_Datas_Abrangidas_Por_Outro_Periodo()
        {
            var idUsuario = 1;
            var idPeriodo = 1;
            
            var periodo = new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            typeof(Periodo).GetProperty("Id").SetValue(periodo, idPeriodo);

            _periodoRepositorio.ObterPorId(idPeriodo, true)
                .Returns(periodo);

            _periodoRepositorio.VerificarExistenciaPorDataInicioFim(idUsuario, DateTime.Now.Date, DateTime.Now.Date.AddDays(5), idPeriodo)
                .Returns(true);

            var alterarEntrada = new AlterarPeriodoEntrada(idPeriodo, "Período 1 alterado", DateTime.Now.Date, DateTime.Now.Date.AddDays(5), idUsuario);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.AlterarPeriodo(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Datas_Abrangidas_Outro_Periodo), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Periodo_De_Outro_Usuario()
        {
            var idUsuario = 1;
            var idPeriodo = 1;

            var periodo = new Periodo(new CadastrarPeriodoEntrada(2, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            typeof(Periodo).GetProperty("Id").SetValue(periodo, idPeriodo);

            _periodoRepositorio.ObterPorId(idPeriodo, true)
                .Returns(periodo);

            var alterarEntrada = new AlterarPeriodoEntrada(idPeriodo, "Período 1 alterado", DateTime.Now.Date, DateTime.Now.Date.AddDays(5), idUsuario);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.AlterarPeriodo(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodo_Alterar_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Alterar_Periodo()
        {
            var idUsuario = 1;
            var idPeriodo = 1;

            var periodo = new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            typeof(Periodo).GetProperty("Id").SetValue(periodo, idPeriodo);

            _periodoRepositorio.ObterPorId(idPeriodo, true)
                .Returns(periodo);

            var alterarEntrada = new AlterarPeriodoEntrada(idPeriodo, "Período 1 alterado", DateTime.Now.Date, DateTime.Now.Date.AddDays(5), idUsuario);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.AlterarPeriodo(alterarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodo_Alterado_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Periodo_Com_Parametros_Invalidos()
        {
            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ExcluirPeriodo(0, 0).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PeriodoMensagem.Id_Periodo_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Periodo_Com_Id_Inexistente()
        {
            var idPeriodo = 1;
            var idUsuario = 1;

            _periodoRepositorio.ObterPorId(idPeriodo)
               .Returns((Periodo)null);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ExcluirPeriodo(idPeriodo, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Periodo_De_Outro_Usuario()
        {
            var idPeriodo = 1;
            var idUsuario = 1;

            var periodo = new Periodo(new CadastrarPeriodoEntrada(2, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            typeof(Periodo).GetProperty("Id").SetValue(periodo, idPeriodo);

            _periodoRepositorio.ObterPorId(idPeriodo)
               .Returns(periodo);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ExcluirPeriodo(idPeriodo, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodo_Excluir_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Excluir_Periodo()
        {
            var idUsuario = 1;
            var idPeriodo = 1;

            var periodo = new Periodo(new CadastrarPeriodoEntrada(idUsuario, "Período 1", DateTime.Now, DateTime.Now.AddDays(5)));
            typeof(Periodo).GetProperty("Id").SetValue(periodo, idPeriodo);

            _periodoRepositorio.ObterPorId(idPeriodo)
                .Returns(periodo);

            _periodoServico = Substitute.For<PeriodoServico>(_periodoRepositorio, _uow);

            var saida = _periodoServico.ExcluirPeriodo(idPeriodo, idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == PeriodoMensagem.Periodo_Excluido_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}
