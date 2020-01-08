using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
using System.Collections.ObjectModel;
using SqlSaucey.Interfaces;
namespace SqlSaucey.ViewModels
{
    public class SetupsViewModel :ViewModel
    {
        #region Properties
        private IConsole _IConsole;
        private SetupViewModel _ActiveSetupViewModel;
        private ObservableCollection<SetupViewModel> _ActiveSetupViewModels = new ObservableCollection<SetupViewModel>();
        public ObservableCollection<SetupViewModel> ActiveSetupViewModels { get { return _ActiveSetupViewModels; } }
        private SetupViewModel ActiveSetupViewModel
        {
            set
            {
                if (_ActiveSetupViewModel == value) return;
                _ActiveSetupViewModels.Clear();
                _ActiveSetupViewModel = value;
                _ActiveSetupViewModels.Add(value);
                OnPropertyChange("ActiveSetupViewModels");
            }
        }
        #endregion Properties
        #region Constructor
        public SetupsViewModel(Setups setups, IConsole iConsole)
        {
            _IConsole = iConsole;
            setups.ActiveSetupChanged += ActiveSetupChanged;
            if (setups.ActiveSetup == null) return;
            ActiveSetupViewModel = new SetupViewModel(setups.ActiveSetup, _IConsole);
        }
        #endregion Constructor
        #region Methods
        private void ActiveSetupChanged(object sender, SetupEventArgs e)
        {
            ActiveSetupViewModel = new SetupViewModel(e.Setup, _IConsole);
        }
        #endregion Methods
    }
}