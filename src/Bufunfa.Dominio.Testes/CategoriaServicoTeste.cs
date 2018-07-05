using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
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
    [TestCategory("CategoriaServico")]
    public class CategoriaServicoTeste
    {
        private ICategoriaRepositorio _categoriaRepositorio;
        private ICategoriaServico _categoriaServico;
        private IUow _uow;

        public CategoriaServicoTeste()
        {
            _uow = Substitute.For<IUow>();

            _categoriaRepositorio = Substitute.For<ICategoriaRepositorio>();
        }

        [TestMethod]
        public void Nao_Deve_Obter_Categoria_Por_Id_De_Outro_Usuario()
        {
            var idCategoria = 2;
            var idUsuario = 1;

            _categoriaRepositorio.ObterPorId(idCategoria)
                .Returns(new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito)));

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriaPorId(idCategoria, 2).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Categoria_Por_Id_Inexistente()
        {
            var idCategoria = 1;
            var idUsuario = 1;

            _categoriaRepositorio.ObterPorId(idCategoria)
                .Returns((Categoria)null);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriaPorId(idCategoria, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Id_Categoria_Nao_Existe, idCategoria)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Categoria_Por_Id_Com_Parametros_Invalidos()
        {
            var idCategoria = 0;
            var idUsuario = 0;

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriaPorId(idCategoria, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Id_Categoria_Invalido, idCategoria)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Categoria_Por_Id()
        {
            var idCategoria = 1;
            var idUsuario = 1;

            _categoriaRepositorio.ObterPorId(idCategoria)
                .Returns(new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito)));

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriaPorId(1, 1).Result;

            Assert.IsTrue(saida.Sucesso, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Obter_Categorias_Por_Usuario_Com_Id_Usuario_Invalido()
        {
            var idUsuario = 0;

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriasPorUsuario(idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, idUsuario)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Obter_Categorias_Por_Usuario()
        {
            var idUsuario = 1;

            _categoriaRepositorio.ObterPorUsuario(idUsuario)
                .Returns(new List<Categoria> { new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito)) });

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ObterCategoriasPorUsuario(idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categorias_Encontradas_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Procurar_Categorias_Com_Parametros_Invalidos()
        {
            var procurarEntrada = new ProcurarCategoriaEntrada(0);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ProcurarCategorias(procurarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Procurar_Categorias()
        {
            var idUsuario = 1;

            var procurarEntrada = new ProcurarCategoriaEntrada(idUsuario);

            var categoria1 = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito));
            var categoria2 = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 2", TipoCategoria.Debito));

            _categoriaRepositorio.Procurar(procurarEntrada)
                .Returns(new ProcurarSaida(new[] { categoria1, categoria2 }));

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ProcurarCategorias(procurarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && (int)saida.Retorno.GetType().GetProperty("TotalRegistros").GetValue(saida.Retorno, null) == 2, string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Categoria_Com_Parametros_Invalidos()
        {
            var cadastroEntrada = new CadastrarCategoriaEntrada(0, string.Empty, "Z");

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.CadastrarCategoria(cadastroEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Nome_Obrigatorio_Nao_Informado), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Categoria_Com_Mesmo_Nome_Tipo()
        {
            var idUsuario = 1;

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito)
                .Returns(true);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.CadastrarCategoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Categoria_Com_Pai_Inexistente()
        {
            var idUsuario = 1;
            var idCategoriaPai = 3;

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito)
                .Returns(false);

            _categoriaRepositorio.ObterPorId(idCategoriaPai)
                .Returns((Categoria)null);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.CadastrarCategoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoriaPai)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Categoria_Pai_Nao_Existe, idCategoriaPai)), string.Join(", ", saida.Mensagens));
        }


        [TestMethod]
        public void Nao_Deve_Cadastrar_Categoria_Com_Tipo_Diferente_Tipo_Pai()
        {
            var idUsuario = 1;
            var idCategoriaPai = 3;

            var categoriaPai = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 3", TipoCategoria.Debito));

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito)
                .Returns(false);

            _categoriaRepositorio.ObterPorId(idCategoriaPai)
                .Returns(categoriaPai);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.CadastrarCategoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Credito, idCategoriaPai)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai, categoriaPai.ObterDescricaoTipo())), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_Com_Parametros_Invalidos()
        {
            var idUsuario = 0;

            var alterarEntrada = new AlterarCategoriaEntrada(1, string.Empty, 1, "Z", idUsuario);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(Mensagem.Id_Usuario_Invalido, idUsuario)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_Com_Id_Inexistente()
        {
            var idUsuario = 1;
            var idCategoria = 1;

            _categoriaRepositorio.ObterPorId(idCategoria, true)
                .Returns((Categoria)null);

            var alterarEntrada = new AlterarCategoriaEntrada(idCategoria, "Categoria 1", null, TipoCategoria.Debito, idUsuario);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Id_Categoria_Nao_Existe, idCategoria)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_De_Outro_Usuario()
        {
            var idUsuario = 1;
            var idCategoria = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(2, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria, true)
                .Returns(categoria);

            var alterarEntrada = new AlterarCategoriaEntrada(idCategoria, "Categoria 1 alterada", null, TipoCategoria.Credito, idUsuario);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(alterarEntrada).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Alterar_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_Com_Mesmo_Nome_Tipo()
        {
            var idUsuario = 1;
            var idCategoria = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria, true)
               .Returns(categoria);

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoria)
                .Returns(true);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(new AlterarCategoriaEntrada(idCategoria, "Categoria 1", null, TipoCategoria.Debito, idUsuario)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_Com_Pai_Inexistente()
        {
            var idUsuario = 1;
            var idCategoria = 1;
            var idCategoriaPai = 3;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoriaPai));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria, true)
               .Returns(categoria);

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoria)
                .Returns(false);

            _categoriaRepositorio.ObterPorId(idCategoriaPai)
                .Returns((Categoria)null);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(new AlterarCategoriaEntrada(idCategoria, "Categoria 1 alterada", idCategoriaPai, TipoCategoria.Debito, idUsuario)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Categoria_Pai_Nao_Existe, idCategoriaPai)), string.Join(", ", saida.Mensagens));
        }


        [TestMethod]
        public void Nao_Deve_Alterar_Categoria_Com_Tipo_Diferente_Tipo_Pai()
        {
            var idUsuario = 1;
            var idCategoria = 1;
            var idCategoriaPai = 3;

            var categoriaPai = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 3", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoriaPai, idCategoriaPai);

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoriaPai));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria, true)
               .Returns(categoria);

            _categoriaRepositorio.VerificarExistenciaPorNomeTipo(idUsuario, "Categoria 1", TipoCategoria.Debito, idCategoria)
                .Returns(false);

            _categoriaRepositorio.ObterPorId(idCategoriaPai)
                .Returns(categoriaPai);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(new AlterarCategoriaEntrada(idCategoria, "Categoria 1 alterada", idCategoriaPai, TipoCategoria.Credito, idUsuario)).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai, categoriaPai.ObterDescricaoTipo())), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Deve_Alterar_Categoria()
        {
            var idUsuario = 1;
            var idCategoria = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria, true)
                .Returns(categoria);

            var alterarEntrada = new AlterarCategoriaEntrada(idCategoria, "Categoria 1 alterada", null, TipoCategoria.Credito, idUsuario);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.AlterarCategoria(alterarEntrada).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Alterada_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Categoria_Com_Parametros_Invalidos()
        {
            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ExcluirCategoria(0, 0).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Id_Categoria_Invalido, 0)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Categoria_Com_Id_Inexistente()
        {
            var idCategoria = 1;
            var idUsuario = 1;

            _categoriaRepositorio.ObterPorId(idCategoria)
               .Returns((Categoria)null);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ExcluirCategoria(idCategoria, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == string.Format(CategoriaMensagem.Id_Categoria_Nao_Existe, idCategoria)), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Categoria_De_Outro_Usuario()
        {
            var idCategoria = 1;
            var idUsuario = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(2, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria)
               .Returns(categoria);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ExcluirCategoria(idCategoria, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Excluir_Nao_Pertence_Usuario), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Categoria_Com_Filhas()
        {
            var idCategoria = 1;
            var idUsuario = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);
            typeof(Categoria).GetProperty("CategoriasFilha").SetValue(categoria, new[] { new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Filha 1", TipoCategoria.Debito, idCategoria)) });

            _categoriaRepositorio.ObterPorId(idCategoria)
               .Returns(categoria);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ExcluirCategoria(idCategoria, idUsuario).Result;

            Assert.IsTrue(!saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Pai_Nao_Pode_Ser_Excluida), string.Join(", ", saida.Mensagens));
        }

        [TestMethod]
        public void Nao_Deve_Excluir_Categoria_Por_Foreign_Key()
        {
            //TODO: Implementar esse teste.
            Assert.Fail();
        }

        [TestMethod]
        public void Deve_Excluir_Categoria()
        {
            var idUsuario = 1;
            var idCategoria = 1;

            var categoria = new Categoria(new CadastrarCategoriaEntrada(idUsuario, "Categoria 1", TipoCategoria.Debito));
            typeof(Categoria).GetProperty("Id").SetValue(categoria, idCategoria);

            _categoriaRepositorio.ObterPorId(idCategoria)
                .Returns(categoria);

            _categoriaServico = Substitute.For<CategoriaServico>(_categoriaRepositorio, _uow);

            var saida = _categoriaServico.ExcluirCategoria(idCategoria, idUsuario).Result;

            Assert.IsTrue(saida.Sucesso && saida.Mensagens.Any(x => x == CategoriaMensagem.Categoria_Excluida_Com_Sucesso), string.Join(", ", saida.Mensagens));
        }
    }
}
