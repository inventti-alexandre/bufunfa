using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

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
    public class PessoaController : BaseController
    {
        private readonly IPessoaRepositorio _pessoaRepositorio;

        public PessoaController(IPessoaRepositorio pessoaRepositorio)
        {
            _pessoaRepositorio = pessoaRepositorio;
        }

        /// <summary>
        /// Realiza uma busca por pessoas a partir dos parâmetros informados
        /// </summary>
        /// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpPost]
        [Route("v1/pessoas/procurar")]
        public ISaida Procurar([FromBody] ProcurarPessoaViewModel viewModel)
        {
            var procurarEntrada = new ProcurarPessoaEntrada(base.ObterIdUsuarioClaim(), viewModel.OrdenarPor, viewModel.OrdenarSentido, viewModel.PaginaIndex, viewModel.PaginaTamanho)
            {
                Nome = viewModel.Nome
            };

            var lst = _pessoaRepositorio.Procurar(procurarEntrada);

            return new Saida(true, new[] { "" }, lst);
        }

        ///// <summary>
        ///// Obtém um período a partir do seu ID
        ///// </summary>
        ///// <param name="idPeriodo">ID do período</param>
        //[Authorize(PermissaoAcesso.Contas)]
        //[HttpGet]
        //[Route("v1/periodos/obter-por-id/{idPeriodo:int}")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Períodos do usuário encontrados.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoPorIdResponseExemplo))]
        //public ISaida ObterContaPorId(int idPeriodo)
        //{
        //    return _periodoServico.ObterPeriodoPorId(idPeriodo, base.ObterIdUsuarioClaim());
        //}

        ///// <summary>
        ///// Obtém um período a partir do seu ID
        ///// </summary>
        //[Authorize(PermissaoAcesso.Periodos)]
        //[HttpGet]
        //[Route("v1/periodos/obter")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Períodos do usuário encontrados.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodosPorUsuarioResponseExemplo))]
        //public ISaida ObterPeriodosPorUsuarioAutenticado()
        //{
        //    return _periodoServico.ObterPeriodosPorUsuario(base.ObterIdUsuarioClaim());
        //}

        ///// <summary>
        ///// Realiza o cadastro de um novo período.
        ///// </summary>
        ///// <param name="viewModel">Informações de cadastro do período.</param>
        //[Authorize(PermissaoAcesso.Periodos)]
        //[HttpPost]
        //[Route("v1/periodos/cadastrar-periodo")]
        //[SwaggerRequestExample(typeof(CadastrarPeriodoViewModel), typeof(CadastrarPeriodoViewModelExemplo))]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período cadastrado com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPeriodoResponseExemplo))]
        //public ISaida CadastrarPeriodo([FromBody] CadastrarPeriodoViewModel viewModel)
        //{
        //    var cadastrarEntrada = new CadastrarPeriodoEntrada(base.ObterIdUsuarioClaim(), viewModel.Nome, viewModel.DataInicio.Value, viewModel.DataFim.Value);

        //    return _periodoServico.CadastrarPeriodo(cadastrarEntrada);
        //}

        ///// <summary>
        ///// Realiza a alteração de um período.
        ///// </summary>
        ///// <param name="viewModel">Informações para alteração de um período.</param>
        //[Authorize(PermissaoAcesso.Periodos)]
        //[HttpPut]
        //[Route("v1/periodos/alterar-periodo")]
        //[SwaggerRequestExample(typeof(AlterarPeriodoViewModel), typeof(AlterarPeriodoViewModelExemplo))]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período alterado com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPeriodoResponseExemplo))]
        //public ISaida AlterarPeriodo([FromBody] AlterarPeriodoViewModel viewModel)
        //{
        //    var alterarEntrada = new AlterarPeriodoEntrada(viewModel.IdPeriodo, viewModel.Nome, viewModel.DataInicio.Value, viewModel.DataFim.Value, base.ObterIdUsuarioClaim());

        //    return _periodoServico.AlterarPeriodo(alterarEntrada);
        //}

        ///// <summary>
        ///// Realiza a exclusão de um período.
        ///// </summary>
        //[Authorize(PermissaoAcesso.Periodos)]
        //[HttpDelete]
        //[Route("v1/periodos/excluir-periodo/{idPeriodo:int}")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período excluído com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPeridoResponseExemplo))]
        //public ISaida ExcluirPeriodo(int idPeriodo)
        //{
        //    return _periodoServico.ExcluirPeriodo(idPeriodo, base.ObterIdUsuarioClaim());
        //}
    }
}
