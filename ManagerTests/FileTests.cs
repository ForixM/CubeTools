using System.Diagnostics;
using Manager;
using NUnit.Framework;


public class FileTests
{
    private DirectoryType env;

    [SetUp]
    public void Setup()
    {
        env = new DirectoryType(
            "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests");
    }

    [Test]
    public void FileTypeConstructor()
    {
        FileType ft = new FileType();
        Assert.AreEqual(ft.Path, "");
        Assert.AreEqual(ft.Name, "");
        Assert.AreEqual(ft.Type, "");
        Assert.AreEqual(ft.Size, 0);
        Assert.AreEqual(ft.Date, "");
        Assert.AreEqual(ft.LastDate, "");
        Assert.AreEqual(ft.AccessDate, "");
        Assert.AreEqual(ft.Hidden, false);
        Assert.AreEqual(ft.Compressed, false);
        Assert.AreEqual(ft.Archived, false);
        Assert.AreEqual(ft.ReadOnly, false);
        Assert.AreEqual(ft.IsDir, false);
    }

    [Test]
    [TestCase("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests/init.txt", true)]
    [TestCase("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests/it.txt", false)]
    [TestCase("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests/reader", true)]
    public void Init(string filename, bool exist)
    {
        FileType ft = new FileType(filename);
        FileType.Init(ref ft);
        if (!exist)
        {
            Assert.AreEqual(true, ft == FileType.NullPointer);
        }
        else
        {
            Assert.AreEqual(true, ft == env.GetChild(ft.Name));
        }
    }
}