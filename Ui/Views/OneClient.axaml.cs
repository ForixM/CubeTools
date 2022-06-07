using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.Error;

namespace Ui.Views
{
    public class OneClient : UserControl
    {
        public Client Client;
        public static OneClient LastReference;
        public Window Main;

        public ActionView ActionView;
        public NavigationView NavigationView;
        public PointersView PointersView;

        public OneClient()
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
        public OneClient(Client client)
        {
            LastReference = this;
            Main = (Window) Parent;
            InitializeComponent();
            ActionView = this.FindControl<ActionView>("ActionView");
            NavigationView = this.FindControl<NavigationView>("NavigationView");
            PointersView = this.FindControl<PointersView>("PointersView");
            Client = client;
            NavigationView.AccessPath(Client.CurrentFolder);
            NavigationView.Add(Client.CurrentFolder);
            PointersView.Refresh();
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
                    NavigationView.AccessPath(pointer);
                    PointersView.Refresh();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void AccessPath(string path)
        {
            try
            {
                AccessPath(Client.GetItem(path)!);
            }
            catch (Exception) {}
        }

        /// <summary>
        /// Reload the current directory
        /// </summary>
        public void Refresh()
        {
            try
            {
                Client.Children = Client.ListChildren()!;
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException) new ErrorBase(@managerException).ShowDialog<object>(Main);
            }
            PointersView.Refresh();
        }

        /// <summary>
        ///  Reload the current directory (not the pointer) by displaying specific pointers
        /// </summary>
        /// <param name="list">the list of pointer to display</param>
        public void Refresh(List<Pointer> list) => PointersView.Refresh(list);
        
        #endregion
    }
}