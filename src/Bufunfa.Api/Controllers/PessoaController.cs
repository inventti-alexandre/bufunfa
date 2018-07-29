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
    public class PessoaController : BaseController
    {
        private readonly IPessoaServico _pessoaServico;

        public PessoaController(IPessoaServico pessoaServico)
        {
            _pessoaServico = pessoaServico;
        }

        /// <summary>
        /// Obtém uma pessoa a partir do seu ID
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpGet]
        [Route("v1/pessoas/obter-por-id/{idPessoa:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPessoaPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId([SwaggerParameter("ID da pessoa.", Required = true)] int idPessoa)
        {
            return await _pessoaServico.ObterPessoaPorId(idPessoa, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por pessoas a partir dos parâmetros informados
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPost]
        [Route("v1/pessoas/procurar")]
        [SwaggerRequestExample(typeof(ProcurarPessoaViewModel), typeof(ProcurarPessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por pessoas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPessoaResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarPessoaViewModel model)
        {
            var procurarEntrada = new ProcurarPessoaEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                Nome = model.Nome
            };

            return await _pessoaServico.ProcurarPessoas(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova pessoa.
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPost]
        [Route("v1/pessoas/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarPessoaViewModel), typeof(CadastrarPessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPessoaResponseExemplo))]
        public async Task<ISaida> CadastrarPessoa([FromBody, SwaggerParameter("Informações de cadastro da pessoa.", Required = true)] CadastrarPessoaViewModel viewModel)
        {
            var cadastrarEntrada = new CadastrarPessoaEntrada(
                base.ObterIdUsuarioClaim(),
                viewModel.Nome);

            return await _pessoaServico.CadastrarPessoa(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma pessoa.
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPut]
        [Route("v1/pessoas/alterar")]
        [SwaggerRequestExample(typeof(AlterarPessoaViewModel), typeof(AlterarPessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPessoaResponseExemplo))]
        public async Task<ISaida> AlterarPessoa([FromBody, SwaggerParameter("Informações para alteração de uma pessoa.", Required = true)] AlterarPessoaViewModel model)
        {
            var alterarEntrada = new AlterarPessoaEntrada(
                model.IdPessoa,
                model.Nome,
                base.ObterIdUsuarioClaim());

            return await _pessoaServico.AlterarPessoa(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma pessoa.
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpDelete]
        [Route("v1/pessoas/excluir/{idPessoa:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPessoaResponseExemplo))]
        public async Task<ISaida> ExcluirPessoa([SwaggerParameter("ID da pessoa que deverá ser excluída.", Required = true)] int idPessoa)
        {
            return await _pessoaServico.ExcluirPessoa(
                idPessoa,
                base.ObterIdUsuarioClaim());
        }
    }
}
