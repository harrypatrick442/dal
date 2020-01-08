using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using SqlSaucey.ViewModels;
using SqlSaucey.Interfaces;
using SqlSaucey.Strings;
namespace SqlSaucey.Models
{
    public class Credentials:ViewModel, ICredentials
    {
        #region Events
        public event EventHandler<EventArgs> Changed;
        #endregion
        #region Properties
        private string _Username;
        private string _Password;
        public string User { get { return _Username; } set { _Username = value; OnPropertyChange();DispatchChanged(); } }
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChange(); DispatchChanged(); } }
        #endregion
        #region Methods
        public string GetConnectionString()
        {
            return "";
        }
        public static Credentials From(XElement xElement)
        {
            Credentials credentials = new Models.Credentials();
            foreach (var childXElement in xElement.Elements())
            {
                if (childXElement.NodeType != XmlNodeType.Element) continue;
                switch (childXElement.Name.LocalName)
                {
                    case SL.USERNAME:
                        credentials._Username = childXElement.GetContent<string>();
                        break;
                    case SL.PASSWORD:
                        credentials._Password = childXElement.GetContent<string>();
                        break;
                }
            }
            return credentials;
        }
        public XElement ToXML() {
            XElement xElement = new XElement(SL.CREDENTIALS);
            xElement.Add(new XElement(SL.USERNAME, _Username));
            xElement.Add(new XElement(SL.PASSWORD, _Password));
            return xElement;
        }
        private void DispatchChanged() {
            if (Changed == null) return;
            Changed.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
