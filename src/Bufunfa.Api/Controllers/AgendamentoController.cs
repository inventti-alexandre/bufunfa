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
    public class AgendamentoController : BaseController
    {
        private readonly IAgendamentoServico _agendamentoServico;

        public AgendamentoController(IAgendamentoServico agendamentoServico)
        {
            _agendamentoServico = agendamentoServico;
        }

        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        /// <param name="idAgendamento">ID do agendamento</param>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpGet]
        [Route("v1/agendamentos/obter-por-id/{idAgendamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Agendamento do usuário encontrada.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAgendamentoPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId(int idAgendamento)
        {
            return await _agendamentoServico.ObterAgendamentoPorId(idAgendamento, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por agendamentos a partir dos parâmetros informados
        /// </summary>
        /// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPost]
        [Route("v1/agendamentos/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Resultado da procura por agendamentos.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarAgendamentoResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody] ProcurarAgendamentoViewModel viewModel)
        {
            var procurarEntrada = new ProcurarAgendamentoEntrada(base.ObterIdUsuarioClaim(), viewModel.OrdenarPor, viewModel.OrdenarSentido, viewModel.PaginaIndex, viewModel.PaginaTamanho)
            {
                DataFimParcela    = viewModel.DataFimParcela,
                DataInicioParcela = viewModel.DataInicioParcela,
                IdCartaoCredito   = viewModel.IdCartaoCredito,
                IdCategoria       = viewModel.IdCategoria,
                IdConta           = viewModel.IdConta,
                IdPessoa          = viewModel.IdPessoa,
                SeConcluido       = viewModel.SeConcluido
            };

            return await _agendamentoServico.ProcurarAgendamentos(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo agendamento.
        /// </summary>
        /// <param name="viewModel">Informações de cadastro do agendamento.</param>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPost]
        [Route("v1/agendamentos/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarAgendamentoViewModel), typeof(CadastrarAgendamentoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Agendamento cadastrado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAgendamentoResponseExemplo))]
        public async Task<ISaida> CadastrarAgendamento([FromBody] CadastrarAgendamentoViewModel viewModel)
        {
            var cadastrarEntrada = new CadastrarAgendamentoEntrada(
                base.ObterIdUsuarioClaim(),
                viewModel.IdCategoria.Value,
                viewModel.IdConta,
                viewModel.IdCartaoCredito,
                viewModel.TipoMetodoPagamento.Value,
                viewModel.ValorParcela.Value,
                viewModel.DataPrimeiraParcela.Value,
                viewModel.QuantidadeParcelas.Value,
                viewModel.PeriodicidadeParcelas.Value,
                viewModel.IdPessoa,
                viewModel.Observacao);

            return await _agendamentoServico.CadastrarAgendamento(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um agendamento.
        /// </summary>
        /// <param name="viewModel">Informações para alteração de um agendamento.</param>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPut]
        [Route("v1/agendamentos/alterar")]
        [SwaggerRequestExample(typeof(AlterarAgendamentoViewModel), typeof(AlterarAgendamentoViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Agendamento alterado com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarAgendamentoResponseExemplo))]
        public async Task<ISaida> AlterarAgendamento([FromBody] AlterarAgendamentoViewModel viewModel)
        {
            var alterarEntrada = new AlterarAgendamentoEntrada(
                viewModel.IdAgendamento.Value,
                viewModel.IdCategoria.Value,
                viewModel.IdConta,
                viewModel.IdCartaoCredito,
                viewModel.TipoMetodoPagamento.Value,
                viewModel.ValorParcela.Value,
                viewModel.DataPrimeiraParcela.Value,
                viewModel.QuantidadeParcelas.Value,
                viewModel.PeriodicidadeParcelas.Value,
                base.ObterIdUsuarioClaim(),
                viewModel.IdPessoa,
                viewModel.Observacao);

            return await _agendamentoServico.AlterarAgendamento(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um agendamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpDelete]
        [Route("v1/agendamentos/excluir/{idAgendamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Agendamento excluído com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAgendamentoResponseExemplo))]
        public async Task<ISaida> ExcluirAgendamento(int idAgendamento)
        {
            return await _agendamentoServico.ExcluirAgendamento(idAgendamento, base.ObterIdUsuarioClaim());
        }
    }
}
