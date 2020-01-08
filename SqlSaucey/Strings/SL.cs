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
namespace SqlSaucey.Strings
{
    public static class SL
    {
        public const string CREDENTIALS = "credentials";
        public const string CONNECTION = "connection";
        public const string DEFINITION = "definition";
        public const string DATABASE = "database";
        public const string DIRECTORY = "directory";
        public const string ID = "id";
        public const string NAME = "name";
        public const string PASSWORD = "password";
        public const string PROCEDURE = "procedure";
        public const string FUNCTION = "function";
        public const string VIEW = "view";
        public const string SERVER = "server";
        public const string SETUP = "setup";
        public const string SETUPS = "setups";
        public const string SHOW_CONNECTIONS = "showConnections";
        public const string TYPE = "type";
        public const string USERNAME = "username";

        public const string INFO = "info";
        public const string ERROR = "error";
        public const string MESSAGE = "message";
        public const string CONFIGURATION_FILE_NAME = "configuration.conf";

        public const string P = "p";
        public const string IF = "if";
        public const string FN = "fn";
        public const string V = "v";
    }
}
