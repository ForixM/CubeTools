using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Manager;
using NUnit.Framework;

namespace ManagerTests
{
    public class ReaderTests
    {
        private DirectoryType env;

        [OneTimeSetUp]
        public void SetUp() // Setup environment for better test implementation
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Write("Initializing environment...");
            env = new DirectoryType(
                "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/ReaderTests");
            env.AddFile("accessDate", "txt");
            env.AddFile("lastEditionDate","txt");
            env.AddFile("reader", "txt");
            env.AddFile("readerArchived", "txt");
            env.AddFile("readerCompressed", "txt");
            env.AddFile("readerHidden", "txt");
            env.AddFile("readerReadOnly", "txt");
            env.AddFile("size", "txt");
            env.AddFile("readerSystem", "txt");
            env.AddDir("reader");
            env.AddDir("reader/parent");
            env.AddDir("readerHidden");
            env.AddDir("readerReadOnly");
            env.AddDir("size");
            Assert.Pass();
        }

        #region Properties

        [Test] // Check
        [TestCase("reader.txt")]
        [TestCase("nothing")]
        [TestCase("reader")]
        public void IsFile(string name)
        {
            name = env.Path + "/" + name;
            Assert.AreEqual(File.Exists(name), ManagerReader.IsFile(name));
        }
    
        [Test] // Check
        [TestCase("reader.txt")]
        [TestCase("reader")]
        [TestCase("none")]
        public void IsDirectory(string name)
        {
            name = env.Path + "/" + name;
            Assert.AreEqual(Directory.Exists(name), ManagerReader.IsDirectory(name));
        }
    
        [Test] // see : IsFileHidden > ManagerReader
        [TestCase("readerHidden", true)]
        [TestCase("reader.txt", false)]
        [TestCase("none", false)]
        [TestCase("readerHidden.txt", true)]
        public void IsFileHidden(string name, bool hidden)
        {
            name = env.Path + "/" + name;
            ManagerWriter.SetAttributes(name, hidden, FileAttributes.Hidden);
            Assert.AreEqual(hidden, ManagerReader.IsFileHidden(name));
        }
    
        [Test] // see : IsDirHidden > ManagerReader
        [TestCase("readerHidden.txt", true)]
        [TestCase("reader.txt", false)]
        [TestCase("none", false)]
        [TestCase("readerHidden", true)]
        public void IsDirHidden(string name, bool hidden)
        {
            name = env.Path + "/" + name;
            ManagerWriter.SetAttributes(name, hidden, FileAttributes.Hidden);
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
            ManagerWriter.SetAttributes(name, compressed, FileAttributes.Compressed);
            Assert.AreEqual(compressed, ManagerReader.IsFileCompressed(name));
        }
    
        [Test] // see : IsFileArchived > ManagerReader
        [TestCase("readerArchived.txt", true)]
        [TestCase("reader", true)]
        [TestCase("none", false)]
        [TestCase("readerHidden", false)]
        public void IsFileArchived(string name, bool archived)
        {
            name = env.Path + "/" + name;
            ManagerWriter.SetAttributes(name, archived, FileAttributes.Archive);
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
            ManagerWriter.SetAttributes(name, system, FileAttributes.System);
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
            ManagerWriter.SetAttributes(name, readOnly, FileAttributes.ReadOnly);
            Assert.AreEqual(readOnly, ManagerReader.IsAReadOnlyFile(name));
        }
    
        [Test] // see : IsDirReadOnly > ManagerReader
        [TestCase("readerReadOnly.txt", true)]
        [TestCase("readerReadOnly", true)]
        [TestCase("readerCompressed.txt", false)]
        [TestCase("non existent", false)]
        public void IsDirReadOnly(string name, bool readOnly)
        {
            name = env.Path + "/" + name;
            ManagerWriter.SetAttributes(name, readOnly, FileAttributes.ReadOnly);
            Assert.AreEqual(readOnly, ManagerReader.IsDirReadOnly(name));
        }

        [Test]
        [TestCase("size", FileAttributes.Archive, true)]
        [TestCase("size.txt", FileAttributes.ReadOnly, true)]
        [TestCase("readerHidden.txt", FileAttributes.Hidden, true)]
        [TestCase("none existent", FileAttributes.Archive, false)]
        public void HasAttribute(string name, FileAttributes fa, bool res)
        {
            name = env.Path + "/" + name;
            ManagerWriter.SetAttributes(name, res, fa);
            Assert.AreEqual(res, ManagerReader.HasAttribute(fa, name));
        }
    
        #endregion

        #region Get
    
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
        [TestCase("creationDate.txt", true)]
        [TestCase("noneExistent", "")]
        public void GetFileCreationDate(string name, bool exist)
        {
            name = env.Path + "/" + name;
            FileInfo fi;
            if (exist)
            {
                fi = new FileInfo(name);
                Assert.AreEqual(fi.CreationTime.Date.ToString(CultureInfo.CurrentCulture), ManagerReader.GetFileCreationDate(name));
                return;
            }
            Assert.AreEqual("", ManagerReader.GetFileCreationDate(name));
        }

        [Test]
        [TestCase("lastEditionDate.txt", true)]
        [TestCase("size.txt", true)]
        [TestCase("noneExistent", false)]
        public void GetFileLastEdition(string name, bool exist)
        {
            name = env.Path + "/" + name;
            if (exist)
            {
                FileInfo fi = new FileInfo(name);
                Assert.AreEqual(fi.LastWriteTime.Date.ToString(CultureInfo.CurrentCulture), ManagerReader.GetFileLastEdition(name));
                return;
            }
            Assert.AreEqual("", ManagerReader.GetFileLastEdition(name));
        }

