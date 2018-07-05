using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, typeof(Response), "Erro não tratado encontrado. (Internal Server Error)")]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(Response), "Endereço não encontrado. (Not found)")]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, typeof(Response), "Acesso negado. Token de autenticação não encontrado. (Unauthorized)")]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Forbidden, typeof(Response), "Acesso negado. Sem permissão de acesso a funcionalidade. (Forbidden)")]
    [SwaggerResponseExample((int)HttpStatusCode.Forbidden, typeof(ForbiddenApiResponse))]
    public class CategoriaController : BaseController
    {
        private readonly ICategoriaServico _categoriaServico;

        public CategoriaController(ICategoriaServico categoriaServico)
        {
            _categoriaServico = categoriaServico;
        }

        /// <summary>
        /// Obtém uma categoria a partir do seu ID
        /// </summary>
        /// <param name="idCategoria">ID da categoria</param>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpGet]
        [Route("v1/categorias/obter-por-id/{idCategoria:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Categoria do usuário encontrada.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriaPorIdResponseExemplo))]
        public async Task<ISaida> ObterCategoriaPorId(int idCategoria)
        {
            return await _categoriaServico.ObterCategoriaPorId(idCategoria, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém as categorias do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpGet]
        [Route("v1/categorias/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Categorias do usuário encontradas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriasPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterCategoriasPorUsuarioAutenticado()
        {
            return await _categoriaServico.ObterCategoriasPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por categorias a partir dos parâmetros informados
        /// </summary>
        /// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPost]
        [Route("v1/categorias/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Resultado da procura por categorias.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarCategoriaResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody] ProcurarCategoriaViewModel viewModel)
        {
            var procurarEntrada = new ProcurarCategoriaEntrada(base.ObterIdUsuarioClaim())
            {
                IdCategoriaPai = viewModel.IdCategoriaPai,
                Nome = viewModel.Nome,
                Tipo = viewModel.Tipo,
                Caminho = viewModel.Caminho
            };

            return await _categoriaServico.ProcurarCategorias(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova categoria.
        /// </summary>
        /// <param name="viewModel">Informações de cadastro da categoria.</param>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPost]
        [Route("v1/categorias/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarCategoriaViewModel), typeof(CadastrarCategoriaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Categoria cadastrada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCategoriaResponseExemplo))]
        public async Task<ISaida> CadastrarCategoria([FromBody] CadastrarCategoriaViewModel viewModel)
        {
            var cadastrarEntrada = new CadastrarCategoriaEntrada(base.ObterIdUsuarioClaim(), viewModel.Nome, viewModel.Tipo, viewModel.IdCategoriaPai);

            return await _categoriaServico.CadastrarCategoria(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma categoria.
        /// </summary>
        /// <param name="viewModel">Informações para alteração de uma categoria.</param>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPut]
        [Route("v1/categorias/alterar")]
        [SwaggerRequestExample(typeof(AlterarCategoriaViewModel), typeof(AlterarCategoriaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Categoria alterada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCategoriaResponseExemplo))]
        public async Task<ISaida> AlterarCategoria([FromBody] AlterarCategoriaViewModel viewModel)
        {
            var alterarEntrada = new AlterarCategoriaEntrada(viewModel.IdCategoria, viewModel.Nome, viewModel.IdCategoriaPai, viewModel.Tipo, base.ObterIdUsuarioClaim());

            return await _categoriaServico.AlterarCategoria(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma categoria.
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpDelete]
        [Route("v1/categorias/excluir/{idCategoria:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Categoria excluída com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCategoriaResponseExemplo))]
        public async Task<ISaida> ExcluirCategoria(int idCategoria)
        {
            return await _categoriaServico.ExcluirCategoria(idCategoria, base.ObterIdUsuarioClaim());
        }
    }
}
