using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.DirectoryPointer;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Library.FilePointer;

namespace Cli
{
    internal class Cli
    {
        #region Variables

        // Attributes
        private static DirectoryPointerLoaded _directoryPointer;
        private static string _promptLine;

        #endregion

        #region Init

        // Constructor : OK
        public Cli()
        {
            _directoryPointer = new DirectoryPointerLoaded();
            _promptLine = ">> ";
        }

        // Constructor : OK
        public Cli(string path)
        {
            path = path.Replace('\\', '/');
            _directoryPointer = new DirectoryPointerLoaded(path);
            _promptLine = ">> ";
        }

        #endregion

        #region Process

        // This function deals with the process of the command line
        public void Process()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
            ░▒█▀▀▄░█░▒█░█▀▀▄░█▀▀░░░▀▀█▀▀░▄▀▀▄░▄▀▀▄░█░░█▀▀
            ░▒█░░░░█░▒█░█▀▀▄░█▀▀░░░░▒█░░░█░░█░█░░█░█░░▀▀▄
            ░▒█▄▄▀░░▀▀▀░▀▀▀▀░▀▀▀░░░░▒█░░░░▀▀░░░▀▀░░▀▀░▀▀▀
            ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("CubeTools command line project");
            Console.WriteLine("Type help to get any information about the available commands");
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(_directoryPointer.Path);
                Console.Write(_promptLine + " ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
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
                    case "cat":
                        foreach (var name in parameters) Cat(name);
                        break;
                    case "rm":
                        foreach (var name in parameters) Rm(name);
                        break;
                    case "find":
                        if (parameters.Count == 1)
                            Find(parameters[0]);
                        break;
                    case "mkdir":
                        foreach (var name in parameters) Mkdir(name);
                        break;
                    case "rmdir":
                        var rec = options.Contains("r");
                        foreach (var name in parameters) Rmdir(name, options.Contains("r"));
                        break;
                    case "touch":
                        foreach (var name in parameters) Touch(name);
                        break;
                    case "mv":
                        if (parameters.Count == 2) Mv(parameters[0], parameters[1], options.Contains("r"));
                        break;
                    case "cd":
                        if (parameters.Count == 1) Cd(parameters[0]);
                        break;
                    case "cp":
                        if (parameters.Count == 2)
                            Cp(parameters[0], parameters[1]);
                        else if (parameters.Count == 1)
                            Cp(parameters[0]);
                        break;
                    default:
                        UnknownCommand();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("# ERROR OCCURED #");
            }
        }

        private string GetCommand(List<string> command)
        {
            if (command.Count >= 1)
            {
                var res = command[0];
                command.Remove(command[0]);
                return res;
            }

            return null;
        }

        private List<string> GetOptions(List<string> command)
        {
            var res = new List<string>();
            while (command.Count >= 1 && command[0].Length >= 1 && command[0][0] == '-')
            {
                var subres = "";
                for (var i = 1; i < command[0].Length; i++) subres += command[0][i];
                res.Add(subres);
                command.Remove(command[0]);
            }

            return res;
        }

        private List<string> GetParameters(List<string> command)
        {
            var res = new List<string>();
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
            Console.ForegroundColor = ConsoleColor.White;
            if (command != null)
            {
                var read = command.Split(' ').ToList();
                InterpretCommand(GetCommand(read), GetOptions(read), GetParameters(read));
            }
            else
            {
                UnknownCommand();
            }
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
            Console.WriteLine("cp : create a copy and rename this copy");
            Console.WriteLine(" => \"cp test.txt test2.txt\" will create a file test2.txt with the content");
            Console.WriteLine("------------------------DEBUG-----------------------------");
            Console.WriteLine("refresh : refresh manually the current directory");
            Console.WriteLine("printInf : execute printInformation function on the whole directory");
            Console.WriteLine("printInfFile : execute printInformation function on one FilePointer");
            Console.WriteLine("__________________________________________________________");
        }

        /// <summary>
        ///     Display all children of the current loaded directory
        /// </summary>
        private static void Ls()
        {
            Console.WriteLine("Children");
            Console.WriteLine("__________");
            foreach (var pointer in _directoryPointer.ChildrenFiles) Console.WriteLine(pointer.Path);
        }

        /// <summary>
        ///     Clear the console
        /// </summary>
        private static void Clear() => Console.Clear();

