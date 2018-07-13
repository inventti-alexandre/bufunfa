using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
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
    [TestCategory("PessoaServico")]
    public class PessoaServicoTeste
    {
        private IPessoaRepositorio _pessoaRepositorio;
        private IPessoaServico _pessoaServico;
        private IUow _uow;

        public PessoaServicoTeste()
        {
            _uow = Substitute.For<IUow>();

            _pessoaRepositorio = Substitute.For<IPessoaRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Obter_Pessoa_Por_Id_De_Outro_Usuario()
        {
            var idPessoa = 2;
            var idUsuario = 1;

            _pessoaRepositorio.ObterPorId(idPessoa)
                .Returns(new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 1")));

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ObterPessoaPorId(idPessoa, 2).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Pessoa_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Pessoa_Por_Id_Inexistente()
        {
            var idPessoa = 1;
            var idUsuario = 1;

            _pessoaRepositorio.ObterPorId(idPessoa)
                .Returns((Pessoa)null);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ObterPessoaPorId(idPessoa, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PessoaMensagem.Id_Pessoa_Nao_Existe, idPessoa)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Pessoa_Por_Id_Com_Parametros_Invalidos()
        {
            var idPessoa = 0;
            var idUsuario = 0;

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ObterPessoaPorId(idPessoa, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PessoaMensagem.Id_Pessoa_Invalido, idPessoa)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Pessoa_Por_Id()
        {
            var idPessoa = 1;
            var idUsuario = 1;

            _pessoaRepositorio.ObterPorId(idPessoa)
                .Returns(new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 1")));

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ObterPessoaPorId(1, 1).Result;

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Procurar_Pessoas_Com_Parametros_Invalidos()
        {
            var procurarEntrada = new ProcurarPessoaEntrada(0, "Abc", "ASC", -1, -1);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ProcurarPessoas(procurarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Paginacao_Pagina_Index_Invalido, -1)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Procurar_Pessoas()
        {
            var idUsuario = 1;

            var procurarEntrada = new ProcurarPessoaEntrada(idUsuario, "Nome", "ASC", 1, 1);

            var periodo1 = new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 1"));
            var periodo2 = new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 2"));

            _pessoaRepositorio.Procurar(procurarEntrada)
                .Returns(new ProcurarSaida(new[] { periodo1, periodo2 }, "Nome", "ASC", 2, 2, 1, 1));

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ProcurarPessoas(procurarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && (int)saida.Retorno.GetType().GetProperty("TotalPaginas").GetValue(saida.Retorno, null) == 2, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Pessoa_Com_Parametros_Invalidos()
        {
            var cadastroEntrada = new CadastrarPessoaEntrada(0, string.Empty);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.CadastrarPessoa(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Nome_Obrigatorio_Nao_Informado), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Pessoa_Com_Parametros_Invalidos()
        {
            var idUsuario = 0;

            var alterarEntrada = new AlterarPessoaEntrada(0, string.Empty, idUsuario);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.AlterarPessoa(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, idUsuario)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Pessoa_Com_Id_Inexistente()
        {
            var idUsuario = 1;
            var idPessoa = 1;

            _pessoaRepositorio.ObterPorId(idPessoa, true)
                .Returns((Pessoa)null);

            var alterarEntrada = new AlterarPessoaEntrada(idPessoa, "Pessoa 1", idUsuario);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.AlterarPessoa(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PessoaMensagem.Id_Pessoa_Nao_Existe, idPessoa)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Pessoa_De_Outro_Usuario()
        {
            var idUsuario = 1;
            var idPessoa = 1;

            var periodo = new Pessoa(new CadastrarPessoaEntrada(2, "Pessoa 1"));
            typeof(Pessoa).GetProperty("Id").SetValue(periodo, idPessoa);

            _pessoaRepositorio.ObterPorId(idPessoa, true)
                .Returns(periodo);

            var alterarEntrada = new AlterarPessoaEntrada(idPessoa, "Pessoa 1 alterada", idUsuario);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.AlterarPessoa(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Pessoa_Alterar_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Alterar_Pessoa()
        {
            var idUsuario = 1;
            var idPessoa = 1;

            var periodo = new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 1"));
            typeof(Pessoa).GetProperty("Id").SetValue(periodo, idPessoa);

            _pessoaRepositorio.ObterPorId(idPessoa, true)
                .Returns(periodo);

            var alterarEntrada = new AlterarPessoaEntrada(idPessoa, "Pessoa 1 alterada", idUsuario);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.AlterarPessoa(alterarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Pessoa_Alterada_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Pessoa_Com_Parametros_Invalidos()
        {
            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ExcluirPessoa(0, 0).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PessoaMensagem.Id_Pessoa_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Pessoa_Com_Id_Inexistente()
        {
            var idPessoa = 1;
            var idUsuario = 1;

            _pessoaRepositorio.ObterPorId(idPessoa)
               .Returns((Pessoa)null);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ExcluirPessoa(idPessoa, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(PessoaMensagem.Id_Pessoa_Nao_Existe, idPessoa)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Pessoa_De_Outro_Usuario()
        {
            var idPessoa = 1;
            var idUsuario = 1;

            var periodo = new Pessoa(new CadastrarPessoaEntrada(2, "Pessoa 1"));
            typeof(Pessoa).GetProperty("Id").SetValue(periodo, idPessoa);

            _pessoaRepositorio.ObterPorId(idPessoa)
               .Returns(periodo);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ExcluirPessoa(idPessoa, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Pessoa_Excluir_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Excluir_Pessoa()
        {
            var idUsuario = 1;
            var idPessoa = 1;

            var periodo = new Pessoa(new CadastrarPessoaEntrada(idUsuario, "Pessoa 1"));
            typeof(Pessoa).GetProperty("Id").SetValue(periodo, idPessoa);

            _pessoaRepositorio.ObterPorId(idPessoa)
                .Returns(periodo);

            _pessoaServico = Substitute.For<PessoaServico>(_pessoaRepositorio, _uow);

            var saida = _pessoaServico.ExcluirPessoa(idPessoa, idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == PessoaMensagem.Pessoa_Excluida_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}
