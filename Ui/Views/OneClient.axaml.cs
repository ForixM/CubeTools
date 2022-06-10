using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.ActionButtons;
using Ui.Views.Error;
using Ui.Views.MenuController;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class ClientUI : UserControl
    {
        public Client Client;
        public static ClientUI LastReference;
        public Window Main;

        public ActionView ActionView;
        public NavigationView NavigationView;
        public PointersView PointersView;
        public Grid Monsieur;

        public ClientUI()
        {
            LastReference = this;
            Main = (Window) Parent;
            InitializeComponent();
            ActionView = this.FindControl<ActionView>("ActionView");
            NavigationView = this.FindControl<NavigationView>("NavigationView");
            PointersView = this.FindControl<PointersView>("PointersView");

            string path = Directory.GetCurrentDirectory().Replace('\\', '/');
            Client = new ClientLocal();
            Pointer? entry = Client.GetItem(path, true);
            NavigationView.AccessPath(entry!);
            NavigationView.Add(entry!);
            PointersView.Refresh();
        }
        public ClientUI(Client client, Window Parent)
        {
            LastReference = this;
            Main = (Window) Parent;
            InitializeComponent();
            ActionView = this.FindControl<ActionView>("ActionView");
            NavigationView = this.FindControl<NavigationView>("NavigationView");
            PointersView = this.FindControl<PointersView>("PointersView");
            Client = client;
            if (Parent is not MainWindow)
            {
                NavigationView.AccessPath(Client.CurrentFolder);
                NavigationView.Add(Client.CurrentFolder);
                PointersView.Refresh();
            }
            else
            {
                
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #region Process
        
        /// <summary>
        /// Access the given path by reloading the current directory or accessing a file
        /// </summary>
        /// <param name="pointer">The given path to access (Either a file or a directory)</param>
        public void AccessPath(Pointer pointer)
        {
            if (Client.Type is ClientType.LOCAL)
            {
                if (File.Exists(pointer.Path))
                {
                    try
                    {
                        ManagerReader.AutoLaunchAppProcess(pointer.Path);
                    }
                    catch (Exception e)
                    {
                        if (e is ManagerException @managerException)
                            new ErrorBase(@managerException).ShowDialog<object>(Main);
                        else
                            new ErrorBase(new SystemErrorException("System was unable to open your file", "AccessPath"))
                                .ShowDialog<bool>(Main);
                    }
                }
                else
                {
                    Client.AccessPath(pointer);
                    NavigationView.AccessPath(pointer);
                    PointersView.Refresh();
                }
            }
            else
            {
                if (!pointer.IsDir)
                {
                    try
                    {
                        //ManagerReader.AutoLaunchAppProcess(Client.DownloadFile(Client, ));
                    }
                    catch (Exception e)
                    {
                        if (e is ManagerException @managerException)
                            new ErrorBase(@managerException).ShowDialog<object>(Main);
                        else
                            new ErrorBase(new SystemErrorException("System was unable to open your file", "AccessPath"))
                                .ShowDialog<bool>(Main);
                    }
                }
                else
                {
                    Client.AccessPath(pointer);
                    NavigationView.AccessPath(pointer);
                    PointersView.Refresh();
                }
            }
        }

        /// <summary>
        /// Access the Pointer with its path
        /// </summary>
        /// <param name="path">The path of the item</param>
        public void AccessPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    NavigationView.CurrentPathXaml.Text = "";
                    new MenuController.Menu();
                    new MenuController.Menu(this);
                }
                else
                {
                    AccessPath(Client.GetItem(path)!);
                }
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).Show();
            }
        }

        /// <summary>
        /// Reload the current directory
        /// </summary>
        public void Refresh()
        {
            try
            {
                Client.Refresh();
                NavigationView.Refresh();
                PointersView.Refresh();
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).ShowDialog<object>(Main);
            }
        }

        /// <summary>
        ///  Reload the current directory (not the pointer) by displaying specific pointers
        /// </summary>
        /// <param name="list">the list of pointer to display</param>
        public void Refresh(List<Pointer> list) => PointersView.Refresh(list);

        public void TreatError(ErrorBase error, object result)
        {
            switch (error.Type)
            {
                case PopUpAction.INFO :
                    bool resultBool = (bool) result;
                    if (resultBool) Refresh();
                    break;
                case PopUpAction.REFRESH:
                    bool resultBool2 = (bool) result;
                    if (resultBool2) Refresh();
                    break;
            }
        }
        
        #endregion
    }
}