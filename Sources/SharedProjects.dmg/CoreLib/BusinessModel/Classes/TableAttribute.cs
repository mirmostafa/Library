using System;

namespace Mohammad.BusinessModel.Classes
{
     [Serializable]
     public sealed class TableAttribute : Attribute
     {
         public string DbSchemaName { get; set; }
         public string DbTableName { get; set; }
     }
}