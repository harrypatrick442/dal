using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using System.IO;
using SqlSaucey.Strings;
using SqlSaucey.Core;

namespace SqlSaucey.Models
{
    public class Configuration
    {
        private string _Path;
        private Connection _Connection;
        public Connection Connection { get { return _Connection; } }
        private Setups _Setups;
        public Setups Setups { get { return _Setups; } }
        public Configuration(string path)
        {
            _Path = path;
            LoadFromXmlFile(path);
            if (_Setups == null)
                _Setups = new Setups();
            if (_Connection == null)
                _Connection = new Connection();
            SetupCallbacks();
        }
        private void LoadFromXmlFile(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath)) return;
            XDocument xDocument = XDocument.Load(xmlFilePath);
            foreach (var xElement in xDocument.Root.Elements())
            {
                if (xElement.NodeType != XmlNodeType.Element) continue;
                switch (xElement.Name.LocalName.ToLower())
                {
                    case SL.SETUPS:
                        _Setups = Setups.From(xElement);
                        break;
                    case SL.CONNECTION:
                        _Connection = Connection.From(xElement);
                        break;
                }
            }
        }
        private void SetupCallbacks()
        {
            _Setups.Changed += SetupsChanged;
            _Setups.SetupChanged += SetupChanged;
        }
        private void SetupsChanged(object sender, SetupsEventArgs e)
        {
            Save();
        }
        private void SetupChanged(object sender, SetupEventArgs e)
        {
            Save();
        }
        private void Save()
        {
            XDocument xDocument = new XDocument();
            XElement xElement = new XElement("Configuration");
            xDocument.Add(xElement);
            xElement.Add(_Setups.ToXML());
            xDocument.Save(_Path);
        }
    }
}
