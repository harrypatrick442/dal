using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Enums;

namespace SqlSaucey.Core
{
    public class StoredProcedure:Programmable
    {
        public override ProgrammableType ProgrammableType
        {
            get
            {
                return ProgrammableType.StoredProcedure;
            }
        }
        private StoredProcedure() { }
        public StoredProcedure(string definition, string name)
        {
            _Definition = definition;
            _Name = name;
        }
        public override void WriteToFile(string directory) {
            base.WriteToFile(directory);
        }
        public static new StoredProcedure From(SqlDataReader reader) {
            StoredProcedure storedProcedure = new Core.StoredProcedure();
            From(storedProcedure, reader);
            return storedProcedure;
        }
    }
}
