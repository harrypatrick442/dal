using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using System;
using SqlSaucey.Core;
using SqlSaucey.Strings;
namespace SqlSaucey.Models
{
    public class Setup
    {
        #region Constants
        #endregion
        #region Properties
        public event EventHandler<SetupEventArgs> Changed;
        private string _Directory;
        public string Directory {
            get {
                return _Directory;
            }
            set { _Directory = value; DispatchChanged(); }
        }
        private Connection _Connection;
        public Connection Connection { get { return _Connection; } }
        private string _Name;
        public string Name { get    { return _Name; 
            } set {
                _Name = value;
                DispatchChanged();
            } }
        #endregion
        #region Constructors
        public Setup() {
            _Connection = new Connection();
            AddEventListeners();
        }
        #endregion
        #region Methods
        public static Setup From(XElement xElement) {
            Setup setup = new Setup();
            foreach (var childXElement in xElement.Elements()) {
                if (childXElement.NodeType != XmlNodeType.Element) continue;
                switch (childXElement.Name.LocalName) {
                    case SL.DIRECTORY:
                        setup._Directory = childXElement.GetContent<string>();
                        break;
                    case SL.NAME:
                        setup._Name = childXElement.GetContent<string>();
                        break;
                    case SL.CONNECTION:
                        setup._Connection = Connection.From(childXElement);
                        break;
                }
            }
            setup.AddEventListeners();
            return setup;
        }
        public XElement ToXML() {
            XElement xElement = new XElement(SL.SETUP);
            xElement.Add(new XElement(SL.DIRECTORY, _Directory));
            xElement.Add(new XElement(SL.NAME, _Name));
            xElement.Add(_Connection.ToXML());
            return xElement;
        }
        public static Setup FromXML(string xmlFilePath)
        {
            XDocument xDocument = XDocument.Load(xmlFilePath);
            return From(xDocument.Root);
        }
        private void DispatchChanged()
        {
            if (Changed == null) return;
            Changed.Invoke(this, new SetupEventArgs(this));
        }
        private void AddEventListeners() {
            _Connection.Changed += ConnectionChanged;
        }
        private void ConnectionChanged(object sender, EventArgs e) {
            DispatchChanged();
        }
        #endregion
    }
}
