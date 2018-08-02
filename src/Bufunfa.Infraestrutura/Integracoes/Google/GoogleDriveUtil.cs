using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.IO;
using GoogleApiV3Data = Google.Apis.Drive.v3.Data;

namespace JNogueira.Bufunfa.Infraestrutura.Integracoes.Google
{
    public class GoogleDriveUtil : Notificavel
    {
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
                this.AdicionarNotificacao("Não foi possível realizar a integração com a API do Google Drive: " + ex.GetBaseException().Message);
            }
        }

        public List<string> ListarPastas()
        {
            FilesResource.ListRequest list = _driveService.Files.List();
            list.PageSize = 50;
            list.Q = "mimeType = 'application/vnd.google-apps.folder' and trashed = false and name != 'Bufunfa - Anexos'";

            GoogleApiV3Data.FileList filesFeed = list.Execute();

            var pastas = new List<string>();

            while (filesFeed.Files != null)
            {
                // Adding each item to the list.
                foreach (GoogleApiV3Data.File item in filesFeed.Files)
                {
                    pastas.Add(item.Name);
                }

                // We will know we are on the last page when the next page token is
                // null.
                // If this is the case, break.
                if (filesFeed.NextPageToken == null)
                {
                    break;
                }

                // Prepare the next page of results
                list.PageToken = filesFeed.NextPageToken;

                // Execute and process the next page request
                filesFeed = list.Execute();
            }

            return pastas;
        }
    }
}
