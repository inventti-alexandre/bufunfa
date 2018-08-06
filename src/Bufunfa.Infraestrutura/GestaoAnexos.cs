using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Infraestrutura;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.Google;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Infraestrutura
{
    /// <summary>
    /// Classe responsável pela gestão dos anexos dos lançamentos
    /// </summary>
    public class GestaoAnexos : Notificavel, IGestaoAnexos
    {
        /// <summary>
        /// Pasta raiz no Google Drive onde os anexos serão armazenados
        /// </summary>
        private const string NOME_PASTA_GOOGLE_DRIVE = "Bufunfa - Anexos";

        private readonly GoogleDriveUtil _googleDriveUtil;

        public GestaoAnexos()
        {
            _googleDriveUtil = new GoogleDriveUtil();
        }

        /// <summary>
        /// Realiza o upload de um arquivo
        /// </summary>
        public async Task RealizarUploadAnexo(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada)
        {
            if (_googleDriveUtil.Invalido)
            {
                this.AdicionarNotificacoes(_googleDriveUtil.Notificacoes);
                return;
            }

            /*
            
            // Pasta raiz no Google Drive
            var pastaRaiz = await _googleDriveUtil.ProcurarPorNome(GoogleDriveUtil.TipoGoogleDriveFile.Pasta, NOME_PASTA_GOOGLE_DRIVE);

            // Pasta referente ao ano do lançamento
            var pastaAno = await _googleDriveUtil.CriarPasta(dataLancamento.Year.ToString(), pastaRaiz);

            // Pasta referente ao mês do lançamento
            var pastaMes = await _googleDriveUtil.CriarPasta(dataLancamento.Month.ToString(), pastaAno);

            // Verifica se um arquivo com o mesmo nome já existe na pasta do mês do lançamento
            var anexoJaExistente = await _googleDriveUtil.ProcurarPorNome(GoogleDriveUtil.TipoGoogleDriveFile.Arquivo, cadastroEntrada.NomeArquivo, pastaMes);

            this.NotificarSeNaoNulo(anexoJaExistente, AnexoMensagem.Nome_Arquivo_Ja_Existe_Google_Drive);

            if (this.Invalido)
                return;

            // Realiza o upload do arquivo
            var anexo = await _googleDriveUtil.RealizarUpload(cadastroEntrada.NomeArquivo, cadastroEntrada.MimeTypeArquivo, cadastroEntrada.ConteudoArquivo, cadastroEntrada.Descricao, pastaMes);

            */

            await _googleDriveUtil.ExcluirPorNome(GoogleDriveUtil.TipoGoogleDriveFile.Pasta, dataLancamento.Year.ToString());
        }

        /// <summary>
        /// Obtém as notificações encontradas
        /// </summary>
        public IReadOnlyCollection<Notificacao> ObterNotificacoes() => this.Notificacoes;
    }
}
