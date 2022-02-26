using System.IO;
using Manager;
using NUnit.Framework;

namespace ManagerTests
{
    public class WriterTests
    {
        private DirectoryType env;
        
        [OneTimeSetUp] public void Setup()
        {
            ManagerWriter.CreateDir(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/WriterTests");
            env = new DirectoryType("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/WriterTests");
            env.AddFile("attributes", "txt");
            // For Rename purposes
            env.AddFile("rename1(1)", "txt");
            env.AddFile("rename2(1)", "txt");
            env.AddFile("rename2", "txt");
            env.AddFile("rename3", "txt");
            env.AddFile("rename3(1)", "txt");
            env.AddFile("rename3(2)", "txt");
            env.AddFile("rename4", "txt");
            env.AddFile("rename4(1)", "txt");
            // For Copy purposes
            env.AddFile("copy", "txt");
            env.AddFile("copy1", "txt");
            env.AddFile("copy2", "txt");
            // Others
            env.AddFile("test1", "txt");
            env.AddFile("test1(1)", "txt");
            env.AddDir("attributes");
            env.AddDir("rename");
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
            ManagerWriter.SetAttributes(name, !set, fa);
        }
        

        [Test]
        [TestCase("rename.txt", "test.txt")]
        [TestCase("lolsijis", "qdssqds")]
        [TestCase("rename", "yes")]
        [TestCase("rename2.txt", "test.txt")]
        [TestCase("rename2(2).txt", "lol.docx")]
        public void Rename2(string nameS, string nameD)
        {
            nameS = env.Path + '/' + nameS;
            nameD = env.Path + '/' + nameD;
            Assert.AreEqual(ManagerWriter.Rename(nameS, nameD), File.Exists(nameD) || Directory.Exists(nameD));
            ManagerWriter.Rename(nameD, nameS);
        }
    
        [Test]
        [TestCase("rename3.txt", "test.txt")]
        [TestCase("lolsijis", "qdssqds")]
        [TestCase("rename", "yes")]
        [TestCase("rename3(1).txt", "test.txt")]
        [TestCase("rename3(2).txt", "lol.docx")]
        public void Rename3(string nameS, string nameD)
        {
            nameS = env.Path + '/' + nameS;
            FileType ftS = new FileType(nameS);
            nameD = env.Path + '/' + nameD;
            Assert.AreEqual(ManagerWriter.Rename(ftS, nameD), File.Exists(nameD) || Directory.Exists(nameD));
            FileType ftD = new FileType(nameD);
            ManagerWriter.Rename(ftD, nameS);
        }
    
        [Test]
        [TestCase("rename4.txt", "test.txt", false)]
        [TestCase("rename4.txt", "rename4(1).txt", true)]
        [TestCase("rename", "yes", true)]
        [TestCase("rename4(1).txt", "test.txt", false)]
        [TestCase("rename4(1).txt", "rename4.txt", true)]
        public void Rename4(string nameS, string nameD, bool overwrite)
        {
            nameS = env.Path + '/' + nameS;
            FileType ftS = new FileType(nameS);
            nameD = env.Path + '/' + nameD;
            if (overwrite)
            {
                Assert.AreEqual(ManagerWriter.Rename(ftS, nameD, true), File.Exists(nameD) || Directory.Exists(nameD));
                File.Create(nameS).Close();
            }
            else
            {
                Assert.AreEqual(ManagerWriter.Rename(ftS, nameD), File.Exists(nameD) || Directory.Exists(nameD));
                FileType ftD = new FileType(nameD);
                ManagerWriter.Rename(ftD, nameS);
            }
        }

        [Test]
        [TestCase("copy1.txt")]
        [TestCase("NonExistent")]
        public void Copy1(string name)
        {
            name = env + name;
            string res = ManagerWriter.Copy(name);
            Assert.IsTrue(ManagerWriter.Delete(res));
        }
        
        [Test]
        [TestCase("copy2.txt")]
        [TestCase("nonExistent")]
        public void Copy2(string name)
        {
            name = env + name;
            
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            env.Delete();
        }
    }
}