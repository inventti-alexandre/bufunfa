using System;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Infraestrutura;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.Google;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Infraestrutura
{
    public class GestaoAnexos : Notificavel, IGestaoAnexos
    {
        private const string NOME_PASTA_GOOGLE_DRIVE = "Bufunfa - Anexos";

        private readonly GoogleDriveUtil _googleDriveUtil;

        public GestaoAnexos()
        {
            _googleDriveUtil = new GoogleDriveUtil();
        }

        public void RealizarUploadAnexo(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada)
        {
            if (_googleDriveUtil.Invalido)
            {
                this.AdicionarNotificacoes(_googleDriveUtil.Notificacoes);
                return;
            }

            var pastaBufunfa = _googleDriveUtil.ObterPastaPorNome(NOME_PASTA_GOOGLE_DRIVE);

            var pastaAno = _googleDriveUtil.CriarPasta(dataLancamento.Year.ToString(), pastaBufunfa);

            var pastaMes = _googleDriveUtil.CriarPasta(dataLancamento.Month.ToString(), pastaAno);

            var anexoJaExistente = _googleDriveUtil.ObterArquivoPorNome(cadastroEntrada.NomeArquivo, pastaMes);

            this.NotificarSeNaoNulo(anexoJaExistente, "Arquivo já existe.");

            if (this.Invalido)
                return;

            var anexo = _googleDriveUtil.RealizarUpload(cadastroEntrada.NomeArquivo, cadastroEntrada.MimeTypeArquivo, cadastroEntrada.ConteudoArquivo, cadastroEntrada.Descricao, pastaMes);

            //_googleDriveUtil.ExcluirPastaPorNome(dataLancamento.Year.ToString());
            //_googleDriveUtil.ExcluirPastaPorNome(dataLancamento.Month.ToString());
        }
    }
}