        /// <summary>
        ///     Display the content
        /// </summary>
        /// <param name="name">The given relative name</param>
        private static void Cat(string name)
        {
            var res = "";
            try
            {
                name = ManagerReader.GetNameToPath(name);
                res = ManagerReader.GetContent(name);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Cat aborted");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Content of " + name);
                Console.WriteLine("_________");
                Console.WriteLine(res);
                Console.WriteLine("_________");
            }
        }

        /// <summary>
        ///     Create a directory
        /// </summary>
        /// <param name="name"></param>
        private static void Mkdir(string name)
        {
            try
            {
                name = ManagerReader.GetNameToPath(name);
                ManagerWriter.CreateDir(name);
                _directoryPointer.ChildrenFiles.Add(new FilePointer(name));
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Mkdir aborted");
            }
        }

        /// <summary>
        ///     Destroy a directory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rec"></param>
        private static void Rmdir(string name, bool rec = false)
        {
            try
            {
                ManagerWriter.Delete(name, rec);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Rmdir aborted");
            }
        }

        /// <summary>
        ///     Rename a file or folder
        /// </summary>
        /// <param name="name">the relative path of the file or folder</param>
        /// <param name="dest">the destination path</param>
        /// <param name="rep">whether it as to replace the value</param>
        private static void Mv(string name, string dest, bool rep = false)
        {
            try
            {
                name = ManagerReader.GetNameToPath(name);
                dest = ManagerReader.GetNameToPath(dest);
                ManagerWriter.Rename(name, dest, rep);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Mv aborted");
            }
        }

        /// <summary>
        ///     Destroy a file
        /// </summary>
        /// <param name="name">the name of the file (its relative path)</param>
        private static void Rm(string name)
        {
            try
            {
                ManagerWriter.DeleteFile(name);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Rm aborted");
            }
        }

        /// <summary>
        ///     Create an empty file
        /// </summary>
        /// <param name="name">the name of the file to create</param>
        private static void Touch(string name)
        {
            try
            {
                _directoryPointer.ChildrenFiles.Add(
                    ManagerWriter.Create(name, ManagerReader.GetFileExtension(name)));
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# touch aborted");
            }
        }

        /// <summary>
        ///     Change the current folder to the new one given with a string 'dest'
        /// </summary>
        /// <param name="dest">the destination</param>
        private static void Cd(string dest)
        {
            try
            {
                Directory.SetCurrentDirectory(dest);
                _directoryPointer = new DirectoryPointerLoaded(Directory.GetCurrentDirectory());
            }
            catch (ManagerException e)
            {
                if (e is AccessException) Console.Error.WriteLine("# Access impossible, Cd aborted");
                else Console.WriteLine(e.Errorstd);
            }
        }

        /// <summary>
        ///     Display the current loaded folder
        /// </summary>
        private static void Pwd()
        {
            Console.WriteLine();
            Console.WriteLine("Path");
            Console.WriteLine("-----");
            Console.WriteLine(_directoryPointer.Path);
            Console.WriteLine();
        }

        /// <summary>
        ///     Copy the source
        /// </summary>
        /// <param name="source"></param>
        private static void Cp(string source)
        {
            try
            {
                source = ManagerReader.GetNameToPath(source);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e is AccessException
                    ? "# Access not possible, copy aborted"
                    : "# Copy aborted");
                return;
            }

            try
            {
                _directoryPointer.ChildrenFiles.Add(ManagerWriter.Copy(source));
            }
            catch (Exception e)
            {
                Console.WriteLine("# Copy aborted");
            }
        }

        /// <summary>
        ///     Copy the source and change its name to the 'dest' one
        /// </summary>
        /// <param name="source">the source file/folder</param>
        /// <param name="dest">the dest file/folder</param>
        private static void Cp(string source, string dest)
        {
            try
            {
                source = ManagerReader.GetNameToPath(source);
                Console.WriteLine(source);
                dest = ManagerReader.GetParent(source).Replace('\\', '/') + '/' + dest;
                Console.WriteLine(dest);
            }
            catch (AccessException)
            {
                Console.Error.WriteLine("# Access not possible, copy aborted");
            }

            try
            {
                ManagerWriter.Copy(source, dest);
                _directoryPointer.ChildrenFiles.Add(new FilePointer(dest));
            }
            catch (Exception e)
            {
                Console.WriteLine("# Copy aborted");
            }
        }

        /// <summary>
        ///     Try to find the file give in parameter
        /// </summary>
        /// <param name="toFind">the file/folder to find</param>
        private static void Find(string toFind)
        {
            var res = "";
            try
            {
                res = ManagerReader.SearchByIndeterminedName(_directoryPointer, toFind).Path;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("# Find aborted");
                return;
            }

            Console.WriteLine("Result of find in the current directory : ");
            Console.WriteLine(res);
        }

        /// <summary>
        ///     Refresh manually the Directory Loaded
        /// </summary>
        private static void Refresh()
        {
            try
            {
                _directoryPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("# Cannot refresh");
            }
        }

        #endregion
    }
}