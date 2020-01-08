using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Strings;
using System.Text.RegularExpressions;
using SqlSaucey.Core;
using SqlSaucey.Exceptions;
namespace SqlSaucey.Enums
{
    public enum ProgrammableType
    {
        StoredProcedure,
        ScalarFunction,
        TableValuedFunction,
        View,
        Other
    }
    public static class ProgrammableTypeExtensions
    {
        public static ProgrammableType ToProgrammableType(this string str) {
            switch (str.ToLower().Replace(" ","")) {
                case SL.P:
                    return ProgrammableType.StoredProcedure;
                case SL.IF:
                    return ProgrammableType.TableValuedFunction;
                case SL.FN:
                    return ProgrammableType.ScalarFunction;
                case SL.V:
                    return ProgrammableType.View;
                default:
                    return ProgrammableType.Other;
            }
        }
        public static string ToString(this ProgrammableType p)
        {
            switch (p)
            {
                case ProgrammableType.StoredProcedure:
                    return SL.P;
                case ProgrammableType.ScalarFunction:
                    return SL.FN;
                case ProgrammableType.TableValuedFunction:
                    return SL.IF;
                case ProgrammableType.View:
                    return SL.V;
                default:
                    throw new NotImplementedException("The type of programmable other has no string representation");
            }
        }
        public static string ToLongString(this ProgrammableType p)
        {
            switch (p)
            {
                case ProgrammableType.StoredProcedure:
                    return SL.PROCEDURE;
                case ProgrammableType.ScalarFunction:
                    return SL.FUNCTION;
                case ProgrammableType.TableValuedFunction:
                    return SL.FUNCTION;
                case ProgrammableType.View:
                    return SL.VIEW;
                default:
                    throw new NotImplementedException("The type of programmable other has no string representation");
            }
        }
    }
    public class ProgrammableTypeHelper
    {
        private const char DELIMITER = '\\';
        private static readonly Regex regExpGetProgrammableType = new Regex("(?:(?:alter\\s+|create\\s+)(procedure|function|view))", RegexOptions.IgnoreCase);
        private static readonly Regex regExpGetProgrammableTypeFunction = new Regex("(return|returns)\\s+table", RegexOptions.IgnoreCase);

        public static ProgrammableType FromDefinition(string definition) {
            Match match = regExpGetProgrammableType.Match(definition);
            if (match.Length > 1)
            {
                switch (match.Groups[1].Value.ToLower())
                {
                    case SL.VIEW:
                        return ProgrammableType.View;
                    case SL.PROCEDURE:
                        return ProgrammableType.StoredProcedure;
                    case SL.FUNCTION:
                        return regExpGetProgrammableTypeFunction.Match(definition).Success ? ProgrammableType.TableValuedFunction : ProgrammableType.ScalarFunction;
                }
            }
            throw new ProgrammableException("Could not determine the type of programmable from the definition");
        }
        public static ProgrammableType FromFilePath(string filePath) {
            string directory = Path.GetDirectoryName(filePath);
            string[] directoryComponents = directory.Split(DELIMITER);
            if (directoryComponents.Length > 0)
            {
                string programmableTypeNamedDirectory = directoryComponents[directoryComponents.Length - 1];
                switch (programmableTypeNamedDirectory.ToLower())
                {
                    case ProgrammableDirectories.SCALAR_FUNCTIONS:
                        return ProgrammableType.ScalarFunction;
                    case ProgrammableDirectories.STORED_PROCEDURES:
                        return ProgrammableType.StoredProcedure;
                    case ProgrammableDirectories.TABLE_VALUED_FUNCTIONS:
                        return ProgrammableType.TableValuedFunction;
                    case ProgrammableDirectories.VIEWS:
                        return ProgrammableType.View;
                }
            }
            throw new ProgrammableException("Could not determine the type of programmable from file \"" + filePath + "\"");
        }
    }
}
