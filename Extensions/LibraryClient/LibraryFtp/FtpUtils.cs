//System
using System;
using System.Threading.Tasks;
using System.Net.Security;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
//Libraries
using Library;
// External Libraries
using FluentFTP;

namespace LibraryClient.LibraryFtp
{
    public class FtpUtils
    {
        /// <summary>
        ///     Connects to the remote server. FTPs isn't fully implemented, so please set ssl bool to false
        /// </summary>
        /// <param name="host">IP of the remote server</param>
        /// <param name="userId">username registered at the remote server</param>
        /// <param name="password">Password associated to the previous given userID</param>
        /// <param name="ssl">Create a secure connection to the remote server (not fully functional)</param>
        public FtpUtils(string host, string userId, string password, bool ssl)
        {
            Client = new FtpClient(host, 21, userId, password);
            if (ssl)
            {
                Client.EncryptionMode = FtpEncryptionMode.Explicit;
                Client.DataConnectionType = FtpDataConnectionType.AutoPassive;
                Client.DataConnectionEncryption = true;
                Client.SslProtocols = SslProtocols.Tls13;
                Client.ValidateCertificate += OnValidateCertificate;
                var cer = new X509Certificate2();
                Client.ClientCertificates.Add(cer);
            }

            Client.Connect();
        }

        public FtpClient Client { get; }

        private void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            e.Accept = true;
        }

        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }

        /// <summary>
        ///     Move the current directory to the directory given in the parameter
        /// </summary>
        /// <param name="dir">The next directory to go in</param>
        /// <returns>Whether the action was done or not</returns>
        public bool Cd(string dir)
        {
            var newdir = Client.GetWorkingDirectory() + dir + "/";
            if (Client.DirectoryExists(newdir))
            {
                Client.SetWorkingDirectory(newdir);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Create a new directory in the given path
        /// </summary>
        /// <param name="path">The path where the directory will be created</param>
        /// <returns>Whether the action was done or not</returns>
        public bool MkDir(string path)
        {
            return Client.CreateDirectory(path);
        }

        /// <summary>
        ///     Deletes the given file from the remote server
        /// </summary>
        /// <param name="path">The path of the file that will be deleted</param>
        /// <returns>The async Task of the file deletion</returns>
        public Task DeleteFile(string path)
        {
            return Client.DeleteFileAsync(path);
        }

        /// <summary>
        ///     Deletes the given directory and all his content from the remote server
        /// </summary>
        /// <param name="path">The path of the directory that will be deleted</param>
        /// <returns>The async Task of the directory deletion</returns>
        public Task DeleteDirectory(string path)
        {
            return Client.DeleteDirectoryAsync(path);
        }

        /// <summary>
        ///     Fetch the files contained in the current directory
        /// </summary>
        /// <returns>A list of files found</returns>
        public List<string> GetFiles()
        {
            FtpListItem[] list = Client.GetListing();
            var files = new List<string>();
            foreach (var item in list)
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    Console.WriteLine(item.FullName);
                    files.Add(item.FullName);
                }

            return files;
        }

        /// <summary>
        ///     Fetch the folders contained in the current directory
        /// </summary>
        /// <returns>A list of folders found</returns>
        public List<string> GetDirectories()
        {
            FtpListItem[] list = Client.GetListing();
            var directories = new List<string>();
            foreach (var item in list)
                if (item.Type == FtpFileSystemObjectType.Directory)
                    directories.Add(item.Name);

            return directories;
        }

        /// <summary>
        ///     Moves a file asynchronously on the remote file system from one directory to another.
        /// </summary>
        /// <param name="path">The full or relative path to the object</param>
        /// <param name="dest">The new full or relative path including the new name of the object</param>
        /// <param name="overwrite">Overwrite or not the file at destination if it's already exists</param>
        /// <returns>The async Task of the action</returns>
        public Task MoveFile(string path, string dest, bool overwrite)
        {
            return Client.MoveFileAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
        }

        /// <summary>
        ///     Moves a directory asynchronously on the remote file system from one directory to another.
        /// </summary>
        /// <param name="path">The full or relative path to the object</param>
        /// <param name="dest">The new full or relative path including the new name of the object</param>
        /// <param name="overwrite">Overwrite or not the file at destination if it's already exists</param>
        /// <returns>The async Task of the action</returns>
        public Task MoveDirectory(string path, string dest, bool overwrite)
        {
            return Client.MoveDirectoryAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
        }

        /// <summary>
        ///     Download a file from the remote server
        /// </summary>
        /// <param name="remotePath">The path of the remote file</param>
        /// <param name="destination">The local destination where the file is downloaded</param>
        /// <param name="progressionEvent">Optional function parameter to track download progression</param>
        /// <returns>The async task of the download</returns>
        public Task<FtpStatus> DownloadFile(string remotePath, string destination,
            Action<object, FtpProgress> progressionEvent = null)
        {
            Progress<FtpProgress> progress = null;
            if (progressionEvent != null)
            {
                progress = new Progress<FtpProgress>();
                progress.ProgressChanged += new EventHandler<FtpProgress>(progressionEvent);
            }

            return Client.DownloadFileAsync(destination, remotePath, FtpLocalExists.Resume, FtpVerify.None,
                progress);
        }

        /// <summary>
        ///     Upload a file to the remote server
        /// </summary>
        /// <param name="remoteDestination">The path of the remote destination</param>
        /// <param name="source">The local file that will be uploaded</param>
        /// <param name="progressionEvent">Optional function parameter to track upload progression</param>
        /// <returns>The async task of the upload</returns>
        public Task<FtpStatus> UploadFile(string remoteDestination, Pointer source,
            Action<object, FtpProgress> progressionEvent = null)
        {
            Progress<FtpProgress> progress = null;
            if (progressionEvent != null)
            {
                progress = new Progress<FtpProgress>();
                progress.ProgressChanged += new EventHandler<FtpProgress>(progressionEvent);
            }

            return Client.UploadFileAsync(source.Path, remoteDestination, FtpRemoteExists.Overwrite, false,
                FtpVerify.None, progress);
        }

        /// <summary>
        ///     Upload a file to the remote server
        /// </summary>
        /// <param name="remotePath">The path of the file in remote server</param>
        /// <param name="newName">The new name of the previously given path</param>
        /// <returns>Whether the action was done or not</returns>
        public bool Rename(string remotePath, string newName)
        {
            var newPath = remotePath;
            while (newPath[newPath.Length - 1] is not '/' or '\\') newPath = newPath.Remove(newPath.Length - 1);

            newPath += newName;
            try
            {
                Client.Rename(remotePath, newPath);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }
    }
}