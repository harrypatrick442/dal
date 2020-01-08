using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
namespace SqlSaucey.Interfaces
{
    public interface ICredentials
    {
         string User { get; }
         string Password { get; }
    }
}
