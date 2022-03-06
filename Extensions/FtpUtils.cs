using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using FluentFTP;
using Manager;

namespace Extensions
{
    public class FtpUtils
    {
        private FtpClient client;

        public FtpClient Client => client;

        /// <summary>
        /// Connects to the remote server. FTPs isn't fully implemented, so please set ssl bool to false
        /// </summary>
        /// <param name="host">IP of the remote server</param>
        /// <param name="userID">username registered at the remote server</param>
        /// <param name="password">Password associated to the previous given userID</param>
        /// <param name="ssl">Create a secure connection to the remote server (not fully functional)</param>
        public FtpUtils(string host, string userId, string password, bool ssl)
        {
            client = new FtpClient(host, 21, userId, password);
            if (ssl)
            {
                client.EncryptionMode = FtpEncryptionMode.Explicit;
                client.DataConnectionType = FtpDataConnectionType.AutoPassive;
                client.DataConnectionEncryption = true;
                client.SslProtocols = SslProtocols.Tls13;
                client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
                var cer = new X509Certificate2();
                client.ClientCertificates.Add(cer);
            }
            client.Connect();
        }

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
            client.Disconnect();
        }

        /// <summary>
        /// Move the current directory to the directory given in the parameter
        /// </summary>
        /// <param name="dir">The next directory to go in</param>
        /// <returns>Whether the action was done or not</returns>
        public bool Cd(string dir)
        {
            string newdir = client.GetWorkingDirectory() + dir + "/";
            if (client.DirectoryExists(newdir))
            {
                client.SetWorkingDirectory(newdir);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create a new directory in the given path
        /// </summary>
        /// <param name="path">The path where the directory will be created</param>
        /// <returns>Whether the action was done or not</returns>
        public bool MkDir(string path)
        {
            return client.CreateDirectory(path);
        }
        
        /// <summary>
        /// Deletes the given file from the remote server
        /// </summary>
        /// <param name="path">The path of the file that will be deleted</param>
        /// <returns>The async Task of the file deletion</returns>
        public Task DeleteFile(string path)
        {
            return client.DeleteFileAsync(path);
        }

        /// <summary>
        /// Deletes the given directory and all his content from the remote server
        /// </summary>
        /// <param name="dir">The path of the directory that will be deleted</param>
        /// <returns>The async Task of the directory deletion</returns>
        public Task DeleteDirectory(string path)
        {
            return client.DeleteDirectoryAsync(path);
        }

        /// <summary>
        /// Fetch the files contained in the current directory
        /// </summary>
        /// <returns>A list of files found</returns>
        public List<string> GetFiles()
        {
            FtpListItem[] list = client.GetListing();
            List<string> files = new List<string>();
            foreach (FtpListItem item in list)
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    Console.WriteLine(item.FullName);
                    files.Add(item.FullName);
                }
            }

            return files;
        }

        /// <summary>
        /// Fetch the folders contained in the current directory
        /// </summary>
        /// <returns>A list of folders found</returns>
        public List<string> GetDirectories()
        {
            FtpListItem[] list = client.GetListing();
            List<string> directories = new List<string>();
            foreach (FtpListItem item in list)
            {
                if (item.Type == FtpFileSystemObjectType.Directory)
                {
                    directories.Add(item.Name);
                }
            }

            return directories;
        }

        /// <summary>
        /// Moves a file asynchronously on the remote file system from one directory to another.
        /// </summary>
        /// <param name="path">The full or relative path to the object</param>
        /// <param name="dest">The new full or relative path including the new name of the object</param>
        /// <param name="overwrite">Overwrite or not the file at destination if it's already exists</param>
        /// <returns>The async Task of the action</returns>
        public Task MoveFile(string path, string dest, bool overwrite)
        {
            return client.MoveFileAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
        }

        /// <summary>
        /// Moves a directory asynchronously on the remote file system from one directory to another.
        /// </summary>
        /// <param name="path">The full or relative path to the object</param>
        /// <param name="dest">The new full or relative path including the new name of the object</param>
        /// <param name="overwrite">Overwrite or not the file at destination if it's already exists</param>
        /// <returns>The async Task of the action</returns>
        public Task MoveDirectory(string path, string dest, bool overwrite)
        {
            return client.MoveDirectoryAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
        }

        /// <summary>
        /// Download a file from the remote server
        /// </summary>
        /// <param name="remotePath">The path of the remote file</param>
        /// <param name="destination">The local destination where the file is downloaded</param>
        /// <param name="progressionEvent">Optional function parameter to track download progression</param>
        /// <returns>The async task of the download</returns>
        public Task<FtpStatus> DownloadFile(string remotePath, FileType destination, Action<object, FtpProgress> progressionEvent = null)
        {
            Progress<FtpProgress> progress = null;
            if (progressionEvent != null)
            {
                progress = new Progress<FtpProgress>();
                progress.ProgressChanged += new EventHandler<FtpProgress>(progressionEvent);
            }
            return client.DownloadFileAsync(destination.Path, remotePath, FtpLocalExists.Resume, FtpVerify.None,
                progress);
        }

        /// <summary>
        /// Upload a file to the remote server
        /// </summary>
        /// <param name="remoteDestination">The path of the remote destination</param>
        /// <param name="destination">The local file that will be uploaded</param>
        /// <param name="progressionEvent">Optional function parameter to track upload progression</param>
        /// <returns>The async task of the upload</returns>
        public Task<FtpStatus> UploadFile(string remoteDestination, FileType source, Action<object, FtpProgress> progressionEvent = null)
        {
            Progress<FtpProgress> progress = null;
            if (progressionEvent != null)
            {
                progress = new Progress<FtpProgress>();
                progress.ProgressChanged += new EventHandler<FtpProgress>(progressionEvent);
            }
            return client.UploadFileAsync(source.Path, remoteDestination, FtpRemoteExists.Overwrite, false,
                FtpVerify.None, progress);
        }
        
        /// <summary>
        /// Upload a file to the remote server
        /// </summary>
        /// <param name="remotePath">The path of the file in remote server</param>
        /// <param name="newName">The new name of the previously given path</param>
        /// <returns>Whether the action was done or not</returns>
        public bool Rename(string remotePath, string newName)
        {
            string newPath = remotePath;
            while (newPath[newPath.Length - 1] is not '/' or '\\')
            {
                newPath = newPath.Remove(newPath.Length - 1);
            }

            newPath += newName;
            try
            {
                client.Rename(remotePath, newPath);
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