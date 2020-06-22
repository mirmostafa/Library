using Microsoft.Win32;

namespace Mohammad.Helpers
{
    public class ApplicationRegistration
    {
        public static void RegisterMe(ApplicationRegistrationInfo e = null)
        {
            if (e == null)
                e = new ApplicationRegistrationInfo();
            using (var software = Registry.LocalMachine.OpenSubKey("SOFTWARE", true))
            using (var mohammad = software.CreateSubKey("Mohammad"))
            using (var app = mohammad.CreateSubKey(e.ApplicationTitle, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                app.SetValue("Application Title", e.ApplicationTitle);
                app.SetValue("Company", e.Company);
                app.SetValue("Copyright", e.Copyright);
                app.SetValue("Description", e.Description);
                app.SetValue("GUID", e.Guid);
                app.SetValue("ProductTitle", e.ProductTitle);
                app.SetValue("Version", e.Version);
                software.Flush();
                mohammad.Flush();
                app.Flush();
            }
        }

        public class ApplicationRegistrationInfo
        {
            public string ApplicationTitle { get; set; }
            public string Company { get; set; }
            public string Copyright { get; set; }
            public string Description { get; set; }
            public string Guid { get; set; }
            public string ProductTitle { get; set; }
            public string Version { get; set; }

            public ApplicationRegistrationInfo(string applicationTitle = null, string company = null, string copyright = null, string description = null,
                string guid = null, string productTitle = null, string version = null)
            {
                this.ApplicationTitle = applicationTitle ?? ApplicationHelper.ApplicationTitle;
                this.Company = company ?? ApplicationHelper.Company;
                this.Copyright = copyright ?? ApplicationHelper.Copyright;
                this.Description = description ?? ApplicationHelper.Description;
                this.Guid = guid ?? ApplicationHelper.Guid;
                this.ProductTitle = productTitle ?? ApplicationHelper.ProductTitle;
                this.Version = version ?? ApplicationHelper.Version;
            }
        }
    }
}