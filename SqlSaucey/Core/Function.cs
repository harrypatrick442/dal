using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Enums;
namespace SqlSaucey.Core
{
    public abstract class Function:Programmable
    {
        public override void WriteToFile(string directory) {
            base.WriteToFile(directory);
        }
        protected static new T From<T>(T t, SqlDataReader reader) where T:Function
        {
            return Programmable.From(t, reader);
        }
    }
}
