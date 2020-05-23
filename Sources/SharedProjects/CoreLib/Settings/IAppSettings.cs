using Mohammad.Dynamic;

namespace Mohammad.Settings
{
    public interface IAppSettings
    {
        Expando Windows { get; set; }
        void Save(bool encryptAfterSave = false, string password = null);
    }
}