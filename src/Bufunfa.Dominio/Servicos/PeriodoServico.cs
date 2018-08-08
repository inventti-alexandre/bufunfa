using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class PeriodoServico : Notificavel, IPeriodoServico
    {
        private readonly IPeriodoRepositorio _periodoRepositorio;
        private readonly IUow _uow;

        public PeriodoServico(IPeriodoRepositorio periodoRepositorio, IUow uow)
        {
            _periodoRepositorio = periodoRepositorio;
            _uow = uow;
        }

        public async Task<ISaida> ObterPeriodoPorId(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, string.Format(PeriodoMensagem.Id_Periodo_Invalido, idPeriodo));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, PeriodoMensagem.Periodo_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Encontrado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> ObterPeriodosPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstPeriodos = await _periodoRepositorio.ObterPorUsuario(idUsuario);

            return lstPeriodos.Any()
                ? new Saida(true, new[] { PeriodoMensagem.Periodos_Encontrados_Com_Sucesso }, lstPeriodos.Select(x => new PeriodoSaida(x)))
                : new Saida(true, new[] { PeriodoMensagem.Nenhum_periodo_encontrado }, null);
        }

        public async Task<ISaida> ProcurarPeriodos(ProcurarPeriodoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            if (!procurarEntrada.Valido())
                return new Saida(false, procurarEntrada.Mensagens, null);

            return await _periodoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarPeriodo(CadastrarPeriodoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                await _periodoRepositorio.VerificarExistenciaPorDataInicioFim(cadastroEntrada.IdUsuario, cadastroEntrada.DataInicio, cadastroEntrada.DataFim),
                PeriodoMensagem.Datas_Abrangidas_Outro_Periodo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = new Periodo(cadastroEntrada);

            await _periodoRepositorio.Inserir(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Cadastrado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> AlterarPeriodo(AlterarPeriodoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(alterarEntrada.IdPeriodo, true);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, alterarEntrada.IdPeriodo));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, alterarEntrada.IdUsuario, PeriodoMensagem.Periodo_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                await _periodoRepositorio.VerificarExistenciaPorDataInicioFim(alterarEntrada.IdUsuario, alterarEntrada.DataInicio, alterarEntrada.DataFim, alterarEntrada.IdPeriodo),
                PeriodoMensagem.Datas_Abrangidas_Outro_Periodo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            periodo.Alterar(alterarEntrada);

            _periodoRepositorio.Atualizar(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Alterado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> ExcluirPeriodo(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, string.Format(PeriodoMensagem.Id_Periodo_Invalido, idPeriodo));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, PeriodoMensagem.Periodo_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _periodoRepositorio.Deletar(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Excluido_Com_Sucesso }, new PeriodoSaida(periodo));
        }
    }
}
