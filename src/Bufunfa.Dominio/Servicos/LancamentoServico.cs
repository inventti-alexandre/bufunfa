using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class LancamentoServico : Notificavel, ILancamentoServico
    {
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IAnexoRepositorio _anexoRepositorio;
        private readonly IUow _uow;

        public LancamentoServico(ILancamentoRepositorio lancamentoRepositorio, IAnexoRepositorio anexoRepositorio, IUow uow)
        {
            _lancamentoRepositorio = lancamentoRepositorio;
            _anexoRepositorio      = anexoRepositorio;
            _uow                   = uow;
        }

        public async Task<ISaida> ObterLancamentoPorId(int idLancamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, string.Format(LancamentoMensagem.Id_Lancamento_Invalido, idLancamento));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, idUsuario));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, string.Format(LancamentoMensagem.Id_Lancamento_Nao_Existe, idLancamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            if (!procurarEntrada.Valido())
                return new Saida(false, procurarEntrada.Mensagens, null);

            return await _lancamentoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarLancamento(CadastrarLancamentoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var lancamento = new Lancamento(cadastroEntrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> AlterarLancamento(AlterarLancamentoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(alterarEntrada.IdLancamento, true);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, string.Format(LancamentoMensagem.Id_Lancamento_Nao_Existe, alterarEntrada.IdLancamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, alterarEntrada.IdUsuario, LancamentoMensagem.Lancamento_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            lancamento.Alterar(alterarEntrada);

            _lancamentoRepositorio.Atualizar(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Alterado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ExcluirLancamento(int idLancamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, string.Format(LancamentoMensagem.Id_Lancamento_Invalido, idLancamento));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, idUsuario));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, string.Format(LancamentoMensagem.Id_Lancamento_Nao_Existe, idLancamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _lancamentoRepositorio.Deletar(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Excluido_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> CadastrarAnexo(CadastrarAnexoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var anexo = new Anexo(cadastroEntrada);

            // TODO: Fazer upload do arquivo para o Google Drive

            await _anexoRepositorio.Inserir(anexo);

            await _uow.Commit();

            return new Saida(true, new[] { AnexoMensagem.Anexo_Cadastrado_Com_Sucesso }, new AnexoSaida(anexo));
        }

        public Task<ISaida> ExcluirAnexo(int idLancamento, int idUsuario)
        {
            throw new System.NotImplementedException();
        }
    }
}
