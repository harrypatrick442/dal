using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Enums;
namespace SqlSaucey.Core
{
    public class View : Programmable
    {
        public override ProgrammableType ProgrammableType
        {
            get
            {
                return ProgrammableType.View;
            }
        }
        public override void WriteToFile(string directory)
        {
            base.WriteToFile(directory);
        }
        private View() { }
        public View(string definition, string name) {
            _Definition = definition;
            _Name = name;
        }
        public static new View From(SqlDataReader reader)
        {
            View view = new Core.View();
            Programmable.From(view, reader);
            return view;
        }
    }
}
