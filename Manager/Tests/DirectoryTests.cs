using System.Collections.Generic;
using System.IO;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
using NUnit.Framework;

namespace Tests
{
    public class DirectoryTests
    {
        private static DirectoryType test;
        private static DirectoryType test2;
        private string path;

        [OneTimeSetUp]
        public void Setup()
        {
            // Load main directory
            path = "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/Tests/Tests/DirectoryTests/";
            ManagerWriter.DeleteDir(path, true);
            ManagerWriter.CreateDir(path);
            // Set up env
            Directory.SetCurrentDirectory(path);
            // Adding dir
            ManagerWriter.CreateDir(path + "test");
            test = new DirectoryType(path + "test");
            test.AddFile("test", "txt"); // ft added
            test.AddFile("test2", "txt");
            // Adding an other dir
            ManagerWriter.CreateDir(path + "test2");
            test2 = new DirectoryType(path + "test2");
            test2.AddDir("test");
        }

        [Test]
        [TestCase("test")]
        [TestCase("test2")]
        [TestCase("NON EXISTENT")]
        public void DirectoryTypeConstructor(string name)
        {
            Directory.SetCurrentDirectory(path);
            var newPath = path + name;
            if (Directory.Exists(newPath))
            {
                DirectoryType dir = new DirectoryType(newPath);
                // Are Equal
                Assert.AreEqual(dir.Name, name);
                Assert.AreEqual(dir.Size, ManagerReader.GetFileSize(newPath));
                Assert.AreEqual(dir.Date, ManagerReader.GetFileCreationDate(newPath));
                Assert.AreEqual(dir.LastDate, ManagerReader.GetFileLastEdition(newPath));
                Assert.AreEqual(dir.AccessDate, ManagerReader.GetFileAccessDate(newPath));
                Assert.AreEqual(dir.Hidden, ManagerReader.IsFileHidden(newPath));
                Assert.AreEqual(dir.ReadOnly, ManagerReader.IsReadOnly(newPath));
            }
            else
            {
                DirectoryType dir = new DirectoryType(newPath);
                Assert.AreEqual(null, dir.Path);
            }
        }

        [Test]
        [TestCase("test")]
        [TestCase("test.txt")]
        [TestCase("test2.txt")]
        public void GetChild(string res)
        {
            res = path + res;
            if (File.Exists(res) || Directory.Exists(res))
                Assert.AreEqual(res, test.GetChild(res).Path, res + " is a child of " + test);
            Assert.Pass("The directory does not exists");
        }

        [Test]
        public void SetChildrenFiles()
        {
            var res = true;
            test.SetChildrenFiles();
            var di = new DirectoryInfo(test.Path);
            var fiList = di.GetFiles();
            var diList = di.GetDirectories();
            var diListString = new List<string>();
            var fiListString = new List<string>();
            foreach (var d in diList)
                diListString.Add(d.FullName);
            foreach (var f in fiList)
                fiListString.Add(f.FullName);
            foreach (var ft in test.ChildrenFiles)
                if (ft.IsDir)
                    Assert.IsTrue(fiListString.Contains(ft.Path), ft.Path + " contains ?");
                else
                    Assert.IsTrue(diListString.Contains(ft.Path), ft.Path + " contains ?");
            Assert.Pass("Set children files implemented");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ManagerWriter.DeleteDir(path + "test");
            ManagerWriter.DeleteDir(path + "test2");
        }
    }
}