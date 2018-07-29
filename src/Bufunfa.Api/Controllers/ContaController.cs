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
    public class ContaController : BaseController
    {
        private readonly IContaServico _contaServico;

        public ContaController(IContaServico contaServico)
        {
            this._contaServico = contaServico;
        }

        /// <summary>
        /// Obtém um conta a partir do seu ID
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpGet]
        [Route("v1/contas/obter-por-id/{idConta:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContaPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId([SwaggerParameter("ID da conta.", Required = true)] int idConta)
        {
            return await _contaServico.ObterContaPorId(
                idConta,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém as contas do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpGet]
        [Route("v1/contas/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Contas do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContasPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterContasPorUsuarioAutenticado()
        {
            return await _contaServico.ObterContasPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de uma nova conta.
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpPost]
        [Route("v1/contas/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarContaViewModel), typeof(CadastrarContaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarContaResponseExemplo))]
        public async Task<ISaida> CadastrarConta([FromBody, SwaggerParameter("Informações de cadastro da conta.", Required = true)] CadastrarContaViewModel model)
        {
            var cadastrarEntrada = new CadastrarContaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo.Value,
                model.ValorSaldoInicial,
                model.NomeInstituicao,
                model.NumeroAgencia,
                model.Numero);

            return await _contaServico.CadastrarConta(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma conta.
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpPut]
        [Route("v1/contas/alterar")]
        [SwaggerRequestExample(typeof(AlterarContaViewModel), typeof(AlterarContaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarContaResponseExemplo))]
        public async Task<ISaida> AlterarConta([FromBody, SwaggerParameter("Informações para alteração da conta.", Required = true)] AlterarContaViewModel model)
        {
            var alterarEntrada = new AlterarContaEntrada(
                model.IdConta,
                model.Nome,
                model.Tipo.Value,
                base.ObterIdUsuarioClaim(),
                model.ValorSaldoInicial,
                model.NomeInstituicao,
                model.NumeroAgencia,
                model.Numero);

            return await _contaServico.AlterarConta(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma conta.
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpDelete]
        [Route("v1/contas/excluir/{idConta:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirContaResponseExemplo))]
        public async Task<ISaida> ExcluirConta([SwaggerParameter("ID da conta que deverá ser excluída.", Required = true)] int idConta)
        {
            return await _contaServico.ExcluirConta(
                idConta,
                base.ObterIdUsuarioClaim());
        }
    }
}
