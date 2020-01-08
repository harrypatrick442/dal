using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
namespace SqlSaucey.Interfaces
{
    public interface IConnection
    {
        string Server { get; }
        string Database { get; }
        ICredentials Credentials { get; }
		string GetConnectionString();
    }
}
