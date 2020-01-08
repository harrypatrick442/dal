using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using SqlSaucey.Core;
using SqlSaucey.Models;
namespace SqlSaucey.Models
{
    public class Setups
    {
        #region Constants
        private const string SETUPS = "Setups";
        #endregion
        #region Properties
        public EventHandler<SetupEventArgs> ActiveSetupChanged;
        public EventHandler<SetupsEventArgs> Changed;
        public EventHandler<SetupEventArgs> SetupChanged;
        private Setup _ActiveSetup;
        public Setup ActiveSetup { get { return _ActiveSetup; } set { SetActiveSetup(value); } }
        private List<Setup> _Setups = new List<Setup>();
        #endregion
        #region Constructors
        public Setups()
        {

        }
        #endregion
        #region Methods
        public Setup[] ToArray() => _Setups.ToArray();
        public Setup CreateSetup()
        {
            var setup = new Setup();
            Add(setup);
            SetActiveSetup(setup);
            return setup;
        }
        public Setup OpenSetup(string fileName) {
            return null;
        }
        private void _Add(Setup setup)
        {
            _Setups.Add(setup);
            setup.Changed += CallbackSetupChanged;
            if (_ActiveSetup == null)
                SetActiveSetup(setup);
        }
        public void Add(Setup setup) {
            _Add(setup);
            DispatchChanged();
        }
        private void SetActiveSetup(Setup setup)
        {
            if (setup == _ActiveSetup) return;
            _ActiveSetup = setup;
            if (ActiveSetupChanged == null) return;
            DispatchActiveSetupChanged(setup);
            DispatchChanged();
        }
        private void DispatchActiveSetupChanged(Setup setup)
        {
            ActiveSetupChanged.Invoke(this, new SetupEventArgs(setup));
        }
        private void DispatchChanged()
        {
            Changed.Invoke(this, new SetupsEventArgs(this));
        }
        private void DispatchSetupChanged(Setup setup)
        {
            SetupChanged.Invoke(this, new SetupEventArgs(setup));
        }
        private void CallbackSetupChanged(object sender, SetupEventArgs e)
        {
            DispatchSetupChanged(e.Setup);
        }
        #endregion
        public XElement ToXML()
        {
            XElement xElement = new XElement(SETUPS);
            foreach (var setup in _Setups)
            {
                xElement.Add(setup.ToXML());
            }
            return xElement;
        }
        public static Setups From(XElement xElement)
        {
            Setups setups = new Setups();
            foreach (var childXElement in xElement.Elements())
            {
                if (childXElement.NodeType != XmlNodeType.Element) continue;
                Setup setup = Setup.From(childXElement);
                setups._Add(setup);
            }
            return setups;
        }
    }
}
