using System;
using SqlSaucey.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using SqlSaucey.Enums;
using SqlSaucey.Interfaces;
using SqlSaucey.Strings;
using SqlSaucey.Core;
namespace SqlSaucey.DAL
{
    public class DALProgrammability
    {
        private const string _GetForDatabase =
        @"
	        SELECT 
                type, 
                o.object_id as objectId,
                o.name AS name,
                o.type_desc as typeDesc,
                definition AS [definition],
                schemas.name scheamaName
	        FROM sys.sql_modules m 
            INNER JOIN sys.objects  o ON m.object_id=o.OBJECT_ID
            INNER JOIN sys.schemas ON schemas.schema_id = o.schema_id
            ;
        ";
        private const string DROP = "drop {0} {1};";
        public Programmable[] GetForDatabase(IConnection iConnection)
        {
            using (SqlConnection connection = new SqlConnection(iConnection.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand( string.Format(_GetForDatabase), connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    List<Programmable> list = new List<Programmable>();
                    while (reader.Read())
                    {
                        ProgrammableType type = ((string)reader[SL.TYPE]).ToProgrammableType();
                        switch (type)
                        {
                            case ProgrammableType.StoredProcedure:
                                list.Add(StoredProcedure.From(reader));
                                break;
                            case ProgrammableType.ScalarFunction:
                                list.Add(ScalarFunction.From(reader));
                                break;
                            case ProgrammableType.TableValuedFunction:
                                list.Add(TableValuedFunction.From(reader));
                                break;
                            case ProgrammableType.View:
                                list.Add(View.From(reader));
                                break;
                        }
                    }
                    return list.ToArray();
                }
            }
        }
        public void ExecuteDefinition(IConnection iConnection, string definition)
        {
            using (SqlConnection connection = new SqlConnection(iConnection.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(definition, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DropProgrammable(IConnection iConnection, Programmable programmable) {
            using (SqlConnection connection = new SqlConnection(iConnection.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(string.Format(DROP, programmable.ProgrammableType.ToLongString(), programmable.Name),
                    connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
