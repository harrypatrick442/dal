using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using SqlSaucey.Core;
using System;

namespace SqlSaucey.Models
{
    public class Menu
    {
        public EventHandler<EventArgs> SetupsChanged;
        public EventHandler<SetupEventArgs> SetupChanged;
        private Setups _Setups;
        public Setup[] SetupOptions { get { return _Setups.ToArray(); } }
        public Setup ActiveSetup { set { _Setups.ActiveSetup = value; } get { return _Setups.ActiveSetup; } }
        public Menu(Setups setups)
        {
            _Setups = setups;
            setups.Changed += CallbackSetupsChanged;
            setups.SetupChanged += CallbackSetupChanged;

        }
        public void CreateNewSetup()
        {
            _Setups.CreateSetup();
        }
        public void OpenSetup(string fileName) {
            _Setups.OpenSetup(fileName);
        }
        private void CallbackSetupsChanged(object sender, SetupsEventArgs e)
        {
            DispatchSetupsChanged();
        }
        private void CallbackSetupChanged(object sender, SetupEventArgs e)
        {
            DispatchSetupChanged(e);
        }
        private void DispatchSetupsChanged()
        {
            if (SetupsChanged == null) return;
            SetupsChanged.Invoke(this, new EventArgs());
        }
        private void DispatchSetupChanged(SetupEventArgs e)
        {
            if (SetupChanged == null) return;
            SetupChanged.Invoke(this, e);
        }
    }
}
