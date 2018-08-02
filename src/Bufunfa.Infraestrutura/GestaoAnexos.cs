using JNogueira.Bufunfa.Dominio.Interfaces.Infraestrutura;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.Google;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Infraestrutura
{
    public class GestaoAnexos : Notificavel, IGestaoAnexos
    {
        public string[] ListarPastas()
        {
            var driveUtil = new GoogleDriveUtil();

            return driveUtil.ListarPastas().ToArray();
            

            //var clientSecrets = new ClientSecrets
            //{
            //    ClientId = "374672954928-4r7av98u74fnhmk5hjf1ecn21nv415ev.apps.googleusercontent.com",
            //    ClientSecret = "JAMvyOGDin10d7OI1-QPuxGj"
            //};

            //var scopes = new[] { DriveService.Scope.Drive };

            //var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None).Result;

            //var services = new DriveService(new BaseClientService.Initializer()
            //{
            //    ApiKey = "AIzaSyC_aDbnrLkBOmVHOQM74VtM7hXOV7qbTj4",  // from https://console.developers.google.com (Public API access)
            //    ApplicationName = "Drive API Sample",
            //});

            //var fileMetadata = new File()
            //{
            //    MimeType = "application/vnd.google-apps.folder",
            //    Trashed = false
            //};

            //var request = services.Files.List();
            //request.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false";
            
            
            //var folders = request.Execute();

            //return folders.Files.Select(x => x.Name).ToArray();
        }
    }
}
