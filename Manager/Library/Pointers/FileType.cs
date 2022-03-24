﻿using System;
using System.IO;
using Library.ManagerExceptions;

namespace Library.Pointers
{
    public class FileType
    {
        #region Variables

        // This region contains every variables that stores the information of the file

        public static FileType NullPointer = new();

        // Basics
        public string Path;
        public string Name;
        public string Type;
        public long Size;

        // Date
        public string Date;
        public string LastDate;
        public string AccessDate;

        // Attributes
        public bool ReadOnly;

        public bool Hidden;
        public bool Compressed;
        public bool Archived;

        public bool IsDir;
        
        #endregion

        #region Init

        // Constructors
        /// <summary>
        ///     - Action : Generate a null pointer
        ///     - Implementation : Check
        /// </summary>
        public FileType()
        {
            Path = "";
            Name = "";
            Type = "";
            Size = 0;
            Date = "";
            LastDate = "";
            AccessDate = "";
            Hidden = false;
            Compressed = false;
            Archived = false;
            ReadOnly = false;
            IsDir = false;
        }

        /// <summary>
        ///     - Action : Generate a pointer with it path only <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to load</param>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        public FileType(string path) : this()
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Dispose();
                throw new PathNotFoundException(path + " does not exist", "Constructor FileType");
            }
            // Path
            Path = path;
            // Primary
            Name = ManagerReader.ManagerReader.GetPathToName(Path);
            Size = ManagerReader.ManagerReader.FastReaderFiles(Path);
            IsDir = Directory.Exists(Path);
            Type = ManagerReader.ManagerReader.GetFileExtension(Path);
            // Properties
            ReadOnly = ManagerReader.ManagerReader.HasAttribute(FileAttributes.ReadOnly, Path);
            Hidden = ManagerReader.ManagerReader.HasAttribute(FileAttributes.Hidden, Path);
            Archived = ManagerReader.ManagerReader.HasAttribute(FileAttributes.Archive, Path);
            // Time
            Date = ManagerReader.ManagerReader.GetFileCreationDate(Path);
            LastDate = ManagerReader.ManagerReader.GetFileLastEdition(Path);
            AccessDate = ManagerReader.ManagerReader.GetFileAccessDate(Path);
        }

        // Initializers

        #endregion

        #region Delete

        // This region is not completed, memory will be fixed later

        // Destructor and Garbage Collector
        ~FileType()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Operator

        private bool IsNull()
        {
            if (File.Exists(Name) || Directory.Exists(Name) || File.Exists(Path) || Directory.Exists(Path))
                return false;

            return true;
        }

        public static bool operator ==(FileType ft, FileType ft2)
        {
            if (ft is null && ft2 is null)
                return true;
            if ((!File.Exists(ft.Path) || !Directory.Exists(ft.Path)) &&
                ft.IsNull() || (!File.Exists(ft2.Path) || !Directory.Exists(ft2.Path)) && ft.IsNull())
                return true;
            if (ft.Name != "" && ft2.Name != null)
            {
                var res = true;
                res &= ft.Path == ft2.Path;
                res &= ft.Name == ft2.Name;
                res &= ft.Size == ft2.Size;
                res &= ft.Date == ft2.Date;
                res &= ft.LastDate == ft2.LastDate;
                res &= ft.AccessDate == ft2.AccessDate;
                res &= ft.Hidden == ft2.Hidden;
                res &= ft.Compressed == ft2.Compressed;
                res &= ft.Archived == ft2.Archived;
                res &= ft.ReadOnly == ft2.ReadOnly;
                res &= ft.IsDir == ft2.IsDir;
                return res;
            }

            return false;
        }

        public static bool operator !=(FileType ft, FileType ft2)
        {
            return !(ft == ft2);
        }

        protected bool Equals(FileType other)
        {
            return Path == other.Path && Name == other.Name && Type == other.Type && Size == other.Size &&
                   Date == other.Date && LastDate == other.LastDate && AccessDate == other.AccessDate &&
                   ReadOnly == other.ReadOnly && Hidden == other.Hidden && Compressed == other.Compressed &&
                   Archived == other.Archived && IsDir == other.IsDir;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FileType) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Path);
            hashCode.Add(Name);
            hashCode.Add(Type);
            hashCode.Add(Size);
            hashCode.Add(Date);
            hashCode.Add(LastDate);
            hashCode.Add(AccessDate);
            hashCode.Add(ReadOnly);
            hashCode.Add(Hidden);
            hashCode.Add(Compressed);
            hashCode.Add(Archived);
            hashCode.Add(IsDir);
            return hashCode.ToHashCode();
        }

        #endregion

        #region CLI

        public void PrintInformation()
        {
            Console.WriteLine("--FileType DEBUG--");
            Console.WriteLine("Path : " + Path);
            Console.WriteLine("Type : " + Type);
            Console.WriteLine("Size : " + Size);
            Console.WriteLine("Date : " + Date);
            Console.WriteLine("LastDate : " + LastDate);
            Console.WriteLine("AccessDate :" + AccessDate);
            Console.WriteLine("ReadOnly : " + ReadOnly);
            Console.WriteLine("Hidden : " + Hidden);
            Console.WriteLine("Directory : " + IsDir);
        }

        public void PrettyPrint()
        {
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");
            Console.WriteLine($"### Pointer to : {Path} ###");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");

            Console.WriteLine($"## Name : {Name} ##  ## Type : {Type} ##");
            Console.WriteLine($"## Size : {Size} ##  ## Creation date : {Date} ##");
            Console.WriteLine($"## Last modification : {LastDate} ##");
            Console.WriteLine($"## Last access : {AccessDate} ##");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");

            Console.WriteLine($"$$ Hidden : {Hidden} $$  $$ Is a directory : {IsDir}  $$");
            Console.WriteLine($"$$ ReadOnly : {ReadOnly} $$  $$ Archived : {Archived} $$");
            Console.WriteLine($"$$ Compressed : {Compressed} $$");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");
        }

        #endregion
    }
}