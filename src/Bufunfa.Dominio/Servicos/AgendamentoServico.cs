using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class AgendamentoServico : Notificavel, IAgendamentoServico
    {
        private readonly IAgendamentoRepositorio _agendamentoRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IUow _uow;

        public AgendamentoServico(IAgendamentoRepositorio agendamentoRepositorio, IParcelaRepositorio parcelaRepositorio, IUow uow)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _parcelaRepositorio = parcelaRepositorio;
            _uow = uow;
        }

        public async Task<ISaida> ObterAgendamentoPorId(int idAgendamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, string.Format(AgendamentoMensagem.Id_Agendamento_Invalido, idAgendamento));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, idUsuario));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, string.Format(AgendamentoMensagem.Id_Agendamento_Nao_Existe, idAgendamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Encontrado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ProcurarAgendamentos(ProcurarAgendamentoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            if (!procurarEntrada.Valido())
                return new Saida(false, procurarEntrada.Mensagens, null);

            return await _agendamentoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarAgendamento(CadastrarAgendamentoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var agendamento = new Agendamento(cadastroEntrada);

            await _agendamentoRepositorio.Inserir(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }








        public Task<ISaida> AlterarAgendamento(AlterarAgendamentoEntrada alterarEntrada)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> AlterarParcela(AlterarParcelaEntrada alterarEntrada)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> AlterarPessoa(AlterarPessoaEntrada alterarEntrada)
        {
            throw new NotImplementedException();
        }

        

        public Task<ISaida> CadastrarParcela(CadastrarParcelaEntrada cadastroEntrada)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> CadastrarPessoa(CadastrarPessoaEntrada cadastroEntrada)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> DescartarParcela(DescartarParcelaEntrada descartarEntrada)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> ExcluirParcela(int idParcela, int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> ExcluirPessoa(int idPessoa, int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<ISaida> LancarParcela(LancarParcelaEntrada lancarEntrada)
        {
            throw new NotImplementedException();
        }

        

        public Task<ISaida> ObterPessoaPorId(int idPessoa, int idUsuario)
        {
            throw new NotImplementedException();
        }

        

        public Task<ISaida> ProcurarPessoas(ProcurarPessoaEntrada procurarEntrada)
        {
            throw new NotImplementedException();
        }
    }
}
