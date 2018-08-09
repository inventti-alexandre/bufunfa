using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class CategoriaServico : Notificavel, ICategoriaServico
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IUow _uow;

        public CategoriaServico(
            ICategoriaRepositorio categoriaRepositorio,
            IUow uow)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _uow                  = uow;
        }

        public async Task<ISaida> ObterCategoriaPorId(int idCategoria, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(idCategoria);

            // Verifica se a categoria existe
            this.NotificarSeNulo(categoria, CategoriaMensagem.Id_Categoria_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria pertece ao usuário informado.
            this.NotificarSeDiferentes(categoria.IdUsuario, idUsuario, CategoriaMensagem.Categoria_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { CategoriaMensagem.Categoria_Encontrada_Com_Sucesso }, new CategoriaSaida(categoria));
        }

        public async Task<ISaida> ObterCategoriasPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstCategorias = await _categoriaRepositorio.ObterPorUsuario(idUsuario);

            return lstCategorias.Any()
                ? new Saida(true, new[] { CategoriaMensagem.Categorias_Encontradas_Com_Sucesso }, lstCategorias.OrderBy(x => x.ObterCaminho()).Select(x => new CategoriaSaida(x)))
                : new Saida(true, new[] { CategoriaMensagem.Nenhuma_categoria_encontrada }, null);
        }

        public async Task<ISaida> ProcurarCategorias(ProcurarCategoriaEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return procurarEntrada.Invalido
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _categoriaRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarCategoria(CadastrarCategoriaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se já existe uma categoria com o mesmo nome e mesmo tipo
            this.NotificarSeVerdadeiro(
                await _categoriaRepositorio.VerificarExistenciaPorNomeTipo(cadastroEntrada.IdUsuario, cadastroEntrada.Nome, cadastroEntrada.Tipo),
                CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo);

            if (cadastroEntrada.IdCategoriaPai.HasValue)
            {
                var categoriaPai = await _categoriaRepositorio.ObterPorId(cadastroEntrada.IdCategoriaPai.Value);

                // Verifica se a categoria pai existe
                this.NotificarSeNulo(categoriaPai, string.Format(CategoriaMensagem.Categoria_Pai_Nao_Existe, cadastroEntrada.IdCategoriaPai.Value));

                if (categoriaPai != null)
                {
                    // Verificar se o tipo da categoria é igual ao tipo da categoria pai
                    this.NotificarSeDiferentes(cadastroEntrada.Tipo, categoriaPai.Tipo, CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai);
                }
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = new Categoria(cadastroEntrada);

            await _categoriaRepositorio.Inserir(categoria);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CategoriaMensagem.Categoria_Cadastrada_Com_Sucesso }, new CategoriaSaida(await _categoriaRepositorio.ObterPorId(categoria.Id)));
        }

        public async Task<ISaida> AlterarCategoria(AlterarCategoriaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
                return new Saida(false, alterarEntrada.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(alterarEntrada.IdCategoria, true);

            // Verifica se a categoria existe
            this.NotificarSeNulo(categoria, CategoriaMensagem.Id_Categoria_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria pertece ao usuário informado.
            this.NotificarSeDiferentes(categoria.IdUsuario, alterarEntrada.IdUsuario, CategoriaMensagem.Categoria_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma categoria com o mesmo nome e mesmo tipo
            this.NotificarSeVerdadeiro(
                await _categoriaRepositorio.VerificarExistenciaPorNomeTipo(alterarEntrada.IdUsuario, alterarEntrada.Nome, alterarEntrada.Tipo, alterarEntrada.IdCategoria),
                CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo);

            if (alterarEntrada.IdCategoriaPai.HasValue)
            {
                var categoriaPai = await _categoriaRepositorio.ObterPorId(alterarEntrada.IdCategoriaPai.Value);

                // Verifica se a categoria pai existe
                this.NotificarSeNulo(categoriaPai, CategoriaMensagem.Categoria_Pai_Nao_Existe);

                if (categoriaPai != null)
                {
                    // Verificar se o tipo da categoria é igual ao tipo da categoria pai
                    this.NotificarSeDiferentes(alterarEntrada.Tipo, categoriaPai.Tipo, CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai);
                }
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            categoria.Alterar(alterarEntrada);

            _categoriaRepositorio.Atualizar(categoria);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CategoriaMensagem.Categoria_Alterada_Com_Sucesso }, new CategoriaSaida(await _categoriaRepositorio.ObterPorId(categoria.Id)));
        }

        public async Task<ISaida> ExcluirCategoria(int idCategoria, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(idCategoria);

            // Verifica se a categoria existe
            this.NotificarSeNulo(categoria, CategoriaMensagem.Id_Categoria_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria pertece ao usuário informado.
            this.NotificarSeDiferentes(categoria.IdUsuario, idUsuario, CategoriaMensagem.Categoria_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verificar se a categoria tem filhas. Se for pai, obrigar a exclusão de todas as filhas primeiro
            this.NotificarSeVerdadeiro(categoria.Pai, CategoriaMensagem.Categoria_Pai_Nao_Pode_Ser_Excluida);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _categoriaRepositorio.Deletar(categoria);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CategoriaMensagem.Categoria_Excluida_Com_Sucesso }, new CategoriaSaida(categoria));
        }
    }
}
