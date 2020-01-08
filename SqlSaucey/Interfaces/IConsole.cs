using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
namespace SqlSaucey.Interfaces
{
    public interface IConsole
    {
        void Info(string message);
        void Error(string message);
        void WriteLine(string message);
        void WriteLine(Exception ex);
    }
}
