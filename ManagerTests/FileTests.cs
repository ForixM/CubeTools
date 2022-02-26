using System.IO;
using Manager;
using NUnit.Framework;

namespace ManagerTests
{
    public class FileTests
    {
        private DirectoryType env;

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests/");
            env = new DirectoryType(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/FileTests/");
            Directory.SetCurrentDirectory(env.Path);
            env.AddFile("init", "txt");
            env.AddFile("init2", "txt");
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
        [TestCase("init.txt", true)]
        [TestCase("it.txt", false)]
        [TestCase("reader", true)]
        public void Init(string filename, bool exist)
        {
            filename = env.Path + filename;
            FileType ft = new FileType(filename);
            ManagerReader.ReadFileType(ref ft);
            if (!exist)
            {
                Assert.AreEqual(true, ft == FileType.NullPointer);
            }
            else
            {
                Assert.AreEqual(true, ft == env.GetChild(ft.Name));
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            env.Delete();
        }
    }
}