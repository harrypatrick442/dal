using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Strings;
using SqlSaucey.Enums;
using System.Text.RegularExpressions;

namespace SqlSaucey.Core
{
    public abstract class Programmable
    {
        public abstract ProgrammableType ProgrammableType { get; }
        protected string _Name;
        public string Name { get { return _Name; } }
        protected string _Definition;
        public string Definition { get { return _Definition; } }

        private static readonly Regex regexReplaceAlterWithCreate = new Regex("alter\\s+(?:procedure|function|view)", RegexOptions.IgnoreCase);
        public string CreateDefinition { get { return regexReplaceAlterWithCreate.Replace(Definition, "create "+ProgrammableType.ToLongString()); } }
        public virtual void WriteToFile(string directory) {
            string fileName = Path.Combine(directory, Name);
            File.WriteAllText(fileName, Definition);
        }
        public bool MatchingDefinition(Programmable programmable) {
            return Definition == programmable.Definition;
        }
        private static readonly Regex regexReplaceOldVlaue = new Regex("OLD\\s+Value", RegexOptions.IgnoreCase);
        private static readonly Regex regexReplaceCreateView = new Regex("Create\\s+View", RegexOptions.IgnoreCase);
        private static readonly Regex regexReplaceCreateFunction = new Regex("Create\\s+Function", RegexOptions.IgnoreCase);
        private static readonly Regex regexReplaceCreateProcedure = new Regex("Create\\s+Procedure", RegexOptions.IgnoreCase);
        protected static T From<T>(T t, SqlDataReader reader) where T:Programmable{
            t._Name = (string)reader[SL.NAME];
            string definition = (string)reader[SL.DEFINITION];
            definition = regexReplaceAlterWithCreate.Replace(definition, "New Value");
            definition = regexReplaceCreateView.Replace(definition, "Alter View");
            definition = regexReplaceCreateFunction.Replace(definition, "Alter Function");
            definition = regexReplaceCreateProcedure.Replace(definition, "Alter Procedure");
            t._Definition = definition;
            return t;
        }
    }
}
