using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Enums;
namespace SqlSaucey.Core
{
    public class TableValuedFunction:Function
    {
        public override ProgrammableType ProgrammableType
        {
            get
            {
                return ProgrammableType.TableValuedFunction;
            }
        }
        private TableValuedFunction() { }
        public TableValuedFunction(string definition, string name) {
            _Definition = definition;
            _Name = name;
        }
        public override void WriteToFile(string directory) {
            base.WriteToFile(directory);
        }
        public static new TableValuedFunction From(SqlDataReader reader) {
            TableValuedFunction tableValuedFunction = new Core.TableValuedFunction();
            Function.From(tableValuedFunction, reader);
            return tableValuedFunction;
        }
    }
}
