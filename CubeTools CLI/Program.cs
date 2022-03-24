using System.IO;
using ConfigLoader.Sections.appInterfaceSection;
using ConfigLoader.Sections.appLauncherSection;
using Library;
using Library.ManagerWriter;

// Testing Library
Console.WriteLine("Let's some stuff baby ...");
Thread.Sleep(1000);
Directory.SetCurrentDirectory("C:/Users/mateo/OneDrive/Documents/My Games");
Console.WriteLine("The Copying has been launched ...");
/*
Task task = ManagerWriter.CopyAsync("C:/Users/mateo/OneDrive/Documents/My Games", "C:/Users/mateo/OneDrive/Documents/New Games");
task.Wait();
*/
Task task = ManagerWriter.CopyAsync("C:/Users/mateo/OneDrive/Documents/My Games", "C:/Users/mateo/OneDrive/Documents/New Games");
Console.WriteLine("Finished !");
Console.WriteLine("Tap enter to delete");
Console.ReadLine();
ManagerWriter.DeleteDir("C:/Users/mateo/OneDrive/Documents/New Games", true);
// Launching CLI
Console.WriteLine("Type Enter to continue ...");
Console.ReadLine();
var cl = new CLI(Directory.GetCurrentDirectory());
cl.Process();