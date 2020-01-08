using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System.Windows.Input;
using SqlSaucey.Helpers;
using SqlSaucey.Interfaces;
using JSON;
using SqlSaucey.Strings;
using System;

namespace SqlSaucey.ViewModels
{
    public class ConsoleViewModel : ViewModel, IConsole
    {
        #region Constants
        #endregion
        #region Properties
        #endregion Properties
        #region Constructor
        public ConsoleViewModel()
        {

        }
        public void Info(string message)
        {
            JObject jObject = new JObject();
            jObject[SL.TYPE] = SL.INFO;
            jObject[SL.MESSAGE] = message;
            SendMessage(jObject.Serialize());
        }
        public void Error(string message)
        {
            JObject jObject = new JObject();
            jObject[SL.TYPE] = SL.ERROR;
            jObject[SL.MESSAGE] = message;
            SendMessage(jObject.Serialize());
        }

        public void WriteLine(string message)
        {
            Info(message);
        }

        public void WriteLine(Exception ex)
        {
            Error(ex.ToString());
        }
        #endregion Constructor
        #region Methods
        #endregion Methods
    }
}