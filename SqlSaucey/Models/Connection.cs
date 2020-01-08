using System;
using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using System.IO;
using SqlSaucey.Core;
using SqlSaucey.ViewModels;
using SqlSaucey.Strings;
using SqlSaucey.Interfaces;
namespace SqlSaucey.Models
{
    public class Connection : ViewModel, IConnection
    {
        #region Events
        public event EventHandler<EventArgs> Changed;
        #endregion
        #region Properties
        private Credentials _Credentials;
        public Credentials Credentials { get { return _Credentials; } }
        private string _Server;
        public string Server { get { return _Server; } set { _Server = value; OnPropertyChange(); DispatchChanged(); } }
        private string _Database;
        public string Database { get { return _Database; } set { _Database = value; OnPropertyChange(); DispatchChanged(); } }

        ICredentials IConnection.Credentials
        {
            get
            {
                return _Credentials;
            }
        }
        #endregion
        #region Constructors
        public Connection()
        {
            _Credentials = new Credentials();
            AddEventListeners();
        }
        #endregion
        #region Methods
        public XElement ToXML()
        {
            XElement xElement = new XElement(SL.CONNECTION);
            xElement.Add(_Credentials.ToXML());
            xElement.Add(new XElement(SL.SERVER, _Server));
            xElement.Add(new XElement(SL.DATABASE, _Database));
            return xElement;
        }
        public static Connection From(XElement xElement)
        {
            Connection connection = new Connection();
            foreach (var childXElement in xElement.Elements())
            {
                if (childXElement.NodeType != XmlNodeType.Element) continue;
                switch (childXElement.Name.LocalName.ToLower())
                {
                    case SL.CREDENTIALS:
                        connection._Credentials = Models.Credentials.From(childXElement);
                        break;
                    case SL.SERVER:
                        connection._Server = childXElement.GetContent<string>();
                        break;
                    case SL.DATABASE:
                        connection._Database = childXElement.GetContent<string>();
                        break;
                }
            }
            connection.AddEventListeners();
            return connection;
        }
		public string GetConnectionString(){
			return string.Format("Data Source={0};Network Library=DBMSSOCN;Initial Catalog={1};User ID={2};Password={3}", Server, Database, Credentials.User, Credentials.Password);
		}
        private void AddEventListeners()
        {
            _Credentials.Changed += CredentialsChanged;
        }
        private void CredentialsChanged(object sender, EventArgs e)
        {
            DispatchChanged();
        }
        private void DispatchChanged()
        {
            if (Changed == null) return;
            Changed.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
