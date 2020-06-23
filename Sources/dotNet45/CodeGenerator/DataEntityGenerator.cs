// Created on     2018/04/21
// Last update on 2018/04/21 by Mohammad Mir mostafa 

using System.Collections.Generic;
using Mohammad.CodeGeneration.Exceptions;
using Mohammad.Data.SqlServer.Dynamics;
using Mohammad.Exceptions;

namespace Mohammad.CodeGeneration
{
    public class DataEntityGenerator
    {
        private readonly Database _Database;

        public DataEntityGenerator(string connectionstring) => this._Database = Database.GetDatabase(connectionstring);

        public IEnumerable<Table> GetDabaseTables() => this._Database.Tables;
        public IEnumerable<StoredProcedure> GetDabaseStoredProcedures() => this._Database.StoredProcedures;

        public static void Generate(IEnumerable<ISqlObject> sqlObjects,
            string baseClassName,
            string languageCompnentsFolder,
            string outputFileFolder,
            bool allInOneFile,
            string @namespace)
        {
            //var generator = new GeneratorBasedOnFile(outputFileFolder, allInOneFile);
            foreach (var o in sqlObjects)
            {
                switch (o)
                {
                    case Table t: break;
                    case Column col:
                        //if (allInOneFile)
                        //{
                        //    var classCode = generator.FindClass(outputFileFolder, col.Owner.Name);
                        //    if (classCode.IsNullOrEmpty())
                        //        classCode = generator.CreateClass(col.Owner.Name, @namespace);
                        //        classCode = generator.AddProperty(classCode, col.Name, col.DataType);
                        //    generator.WriteClass(classCode);
                        //}
                        break;
                    case StoredProcedure sp: break;
                    case null:
                        ExceptionBase.WrapThrow<UnsupportedSqlObjectException>();
                        break;
                }
            }
        }
    }
}