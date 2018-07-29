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
    public class CartaoCreditoController : BaseController
    {
        private readonly ICartaoCreditoServico _cartaoCreditoServico;

        public CartaoCreditoController(ICartaoCreditoServico cartaoCreditoServico)
        {
            this._cartaoCreditoServico = cartaoCreditoServico;
        }

        /// <summary>
        /// Obtém um cartão de crédito a partir do seu ID
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpGet]
        [Route("v1/cartoes/obter-por-id/{idCartaoCredito:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditoPorIdResponseExemplo))]
        public async Task<ISaida> ObterCartaoCreditoPorId([SwaggerParameter("ID do cartão de crédito.", Required = true)] int idCartaoCredito)
        {
            return await _cartaoCreditoServico.ObterCartaoCreditoPorId(
                idCartaoCredito,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém os cartões do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpGet]
        [Route("v1/cartoes/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartões do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditosPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterCartaoCreditosPorUsuarioAutenticado()
        {
            return await _cartaoCreditoServico.ObterCartoesCreditoPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de um novo cartão.
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpPost]
        [Route("v1/cartoes/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarCartaoCreditoViewModel), typeof(CadastrarCartaoCreditoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCartaoCreditoResponseExemplo))]
        public async Task<ISaida> CadastrarCartaoCredito([FromBody, SwaggerParameter("Informações de cadastro do cartão.", Required = true)] CadastrarCartaoCreditoViewModel model)
        {
            var cadastrarEntrada = new CadastrarCartaoCreditoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.ValorLimite.Value,
                model.DiaVencimentoFatura.Value);

            return await _cartaoCreditoServico.CadastrarCartaoCredito(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um cartão.
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpPut]
        [Route("v1/cartoes/alterar")]
        [SwaggerRequestExample(typeof(AlterarCartaoCreditoViewModel), typeof(AlterarCartaoCreditoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCartaoCreditoResponseExemplo))]
        public async Task<ISaida> AlterarCartaoCredito([FromBody, SwaggerParameter("Informações para alteração do cartão.", Required = true)] AlterarCartaoCreditoViewModel model)
        {
            var alterarEntrada = new AlterarCartaoCreditoEntrada(
                model.IdCartao,
                model.Nome,
                base.ObterIdUsuarioClaim(),
                model.ValorLimite.Value,
                model.DiaVencimentoFatura.Value);

            return await _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um cartão.
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpDelete]
        [Route("v1/cartoes/excluir/{idCartaoCredito:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCartaoCreditoResponseExemplo))]
        public async Task<ISaida> ExcluirCartaoCredito([SwaggerParameter("ID do cartão que deverá ser excluído.", Required = true)] int idCartaoCredito)
        {
            return await _cartaoCreditoServico.ExcluirCartaoCredito(
                idCartaoCredito,
                base.ObterIdUsuarioClaim());
        }
    }
}
