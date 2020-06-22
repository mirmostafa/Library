using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mohammad.Helpers;

namespace Mohammad.AddIns.Contacts
{
    [Obsolete]
    public class AddIn<TBaseClass>
        where TBaseClass : class
    {
        public string FilePath { get; set; }
        public TBaseClass Instance { get; set; }

        public static bool HasAddIn<TAttribute>(string file) where TAttribute : Attribute
        {
            try
            {
                return
                    Assembly.LoadFrom(file)
                        .GetTypes()
                        .Select(type => new {type, attribute = type.GetCustomAttribute<TAttribute>()})
                        .Where(t => t.attribute != null)
                        .Select(t => t.type)
                        .Any(type => type.IsSubclassOf(typeof(TBaseClass)) || type.GetInterface(typeof(TBaseClass).Name) != null);
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<AddIn<TBaseClass>> GetServices<TAttribute>(string file) where TAttribute : Attribute
        {
            return
                Assembly.LoadFrom(file)
                    .GetTypes()
                    .Select(type => new {type, attribute = type.GetCustomAttribute<TAttribute>()})
                    .Where(t => t.attribute != null)
                    .Select(t => t.type.GetConstructor(new Type[] {}))
                    .Where(ctor => ctor != null)
                    .Select(ctor => ctor.Invoke(null).As<TBaseClass>())
                    .Where(baseClass => baseClass != null)
                    .Select(bc => new AddIn<TBaseClass> {FilePath = file, Instance = bc});
        }

        public static IEnumerable<AddIn<TBaseClass>> LoadAddIns<TAttribute>(string path = ".", params string[] searchPatterns) where TAttribute : Attribute
        {
            if (searchPatterns == null || searchPatterns.Length == 0)
                searchPatterns = new[] {"*.dll", "*.exe"};
            return
                FileSystemHelper.GetFiles(path, searchPatterns).Where(HasAddIn<TAttribute>).ToList().Select(GetServices<TAttribute>).SelectMany(services => services);
        }

        public override string ToString() { return this.Instance?.ToString() ?? typeof(TBaseClass).FullName; }
    }
}