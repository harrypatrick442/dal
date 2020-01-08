using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Interfaces;
using SqlSaucey.Strings;
namespace SqlSaucey.Core
{
    public class Database
    {
        public string GetConnectionString(ICredentials credentials)
        {
            return "";
        }
        private int _Id;
        public int Id { get { return _Id; } }
        private string _Name;
        public string Name { get { return _Name; } }
        private Database()
        {

        }
        public static Database From(SqlDataReader reader)
        {
            Database database = new Database();
            database._Id = Convert.ToInt32(reader[SL.ID]);
            database._Name = (string)reader[SL.NAME];
            return database;
        }
    }
}
