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
        /// <param name="idConta">ID da conta</param>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpGet]
        [Route("v1/contas/obter-por-id/{idConta:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Contas do usuário encontradas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContaPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId(int idConta)
        {
            return await _contaServico.ObterContaPorId(idConta, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém as contas do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpGet]
        [Route("v1/contas/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Contas do usuário encontradas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContasPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterContasPorUsuarioAutenticado()
        {
            return await _contaServico.ObterContasPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de uma nova conta.
        /// </summary>
        /// <param name="cadastroModel">Informações de cadastro da conta.</param>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpPost]
        [Route("v1/contas/cadastrar-conta")]
        [SwaggerRequestExample(typeof(CadastrarContaViewModel), typeof(CadastrarContaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Conta cadastrada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarContaResponseExemplo))]
        public async Task<ISaida> CadastrarConta([FromBody] CadastrarContaViewModel cadastroModel)
        {
            var cadastrarEntrada = new CadastrarContaEntrada(base.ObterIdUsuarioClaim(), cadastroModel.Nome, cadastroModel.ValorSaldoInicial, cadastroModel.NomeInstituicao, cadastroModel.NumeroAgencia, cadastroModel.Numero);

            return await _contaServico.CadastrarConta(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma conta.
        /// </summary>
        /// <param name="alterarModel">Informações para alteração da conta.</param>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpPut]
        [Route("v1/contas/alterar-conta")]
        [SwaggerRequestExample(typeof(AlterarContaViewModel), typeof(AlterarContaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Conta alterada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarContaResponseExemplo))]
        public async Task<ISaida> AlterarConta([FromBody] AlterarContaViewModel alterarModel)
        {
            var alterarEntrada = new AlterarContaEntrada(alterarModel.IdConta, alterarModel.Nome, base.ObterIdUsuarioClaim(), alterarModel.ValorSaldoInicial, alterarModel.NomeInstituicao, alterarModel.NumeroAgencia, alterarModel.Numero);

            return await _contaServico.AlterarConta(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma conta.
        /// </summary>
        [Authorize(PermissaoAcesso.Contas)]
        [HttpDelete]
        [Route("v1/contas/excluir-conta/{idConta:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Conta excluída com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirContaResponseExemplo))]
        public async Task<ISaida> ExcluirConta(int idConta)
        {
            return await _contaServico.ExcluirConta(idConta, base.ObterIdUsuarioClaim());
        }
    }
}
