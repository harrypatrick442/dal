using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Strings;
using SqlSaucey.DAL;

using SqlSaucey.Interfaces;
using SqlSaucey.Core;
using SqlSaucey.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SqlSaucey.Exceptions;

namespace SqlSaucey.Helpers
{
    public class Transfer
    {
        private static DALProgrammability __DalProgrammability;
        private static DALProgrammability _DalProgrammability
        {
            get
            {

                if (__DalProgrammability == null) __DalProgrammability = new DALProgrammability();
                return __DalProgrammability;
            }
        }
        public static void WriteToDatabase(IConnection iConnection, string directory, IConsole iConsole)
        {
            ProgrammableDirectories programmableDirectories = new ProgrammableDirectories(directory);
            Programmable[] exitingProgrammables = _DalProgrammability.GetForDatabase(iConnection);
            Dictionary<string, Programmable> unseenProgrammablesInDatabase = exitingProgrammables.ToDictionary(x => x.Name, y => y);
            Programmable[] programmablesFromDirectories = GetProgrammablesFromDirectory(directory, programmableDirectories);
            iConsole.WriteLine(string.Format("Writing from directory {0} to database {1} {2}", directory, iConnection.Server,iConnection.Database));
            foreach (Programmable programmable in programmablesFromDirectories)
            {
                string definition = programmable.Definition;
                if (!unseenProgrammablesInDatabase.ContainsKey(programmable.Name))
                {
                    definition = programmable.CreateDefinition;
                }
                else
                {
                    Programmable programmableInDatabase = unseenProgrammablesInDatabase[programmable.Name];
                    unseenProgrammablesInDatabase.Remove(programmable.Name);
                    if (programmableInDatabase.MatchingDefinition(programmable))
                    {
                        iConsole.WriteLine(string.Format("Match between {0} file and database definition.", programmable.Name));
                        continue;
                    }
                }
                iConsole.WriteLine(string.Format("Writing {0} to database", programmable.Name));
                try
                {
                    _DalProgrammability.ExecuteDefinition(iConnection, definition);
                }
                catch (Exception ex)
                {
                    ex = new ProgrammableException("Error writing " + programmable.Name, ex);
                    iConsole.WriteLine(ex);
                    throw ex;
                }

            }
            DeleteProgrammablesFromDatabase(iConnection, iConsole, unseenProgrammablesInDatabase.Select(x => x.Value).ToArray());
            iConsole.WriteLine("Finished");

        }
        public static void ReadFromDatabase(IConnection iConnection, string directory, IConsole iConsole)
        {
            ProgrammableDirectories programmableDirectories = new ProgrammableDirectories(directory);
            Dictionary<string, Programmable> unseenProgrammables = GetProgrammablesFromDirectory(directory, programmableDirectories).ToDictionary(x=>x.Name, y=>y);
            Programmable[] programmables = _DalProgrammability.GetForDatabase(iConnection);
            iConsole.WriteLine(string.Format("Reading from database {0} {1} to directory {2}" , iConnection.Server ,iConnection.Database, directory));
            foreach (Programmable programmable in programmables)
            {
                if (programmable.ProgrammableType == ProgrammableType.Other) continue;

                string path = GetFilePath(programmableDirectories, programmable);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (unseenProgrammables.ContainsKey(programmable.Name))
                {
                    Programmable programmableFromDirectory = unseenProgrammables[programmable.Name];
                    if (programmableFromDirectory.MatchingDefinition(programmable))
                    {
                        iConsole.WriteLine(string.Format("Match between {0} file and database", programmable.Name));
                        unseenProgrammables.Remove(programmable.Name);
                        continue;
                    }
                }
                else
                {
                    file.Directory.Create();
                }
                iConsole.WriteLine(string.Format("Writing {0} to file", programmable.Name));
                File.WriteAllText(file.FullName, programmable.Definition);
                unseenProgrammables.Remove(programmable.Name);
            }
            DeleteProgrammablesFromDirectory(iConnection, iConsole, programmableDirectories, unseenProgrammables.Select(x => x.Value).ToArray());
            iConsole.WriteLine("Finished");
        }
        private static Programmable[] GetProgrammablesFromDirectory(string directory, ProgrammableDirectories programmableDirectories)
        {
            List<Programmable> list = new List<Programmable>();
            foreach (ProgrammableType programmableType in new ProgrammableType[] { ProgrammableType.ScalarFunction, ProgrammableType.TableValuedFunction, ProgrammableType.View, ProgrammableType.StoredProcedure })
            {
                string folder = programmableDirectories.From(programmableType);
                Directory.CreateDirectory(folder);
                string[] fileNames = Directory.GetFiles(folder);
                foreach (string fullName in fileNames)
                {
                    list.Add(ProgrammableFactory.FromFile(fullName));
                }
            }
            return list.ToArray();
        }
        private static void DeleteProgrammablesFromDatabase(IConnection iConnection, IConsole iConsole, Programmable[] programmables)
        {
            foreach (Programmable programmable in programmables)
            {
                iConsole.WriteLine(string.Format("Deleting {0} from directory database since it doesn't exist in directory", programmable.Name));
                _DalProgrammability.DropProgrammable(iConnection, programmable);
            }
        }
        private static void DeleteProgrammablesFromDirectory(IConnection iConnection, IConsole iConsole, ProgrammableDirectories directories, Programmable[] programmables)
        {
            foreach (Programmable programmable in programmables)
            {
                iConsole.WriteLine(string.Format("Deleting {0} from directory diectory since it doesn't exist in database", programmable.Name));
                File.Delete(GetFilePath(directories, programmable));
            }
        }
        private static string GetFilePath(ProgrammableDirectories directories, Programmable programmable)
        {
            return Path.Combine(directories.From(programmable.ProgrammableType), programmable.Name + ".sql");
        }
    }
}
