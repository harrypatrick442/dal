using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Strings;
using SqlSaucey.Enums;
using System.Text.RegularExpressions;
using SqlSaucey.Exceptions;
namespace SqlSaucey.Core
{
    public static class ProgrammableFactory
    {
       public static Programmable FromFile(string filePath) {
            string definition = File.ReadAllText(filePath);
            string programmableName = Path.GetFileNameWithoutExtension(filePath);
            ProgrammableType programmableTypeFromDefinition = ProgrammableTypeHelper.FromDefinition(definition);
            ProgrammableType programmableTypeFromFilePath = ProgrammableTypeHelper.FromFilePath(filePath);
            if (programmableTypeFromDefinition != programmableTypeFromFilePath) throw new ProgrammableException("The programmable type determined from the definition did not match the programmable type determined by the file path for file \"" + filePath + "\". You might have put the file in the wrong programmable type folder.");
            switch (programmableTypeFromDefinition) {
                case ProgrammableType.StoredProcedure:
                    return new StoredProcedure(definition, programmableName);
                case ProgrammableType.TableValuedFunction:
                    return new TableValuedFunction(definition, programmableName);
                case ProgrammableType.ScalarFunction:
                    return new ScalarFunction(definition, programmableName);
                default:
                    return new View(definition, programmableName);
            }
        }
    }
}
