#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using Mohammad.Dynamic;

namespace Mohammad.Settings
{
    public interface IAppSettings
    {
        Expando Windows { get; set; }
        void Save(bool encryptAfterSave = false, string password = null);
    }
}