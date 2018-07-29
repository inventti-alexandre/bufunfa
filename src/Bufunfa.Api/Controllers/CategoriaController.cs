using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro não tratado encontrado. (Internal Server Error)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Acesso negado. Token de autenticação não encontrado. (Unauthorized)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Forbidden, "Acesso negado. Sem permissão de acesso a funcionalidade. (Forbidden)", typeof(Response))]
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
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpGet]
        [Route("v1/categorias/obter-por-id/{idCategoria:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriaPorIdResponseExemplo))]
        public async Task<ISaida> ObterCategoriaPorId([SwaggerParameter("ID da categoria.", Required = true)] int idCategoria)
        {
            return await _categoriaServico.ObterCategoriaPorId(
                idCategoria,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém as categorias do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpGet]
        [Route("v1/categorias/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categorias do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriasPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterCategoriasPorUsuarioAutenticado()
        {
            return await _categoriaServico.ObterCategoriasPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por categorias a partir dos parâmetros informados
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPost]
        [Route("v1/categorias/procurar")]
        [SwaggerRequestExample(typeof(ProcurarCategoriaViewModel), typeof(ProcurarCategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por categorias.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarCategoriaResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarCategoriaViewModel model)
        {
            var procurarEntrada = new ProcurarCategoriaEntrada(base.ObterIdUsuarioClaim())
            {
                IdCategoriaPai = model.IdCategoriaPai,
                Nome = model.Nome,
                Tipo = model.Tipo,
                Caminho = model.Caminho
            };

            return await _categoriaServico.ProcurarCategorias(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova categoria.
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPost]
        [Route("v1/categorias/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarCategoriaViewModel), typeof(CadastrarCategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCategoriaResponseExemplo))]
        public async Task<ISaida> CadastrarCategoria([FromBody, SwaggerParameter("Informações de cadastro da categoria.", Required = true)] CadastrarCategoriaViewModel model)
        {
            var cadastrarEntrada = new CadastrarCategoriaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo,
                model.IdCategoriaPai);

            return await _categoriaServico.CadastrarCategoria(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma categoria.
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpPut]
        [Route("v1/categorias/alterar")]
        [SwaggerRequestExample(typeof(AlterarCategoriaViewModel), typeof(AlterarCategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCategoriaResponseExemplo))]
        public async Task<ISaida> AlterarCategoria([FromBody, SwaggerParameter("Informações para alteração de uma categoria.", Required = true)] AlterarCategoriaViewModel model)
        {
            var alterarEntrada = new AlterarCategoriaEntrada(
                model.IdCategoria,
                model.Nome,
                model.IdCategoriaPai,
                model.Tipo,
                base.ObterIdUsuarioClaim());

            return await _categoriaServico.AlterarCategoria(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma categoria.
        /// </summary>
        [Authorize(PermissaoAcesso.Categorias)]
        [HttpDelete]
        [Route("v1/categorias/excluir/{idCategoria:int}")]
        [SwaggerOperation(Description = "<b>Atenção - </b> Nas seguintes situações a exclusão da categoria não será permitida:<br><ul><li>Caso a categoria possua categorias-filha.</li><li>Caso a categoria possua lançamentos associados.</li></ul>")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCategoriaResponseExemplo))]
        public async Task<ISaida> ExcluirCategoria([SwaggerParameter("ID da categoria que deverá ser excluída.", Required = true)] int idCategoria)
        {
            return await _categoriaServico.ExcluirCategoria(idCategoria, base.ObterIdUsuarioClaim());
        }
    }
}
