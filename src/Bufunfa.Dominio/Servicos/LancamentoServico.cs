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
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IAnexoRepositorio _anexoRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IUow _uow;

        public LancamentoServico(
            ILancamentoRepositorio lancamentoRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IContaRepositorio contaRepositorio,
            IAnexoRepositorio anexoRepositorio,
            IPessoaRepositorio pessoaRepositorio,
            IParcelaRepositorio parcelaRepositorio,
            IUow uow)
        {
            _lancamentoRepositorio = lancamentoRepositorio;
            _categoriaRepositorio  = categoriaRepositorio;
            _contaRepositorio      = contaRepositorio;
            _anexoRepositorio      = anexoRepositorio;
            _pessoaRepositorio     = pessoaRepositorio;
            _parcelaRepositorio    = parcelaRepositorio;
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

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return procurarEntrada.Invalido
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _lancamentoRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarLancamento(CadastrarLancamentoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta existe a partir do ID informado.
            this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdConta), ContaMensagem.Id_Conta_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (cadastroEntrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            // Verifica se a parcela existe a partir do ID informado.
            if (cadastroEntrada.IdParcela.HasValue)
                this.NotificarSeFalso(await _parcelaRepositorio.VerificarExistenciaPorId(cadastroEntrada.IdUsuario, cadastroEntrada.IdParcela.Value), ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = new Lancamento(cadastroEntrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso }, new LancamentoSaida(await _lancamentoRepositorio.ObterPorId(lancamento.Id)));
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

            // Verifica se a categoria existe a partir do ID informado.
            if (lancamento.IdCategoria != alterarEntrada.IdCategoria)
                this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta existe a partir do ID informado.
            if (lancamento.IdConta != alterarEntrada.IdConta)
                this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdConta), ContaMensagem.Id_Conta_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (lancamento.IdPessoa != alterarEntrada.IdPessoa && alterarEntrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(alterarEntrada.IdUsuario, alterarEntrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

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
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(cadastroEntrada.IdLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Insere as informações do anexo no banco de dados e realiza o upload do arquivo para o Google Drive
            var anexo = await _anexoRepositorio.Inserir(lancamento.Data, cadastroEntrada);

            if (_anexoRepositorio.Invalido)
                return new Saida(false, _anexoRepositorio.Mensagens, null);

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

            if (_anexoRepositorio.Invalido)
                return new Saida(false, _anexoRepositorio.Mensagens, null);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { AnexoMensagem.Anexo_Excluido_Com_Sucesso }, new AnexoSaida(anexo));
        }
    }
}
