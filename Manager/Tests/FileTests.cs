using System.IO;
using Library.ManagerReader;
using Library.Pointers;
using NUnit.Framework;

namespace Tests
{
    public class FileTests
    {
        private DirectoryType env;

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/Tests/Tests/FileTests/");
            env = new DirectoryType(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/Tests/Tests/FileTests/");
            Directory.SetCurrentDirectory(env.Path);
            env.AddFile("init", "txt");
            env.AddFile("init2", "txt");
        }

        [Test]
        public void FileTypeConstructor()
        {
            var ft = new FileType();
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
        [TestCase("init.txt", true)]
        [TestCase("it.txt", false)]
        [TestCase("reader", true)]
        public void Init(string filename, bool exist)
        {
            filename = env.Path + filename;
            var ft = new FileType(filename);
            ManagerReader.ReadFileType(ref ft);
            if (!exist)
                Assert.AreEqual(true, ft == FileType.NullPointer);
            else
                Assert.AreEqual(true, ft == env.GetChild(ft.Name));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            env.Delete();
        }
    }
}