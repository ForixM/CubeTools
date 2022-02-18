using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    internal class ManagerCLI
    {
        #region Variables

        // Attributes
        private static DirectoryType _directoryType;
        private static string _promptLine;

        #endregion

        #region Init

        // Constructor : OK
        public ManagerCLI()
        {
            _directoryType = new DirectoryType();
            _promptLine = ">> ";
        }

        public ManagerCLI(string path)
        {
            _directoryType = new DirectoryType(path);
            _promptLine = ">> ";
        }

        #endregion

        #region Process

        // This function deals with the process of the command line
        public void Process()
        {
            Console.WriteLine("CubeTools command line project");
            Console.WriteLine("Type help to get any information about the available commands");
            Console.Write("\n");
            using var watcher = new FileSystemWatcher(_directoryType.Path);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;
            /*
            watcher.Changed += _directoryType.ActualizeFiles();
            watcher.Created += _directoryType.ActualizeFiles();
            watcher.Deleted += _directoryType.ActualizeFiles();
            */
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            while (true)
            {
                Console.Write(_directoryType.Path);
                Console.Write(_promptLine + " ");
                ReadCommand();
            }
        }

        // This function ends the process of the app
        public void End()
        {
            Console.WriteLine("Process finished ...");
            Environment.Exit(0);
        }

        #endregion

        #region Methods

        // This function modifies the current directory
        public void ChangeDirectory(string path)
        {
            if (ManagerReader.IsDirectory(path))
            {
                _directoryType.ChangeDirectory(path);
            }
        }

        // This function reads the command that is ask
        public void ReadCommand()
        {
            var command = Console.ReadLine();
            if (command != null)
            {
                string[] read = command.Split(' ');
                if (command != null && read.Length <= 3)
                {
                    switch (read[0])
                    {
                        case "ls":
                            _directoryType.DisplayChildren();
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                        case "cat":
                            if (read.Count() == 2)
                                ManagerReader.ReadContent(read[1]);
                            else
                                Console.WriteLine("Cat takes 2 arguments");
                            break;
                        case "rm":
                            if (read.Count() == 2)
                                ManagerWriter.Delete(read[1]);
                            else
                                Console.WriteLine("rm takes 2 arguments");
                            break;
                        case "mkdir":
                            if (read.Count() == 2)
                                ManagerWriter.CreateDir(read[1]);
                            else
                                Console.WriteLine("rm takes 2 arguments");
                            break;
                        case "rmdir":
                            if (read.Count() == 2)
                                ManagerWriter.DeleteDir(read[1]);
                            else
                                Console.WriteLine("rmdir takes 2 arguments");
                            break;
                        case "touch":
                            if (read.Count() == 2)
                                ManagerWriter.Create(read[1]);
                            else
                                Console.WriteLine("touch takes 2 arguments");
                            break;
                        case "mv":
                            if (read.Count() == 3)
                                ManagerWriter.Rename(read[1], read[2]);
                            else
                                Console.WriteLine("touch takes 2 arguments");
                            break;
                        case "cd":
                            if (read.Count() == 2 && System.IO.Directory.Exists(read[1]))
                            {
                                _directoryType.ChangeDirectory(read[1]);
                            }
                            break;
                        case "pwd":
                            Console.WriteLine();
                            Console.WriteLine("Path");
                            Console.WriteLine("-----");
                            Console.WriteLine(_directoryType.Path);
                            Console.WriteLine();
                            break;
                        case "refresh":
                            _directoryType.SetChildrenFiles();
                            break;
                        case "exit":
                            End();
                            break;
                        case "help":
                            Help();
                            break;
                        case "debug":
                            _directoryType.PrintInformation();
                            break;
                        default:
                            Console.WriteLine("The given command is unknown ...");
                            Console.WriteLine("Please, enter help to display the available commands");
                            Console.WriteLine("");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("The given command is unknown ...");
                    Console.WriteLine("Please, enter help to display the available commands");
                    Console.WriteLine("");
                }
            }
        }

        // Help command
        public void Help()
        {
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine(">> Help commands : Here are the available commands <<");
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("help : open this help prompt");
            Console.WriteLine("ls : display all files in your directory");
            Console.WriteLine("clear : clear the console");
            Console.WriteLine("cat : display the content of a given file given in parameter");
            Console.WriteLine("mkdir : create a directory with the specified name");
            Console.WriteLine("rmdir : delete a given directory given in the parameter");
            Console.WriteLine("mv : rename or move a file to a new position using a 2 paths");
            Console.WriteLine("rm : delete the specified file");
            Console.WriteLine("touch : create an empty file with a given name");
            Console.WriteLine("cd : change the directory given with the second parameter");
            Console.WriteLine("pwd : display the current directory");
            Console.WriteLine("exit : leave the console");
            Console.WriteLine("_____________________________________________________");
        }

        #endregion
    }
}