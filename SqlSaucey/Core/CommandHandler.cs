using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System.Windows.Input;
using System;

namespace SqlSaucey.Core
{
    public class CommandHandler : ICommand
    {
        private Action _Callback;
        public CommandHandler(Action callback)
        {
            _Callback = callback;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _Callback();
        }
    }
}