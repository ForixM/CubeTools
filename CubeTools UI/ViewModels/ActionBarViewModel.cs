using Avalonia;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using ReactiveUI;

namespace CubeTools_UI.ViewModels;

public class ActionBarViewModel : ReactiveObject
{
    // INIT AND VARIABLES :
    
    public MainWindowViewModel MainWindow;

    public ActionBarViewModel()
    {
        MainWindow = new MainWindowViewModel();
    }
    public ActionBarViewModel(MainWindowViewModel parent)
    {
        MainWindow = parent;
    }

    #region Bindings

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void CreateBtnClick()
    {
        try
        {
            //Create("New File");
            MainWindow.ErrorMessageBox(new ManagerException(), "Test");
        }
        catch (ManagerException e)
        {
            //MainWindow.ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
        }
    }
    

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void CopyBtnClick()
    {
        MainWindow.Copied.Clear();
        foreach (var ft in MainWindow.Selected) MainWindow.Copied.Add(ft);
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void CutBtnClick()
    {
        // TODO Implement cut function
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void PasteBtnClick()
    {
        try
        {
            MainWindow.Copy(MainWindow.Copied);
        }
        catch (ManagerException e)
        {
            //MainWindow.ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
        }
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void RenameBtnClick()
    {
    }

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    public void DeleteBtnClick()
    {
        try
        {
            MainWindow.DeleteSelected(MainWindow.Selected);
        }
        catch (ManagerException e)
        {
            //MainWindow.ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
        // TODO Display the Pointer
    }
    #endregion

    #region Actions

    /// <summary>
    ///     - Action : <br></br>
    ///     - XAML : <br></br>
    ///     - Implementation :
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private void Create(string name)
    {
        name = ManagerReader.GetNameToPath(name);
        ManagerWriter.Create(name);
        MainWindow.DirectoryPointer.ChildrenFiles.Add(ManagerReader.ReadFileType(name));
    }
    
    

    #endregion
}