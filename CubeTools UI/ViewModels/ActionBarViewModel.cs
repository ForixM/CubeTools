using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using CubeTools_UI.Models;
using CubeTools_UI.Views;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using ReactiveUI;

namespace CubeTools_UI.ViewModels;

public class ActionBarViewModel : ReactiveObject
{
    // REFERENCE TO M
    private MainWindowModel _model;
    public MainWindowModel ModelXaml
    {
        get => _model;
        set => _model = value;
    }

    private ActionBarModel _modelActionBar;
    public ActionBarModel ModelActionBarXaml
    {
        get => _modelActionBar;
        set => _modelActionBar = value;
    }

    public ActionBarViewModel()
    {
        _model = null;
        _modelActionBar = null;
    }

    #region Bindings

    /// <summary>
    /// Binding Method : Create File within XAML code
    /// </summary>
    public void CreateFile()
    {
        try
        {
            ModelXaml.ModelNavigationBar.DirectoryPointer.AddFile("New File","txt");
        }
        catch (Exception e)
        {
            if (e is ManagerException @managerException)
                ModelXaml.ViewModel.ErrorMessageBox(@managerException, "Unable to create a new file");
            else 
                throw;
        }
    }
    
    /// <summary>
    /// Binding Method : Create a directory within XAML code
    /// </summary>
    public void CreateDir()
    {
        try
        {
            ModelXaml.ModelNavigationBar.DirectoryPointer.AddDir( 
                ManagerReader.GenerateNameForModification(ModelXaml.ModelNavigationBar.DirectoryPointer.Path + "/New Folder"));
        }
        catch (Exception e)
        {
            if (e is ManagerException @managerException)
                ModelXaml.ViewModel.ErrorMessageBox(@managerException, "Unable to create a new file");
            else 
                throw;
        }
    }
    
    /// <summary>
    /// Binding Method : Copy Selected and Clear Cut
    /// </summary>
    public void Copy()
    {
        ModelActionBarXaml.CopiedXaml.Clear();
        ModelActionBarXaml.CutXaml.Clear();
        foreach (var ft in ModelActionBarXaml.SelectedXaml) ModelActionBarXaml.CopiedXaml.Add(ft);
    }

    /// <summary>
    /// Binding Method : Clear Cut and Copy and then Fill them with Selected
    /// </summary>
    public void Cut()
    {
        ModelActionBarXaml.CopiedXaml.Clear();
        ModelActionBarXaml.CutXaml.Clear();
        foreach (var ft in ModelActionBarXaml.SelectedXaml)
        {
            ModelActionBarXaml.CopiedXaml.Add(ft);
            ModelActionBarXaml.CutXaml.Add(ft);
        }
    }

    /// <summary>
    /// Binding Method : Copy copied and destroy cut
    /// </summary>
    public void Paste()
    {
        try
        {
            foreach (var ft in ModelXaml.ModelActionBar.CopiedXaml)
                _model.ModelNavigationBar.DirectoryPointer.AddChild(ManagerWriter.Copy(ft.Name));
            foreach (var ft in ModelXaml.ModelActionBar.CutXaml)
            {
                ManagerWriter.Delete(ft);
                _model.ModelNavigationBar.DirectoryPointer.Remove(ft);
            }
        }
        catch (Exception e)
        {
            if (e is ManagerException @managerException)
                ModelXaml.ViewModel.ErrorMessageBox(@managerException, "Unable to copy");
            else 
                throw;
        }
    }

    /// <summary>
    /// Binding Method : Rename selected files
    /// </summary>
    public void Rename()
    {
        if (ModelXaml.ModelActionBar.SelectedXaml.Count >= 1)
        {
            if (ModelXaml.ModelActionBar.SelectedXaml.Count == 1)
            {
                // TODO Implement system of popup dialog
            }
            else
            {
                ModelXaml.ViewModel.ErrorMessageBox(new ManagerException(), "Unable to rename multiple data");
            }
        }
    }

    /// <summary>
    /// Binding Method : Destroy all selected
    /// </summary>
    public void Delete()
    {
        List<string> undestroyed = new List<string>();
        ManagerException lastError = new ManagerException();
        foreach (var ft in ModelXaml.ModelActionBar.SelectedXaml)
        {
            try
            {
                ManagerWriter.Delete(ft);
                ModelXaml.ModelNavigationBar.DirectoryPointer.Remove(ft);
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                {
                    undestroyed.Add(ft.Path);
                    lastError = managerException;
                }
                else throw;
            }
        }

        if (undestroyed.Count != 0)
        {
            string res = "";
            foreach (var s in undestroyed)
                res += s + ",";
            ModelXaml.ViewModel.ErrorMessageBox(lastError, "CubeTools was unable to destroy : " +res);
        }
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void NearbySendBtnClick()
    {
        // TODO Add method for NearBySend
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void SearchBtnClick()
    {
        // TODO Implement Popup for searching
    }
    #endregion
    
}