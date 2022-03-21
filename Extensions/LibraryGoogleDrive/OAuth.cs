using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace LibraryGoogleDrive;

public static class OAuth
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/drive-dotnet-quickstart.json

    private static readonly string[] Scopes = {DriveService.Scope.Drive};
    private static readonly string ApplicationName = "Drive API .NET Quickstart";

    // Create Crendential 
    public static UserCredential GetUserCredential()
    {
        using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            var credPath = "token.json";
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
        }
    }

    // Create Drive API service.
    public static DriveService GetDriveService()
    {
        UserCredential credential = GetUserCredential();

        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }
}