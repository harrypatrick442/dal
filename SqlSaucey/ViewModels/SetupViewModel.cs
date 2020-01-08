using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System.Windows.Input;
using SqlSaucey.Helpers;
using SqlSaucey.Interfaces;
using System.Threading.Tasks;
using System;

namespace SqlSaucey.ViewModels
{
    public class SetupViewModel : ViewModel
    {
        #region Properties
        private Setup _Setup;
        private IConsole _IConsole;
        public bool RunButtonsEnabled { get { return !_Running; } }
        public string Directory
        {
            get
            {
                return _Setup.Directory;
            }
            set
            {
                _Setup.Directory = value;
                OnPropertyChange();
            }
        }
        public string Name
        {
            get
            {
                return _Setup.Name;
            }
            set
            {
                _Setup.Name = value;
                OnPropertyChange("Name");
            }
        }
        private ICommand _WriteToDatabaseCommand;
        public ICommand WriteToDatabaseCommand
        {
            get
            {
                //Console.WriteLine("Test");
                if (_WriteToDatabaseCommand == null) _WriteToDatabaseCommand = new CommandHandler(WriteToDatabase);
                return _WriteToDatabaseCommand;
            }
        }
        private ICommand _ReadFromDatabaseCommand;
        public ICommand ReadFromDatabaseCommand
        {
            get
            {
                if (_ReadFromDatabaseCommand == null) _ReadFromDatabaseCommand = new CommandHandler(ReadFromDatabase);
                return _ReadFromDatabaseCommand;
            }
        }
        private volatile bool __ReadingFromDatabase = false;
        private bool _Running { get { return __ReadingFromDatabase; } set { __ReadingFromDatabase = value; OnPropertyChange(nameof(RunButtonsEnabled)); } }
        private void ReadFromDatabase()
        {
            Task.Run((Action)(() =>
            {
                if (_Running) return;
                _Running = true;
                try
                {
                    Transfer.ReadFromDatabase(_Setup.Connection, _Setup.Directory, _IConsole);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                _Running = false;
            }));
        }
        private void WriteToDatabase()
        {
            Task.Run((Action)(() =>
            {
                if (this._Running) return;
                this._Running = true;
                try
                {
                    Transfer.WriteToDatabase(_Setup.Connection, _Setup.Directory, _IConsole);
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                this._Running = false;
            }));
        }
        private ConnectionViewModel _ConnectionViewModel;
        public ConnectionViewModel Connection
        {
            get
            {
                if (_ConnectionViewModel == null)
                {
                    _ConnectionViewModel = new ConnectionViewModel(_Setup.Connection);
                }
                return _ConnectionViewModel;
            }
        }
        #endregion Properties
        #region Constructor
        public SetupViewModel(Setup setup, IConsole iConsole)
        {
            _Setup = setup;
            _IConsole = iConsole;
        }
        private void Log(string message)
        {
            SendMessage(message);
        }
        #endregion Constructor
        #region Methods
        #endregion Methods
    }
}