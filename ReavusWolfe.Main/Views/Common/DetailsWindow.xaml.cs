using ReavusWolfe.Main.ViewModels.Common;

namespace ReavusWolfe.Main.Views.Common
{
    public partial class DetailsWindow
    {
        public DetailsWindow(DetailsWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
