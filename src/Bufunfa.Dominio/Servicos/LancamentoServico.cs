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
            this.NotificarSeMenorOuIgualA(idLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Nao_Pertence_Usuario);

            return (this.Invalido)
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return (procurarEntrada.Invalido)
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _lancamentoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarLancamento(CadastrarLancamentoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var lancamento = new Lancamento(cadastroEntrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            if (_uow.Invalido)
                return new Saida(false, _uow.Mensagens, null);

            lancamento = await _lancamentoRepositorio.ObterPorId(lancamento.Id);

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> AlterarLancamento(AlterarLancamentoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
                return new Saida(false, alterarEntrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(alterarEntrada.IdLancamento, true);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, alterarEntrada.IdUsuario, LancamentoMensagem.Lancamento_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            lancamento.Alterar(alterarEntrada);

            _lancamentoRepositorio.Atualizar(lancamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Alterado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ExcluirLancamento(int idLancamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            foreach(var anexo in lancamento.Anexos)
            {
                // Exclui os anexos do banco de dados e os arquivos do Google Drive
                await _anexoRepositorio.Deletar(anexo);
            }

            _lancamentoRepositorio.Deletar(lancamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Excluido_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> CadastrarAnexo(CadastrarAnexoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(cadastroEntrada.IdLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Insere as informações do anexo no banco de dados e realiza o upload do arquivo para o Google Drive
            var anexo = await _anexoRepositorio.Inserir(lancamento.Data, cadastroEntrada);

            if (_anexoRepositorio.Invalido)
            {
                this.AdicionarNotificacoes(_anexoRepositorio.Notificacoes);

                return new Saida(false, this.Mensagens, null);
            }

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AnexoMensagem.Anexo_Cadastrado_Com_Sucesso }, new AnexoSaida(anexo));
        }

        public async Task<ISaida> ExcluirAnexo(int idAnexo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAnexo, 0, AnexoMensagem.Id_Anexo_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var anexo = await _anexoRepositorio.ObterPorId(idAnexo);

            // Verifica se o anexo existe
            this.NotificarSeNulo(anexo, AnexoMensagem.Id_Anexo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o anexo pertece a um lançamento ao usuário informado.
            this.NotificarSeDiferentes(anexo.Lancamento.IdUsuario, idUsuario, AnexoMensagem.Anexo_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui o anexo do banco de dados e também o arquivo do Google Drive.
            await _anexoRepositorio.Deletar(anexo);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AnexoMensagem.Anexo_Excluido_Com_Sucesso }, new AnexoSaida(anexo));
        }
    }
}
