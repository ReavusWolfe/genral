using ReavusWolfe.Main.ViewModels.Common;

namespace ReavusWolfe.Main.Views.Common
{
    public partial class MainWindow
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
