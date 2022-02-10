using System.IO;
using Manager;
using NUnit.Framework;


public class DirectoryTests
{
    private string path;
    [SetUp]
    public void Setup()
    {
        path = "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/DirectoryTests";
        Directory.SetCurrentDirectory(path);
    }

    [Test]
    [TestCase("test")]
    [TestCase(" ll")]
    [TestCase("NON EXISTENT")]
    public void DirectoryTypeConstructor(string name)
    {
        Directory.SetCurrentDirectory(path);
        string newPath = path + "/" + name;
        if (Directory.Exists(newPath))
        {
            DirectoryType dir = new DirectoryType(newPath);
            // Are Equal
            Assert.AreEqual(dir.Name, name);
            Assert.AreEqual(dir.Path, ManagerReader.GetNameToPath(name));
            Assert.AreEqual(dir.Size, ManagerReader.GetFileSize(newPath));
            Assert.AreEqual(dir.Date, ManagerReader.GetFileCreationDate(newPath));
            Assert.AreEqual(dir.LastDate, ManagerReader.GetFileLastEdition(newPath));
            Assert.AreEqual(dir.AccessDate, ManagerReader.GetFileAccessDate(newPath));
            Assert.AreEqual(dir.Hidden, ManagerReader.IsDirHidden(newPath));
            Assert.AreEqual(dir.ReadOnly, ManagerReader.IsDirReadOnly(newPath));
        }
        else
        {
            DirectoryType dir = new DirectoryType(newPath);
            Assert.AreEqual("", dir.Path);
        }
    }
}