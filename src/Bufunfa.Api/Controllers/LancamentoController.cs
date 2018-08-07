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
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro não tratado encontrado. (Internal Server Error)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Acesso negado. Token de autenticação não encontrado. (Unauthorized)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Forbidden, "Acesso negado. Sem permissão de acesso a funcionalidade. (Forbidden)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Forbidden, typeof(ForbiddenApiResponse))]
    public class LancamentoController : BaseController
    {
        private readonly ILancamentoServico _lancamentoServico;

        public LancamentoController(ILancamentoServico lancamentoServico)
        {
            _lancamentoServico = lancamentoServico;
        }

        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("v1/lancamentos/obter-por-id/{idLancamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterLancamentoPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId([SwaggerParameter("ID do lançamento.", Required = true)] int idLancamento)
        {
            return await _lancamentoServico.ObterLancamentoPorId(
                idLancamento,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por lançamentos a partir dos parâmetros informados
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpPost]
        [Route("v1/lancamentos/procurar")]
        [SwaggerRequestExample(typeof(ProcurarLancamentoViewModel), typeof(ProcurarLancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por lançamentos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarLancamentoResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarLancamentoViewModel model)
        {
            var procurarEntrada = new ProcurarLancamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                DataFim     = model.DataFim,
                DataInicio  = model.DataInicio,
                IdCategoria = model.IdCategoria,
                IdConta     = model.IdConta,
                IdPessoa    = model.IdPessoa
            };

            return await _lancamentoServico.ProcurarLancamentos(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo lançamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpPost]
        [Route("v1/lancamentos/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarLancamentoViewModel), typeof(CadastrarLancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarLancamentoResponseExemplo))]
        public async Task<ISaida> CadastrarLancamento([FromBody, SwaggerParameter("Informações de cadastro do lançamento.", Required = true)] CadastrarLancamentoViewModel model)
        {
            var cadastrarEntrada = new CadastrarLancamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdConta.Value,
                model.IdCategoria.Value,
                model.Data.Value,
                model.Valor.Value,
                model.IdPessoa,
                null,
                model.Observacao);

            return await _lancamentoServico.CadastrarLancamento(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um lançamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpPut]
        [Route("v1/lancamentos/alterar")]
        [SwaggerRequestExample(typeof(AlterarLancamentoViewModel), typeof(AlterarLancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarLancamentoResponseExemplo))]
        public async Task<ISaida> AlterarLancamento([FromBody, SwaggerParameter("Informações para alteração de um lançamento.", Required = true)] AlterarLancamentoViewModel model)
        {
            var alterarEntrada = new AlterarLancamentoEntrada(
                model.IdLancamento.Value,
                model.IdConta.Value,
                model.IdCategoria.Value,
                model.Data.Value,
                model.Valor.Value,
                base.ObterIdUsuarioClaim(),
                model.IdPessoa,
                model.Observacao);

            return await _lancamentoServico.AlterarLancamento(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um lançamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpDelete]
        [Route("v1/lancamentos/excluir/{idLancamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirLancamentoResponseExemplo))]
        public async Task<ISaida> ExcluirLancamento([SwaggerParameter("ID do lançamento que deverá ser excluído.", Required = true)] int idLancamento)
        {
            return await _lancamentoServico.ExcluirLancamento(idLancamento, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de um novo anexo para um lançamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [HttpPost]
        [Route("v1/lancamentos/cadastrar-anexo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Anexo cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAnexoResponseExemplo))]
        public async Task<ISaida> CadastrarAnexo([FromForm, SwaggerParameter("Informações de cadastro do anexo.", Required = true)] CadastrarAnexoViewModel model)
        {
            CadastrarAnexoEntrada cadastrarEntrada;

            using (var memoryStream = new MemoryStream())
            {
                await model.Arquivo.CopyToAsync(memoryStream);

                cadastrarEntrada = new CadastrarAnexoEntrada(
                    base.ObterIdUsuarioClaim(),
                    model.IdLancamento.Value,
                    model.Descricao,
                    model.NomeArquivo + model.Arquivo.FileName.Substring(model.Arquivo.FileName.LastIndexOf(".")),
                    memoryStream.ToArray(),
                    model.Arquivo.ContentType);
            }

            return await _lancamentoServico.CadastrarAnexo(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um anexo.
        /// </summary>
        [Authorize(PermissaoAcesso.Lancamentos)]
        [Consumes("application/json")]
        [HttpDelete]
        [Route("v1/lancamentos/excluir-anexo/{idAnexo:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Anexo excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAnexoResponseExemplo))]
        public async Task<ISaida> ExcluirAnexo([SwaggerParameter("ID do anexo que deverá ser excluído.", Required = true)] int idAnexo)
        {
            return await _lancamentoServico.ExcluirAnexo(idAnexo, base.ObterIdUsuarioClaim());
        }
    }
}
