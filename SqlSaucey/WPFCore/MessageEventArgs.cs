using System;
namespace WPFCore
{
    public class MessageEventArgs : EventArgs
    {
        private string _Message;
        public string Message { get { return _Message; } }
        public MessageEventArgs(string message) : base() { 
             _Message = message;
        }
    }
}
