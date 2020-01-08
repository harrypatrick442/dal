using System;
using System.Windows;
using System.Windows.Controls;
using WPFCore.Exceptions;
namespace WPFCore
{
    public class MessageReciever
    {
        private object _DataContext;
        private bool _ExceptionIfCantBind;
        private Binding _Binding;
        private Action<string> _Callback;
        public MessageReciever(ContentControl view, Action<string> callback, bool exceptionIfCantBind = false)
        {
            _DataContext = view.DataContext;
            _Callback = callback;
            _ExceptionIfCantBind = exceptionIfCantBind;
            view.DataContextChanged += new DependencyPropertyChangedEventHandler(DataContextChanged);
            if (_DataContext == null) return;
            UpdateDataContext(_DataContext);
        }
        private void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateDataContext(e.NewValue);
        }
        private void UpdateDataContext(object dataContext)
        {
            if (_DataContext != null) ReleaseDataContext();
            _DataContext = dataContext;
            if (_DataContext == null) return;
            GraspDataContext();
        }
        private void ReleaseDataContext()
        {
            if (_Binding == null) return;
            _Binding.Release();
            _Binding = null;
        }
        private void GraspDataContext()
        {
            if (!typeof(IGetMessageSender).IsAssignableFrom(_DataContext.GetType()))
            {
                if (_ExceptionIfCantBind) throw new MessageReceiverException("Could not recieve messages from object of type " + _DataContext.GetType() + " because it does not implement IGetMessageSender");
                return;
            }
            IGetMessageSender iGetMessageSender = (IGetMessageSender)_DataContext;
            MessageSender messageSender = iGetMessageSender.MessageSender;
            messageSender.Message += OnMessage;
            _Binding = new Binding(iGetMessageSender, OnMessage);
        }
        private void OnMessage(object sender, MessageEventArgs e)
        {
            _Callback(e.Message);
        }
        private class Binding
        {
            private IGetMessageSender _IGetMessageSender;
            private EventHandler<MessageEventArgs> _Handler;
            public Binding(IGetMessageSender iGetMessageSender, EventHandler<MessageEventArgs> handler)
            {
                _IGetMessageSender = iGetMessageSender;
                _Handler = handler;
            }
            public void Release()
            {
                _IGetMessageSender.MessageSender.Message -= _Handler;
            }
        }
    }
}