using System;
using System.IO;
using Manager;
using NUnit.Framework;


public class WriterTests
{
    private DirectoryType env;
    
    [SetUp]
    public void Setup()
    {
        env = new DirectoryType("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/WriterTests");
    }

    [Test]
    [TestCase("attributes.txt", FileAttributes.Hidden, false)] 
    [TestCase("attributes.txt", FileAttributes.Hidden, true)] 
    [TestCase("non existent", FileAttributes.Archive, false)]
    [TestCase("attributes.txt", FileAttributes.ReadOnly, false)]
    [TestCase("attributes.txt", FileAttributes.ReadOnly, true)]
    public void SetAttributes(string name, FileAttributes fa, bool set)
    {
        name = env.Path + "/" + name;
        ManagerWriter.SetAttributes(name, set, fa);
        Assert.AreEqual(true, set == ManagerReader.HasAttribute(fa, name));
    }
}