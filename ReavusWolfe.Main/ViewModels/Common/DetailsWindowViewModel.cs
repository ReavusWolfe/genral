using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ReavusWolfe.Main.ViewModels.Common
{
    public class DetailsWindowViewModel : DetailsViewModelBase
    {
        public DetailsWindowViewModel()
        {
            WindowTitle = "Details Window";
        }

        public ICommand CloseCommand => new RelayCommand(Close);
    }
}