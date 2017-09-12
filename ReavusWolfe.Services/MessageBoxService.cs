using System.Windows;

namespace ReavusWolfe.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message, string title, MessageBoxButton buttons, MessageBoxImage messageBoxImage);
        MessageBoxResult ShowDeleteConfirmation();
        MessageBoxResult ShowRestoreConfirmation();
        MessageBoxResult Show(string message, string title);
        void ShowError(string message);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message, string title, MessageBoxButton buttons,
            MessageBoxImage messageBoxImage)
        {
            return MessageBox.Show(message, title, buttons, messageBoxImage);
        }

        public MessageBoxResult Show(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageBoxResult ShowDeleteConfirmation()
        {
            return MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public MessageBoxResult ShowRestoreConfirmation()
        {
            return MessageBox.Show("Are you sure you want to restore this record?", "Confirm Restoration", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}