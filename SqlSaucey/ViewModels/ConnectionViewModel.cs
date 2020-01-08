using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System.Windows.Input;
using System;

namespace SqlSaucey.ViewModels
{
    public class ConnectionViewModel : ViewModel
    {
        #region Properties
        private Connection _Connection;
        public string User { get { return _Connection.Credentials.User; } set { _Connection.Credentials.User = value; OnPropertyChange(); } }
        public string Password { get{ return _Connection.Credentials.Password; }set { _Connection.Credentials.Password = value;  OnPropertyChange(); } }
        public string Server { get { return _Connection.Server; } set { _Connection.Server = value; OnPropertyChange(); } }
        public string Database { get { return _Connection.Database; } set { _Connection.Database = value; OnPropertyChange(); } }
        #endregion
        #region Constructors
        public ConnectionViewModel(Connection connection) {
            _Connection = connection;
        }
        #endregion
        #region Functions
        #endregion
    }
}