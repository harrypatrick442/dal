using System;
using SqlSaucey.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using SqlSaucey.Enums;
using SqlSaucey.Interfaces;
using SqlSaucey.Core;
namespace SqlSaucey.DAL
{
    public class DALDatabases
    {
        private const string _GetDatabases =
        @"select * from sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
        public Database[] Get(IConnection iConnection)
        {
            using (SqlConnection connection = new SqlConnection(iConnection.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(
                   string.Format(_GetDatabases)
                    , connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    List<Database> list = new List<Database>();
                    while (reader.Read())
                    {
                        list.Add(Database.From(reader));
                    }
                    return list.ToArray();
                }
            }
        }
    }
}
