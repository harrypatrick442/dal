using System.ComponentModel;
using System.Linq;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using SqlSaucey.Interfaces;
using WPFCore;

namespace SqlSaucey.ViewModels
{
    public class ViewModel : INotifyPropertyChanged, IGetMessageSender
    {
        private MessageSender _MessageSender;
        public MessageSender MessageSender
        {
            get
            {
                if (_MessageSender == null) {
                    _MessageSender = new MessageSender(this);
                }
                return _MessageSender;
            }
        }
        public void SendMessage(string message) {
            MessageSender.Send(message);
        }
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Properties
        #region Constructor
        #endregion Constructor
        #region Methods
        protected void OnPropertyChange([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Methods
    }
}