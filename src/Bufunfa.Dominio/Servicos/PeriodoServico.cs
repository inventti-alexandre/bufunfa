using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;

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

        public ISaida ObterPeriodoPorId(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, $"O ID do período informado ({idPeriodo}) é inválido.");
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = _periodoRepositorio.ObterPorId(idPeriodo);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, $"O período de ID {idPeriodo} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, "O período não pertence ao usuário autenticado.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { "Período encontrado com sucesso." }, new PeriodoSaida(periodo));
        }

        public ISaida ObterPeriodosPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstPeriodos = _periodoRepositorio.ObterPorUsuario(idUsuario);

            return lstPeriodos.Any()
                ? new Saida(true, new[] { "Períodos do usuário encontrados com sucesso." }, lstPeriodos.Select(x => new PeriodoSaida(x)))
                : new Saida(true, new[] { "Nenhuma período do usuário foi encontrado." }, null);
        }

        public ISaida CadastrarPeriodo(CadastrarPeriodoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(_periodoRepositorio.VerificarExistenciaPorDataInicioFim(cadastroEntrada.IdUsuario, cadastroEntrada.DataInicio, cadastroEntrada.DataFim), "Você já possui um período cadastrado que abrange as datas informadas.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                _periodoRepositorio.VerificarExistenciaPorDataInicioFim(cadastroEntrada.IdUsuario, cadastroEntrada.DataInicio, cadastroEntrada.DataFim),
                "As datas informadas pelo período, já são abrangidas por outro período.");

            var periodo = new Periodo(cadastroEntrada.IdUsuario, cadastroEntrada.Nome, cadastroEntrada.DataInicio, cadastroEntrada.DataFim);

            _periodoRepositorio.Inserir(periodo);

            _uow.Commit();

            return new Saida(true, new[] { $"Período \"{periodo.Nome}\" cadastrada com sucesso." }, new PeriodoSaida(periodo));
        }

        public ISaida AlterarPeriodo(AlterarPeriodoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var periodo = _periodoRepositorio.ObterPorId(alterarEntrada.IdPeriodo, true);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, $"O período de ID {alterarEntrada.IdPeriodo} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, alterarEntrada.IdUsuario, "O período que se deseja alterar, não pertence ao usuário autenticado.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                _periodoRepositorio.VerificarExistenciaPorDataInicioFim(alterarEntrada.IdUsuario, alterarEntrada.DataInicio, alterarEntrada.DataFim, alterarEntrada.IdPeriodo),
                "As datas informadas pelo período, já são abrangidas por outro período.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            periodo.Nome       = alterarEntrada.Nome;
            periodo.DataInicio = alterarEntrada.DataInicio;
            periodo.DataFim    = alterarEntrada.DataFim;

            _periodoRepositorio.Atualizar(periodo);

            _uow.Commit();

            return new Saida(true, new[] { "Período alterado com sucesso." }, new PeriodoSaida(periodo));
        }

        public ISaida ExcluirPeriodo(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, $"O ID do período informado ({idPeriodo}) é inválido.");
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = _periodoRepositorio.ObterPorId(idPeriodo);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, $"O período de ID {idPeriodo} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, "O período não pertence ao usuário autenticado, por isso não é permitido excluí-lo.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _periodoRepositorio.Deletar(idPeriodo);

            return new Saida(true, new[] { "Período excluído com sucesso." }, new PeriodoSaida(periodo));
        }
    }
}
