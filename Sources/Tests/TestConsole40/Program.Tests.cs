using System;
using System.Collections.Generic;
using Library35.Bcl;

namespace TestConsole
{
  partial class Program
  {
      private static void Test01()
      {
          var actions = new List<Action>
                {
                    () => Say("P1"),
                    () => Say("P2"),
                    () =>
                        {
                            Say("P3");
                            throw new Exception("This is a sample exception.");
                        },
                    () => Say("P4")
                };

          MultiStepOperation operations = MultiStepOperation.GetMultiStepOperation(actions);

          operations.Started += (sender, e) => Console.WriteLine("Started.\n");
          operations.Ended += (sender, e) => Console.WriteLine("\nEnded {0}.\n", e.Succeed ? "successfully" : "unsuccessfully");
          operations.ProgressIncreasing += (sender, e) => Console.WriteLine("Starting ({0} of {1}).", e.Step, e.Max);
          operations.ProgressIncreased += (sender, e) => Console.WriteLine("Done  ({0} of {1}).\n", e.Step, e.Max);
          operations.ExceptionOccurred += (sender, e) =>
          {
              Console.WriteLine("\nException: {0} ", e.Exception.Message);
              e.Throw = false;
          };

          operations.Start();
      }

      private static void Say(string message, int millisecondsTimeout = 500)
      {
          message.WriteLineTimeStamp();
          Thread.Sleep(millisecondsTimeout);
      }

      private static void Test02()
      {
          Database database = Database.GetDatabase(Settings.Default.iLedgErpConnectionString);

          var ns = new CodeNamespace("iLedge.Tests.CodeCOMTest");
          CodeTypeDeclaration dbClass = CodeDomHelper.InitClass(database.Name, "", ns);
          foreach (Table table in database.Tables)
          {
              CodeTypeDeclaration tableClass = CodeDomHelper.InitClass(table.Name, "", ns);
              foreach (Column column in table.Columns)
                  tableClass.AddPropertyWithBackingField(Database.SqlTypeToType(column.DataType, column.IsNullable), column.Name);
              dbClass.AddPropertyWithBackingField(table.Name, table.Name.Replace("#", "_"));
          }
          ns.AddUsing("System", "System.Linq");
          var outputFile = new FileInfo("..\\..\\..\\Test.cs");
          var codeCompileUnit = new CodeCompileUnit();
          codeCompileUnit.AddNamespace(ns);
          codeCompileUnit.GenerateCSharpCode(outputFile);
      }
  }
}