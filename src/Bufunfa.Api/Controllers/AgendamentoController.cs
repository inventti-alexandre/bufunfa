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
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpGet]
        [Route("v1/agendamentos/obter-por-id/{idAgendamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAgendamentoPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId([SwaggerParameter("ID do agendamento.", Required = true)] int idAgendamento)
        {
            return await _agendamentoServico.ObterAgendamentoPorId(
                idAgendamento,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por agendamentos a partir dos parâmetros informados
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPost]
        [Route("v1/agendamentos/procurar")]
        [SwaggerRequestExample(typeof(ProcurarAgendamentoViewModel), typeof(ProcurarAgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por agendamentos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarAgendamentoResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarAgendamentoViewModel model)
        {
            var procurarEntrada = new ProcurarAgendamentoEntrada(base.ObterIdUsuarioClaim(), model.OrdenarPor, model.OrdenarSentido, model.PaginaIndex, model.PaginaTamanho)
            {
                DataFimParcela    = model.DataFimParcela,
                DataInicioParcela = model.DataInicioParcela,
                IdCartaoCredito   = model.IdCartaoCredito,
                IdCategoria       = model.IdCategoria,
                IdConta           = model.IdConta,
                IdPessoa          = model.IdPessoa,
                Concluido         = model.Concluido
            };

            return await _agendamentoServico.ProcurarAgendamentos(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo agendamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPost]
        [Route("v1/agendamentos/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarAgendamentoViewModel), typeof(CadastrarAgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAgendamentoResponseExemplo))]
        public async Task<ISaida> CadastrarAgendamento([FromBody, SwaggerParameter("Informações de cadastro do agendamento.", Required = true)] CadastrarAgendamentoViewModel model)
        {
            var cadastrarEntrada = new CadastrarAgendamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdCategoria.Value,
                model.IdConta,
                model.IdCartaoCredito,
                model.TipoMetodoPagamento.Value,
                model.ValorParcela.Value,
                model.DataPrimeiraParcela.Value,
                model.QuantidadeParcelas.Value,
                model.PeriodicidadeParcelas.Value,
                model.IdPessoa,
                model.Observacao);

            return await _agendamentoServico.CadastrarAgendamento(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de um agendamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPut]
        [Route("v1/agendamentos/alterar")]
        [SwaggerRequestExample(typeof(AlterarAgendamentoViewModel), typeof(AlterarAgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarAgendamentoResponseExemplo))]
        public async Task<ISaida> AlterarAgendamento([FromBody, SwaggerParameter("Informações para alteração de um agendamento.", Required = true)] AlterarAgendamentoViewModel model)
        {
            var alterarEntrada = new AlterarAgendamentoEntrada(
                model.IdAgendamento.Value,
                model.IdCategoria.Value,
                model.IdConta,
                model.IdCartaoCredito,
                model.TipoMetodoPagamento.Value,
                model.ValorParcela.Value,
                model.DataPrimeiraParcela.Value,
                model.QuantidadeParcelas.Value,
                model.PeriodicidadeParcelas.Value,
                base.ObterIdUsuarioClaim(),
                model.IdPessoa,
                model.Observacao);

            return await _agendamentoServico.AlterarAgendamento(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de um agendamento.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpDelete]
        [Route("v1/agendamentos/excluir/{idAgendamento:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAgendamentoResponseExemplo))]
        public async Task<ISaida> ExcluirAgendamento([SwaggerParameter("ID do agendamento que deverá ser excluído.", Required = true)] int idAgendamento)
        {
            return await _agendamentoServico.ExcluirAgendamento(
                idAgendamento,
                base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza o cadastro de uma nova parcela.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPost]
        [Route("v1/agendamentos/cadastrar-parcela")]
        [SwaggerRequestExample(typeof(CadastrarParcelaViewModel), typeof(CadastrarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarParcelaResponseExemplo))]
        public async Task<ISaida> CadastrarParcela([FromBody, SwaggerParameter("Informações de cadastro da parcela.", Required = true)] CadastrarParcelaViewModel model)
        {
            var cadastrarEntrada = new CadastrarParcelaEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdAgendamento.Value,
                null,
                model.Data.Value,
                model.Valor.Value,
                model.Observacao);

            return await _agendamentoServico.CadastrarParcela(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma parcela.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPut]
        [Route("v1/agendamentos/alterar-parcela")]
        [SwaggerRequestExample(typeof(AlterarParcelaViewModel), typeof(AlterarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarParcelaResponseExemplo))]
        public async Task<ISaida> AlterarParcela([FromBody, SwaggerParameter("Informações para alteração de uma parcela.", Required = true)] AlterarParcelaViewModel model)
        {
            var alterarEntrada = new AlterarParcelaEntrada(
                model.IdParcela.Value,
                model.Data.Value,
                model.Valor.Value,
                base.ObterIdUsuarioClaim(),
                model.Observacao);

            return await _agendamentoServico.AlterarParcela(alterarEntrada);
        }

        /// <summary>
        /// Realiza o lançamento de uma parcela.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPut]
        [Route("v1/agendamentos/lancar-parcela")]
        [SwaggerRequestExample(typeof(LancarParcelaViewModel), typeof(LancarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela lançada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(LancarParcelaResponseExemplo))]
        public async Task<ISaida> LancarParcela([FromBody, SwaggerParameter("Informações de lançamento da parcela.", Required = true)] LancarParcelaViewModel model)
        {
            var lancarEntrada = new LancarParcelaEntrada(
                model.IdParcela.Value,
                model.Data.Value,
                model.Valor.Value,
                base.ObterIdUsuarioClaim(),
                model.Observacao);

            return await _agendamentoServico.LancarParcela(lancarEntrada);
        }

        /// <summary>
        /// Realiza o descarte de uma parcela.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpPut]
        [Route("v1/agendamentos/descartar-parcela")]
        [SwaggerRequestExample(typeof(DescartarParcelaViewModel), typeof(DescartarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela descartada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DescartarParcelaResponseExemplo))]
        public async Task<ISaida> DescartarParcela([FromBody, SwaggerParameter("Informações de descarte da parcela.", Required = true)] DescartarParcelaViewModel model)
        {
            var descartarEntrada = new DescartarParcelaEntrada(
                model.IdParcela.Value,
                base.ObterIdUsuarioClaim(),
                model.MotivoDescarte);

            return await _agendamentoServico.DescartarParcela(descartarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma parcela.
        /// </summary>
        [Authorize(PermissaoAcesso.Agendamentos)]
        [HttpDelete]
        [Route("v1/agendamentos/excluir-parcela/{idParcela:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirParcelaResponseExemplo))]
        public async Task<ISaida> ExcluirParcela([SwaggerParameter("ID da parcela que deverá ser excluído.", Required = true)] int idParcela)
        {
            return await _agendamentoServico.ExcluirParcela(
                idParcela,
                base.ObterIdUsuarioClaim());
        }
    }
}
