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
        /// <param name="idPeriodo">ID do período</param>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpGet]
        [Route("v1/periodos/obter-por-id/{idPeriodo:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período do usuário encontrado.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId(int idPeriodo)
        {
            return await _periodoServico.ObterPeriodoPorId(idPeriodo, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém os períodos do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpGet]
        [Route("v1/periodos/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Períodos do usuário encontrados.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodosPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterPeriodosPorUsuarioAutenticado()
        {
            return await _periodoServico.ObterPeriodosPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por períodos a partir dos parâmetros informados
        /// </summary>
        /// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPost]
        [Route("v1/periodos/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Resultado da procura por períodos.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPeriodoResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody] ProcurarPeriodoViewModel viewModel)
        {
            var procurarEntrada = new ProcurarPeriodoEntrada(base.ObterIdUsuarioClaim(), viewModel.OrdenarPor, viewModel.OrdenarSentido, viewModel.PaginaIndex, viewModel.PaginaTamanho)
            {
                Nome = viewModel.Nome,
                Data = viewModel.Data
            };

            return await _periodoServico.ProcurarPeriodos(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        /// <param name="viewModel">Informações de cadastro do período.</param>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPost]
        [Route("v1/periodos/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarPeriodoViewModel), typeof(CadastrarPeriodoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período cadastrado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPeriodoResponseExemplo))]
        public async Task<ISaida> CadastrarPeriodo([FromBody] CadastrarPeriodoViewModel viewModel)
        {
            var cadastrarEntrada = new CadastrarPeriodoEntrada(base.ObterIdUsuarioClaim(), viewModel.Nome, viewModel.DataInicio.Value, viewModel.DataFim.Value);

            return await _periodoServico.CadastrarPeriodo(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um período.
        /// </summary>
        /// <param name="viewModel">Informações para alteração de um período.</param>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpPut]
        [Route("v1/periodos/alterar")]
        [SwaggerRequestExample(typeof(AlterarPeriodoViewModel), typeof(AlterarPeriodoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período alterado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPeriodoResponseExemplo))]
        public async Task<ISaida> AlterarPeriodo([FromBody] AlterarPeriodoViewModel viewModel)
        {
            var alterarEntrada = new AlterarPeriodoEntrada(viewModel.IdPeriodo, viewModel.Nome, viewModel.DataInicio.Value, viewModel.DataFim.Value, base.ObterIdUsuarioClaim());

            return await _periodoServico.AlterarPeriodo(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um período.
        /// </summary>
        [Authorize(PermissaoAcesso.Periodos)]
        [HttpDelete]
        [Route("v1/periodos/excluir/{idPeriodo:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Período excluído com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPeridoResponseExemplo))]
        public async Task<ISaida> ExcluirPeriodo(int idPeriodo)
        {
            return await _periodoServico.ExcluirPeriodo(idPeriodo, base.ObterIdUsuarioClaim());
        }
    }
}
