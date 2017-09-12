using System;
using GalaSoft.MvvmLight.Messaging;

namespace ReavusWolfe.Services
{
    public interface IMessengerService : IDisposable
    {
        void Send<TMessage>(TMessage message);
        void Register<TMessage>(Object receipient, Action<TMessage> action);
        void Unregister<TMessage>(Object receipient, Action<TMessage> action);
        void UnregisterAllMessages(object receipient);
    }

    public class MessengerService : IMessengerService
    {
        public void Send<TMessage>(TMessage message)
        {
            Messenger.Default.Send(message);
        }

        public void Register<TMessage>(object receipient, Action<TMessage> action)
        {
            Messenger.Default.Register(receipient, action);
        }

        public void Unregister<TMessage>(object receipient, Action<TMessage> action)
        {
            Messenger.Default.Unregister(receipient, action);
        }

        public void UnregisterAllMessages(object receipient)
        {
            Messenger.Default.Unregister(receipient);
        }

        public void Dispose()
        {

        }
    }
}