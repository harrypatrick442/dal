using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Enums;
namespace SqlSaucey.Core
{
    public class ScalarFunction : Function
    {
        public override ProgrammableType ProgrammableType
        {
            get
            {
                return ProgrammableType.ScalarFunction;
            }
        }
        private ScalarFunction() { }
        public ScalarFunction(string definition, string name) {
            _Definition = definition;
            _Name = name;
        }
        public override void WriteToFile(string directory)
        {
            base.WriteToFile(directory);
        }
        public static new ScalarFunction From(SqlDataReader reader)
        {
            ScalarFunction scalarFunction = new Core.ScalarFunction();
            Function.From(scalarFunction, reader);
            return scalarFunction;
        }
    }
}
