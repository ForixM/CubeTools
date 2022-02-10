using System;
using System.IO;
using Manager;
using NUnit.Framework;


public class ReaderTests
{
    private DirectoryType env;

    [SetUp]
    public void Setup() // Setup environment for better test implementation
    {
        env = new DirectoryType(
            "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/ReaderTests");
    }

    [Test] // see : IsFile> ManagerReader
    [TestCase("reader.txt")]
    [TestCase("nothing")]
    [TestCase("reader")]
    public void IsFile(string name)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(File.Exists(name), ManagerReader.IsFile(name));
    }
    
    [Test] // see : IsDir > ManagerReader
    [TestCase("reader.txt")]
    [TestCase("reader")]
    [TestCase("none")]
    public void IsDirectory(string name)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(Directory.Exists(name), ManagerReader.IsDirectory(name));
    }
    
    [Test] // see : IsFileHidden > ManagerReader
    [TestCase("readerHidden", false)]
    [TestCase("reader.txt", false)]
    [TestCase("none", false)]
    [TestCase("readerHidden.txt", true)]
    public void IsFileHidden(string name, bool hidden)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(hidden, ManagerReader.IsFileHidden(name));
    }
    
    [Test] // see : IsDirHidden > ManagerReader
    [TestCase("readerHidden.txt", false)]
    [TestCase("reader.txt", false)]
    [TestCase("none", false)]
    [TestCase("readerHidden", true)]
    public void IsDirHidden(string name, bool hidden)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(hidden, ManagerReader.IsDirHidden(name));
    }
    
    [Test] // see : IsFileCompressed > ManagerReader
    [TestCase("readerCompressed.txt", true)]
    [TestCase("readerCompressed", false)]
    [TestCase("none", false)]
    [TestCase("readerHidden", false)]
    public void IsFileCompressed(string name, bool compressed)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(compressed, ManagerReader.IsFileCompressed(name));
    }
    
    [Test] // see : IsFileArchived > ManagerReader
    [TestCase("readerArchived.txt", true)]
    [TestCase("reader", false)]
    [TestCase("none", false)]
    [TestCase("readerHidden", false)]
    public void IsFileArchived(string name, bool archived)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(archived, ManagerReader.IsFileArchived(name));
    }
    
    [Test] // see : IsASystemFile > ManagerReader
    [TestCase("readerSystem.txt", true)]
    [TestCase("reader", false)]
    [TestCase("readerCompressed.txt", false)]
    [TestCase("non existent", false)]
    public void IsASystemFile(string name, bool system)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(system, ManagerReader.IsASystemFile(name));
    }
    
    [Test] // see : IsAReadOnlyFile > ManagerReader
    [TestCase("readerReadOnly.txt", true)]
    [TestCase("reader", false)]
    [TestCase("readerCompressed.txt", false)]
    [TestCase("non existent", false)]
    public void IsAReadOnlyFile(string name, bool readOnly)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(readOnly, ManagerReader.IsAReadOnlyFile(name));
    }
    
    [Test] // see : IsDirReadOnly > ManagerReader
    [TestCase("readerReadOnly.txt", false)]
    [TestCase("readerReadOnly", true)]
    [TestCase("readerCompressed.txt", false)]
    [TestCase("non existent", false)]
    public void IsDirReadOnly(string name, bool readOnly)
    {
        name = env.Path + "/" + name;
        if (readOnly)
        {
            DirectoryInfo n;
            n = new DirectoryInfo(name);
            n.Attributes |= FileAttributes.ReadOnly;
        }
        Assert.AreEqual(readOnly, ManagerReader.IsDirReadOnly(name));
    }

    [Test]
    [TestCase("reader/parent", "reader")]
    [TestCase("reader", "ReaderTests")]
    [TestCase("none", "")]
    public void GetParent(string name, string parentName)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(parentName,ManagerReader.GetParent(name));
    }

    [Test]
    [TestCase("creationDate.txt", "2/10/2022 4:30:12 PM")]
    [TestCase("noneExistent", "")]
    public void GetFileCreationDate(string name, string date)
    {
        name = env.Path + "/" + name;
        Console.Write(name);
        Assert.AreEqual(date, ManagerReader.GetFileCreationDate(name));
    }

    [Test]
    [TestCase("lastEditionDate.txt", "2/10/2022 4:30:12 PM")]
    [TestCase("noneExistent", "")]
    public void GetFileLastEdition(string name, string date)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(date,ManagerReader.GetFileLastEdition(name));
    }

    [Test]
    [TestCase("accessDate.txt", "2/10/2022 4:51:58 PM")]
    [TestCase("noneExistent", "")]
    public void GetFileAccessDate(string name, string date)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(date, ManagerReader.GetFileAccessDate(name));
    }

    [Test]
    [TestCase("size.txt", 194040)]
    [TestCase("noneExistent", 0)]
    [TestCase("size",356720)]
    public void GetFileSize(string name, int size)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(size, ManagerReader.GetFileSize(name));
    }

    [Test]
    [TestCase("size", "size")]
    [TestCase("accessDate.txt", "accessDate.txt")]
    [TestCase("lol", "")]
    public void GetPathToName(string path, string name)
    {
        path = env.Path + "/" + path;
        Assert.AreEqual(name, ManagerReader.GetPathToName(path));
    }

    [Test]
    [TestCase("size.txt","size.txt")]
    [TestCase("size", "size")]
    [TestCase("none", "")]
    public void GetPathToNameNoExtension(string path, string name)
    {
        path = env.Path + "/" + path;
        Assert.AreEqual(name, ManagerReader.GetPathToName(path));
    }

    [Test]
    [TestCase("reader.txt", "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/ReaderTests/reader.txt")]
    [TestCase("reader", "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/ReaderTests/reader")]
    [TestCase("None", "None")]
    public void GetNameToPath(string name, string path)
    {
        Directory.SetCurrentDirectory(env.Path);
        Assert.AreEqual(path, ManagerReader.GetNameToPath(name));
    }

    [Test]
    [TestCase("reader.txt", ".txt")]
    [TestCase("reader", "")]
    [TestCase("nimportequoi", "")]
    public void GetFileExtension(string name, string extension)
    {
        name = env.Path + "/" + name;
        Assert.AreEqual(extension, ManagerReader.GetFileExtension(name));
    }
}