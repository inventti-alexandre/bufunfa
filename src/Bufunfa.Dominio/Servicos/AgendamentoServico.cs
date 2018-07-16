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
    public class AgendamentoServico : Notificavel, IAgendamentoServico
    {
        private readonly IAgendamentoRepositorio _agendamentoRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IUow _uow;

        public AgendamentoServico(IAgendamentoRepositorio agendamentoRepositorio, IParcelaRepositorio parcelaRepositorio, ILancamentoRepositorio lancamentoRepositorio, IUow uow)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _parcelaRepositorio     = parcelaRepositorio;
            _lancamentoRepositorio  = lancamentoRepositorio;
            _uow                    = uow;
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

            agendamento = await _agendamentoRepositorio.ObterPorId(agendamento.Id);

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> AlterarAgendamento(AlterarAgendamentoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(alterarEntrada.IdAgendamento, true);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, string.Format(AgendamentoMensagem.Id_Agendamento_Nao_Existe, alterarEntrada.IdAgendamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, alterarEntrada.IdUsuario, AgendamentoMensagem.Agendamento_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui todas as parcelas agendamento.
            foreach (var parcelaAberta in agendamento.Parcelas)
            {
                _parcelaRepositorio.Deletar(parcelaAberta);
            }

            agendamento.Alterar(alterarEntrada);

            _agendamentoRepositorio.Atualizar(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Alterado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario)
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
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _agendamentoRepositorio.Deletar(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Excluido_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> CadastrarParcela(CadastrarParcelaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var parcela = new Parcela(cadastroEntrada);

            await _parcelaRepositorio.Inserir(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Cadastrada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> AlterarParcela(AlterarParcelaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(alterarEntrada.IdParcela, true);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, string.Format(ParcelaMensagem.Id_Parcela_Nao_Existe, alterarEntrada.IdParcela));

            if (parcela != null)
            {
                // Verifica se a parcela já foi lançada ou descartada
                this.NotificarSeVerdadeiro(parcela.Lancada || parcela.Descartada, ParcelaMensagem.Parcela_Alterar_Ja_Desacartada_Lancada);
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, alterarEntrada.IdUsuario, ParcelaMensagem.Parcela_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Alterar(alterarEntrada);

            _parcelaRepositorio.Atualizar(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Alterada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> LancarParcela(LancarParcelaEntrada lancarEntrada)
        {
            // Verifica se as informações para o lançamento foram informadas corretamente
            if (!lancarEntrada.Valido())
                return new Saida(false, lancarEntrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(lancarEntrada.IdParcela, true);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, string.Format(ParcelaMensagem.Id_Parcela_Nao_Existe, lancarEntrada.IdParcela));

            if (parcela != null)
            {
                // Verifica se a parcela já foi lançada ou descartada
                this.NotificarSeVerdadeiro(parcela.Lancada, ParcelaMensagem.Parcela_Lancar_Ja_Lancada);
                this.NotificarSeVerdadeiro(parcela.Descartada, ParcelaMensagem.Parcela_Lancar_Ja_Descartada);
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, lancarEntrada.IdUsuario, ParcelaMensagem.Parcela_Lancar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Lancar(lancarEntrada);

            _parcelaRepositorio.Atualizar(parcela);

            // Cadastro o lançamento
            var cadastrarEntrada = new CadastrarLancamentoEntrada(
                parcela.Agendamento.IdUsuario,
                parcela.Agendamento.IdConta.Value,
                parcela.Agendamento.IdCategoria,
                lancarEntrada.Data,
                lancarEntrada.Valor,
                parcela.Agendamento.IdPessoa,
                parcela.Id,
                lancarEntrada.Observacao);

            var lancamento = new Lancamento(cadastrarEntrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Lancada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> DescartarParcela(DescartarParcelaEntrada descartarEntrada)
        {
            // Verifica se as informações para descartar foram informadas corretamente
            if (!descartarEntrada.Valido())
                return new Saida(false, descartarEntrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(descartarEntrada.IdParcela, true);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, string.Format(ParcelaMensagem.Id_Parcela_Nao_Existe, descartarEntrada.IdParcela));

            if (parcela != null)
            {
                // Verifica se a parcela já foi lançada ou descartada
                this.NotificarSeVerdadeiro(parcela.Lancada, ParcelaMensagem.Parcela_Lancar_Ja_Lancada);
                this.NotificarSeVerdadeiro(parcela.Descartada, ParcelaMensagem.Parcela_Lancar_Ja_Descartada);
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, descartarEntrada.IdUsuario, ParcelaMensagem.Parcela_Lancar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Descartar(descartarEntrada);

            _parcelaRepositorio.Atualizar(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Descartada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> ExcluirParcela(int idParcela, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idParcela, 0, string.Format(ParcelaMensagem.Id_Parcela_Invalido, idParcela));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, idUsuario));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, string.Format(ParcelaMensagem.Id_Parcela_Nao_Existe, idParcela));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, idUsuario, ParcelaMensagem.Parcela_Lancar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _parcelaRepositorio.Deletar(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Excluida_Com_Sucesso }, new ParcelaSaida(parcela));
        }
    }
}
