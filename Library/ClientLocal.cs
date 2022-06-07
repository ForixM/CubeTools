using System;
using System.Collections.Generic;
using System.IO;
using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerReader;
using Library.ManagerWriter;

namespace Ui.Views.Remote;

public class ClientLocal : Client
{
    public ClientLocal(ClientType type) : base(type)
    {
        
    }

    public override Pointer CreateFile(string name)
    {
        try
        {
            return ManagerWriter.Create(name);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public override Pointer CreateFolder(string name)
    {
        try
        {
            return ManagerWriter.CreateDir(name);
        } catch (Exception e)
        {
            return null;
        }
    }

    public override Pointer? Copy(Pointer pointer)
    {
        try
        {
            return ManagerWriter.Copy(pointer.Path, CurrentFolder.Path);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public override void Delete(Pointer pointer)
    {
        try
        {
            ManagerWriter.Delete(pointer.Path);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public override void Rename(Pointer pointer, string newName)
    {
        try
        {
            ManagerWriter.Rename(pointer.Path, newName);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public override void UploadFile(Client source, Pointer pointer, Pointer destination)
    {
        if (pointer is LocalPointer)
        {
            source.UploadFile(source, pointer, destination);
        }
    }

    public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
    {
        if (localPointer is LocalPointer)
        {
            source.UploadFolder(source, localPointer, destination);
        }
    }

    public override void AccessPath(Pointer destination)
    {
        CurrentFolder = new DirectoryLocalPointer(destination.Path);
        Directory.SetCurrentDirectory(CurrentFolder.Path);
        foreach (var pointer in Children) pointer.Dispose();
        Children.Clear();
        Children = ListChildren();
    }

    public override Pointer? GetItem(string path, bool isAbsolute = false)
    {
        if (!isAbsolute)
        {
            path = CurrentFolder.Path + "/" + path;
        }
        
        if (File.Exists(path))
        {
            return new FileLocalPointer(path);
        }
        
        if (Directory.Exists(path))
        {
            return new DirectoryLocalPointer(path);
        }

        return null;
    }

    public override string GetItemName(Pointer pointer) => ManagerReader.GetPathToName(pointer.Path);

    public override string GetItemType(Pointer pointer) => ManagerReader.GetFileExtension(pointer.Path);

    public override long GetItemSize(Pointer pointer) => pointer.Size;

    public override Pointer GetParentReference(Pointer pointer) =>  ManagerReader.GetRootPath(pointer.Path) == pointer.Path ? pointer : new DirectoryLocalPointer(pointer.ParentPath);

    public override List<Pointer>? ListChildren() => ((DirectoryLocalPointer) CurrentFolder).GetChildren().Cast<Pointer>().ToList();

    public override void InitializeProperties(Pointer pointer)
    {
        throw new System.NotImplementedException();
    }
}