﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentFTP;
using Manager;

namespace Extensions
{
    public class FtpUtils
    {
        private FtpClient client;

        public FtpUtils(string host, string userId, string password)
        {
            client = new FtpClient(host, 21, userId, password);
            client.Connect();
        }

        public bool MoveDir(string dir)
        {
            string newdir = client.GetWorkingDirectory() + dir + "/";
            if (client.DirectoryExists(newdir))
            {
                client.SetWorkingDirectory(newdir);
                return true;
            }

            return false;
        }

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

        public Task<FtpStatus> DownloadFile(string remotePath, FileType destination)
        {
            return client.DownloadFileAsync(destination.Path, remotePath);
        }

        public Task<FtpStatus> UploadFile(string remoteDestination, FileType source)
        {
            return client.UploadFileAsync(source.Path, remoteDestination);
        }
    }
}