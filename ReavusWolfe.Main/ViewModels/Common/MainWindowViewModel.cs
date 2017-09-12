using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ReavusWolfe.Main.Views.Common;
using ReavusWolfe.Main.Injection;
using ReavusWolfe.Services;

namespace ReavusWolfe.Main.ViewModels.Common
{
    public class MainWindowViewModel : ReavusWolfeViewModelBase
    {
        private readonly IInjector _injector;
        private readonly IMessageBoxService _messageBoxService;

        public MainWindowViewModel(IInjector injector, IMessageBoxService messageBoxService)
        {
            _injector = injector;
            _messageBoxService = messageBoxService;

            WindowTitle = "Main Window";
        }

        public string WelcomeMessageTestBinding => "This is a string set in the MainViewModel";

        public ICommand TestDialogCommand => new RelayCommand(ShowTestDialog);

        private void ShowTestDialog()
        {
            _messageBoxService.Show("This is a message box!", "Message Box", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public ICommand OpenDetailsWindowCommand => new RelayCommand(OpenDetailsWindow);

        private void OpenDetailsWindow()
        {
            var window = _injector.GetUniqueInstance<DetailsWindow>();
            window.ShowDialog();
        }
    }
}
