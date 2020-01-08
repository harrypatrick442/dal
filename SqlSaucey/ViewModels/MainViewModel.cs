using System.ComponentModel;
using System.Linq;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SqlSaucey.Strings;
namespace SqlSaucey.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region EventHandlers
        #endregion
        #region Properties
        private Configuration _Configuration;
        private Setups _Setups { get { return _Configuration.Setups; } }
        private Models.Menu __Menu;
        private Models.Menu _Menu { get { if (__Menu == null) __Menu = new Models.Menu(_Setups); return __Menu; } }
        private Connection _Connection { get { return _Configuration.Connection; } }
        private MenuViewModel _MenuViewModel;
        public MenuViewModel Menu
        {
            get
            {
                if (_MenuViewModel == null)
                {
                    _MenuViewModel = new MenuViewModel(_Menu, Console);
                    _MenuViewModel.ShowConnections += ShowConnectionsCallback;
                }
                return _MenuViewModel;
            }
        }
        private ConnectionViewModel _ConnectionViewModel;
        public ConnectionViewModel Connection
        {
            get
            {
                if (_ConnectionViewModel == null) _ConnectionViewModel = new ConnectionViewModel(_Connection);
                return _ConnectionViewModel;
            }
        }
        private ConsoleViewModel _ConsoleViewModel;
        public ConsoleViewModel Console
        {
            get
            {
                if (_ConsoleViewModel == null)
                    _ConsoleViewModel = new ConsoleViewModel();
                return _ConsoleViewModel;
            }
        }
        private SetupsViewModel _SetupsViewModel;
        public SetupsViewModel Setups
        {
            get
            {
                if (_SetupsViewModel == null)
                {
                    _SetupsViewModel = new SetupsViewModel(_Setups, Console);
                }
                return _SetupsViewModel;
            }
        }
        #endregion Properties
        #region Constructor
        public MainViewModel(Configuration configuration)
        {
            _Configuration = configuration;
        }
        #endregion Constructor
        #region Events
        private void ShowConnectionsCallback(object sender, EventArgs e)
        {
            DispatchShowConnections();
        }
        #endregion
        #region Methods
        private void DispatchShowConnections()
        {
            SendMessage(SL.SHOW_CONNECTIONS);
        }
        #endregion Methods
    }
}