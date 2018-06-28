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
        /// <param name="idPessoa">ID da pessoa</param>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpGet]
        [Route("v1/pessoas/obter-por-id/{idPessoa:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Pessoa do usuário encontrada.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPessoaPorIdResponseExemplo))]
        public async Task<ISaida> ObterContaPorId(int idPessoa)
        {
            return await _pessoaServico.ObterPessoaPorId(idPessoa, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Obtém as pessoas do usuário autenticado
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpGet]
        [Route("v1/pessoas/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Pessoas do usuário encontradas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPessoasPorUsuarioResponseExemplo))]
        public async Task<ISaida> ObterPessoasPorUsuarioAutenticado()
        {
            return await _pessoaServico.ObterPessoasPorUsuario(base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza uma procura por pessoas a partir dos parâmetros informados
        /// </summary>
        /// <param name="viewModel">Parâmetros utilizados para realizar a procura.</param>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPost]
        [Route("v1/pessoas/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Resultado da procura por pessoas.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPessoaResponseExemplo))]
        public async Task<ISaida> Procurar([FromBody] ProcurarPessoaViewModel viewModel)
        {
            var procurarEntrada = new ProcurarPessoaEntrada(base.ObterIdUsuarioClaim(), viewModel.OrdenarPor, viewModel.OrdenarSentido, viewModel.PaginaIndex, viewModel.PaginaTamanho)
            {
                Nome = viewModel.Nome
            };

            return await _pessoaServico.ProcurarPessoas(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova pessoa.
        /// </summary>
        /// <param name="viewModel">Informações de cadastro da pessoa.</param>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPost]
        [Route("v1/pessoas/cadastrar")]
        [SwaggerRequestExample(typeof(CadastrarPessoaViewModel), typeof(CadastrarPessoaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Pessoa cadastrada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPessoaResponseExemplo))]
        public async Task<ISaida> CadastrarPessoa([FromBody] CadastrarPessoaViewModel viewModel)
        {
            var cadastrarEntrada = new CadastrarPessoaEntrada(base.ObterIdUsuarioClaim(), viewModel.Nome);

            return await _pessoaServico.CadastrarPessoa(cadastrarEntrada);
        }

        /// <summary>
        /// Realiza a alteração de uma pessoa.
        /// </summary>
        /// <param name="viewModel">Informações para alteração de uma pessoa.</param>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpPut]
        [Route("v1/pessoas/alterar")]
        [SwaggerRequestExample(typeof(AlterarPessoaViewModel), typeof(AlterarPessoaViewModelExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Pessoa alterada com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPessoaResponseExemplo))]
        public async Task<ISaida> AlterarPessoa([FromBody] AlterarPessoaViewModel viewModel)
        {
            var alterarEntrada = new AlterarPessoaEntrada(viewModel.IdPessoa, viewModel.Nome, base.ObterIdUsuarioClaim());

            return await _pessoaServico.AlterarPessoa(alterarEntrada);
        }

        /// <summary>
        /// Realiza a exclusão de uma pessoa.
        /// </summary>
        [Authorize(PermissaoAcesso.Pessoas)]
        [HttpDelete]
        [Route("v1/pessoas/excluir/{idPessoa:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Pessoa excluída com sucesso.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPessoaResponseExemplo))]
        public async Task<ISaida> ExcluirPessoa(int idPessoa)
        {
            return await _pessoaServico.ExcluirPessoa(idPessoa, base.ObterIdUsuarioClaim());
        }
    }
}
