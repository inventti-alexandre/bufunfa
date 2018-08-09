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
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IUow _uow;

        public AgendamentoServico(
            IAgendamentoRepositorio agendamentoRepositorio,
            ICartaoCreditoRepositorio cartaoCreditoRepositorio,
            IContaRepositorio contaRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IPessoaRepositorio pessoaRepositorio,
            IParcelaRepositorio parcelaRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            IUow uow)
        {
            _agendamentoRepositorio   = agendamentoRepositorio;
            _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
            _contaRepositorio         = contaRepositorio;
            _parcelaRepositorio       = parcelaRepositorio;
            _pessoaRepositorio        = pessoaRepositorio;
            _lancamentoRepositorio    = lancamentoRepositorio;
            _categoriaRepositorio     = categoriaRepositorio;
            _uow                      = uow;
        }

        public async Task<ISaida> ObterAgendamentoPorId(int idAgendamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, AgendamentoMensagem.Id_Agendamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, AgendamentoMensagem.Id_Agendamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { AgendamentoMensagem.Agendamento_Encontrado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ProcurarAgendamentos(ProcurarAgendamentoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return procurarEntrada.Invalido
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _agendamentoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarAgendamento(CadastrarAgendamentoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta ou cartão de crédito existem a partir do ID informado.
            if (cadastroEntrada.IdConta.HasValue)
                this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdConta.Value), ContaMensagem.Id_Conta_Nao_Existe);
            else
                this.NotificarSeFalso(await _cartaoCreditoRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdCartaoCredito.Value), CartaoCreditoMensagem.Id_Cartao_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (cadastroEntrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = new Agendamento(cadastroEntrada);

            await _agendamentoRepositorio.Inserir(agendamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso }, new AgendamentoSaida(await _agendamentoRepositorio.ObterPorId(agendamento.Id)));
        }

        public async Task<ISaida> AlterarAgendamento(AlterarAgendamentoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
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

            // Verifica se a categoria existe a partir do ID informado.
            if (agendamento.IdCategoria != alterarEntrada.IdCategoria)
                this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta ou cartão de crédito existem a partir do ID informado.
            if (agendamento.IdConta != alterarEntrada.IdConta && alterarEntrada.IdConta.HasValue)
                this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdConta.Value), ContaMensagem.Id_Conta_Nao_Existe);
            else if (alterarEntrada.IdCartaoCredito.HasValue && agendamento.IdCartaoCredito != alterarEntrada.IdCartaoCredito)
                this.NotificarSeFalso(await _cartaoCreditoRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdCartaoCredito.Value), CartaoCreditoMensagem.Id_Cartao_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (agendamento.IdPessoa != alterarEntrada.IdPessoa && alterarEntrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui todas as parcelas abertas do agendamento.
            foreach (var parcelaAberta in agendamento.Parcelas.Where(x => x.Status == StatusParcela.Aberta))
            {
                _parcelaRepositorio.Deletar(parcelaAberta);
            }

            agendamento.Alterar(alterarEntrada);

            _agendamentoRepositorio.Atualizar(agendamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AgendamentoMensagem.Agendamento_Alterado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, AgendamentoMensagem.Id_Agendamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, AgendamentoMensagem.Id_Agendamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento possui alguma parcela lançada.
            this.NotificarSeVerdadeiro(agendamento.Parcelas.Any(x => x.Lancada), AgendamentoMensagem.Agendamento_Excluir_Possui_Parcela_Lancada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui todas as parcelas do agendamento.
            _parcelaRepositorio.Deletar(agendamento.Parcelas);

            _agendamentoRepositorio.Deletar(agendamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AgendamentoMensagem.Agendamento_Excluido_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> CadastrarParcela(CadastrarParcelaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var parcela = new Parcela(cadastroEntrada);

            await _parcelaRepositorio.Inserir(parcela);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcela_Cadastrada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> AlterarParcela(AlterarParcelaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
                return new Saida(false, alterarEntrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(alterarEntrada.IdParcela, true);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, alterarEntrada.IdUsuario, ParcelaMensagem.Parcela_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela está fechada (lançada ou descartada)
            this.NotificarSeVerdadeiro(parcela.Status == StatusParcela.Fechada, ParcelaMensagem.Parcela_Alterar_Ja_Fechada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Alterar(alterarEntrada);

            _parcelaRepositorio.Atualizar(parcela);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcela_Alterada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> LancarParcela(LancarParcelaEntrada lancarEntrada)
        {
            // Verifica se as informações para o lançamento foram informadas corretamente
            if (lancarEntrada.Invalido)
                return new Saida(false, lancarEntrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(lancarEntrada.IdParcela, true);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, lancarEntrada.IdUsuario, ParcelaMensagem.Parcela_Lancar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela já foi lançada ou descartada
            this.NotificarSeVerdadeiro(parcela.Lancada, ParcelaMensagem.Parcela_Lancar_Ja_Lancada);
            this.NotificarSeVerdadeiro(parcela.Descartada, ParcelaMensagem.Parcela_Lancar_Ja_Descartada);

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

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcela_Lancada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> DescartarParcela(DescartarParcelaEntrada descartarEntrada)
        {
            // Verifica se as informações para descartar foram informadas corretamente
            if (descartarEntrada.Invalido)
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

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcela_Descartada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> ExcluirParcela(int idParcela, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idParcela, 0, ParcelaMensagem.Id_Parcela_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, idUsuario, ParcelaMensagem.Parcela_Lancar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela está fechada (lançada ou descartada)
            this.NotificarSeVerdadeiro(parcela.Status == StatusParcela.Fechada, ParcelaMensagem.Parcela_Excluir_Ja_Fechada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _parcelaRepositorio.Deletar(parcela);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcela_Excluida_Com_Sucesso }, new ParcelaSaida(parcela));
        }
    }
}
