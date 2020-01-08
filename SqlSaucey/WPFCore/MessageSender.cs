using System;
namespace WPFCore
{
    public class MessageSender
    {
        private object _Owner;
        public event EventHandler<MessageEventArgs> Message;
        public void Send(string message)
        {
            if (Message == null) return;
            Message.Invoke(_Owner, new MessageEventArgs(message));
        }
        public MessageSender(object owner) {
            _Owner = owner;
        }
    }
}
