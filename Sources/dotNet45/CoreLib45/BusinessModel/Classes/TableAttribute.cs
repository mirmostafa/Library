#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.BusinessModel.Classes
{
    [Serializable]
    public sealed class TableAttribute : Attribute
    {
        public string DbSchemaName { get; set; }
        public string DbTableName  { get; set; }
    }
}