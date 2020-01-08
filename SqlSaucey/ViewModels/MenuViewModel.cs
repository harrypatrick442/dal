using System.ComponentModel;
using System.Linq;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using SqlSaucey.Interfaces;

namespace SqlSaucey.ViewModels
{
    public class MenuViewModel : ViewModel
    {
        #region EventHandlers
        public event EventHandler<EventArgs> ShowConnections;
        private ICommand _ClickNewSetup;
        public ICommand ClickNewSetup
        {
            get
            {
                if (_ClickNewSetup == null)
                {
                    _ClickNewSetup = new CommandHandler(_Menu.CreateNewSetup);
                }
                return _ClickNewSetup;
            }
        }
        private ICommand _ClickShowConnections;
        public ICommand ClickShowConnections
        {
            get
            {
                if (_ClickShowConnections == null)
                {
                    _ClickShowConnections = new CommandHandler(DispatchShowConnections);
                }
                return _ClickShowConnections;
            }
        }
        #endregion
        #region Properties
        private Models.Menu _Menu;
        private IConsole _IConsole;
        //private ObservableCollection<Setup> _SetupItems;
        public ObservableCollection<Setup> SetupItems { get {
                return new ObservableCollection<Setup>( _Menu.SetupOptions);
                //return _SetupItems;
            }
        }
        public Setup SelectedSetupItem { get {
                return _Menu.ActiveSetup;
            } set {
                _Menu.ActiveSetup = value;
            }
        }
        private ICommand _ImportClicked;
        public ICommand ImportClicked
        {
            get
            {
                if (_ImportClicked == null) _ImportClicked = new CommandHandler(() => {
                    var openFileDialog = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog.ShowDialog();
                    if (string.IsNullOrEmpty(openFileDialog.FileName)) return;
                    try
                    {
                        _Menu.OpenSetup(openFileDialog.FileName);
                    }
                    catch (Exception ex) {
                        _IConsole.WriteLine(ex);
                    }
                });
                return _ImportClicked;
            }
        }
        private ICommand _ExportClicked;
        public ICommand ExportClicked
        {
            get
            {
                if (_ExportClicked == null) _ExportClicked = new CommandHandler(() => {

                });
                return _ExportClicked;
            }
        }
        #endregion Properties
        #region Constructor
        public MenuViewModel(Menu menu, IConsole iConsole)
        {
            _Menu = menu;
            _IConsole = iConsole;
            _Menu.SetupsChanged += SetupsChanged;   
            _Menu.SetupChanged += SetupChanged;
        }
        #endregion Constructor
        #region Events
        private void SetupsChanged(object sender, EventArgs e)
        {
            UpdateSetupItems();
        }
        private void SetupChanged(object sender, SetupEventArgs e)
        {
            UpdateSetupItems();
        }
        #endregion
        #region Methods
        private void DispatchShowConnections() {
            if (ShowConnections == null) return;
            ShowConnections.Invoke(this, new EventArgs());
        }
        private void UpdateSetupItems()
        {
            OnPropertyChange("SetupItems");
            OnPropertyChange("SelectedSetupItem");
        }
        #endregion Methods
    }
}