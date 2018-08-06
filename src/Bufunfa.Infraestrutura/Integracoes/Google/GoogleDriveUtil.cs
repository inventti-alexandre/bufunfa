using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleApiV3Data = Google.Apis.Drive.v3.Data;

namespace JNogueira.Bufunfa.Infraestrutura.Integracoes.Google
{
    /// <summary>
    /// Classe que permite a integração com o Google Drive
    /// </summary>
    public class GoogleDriveUtil : Notificavel
    {
        public enum TipoGoogleDriveFile
        {
            Arquivo,
            Pasta
        }

        private readonly DriveService _driveService;

        public GoogleDriveUtil()
        {
            try
            {
                var googleCredentialsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "google_credentials.json");

                this.NotificarSeFalso(File.Exists(googleCredentialsFilePath), "O arquivo \"google_credentials.json\" contendo as credenciais de acesso para utilização do Google Service Account não foi encontrado.");

                if (this.Invalido)
                    return;

                string[] scopes = new string[] { DriveService.Scope.Drive };

                GoogleCredential credential;

                using (var stream = new FileStream(googleCredentialsFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential
                                        .FromStream(stream)
                                        .CreateScoped(scopes);
                }

                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
            }
            catch (Exception ex)
            {
                this.AdicionarNotificacao("Não é possível realizar a integração com a API do Google Drive: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Realiza a procura de um item pelo nome
        /// </summary>
        public async Task<GoogleApiV3Data.File> ProcurarPorNome(TipoGoogleDriveFile tipo, string nome, GoogleApiV3Data.File pastaProcura = null)
        {
            FilesResource.ListRequest list = _driveService.Files.List();
            list.Fields = "files(id, name, trashed, parents)";
            list.PageSize = 5;
            list.Q = $"name = '{nome}' and trashed = false";

            if (tipo == TipoGoogleDriveFile.Pasta)
                list.Q += " and mimeType = 'application/vnd.google-apps.folder'";

            if (pastaProcura != null)
                list.Q += " and '" + pastaProcura.Id + "' in parents";

            var filesFeed = await list.ExecuteAsync();

            while (filesFeed.Files != null)
            {
                var encontrado = filesFeed.Files.FirstOrDefault(x => string.Equals(x.Name, nome, StringComparison.InvariantCultureIgnoreCase));

                if (encontrado != null)
                    return encontrado;

                if (filesFeed.NextPageToken == null)
                    break;

                list.PageToken = filesFeed.NextPageToken;

                filesFeed = await list.ExecuteAsync();
            }

            return null;
        }

        /// <summary>
        /// Cria uma nova pasta
        /// </summary>
        public async Task<GoogleApiV3Data.File> CriarPasta(string nome, GoogleApiV3Data.File pastaPai = null)
        {
            var pasta = await ProcurarPorNome(TipoGoogleDriveFile.Pasta, nome, pastaPai);

            if (pasta != null)
                return pasta;

            var fileMetadata = new GoogleApiV3Data.File()
            {
                Name = nome,
                MimeType = "application/vnd.google-apps.folder",
                Parents = pastaPai != null ? new List<string>() { pastaPai.Id } : null
            };

            var request = _driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            return await request.ExecuteAsync();
        }

        /// <summary>
        /// Realiza o upload de um arquivo
        /// </summary>
        public async Task<GoogleApiV3Data.File> RealizarUpload(string nomeArquivo, string mimeType, byte[] conteudoArquivo, string descricaoArquivo = null, GoogleApiV3Data.File pastaPai = null)
        {
            var fileMetadata = new GoogleApiV3Data.File
            {
                Name = nomeArquivo,
                Description = descricaoArquivo,
                MimeType = mimeType,
                Parents = pastaPai != null ? new List<string>() { pastaPai.Id } : null
            };

            var stream = new MemoryStream(conteudoArquivo);

            FilesResource.CreateMediaUpload request = _driveService.Files.Create(fileMetadata, stream, mimeType);
            await request.UploadAsync();
            return request.ResponseBody;
        }

        /// <summary>
        /// Realiza a exclusão de um item a partir do seu nome.
        /// </summary>
        public async Task ExcluirPorNome(TipoGoogleDriveFile tipo, string nome, GoogleApiV3Data.File pastaPai = null)
        {
            var file = await ProcurarPorNome(tipo, nome, pastaPai);

            if (file == null)
                return;

            FilesResource.DeleteRequest deleteRequest = _driveService.Files.Delete(file.Id);
            await deleteRequest.ExecuteAsync();
        }
    }
}
