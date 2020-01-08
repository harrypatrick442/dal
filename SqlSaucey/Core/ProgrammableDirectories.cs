using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Interfaces;
using SqlSaucey.Strings;
using SqlSaucey.Enums;

namespace SqlSaucey.Core
{
    public class ProgrammableDirectories
    {
        public const string STORED_PROCEDURES = "stored_procedures";
        public const string TABLE_VALUED_FUNCTIONS = "table_valued_functions";
        public const string SCALAR_FUNCTIONS = "scalar_functions";
        public const string VIEWS = "views";
        private string _StoredProcedure;
        private string _TableValuedFunction;
        private string _ScalarFunction;
        private string _View;
        public string StoredProcedure { get { return _StoredProcedure; } }
        public string TableValuedFunction { get { return _TableValuedFunction; } }
        public string ScalarFunction { get { return _ScalarFunction; } }
        public string View { get { return _View; } }
        public ProgrammableDirectories(string root)
        {
            _StoredProcedure = Path.Combine(root, STORED_PROCEDURES);
            _TableValuedFunction = Path.Combine(root, TABLE_VALUED_FUNCTIONS);
            _ScalarFunction = Path.Combine(root, SCALAR_FUNCTIONS);
            _View = Path.Combine(root, VIEWS);
        }
        public string From(ProgrammableType programmableType)
        {

            switch (programmableType)
            {
                case Enums.ProgrammableType.ScalarFunction: return ScalarFunction;
                case Enums.ProgrammableType.StoredProcedure: return StoredProcedure;
                case Enums.ProgrammableType.TableValuedFunction: return TableValuedFunction;
                case Enums.ProgrammableType.View: return View;
            }
            return null;
        }
    }
}
