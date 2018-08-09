using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.Google;
using JNogueira.Infraestrutura.NotifiqueMe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class AnexoRepositorio : Notificavel, IAnexoRepositorio
    {
        private readonly EfDataContext _efContext;
        private readonly GoogleDriveUtil _googleDriveUtil;

        /// <summary>
        /// ID da pasta raiz no Google Drive onde os anexos serão armazenados ("Bufunfa - Anexos")
        /// </summary>
        private const string ID_PASTA_GOOGLE_DRIVE = "1SIlmDfZBepgzZ4qOEdIwV9Rrmc8wBGwS";

        public AnexoRepositorio(EfDataContext efContext)
        {
            _googleDriveUtil = new GoogleDriveUtil();
            _efContext = efContext;
        }

        public async Task<Anexo> ObterPorId(int idAnexo, bool habilitarTracking = false)
        {
            var query = _efContext.Anexos
                .Include(x => x.Lancamento)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idAnexo);
        }

        public async Task<Anexo> Inserir(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada)
        {
            // Realiza o upload do arquivo do anexo para o Google Drive
            var idGoogleDrive = await RealizarUploadAnexo(dataLancamento, cadastroEntrada);

            if (this.Invalido)
                return null;

            var anexo = new Anexo(cadastroEntrada, idGoogleDrive);

            await _efContext.AddAsync(anexo);

            return anexo;
        }

        public async Task Deletar(Anexo anexo)
        {
            if (_googleDriveUtil.Invalido)
            {
                this.AdicionarNotificacoes(_googleDriveUtil.Notificacoes);
                return;
            }

            // Exclui o arquivo do anexo do Google Drive
            await _googleDriveUtil.ExcluirPorId(anexo.IdGoogleDrive);

            _efContext.Anexos.Remove(anexo);
        }

        /// <summary>
        /// Realiza o upload de um arquivo
        /// </summary>
        private async Task<string> RealizarUploadAnexo(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada)
        {
            if (_googleDriveUtil.Invalido)
            {
                this.AdicionarNotificacoes(_googleDriveUtil.Notificacoes);
                return null;
            }

            // Pasta referente ao ano do lançamento
            var pastaAno = await _googleDriveUtil.CriarPasta(dataLancamento.Year.ToString(), ID_PASTA_GOOGLE_DRIVE);

            // Pasta referente ao mês do lançamento
            var pastaMes = await _googleDriveUtil.CriarPasta(dataLancamento.Month.ToString(), pastaAno.Id);

            // Verifica se um arquivo com o mesmo nome já existe na pasta do mês do lançamento
            var anexoJaExistente = await _googleDriveUtil.ProcurarPorNome(GoogleDriveUtil.TipoGoogleDriveFile.Arquivo, cadastroEntrada.NomeArquivo, pastaMes.Id);

            this.NotificarSeNaoNulo(anexoJaExistente, AnexoMensagem.Nome_Arquivo_Ja_Existe_Google_Drive);

            if (this.Invalido)
                return null;

            // Realiza o upload do arquivo
            return await _googleDriveUtil.RealizarUpload(cadastroEntrada.NomeArquivo, cadastroEntrada.MimeTypeArquivo, cadastroEntrada.ConteudoArquivo, cadastroEntrada.Descricao, pastaMes.Id);

            //await _googleDriveUtil.ExcluirPorNome(GoogleDriveUtil.TipoGoogleDriveFile.Pasta, dataLancamento.Year.ToString());
        }
    }
}