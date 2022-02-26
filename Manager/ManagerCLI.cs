﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Manager.ManagerExceptions;

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

        // Constructor : OK
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
            Console.WriteLine(@"
            ░▒█▀▀▄░█░▒█░█▀▀▄░█▀▀░░░▀▀█▀▀░▄▀▀▄░▄▀▀▄░█░░█▀▀
            ░▒█░░░░█░▒█░█▀▀▄░█▀▀░░░░▒█░░░█░░█░█░░█░█░░▀▀▄
            ░▒█▄▄▀░░▀▀▀░▀▀▀▀░▀▀▀░░░░▒█░░░░▀▀░░░▀▀░░▀▀░▀▀▀
            ");
            Console.WriteLine("CubeTools command line project");
            Console.WriteLine("Type help to get any information about the available commands");
            Console.Write("\n");
            
            while (true)
            {
                Console.Write(_directoryType.Path);
                Console.Write(_promptLine + " ");
                ReadCommand();
            }
        }

        // This function ends the process of the app
        public static void End()
        {
            Console.WriteLine("Process finished ...");
            Environment.Exit(0);
        }

        #endregion

        #region Reader

        private void InterpretCommand(string command, List<string> options, List<string> parameters)
        {
            try
            {
                switch (command)
                {
                    case "ls":
                        Ls();
                        break;
                    case "clear":
                        Clear();
                        break;
                    case "pwd":
                        Pwd();
                        break;
                    case "refresh":
                        Refresh();
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
                    case "cat":
                        foreach (var name in parameters)
                        {
                            Cat(name);
                        }
                        break;
                    case "rm":
                        foreach (var name in parameters)
                        {
                            Rm(name);
                        }
                        break;
                    case "mkdir":
                        foreach (var name in parameters)
                        {
                            Mkdir(name);
                        }
                        break;
                    case "rmdir":
                        bool rec = options.Contains("r");
                        foreach (var name in options)
                        {
                            Rmdir(name, options.Contains("r"));
                        }
                        break;
                    case "touch":
                        foreach (var name in parameters)
                        {
                            Touch(name);
                        }
                        break;
                    case "mv":
                        if (parameters.Count == 2)
                        {
                            Mv(parameters[0],parameters[1],options.Contains("r"));
                        }
                        break;
                    case "cd":
                        if (parameters.Count == 1)
                        {
                            Cd(parameters[0]);
                        }
                        break;
                    default:
                        UnknownCommand();
                        break;
                }

                Refresh();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private string GetCommand(List<string> command)
        {
            if (command.Count >= 1 && command[0] != null)
            {
                string res = command[0];
                command.Remove(command[0]);
                return res;
            }

            return null;
        }

        private List<string> GetOptions(List<string> command)
        {
            List<string> res = new List<string>();
            while (command.Count >= 1 && command[0].Length >= 1 && command[0][0] == '-')
            {
                string subres = "";
                for (int i = 1; i < command[0].Length; i++)
                {
                    subres += command[0][i];
                }
                res.Add(subres);
                command.Remove(command[0]);
            }

            return res;
        }

        private List<string> GetParameters(List<string> command)
        {
            List<string> res = new List<string>();
            while (command.Count >= 1 && command[0].Length >= 1)
            {
                res.Add(command[0]);
                command.Remove(command[0]);
            }

            return res;
        }

        private void UnknownCommand()
        {
            Console.WriteLine("Please enter a valid command");
            Console.WriteLine("Consider pressing \"help\" to display commands");
        }

        // This function reads the command that is ask
        private void ReadCommand()
        {
            var command = Console.ReadLine();
            if (command != null)
            {
                List<string> read = command.Split(' ').ToList();
                InterpretCommand(GetCommand(read),GetOptions(read), GetParameters(read));
            }
            else
                UnknownCommand();
        }

        #endregion
        
        #region Commands
        // Help command
        public void Help()
        {
            Console.WriteLine("__________________________________________________________");
            Console.WriteLine(">> Help commands : Here are the avail4ble commands <<");
            Console.WriteLine("__________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("------------------------BASICS---------------------------");
            Console.WriteLine("help : open this help prompt");
            Console.WriteLine("clear : clear the console");
            Console.WriteLine("exit : leave the console");
            Console.WriteLine("-------------------LOADED DIRECTORY----------------------");
            Console.WriteLine("ls : display all files in your directory");
            Console.WriteLine(" => \"ls\" will display all file and folders of the current directory");
            Console.WriteLine("cd : change the directory given with the second parameter");
            Console.WriteLine(" => \"cd dir\" will change the loaded directory to its child named dir");
            Console.WriteLine("pwd : display the current loaded directory");
            Console.WriteLine(" => \"pwd\" will display the path of the loaded directory");
            Console.WriteLine("------------------------DIRECTORY------------------------");
            Console.WriteLine("mkdir : create a directory with the specified name");
            Console.WriteLine(" => \"mkdir dir\" will create a new folder named dir");
            Console.WriteLine("rmdir : delete a given directory given in the parameter");
            Console.WriteLine(" => \"rmdir dir\" will delete the directory named dir");
            Console.WriteLine("--------------------------FILES--------------------------");
            Console.WriteLine("touch : create an empty file with a given name");
            Console.WriteLine(" => \"touch file.txt\" will create a file name file.txt");
            Console.WriteLine("rm : delete the specified file");
            Console.WriteLine(" => \"rm file.txt\" will delete file.txt contained in the directory");
            Console.WriteLine("cat : display the content of a given file given in parameter");
            Console.WriteLine(" => \"cat test.txt\" will display the content of test.txt");
            Console.WriteLine("----------------------TREATMENT--------------------------");
            Console.WriteLine("mv : rename or move a file to a new position using a 2 paths");
            Console.WriteLine(" => \"mv file.txt file2.txt\" will rename file.txt to file2.txt");
            Console.WriteLine("find : find a file or folder in the current loaded directory");
            Console.WriteLine(" => \"find te\" will returns the path of test.txt (the more relevant file/folder)");
            Console.WriteLine("------------------------DEBUG-----------------------------");
            Console.WriteLine("refresh : refresh manually the current directory");
            Console.WriteLine("printInf : execute printInformation function on the whole directory");
            Console.WriteLine("printInfFile : execute printInformation function on one FileType");
            Console.WriteLine("__________________________________________________________");
        }

        private static void Ls()
        {
            _directoryType.DisplayChildren();
        }

        private static void Clear()
        {
            Console.Clear();
        }

        private static void Cat(string name)
        {
            Console.WriteLine();
            Console.WriteLine("Content of " + name);
            Console.WriteLine("_________");
            name = ManagerReader.GetNameToPath(name);
            ManagerReader.ReadContent(name);
        }

        private static void Mkdir(string name)
        {
            name = ManagerReader.GetNameToPath(name);
            _directoryType.ChildrenFiles.Add(ManagerWriter.CreateDir(name));
        }

        private static void Rmdir(string name, bool rec = false)
        {
            name = ManagerReader.GetNameToPath(name);
            ManagerWriter.DeleteDir(name, rec);
        }

        private static void Mv(string name, string dest, bool rep = false)
        {
            name = ManagerReader.GetNameToPath(name);
            dest = ManagerReader.GetNameToPath(dest);
            if (!rep)
                ManagerWriter.Rename(name, dest);
            else 
                ManagerWriter.RenameMerge(name, dest);
        }

        private static void Rm(string name)
        {
            name = ManagerReader.GetNameToPath(name);
            ManagerWriter.Delete(name);
        }

        private static void Touch(string name)
        {
            FileType ft = ManagerWriter.Create(name, ManagerReader.GetFileExtension(name));
            ManagerReader.ReadFileType(ref ft);
            _directoryType.ChildrenFiles.Add(ft);
        }

        private static void Cd(string dest)
        {
            _directoryType.ChangeDirectory(Path.GetFullPath(dest));
        }
        private static void Pwd()
        {
            Console.WriteLine();
            Console.WriteLine("Path");
            Console.WriteLine("-----");
            Console.WriteLine(_directoryType.Path);
            Console.WriteLine();
        }

        private static void Find(string fileToFind)
        {
            Console.WriteLine("Result of find in the current directory : ");
            Console.WriteLine(ManagerReader.SearchByIndeterminedName(_directoryType, fileToFind).Path);
        }

        private static void Refresh()
        {
            Console.WriteLine("refreshing directory : " + _directoryType.Path);
            _directoryType.SetChildrenFiles();
        }
        
        #endregion
    }
}