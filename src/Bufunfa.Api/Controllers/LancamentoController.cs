using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
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
    public class LancamentoController : BaseController
    {
        private readonly ILancamentoServico _pessoaServico;

        public LancamentoController(ILancamentoServico pessoaServico)
        {
            _pessoaServico = pessoaServico;
        }

        ///// <summary>
        ///// Obtém uma pessoa a partir do seu ID
        ///// </summary>
        ///// <param name="idLancamento">ID da pessoa</param>
        //[Authorize(PermissaoAcesso.Lancamentos)]
        //[HttpGet]
        //[Route("v1/pessoas/obter-por-id/{idLancamento:int}")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Lancamento do usuário encontrada.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterLancamentoPorIdResponseExemplo))]
        //public async Task<ISaida> ObterContaPorId(int idLancamento)
        //{
        //    return await _pessoaServico.ObterLancamentoPorId(idLancamento, base.ObterIdUsuarioClaim());
        //}

        ///// <summary>
        ///// Realiza uma procura por pessoas a partir dos parâmetros informados
        ///// </summary>
        ///// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        //[Authorize(PermissaoAcesso.Lancamentos)]
        //[HttpPost]
        //[Route("v1/pessoas/procurar")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Resultado da procura por pessoas.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarLancamentoResponseExemplo))]
        //public async Task<ISaida> Procurar([FromBody] ProcurarLancamentoViewModel viewModel)
        //{
        //    var procurarEntrada = new ProcurarLancamentoEntrada(base.ObterIdUsuarioClaim(), viewModel.OrdenarPor, viewModel.OrdenarSentido, viewModel.PaginaIndex, viewModel.PaginaTamanho)
        //    {
        //        Nome = viewModel.Nome
        //    };

        //    return await _pessoaServico.ProcurarLancamentos(procurarEntrada);
        //}

        ///// <summary>
        ///// Realiza o cadastro de uma nova pessoa.
        ///// </summary>
        ///// <param name="viewModel">Informações de cadastro da pessoa.</param>
        //[Authorize(PermissaoAcesso.Lancamentos)]
        //[HttpPost]
        //[Route("v1/pessoas/cadastrar")]
        //[SwaggerRequestExample(typeof(CadastrarLancamentoViewModel), typeof(CadastrarLancamentoViewModelExemplo))]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Lancamento cadastrada com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarLancamentoResponseExemplo))]
        //public async Task<ISaida> CadastrarLancamento([FromBody] CadastrarLancamentoViewModel viewModel)
        //{
        //    var cadastrarEntrada = new CadastrarLancamentoEntrada(base.ObterIdUsuarioClaim(), viewModel.Nome);

        //    return await _pessoaServico.CadastrarLancamento(cadastrarEntrada);
        //}

        ///// <summary>
        ///// Realiza a alteração de uma pessoa.
        ///// </summary>
        ///// <param name="viewModel">Informações para alteração de uma pessoa.</param>
        //[Authorize(PermissaoAcesso.Lancamentos)]
        //[HttpPut]
        //[Route("v1/pessoas/alterar")]
        //[SwaggerRequestExample(typeof(AlterarLancamentoViewModel), typeof(AlterarLancamentoViewModelExemplo))]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Lancamento alterada com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarLancamentoResponseExemplo))]
        //public async Task<ISaida> AlterarLancamento([FromBody] AlterarLancamentoViewModel viewModel)
        //{
        //    var alterarEntrada = new AlterarLancamentoEntrada(viewModel.IdLancamento, viewModel.Nome, base.ObterIdUsuarioClaim());

        //    return await _pessoaServico.AlterarLancamento(alterarEntrada);
        //}

        ///// <summary>
        ///// Realiza a exclusão de uma pessoa.
        ///// </summary>
        //[Authorize(PermissaoAcesso.Lancamentos)]
        //[HttpDelete]
        //[Route("v1/pessoas/excluir/{idLancamento:int}")]
        //[SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Lancamento excluída com sucesso.")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirLancamentoResponseExemplo))]
        //public async Task<ISaida> ExcluirLancamento(int idLancamento)
        //{
        //    return await _pessoaServico.ExcluirLancamento(idLancamento, base.ObterIdUsuarioClaim());
        //}
    }
}
