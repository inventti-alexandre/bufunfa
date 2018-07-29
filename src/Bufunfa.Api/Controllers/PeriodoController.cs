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
    public class PeriodoController : BaseController
    {
        private readonly IPeriodoServico _periodoServico;

        public PeriodoController(IPeriodoServico periodoServico)
        {
            _periodoServico = periodoServico;
        }

        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpGet]
        [Route("v1/periodos/obter-por-id/{idPeriodo:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId([SwaggerParameter("ID do período.", Required = true)] int idPeriodo)
        {
            return await _periodoServico.ObterPeriodoPorId(
                idPeriodo,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém os períodos do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpGet]
        [Route("v1/periodos/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Períodos do usuário encontrados.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodosPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterPeriodosPorUsuarioAutenticado()
        {
            return await _periodoServico.ObterPeriodosPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por períodos a partir dos parâmetros informados
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPost]
        [Route("v1/periodos/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por períodos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPeriodoResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarPeriodoViewModel model)
        {
            var procurarEntrada = new ProcurarPeriodoEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                Nome = model.Nome,
                Data = model.Data
            };

            return await _periodoServico.ProcurarPeriodos(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPost]
        [Route("v1/periodos/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarPeriodoViewModel), typeof(CadastrarPeriodoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPeriodoResponseExemplo))]
        public async Task<ISaida> CadastrarPeriodo([FromBody, SwaggerParameter("Informações de cadastro do período.", Required = true)] CadastrarPeriodoViewModel model)
        {
            var cadastrarEntrada = new CadastrarPeriodoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.DataInicio.Value,
                model.DataFim.Value);

            return await _periodoServico.CadastrarPeriodo(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um período.
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPut]
        [Route("v1/periodos/alterar")]
        [SwaggerRequestExample(typeof(AlterarPeriodoViewModel), typeof(AlterarPeriodoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPeriodoResponseExemplo))]
        public async Task<ISaida> AlterarPeriodo([FromBody, SwaggerParameter("Informações para alteração de um período.", Required = true)] AlterarPeriodoViewModel model)
        {
            var alterarEntrada = new AlterarPeriodoEntrada(
                model.IdPeriodo,
                model.Nome,
                model.DataInicio.Value,
                model.DataFim.Value,
                base.ObterIdUsuarioClaim());

            return await _periodoServico.AlterarPeriodo(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um período.
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpDelete]
        [Route("v1/periodos/excluir/{idPeriodo:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPeridoResponseExemplo))]
        public async Task<ISaida> ExcluirPeriodo([SwaggerParameter("ID do período que deverá ser excluído.", Required = true)] int idPeriodo)
        {
            return await _periodoServico.ExcluirPeriodo(
                idPeriodo,
                base.ObterIdUsuarioClaim());
        }
    }
}