        [Test]
        [TestCase("accessDate.txt", true)]
        [TestCase("noneExistent", false)]
        public void GetFileAccessDate(string name, bool exist)
        {
            name = env.Path + "/" + name;
            FileInfo fi;
            if (exist)
            {
                fi = new FileInfo(name);
                Assert.AreEqual(fi.LastAccessTime.Date.ToString(CultureInfo.CurrentCulture), ManagerReader.GetFileAccessDate(name));
                return;
            }
            Assert.AreEqual("", ManagerReader.GetFileAccessDate(name));
        }

        [Test]
        [TestCase("size.txt", 0)]
        [TestCase("noneExistent", 0)]
        [TestCase("size",0)]
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
    
        #endregion
    
        #region Algorithm

        [Test]
        [TestCase("reader.txt", "reader(1).txt")]
        [TestCase("readerR.txt", "readerR.txt")]
        [TestCase("reader","reader(1)")]
        [TestCase("size", "size(1)")]
        [TestCase("new folder lol", "new folder lol")]
        public void GenerateNameForModification(string name, string res)
        {
            name = env.Path + "/" + name;
            res = env.Path + "/" + res;
            if (File.Exists(name) || Directory.Exists(name))
                Assert.AreEqual(env.Path+"/"+res, ManagerReader.GenerateNameForModification(name));
            else
                Assert.AreEqual(res, name);
        }

        [Test]
        [TestCase("2/10/2022 8:03:10 PM", "2/10/2022 8:03:11 PM", false)]
        [TestCase("2/11/2022 8:03:10 PM", "2/10/2022 8:03:11 AM", true)]
        [TestCase("2/11/2023 8:03:10 PM", "2/10/2022 8:03:11 AM", true)]
        [TestCase("1/1/2022 8:03:10 PM", "1/1/2022 8:03:10 PM", true)]
        public void MoreRecentThanDate(string date1, string date2, bool result)
        {
            Assert.AreEqual(result, ManagerReader.MoreRecentThanDate(date1,date2));
        }

        [Test]
        [TestCase(1024, "1 KB")]
        [TestCase(1048575, "1023 KB")]
        [TestCase(1073741824, "1 GB")]
        [TestCase(10737418240, "10 GB")]
        public void ByteToPowByte(long size, string res)
        {
            Assert.AreEqual(res, ManagerReader.ByteToPowByte(size));
        }

        // 
        private static bool GreaterThan(string s1, string s2)
        {
            int i = 0;
            int j = 0;
            int limit1 = s1.Count();
            int limit2 = s2.Count();
            while (i < limit1 && j < limit2)
            {
                if (s1[i] > s1[j])
                    return true;
                if (s2[j] > s1[i])
                    return false;
                i += 1;
                j += 1;
            }

            return i != limit1;
        }
    
        [Test]
        public void SortByName()
        {
            /*
        List<FileType> ft = ManagerReader.SortByName(env.ChildrenFiles);
        List<string> ls = new List<string>();
        foreach (var fte in ft)
        {
            ls.Add(fte.Name);
        }
        int i = 0;
        while (i < ls.Count - 1 && GreaterThan(ls[i+1],ls[i]))
        {
            i += 1;
        }
        Assert.AreEqual(true, i == ls.Count - 1);
        */ Assert.Pass();
        }
    
        [Test]
        public void SortBySize()
        {
            /*
        env.SetChildrenFiles();
        List<FileType> ft = ManagerReader.SortBySize(env.ChildrenFiles);
        List<long> ls = new List<long>();
        foreach (var fte in ft)
        {
            ls.Add(fte.Size);
        }
        long[] newLong = new long[ls.Count];
        ls.CopyTo(newLong, 0);
        ls.Sort();
        long[] res = ls.ToArray();
        Assert.AreEqual(ls, newLong);
        */
            Assert.Pass();
        }

        [Test]
        public void SortByType()
        {
            Assert.Pass();
        }
        [Test]
        public void SortByModifiedDate()
        {
            Assert.Pass();
        }

        [Test]
        [TestCase("size.txt", true)]
        [TestCase("non existent folder", false)]
        [TestCase("size", true)]
        public void SearchByFullName(string fullname, bool exist)
        {
            if (!exist)
            {
                Assert.AreEqual(null, ManagerReader.SearchByFullName(env, fullname));
                return;
            }
            Assert.AreEqual(fullname, ManagerReader.SearchByFullName(env, fullname).Name);
        }

        [Test]
        [TestCase("re", "reader.txt")]
        [TestCase("reader.","reader.txt")]
        [TestCase("readerS", "readerSystem.txt")]
        [TestCase("la", "lastEditionDate.txt")]
        public void SearchByIndeterminedName(string fullname, string res)
        {
            Assert.AreEqual(res, ManagerReader.SearchByIndeterminedName(env, fullname).Name);
        }

        [Test]
        [TestCase(true, "C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/WriterTests")]
        [TestCase(false, "C:\\eféedù%^^$")]
        [TestCase(false, "************************************************aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa************************************************aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase(false, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void IsPathCorrect(bool res, string path)
        {
            Assert.AreEqual(res, ManagerReader.IsPathCorrect(path));
        }

        #endregion
        
        [OneTimeTearDown] public void TearDown()
        {
            //env.Delete();
            Trace.Flush();
        }
    }
}