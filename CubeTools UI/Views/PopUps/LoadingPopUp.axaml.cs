using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models.PopUps;

namespace CubeTools_UI.Views.PopUps
{
    public class LoadingPopUp : Window
    {
        private operationType _type;
        private double max;
        
        private ProgressBar _progressBar;
        private TextBlock _operationType;
        private LoadingPopUpModel _viewModel;
        
        #region Init
        
        public LoadingPopUp()
        {
            InitializeComponent();
            _progressBar = this.FindControl<ProgressBar>("ProgressBar");
            _operationType = this.FindControl<TextBlock>("OperationType");
            SelectMessage(operationType.None);
            max = 100;
        }
        public LoadingPopUp(ref int current, int max, operationType type) : this()
        {
            _type = type;
            SelectMessage(type);
            this.max = max;
        }
        
        public enum operationType
        {
            None,
            Copy,
            Delete
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Process

        private void SelectMessage(operationType type)
        {
            switch (type)
            {
                case operationType.Copy :
                    _operationType.Text = "Copying ";
                    break;
                case operationType.Delete :
                    _operationType.Text = "Deleting ";
                    break;
                default:
                    _operationType.Text = "Loading ...";
                    break;
            }
        }
        
        #endregion
    }
}