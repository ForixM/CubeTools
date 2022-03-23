using System.IO;
using ConfigLoader.Sections.appInterfaceSection;
using ConfigLoader.Sections.appLauncherSection;
using Library;

// Testing Library
/*
Console.WriteLine(ConfigLoader.ConfigLoader.AppPathSettings);
ConfigLoader.ConfigLoader.AppInterface = appInterfaceConfig.GetConfig();
ConfigLoader.ConfigLoader.AppLauncher = appLauncherConfig.GetConfig();

foreach (var tuple in ConfigLoader.ConfigLoader.GetAllSettings())
{
    Console.WriteLine(tuple.Item1 + " :" + tuple.Item2);
}
*/
// Launching CLI
Console.WriteLine("Type Enter to continue ...");
Console.ReadLine();
var cl = new CLI(Directory.GetCurrentDirectory());
cl.Process();