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
        /// <param name="idCartaoCredito">ID do cartão de crédito.</param>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpGet]
        [Route("v1/cartoes/obter-por-id/{idCartaoCredito:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Cartões do usuário encontrados.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditoPorIdResponseExemplo))]
        public async Task<ISaida> ObterCartaoCreditoPorId(int idCartaoCredito)
        {
            return await _cartaoCreditoServico.ObterCartaoCreditoPorId(idCartaoCredito, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém os cartões do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpGet]
        [Route("v1/cartoes/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Cartões do usuário encontradas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditosPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterCartaoCreditosPorUsuarioAutenticado()
        {
            return await _cartaoCreditoServico.ObterCartoesCreditoPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de um novo cartão.
        /// </summary>
        /// <param name="cadastroModel">Informações de cadastro do cartão.</param>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpPost]
        [Route("v1/cartoes/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarCartaoCreditoViewModel), typeof(CadastrarCartaoCreditoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Cartão cadastrado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCartaoCreditoResponseExemplo))]
        public async Task<ISaida> CadastrarCartaoCredito([FromBody] CadastrarCartaoCreditoViewModel cadastroModel)
        {
            var cadastrarEntrada = new CadastrarCartaoCreditoEntrada(base.ObterIdUsuarioClaim(), cadastroModel.Nome, cadastroModel.ValorLimite.Value, cadastroModel.DiaVencimentoFatura.Value);

            return await _cartaoCreditoServico.CadastrarCartaoCredito(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um cartão.
        /// </summary>
        /// <param name="alterarModel">Informações para alteração do cartão.</param>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpPut]
        [Route("v1/cartoes/alterar")]
        [SwaggerRequestExample(typeof(AlterarCartaoCreditoViewModel), typeof(AlterarCartaoCreditoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Cartão alterado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCartaoCreditoResponseExemplo))]
        public async Task<ISaida> AlterarCartaoCredito([FromBody] AlterarCartaoCreditoViewModel alterarModel)
        {
            var alterarEntrada = new AlterarCartaoCreditoEntrada(alterarModel.IdCartao, alterarModel.Nome, base.ObterIdUsuarioClaim(), alterarModel.ValorLimite.Value, alterarModel.DiaVencimentoFatura.Value);

            return await _cartaoCreditoServico.AlterarCartaoCredito(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um cartão.
        /// </summary>
        [Authorize(PermissaoAcesso.CartoesCredito)]
        [HttpDelete]
        [Route("v1/cartoes/excluir/{idCartaoCredito:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Cartão excluído com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCartaoCreditoResponseExemplo))]
        public async Task<ISaida> ExcluirCartaoCredito(int idCartaoCredito)
        {
            return await _cartaoCreditoServico.ExcluirCartaoCredito(idCartaoCredito, base.ObterIdUsuarioClaim());
        }
    }
}
