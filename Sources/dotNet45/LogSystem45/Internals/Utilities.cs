using System;
using System.IO;
using System.Reflection;
using Mohammad.Globalization;
using Mohammad.Helpers;
using Mohammad.Logging.Entities;

namespace Mohammad.Logging.Internals
{
    internal class Utilities
    {
        internal static DirectoryInfo DefaultDirectory
            => new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationHelper.Company, "Logs"));

        internal static string GernerateFileSpec(DirectoryInfo directory, string prefix, string extension)
        {
            return string.Concat(directory.FullName, "\\", prefix, "_", PersianDateTime.Now.ToDateString("-"), extension);
        }

        internal static string GernerateFileSpec(FileInfo file, string extension)
            => string.Concat(file.Directory.FullName, "\\", "_", PersianDateTime.Now.ToDateString("-"), extension);

        internal static Assembly GetCaller() => Assembly.GetAssembly(CodeHelper.GetCallerMethod(SkipFrameCount() - 1).DeclaringType);

        internal static int SkipFrameCount()
        {
            var i = 1;
            while (CodeHelper.GetCallerMethod(i) != null)
            {
                var type = CodeHelper.GetCallerMethod(i).DeclaringType;
                if (type != null && type.BaseType != typeof(LogEntity))
                    if (Assembly.GetAssembly(CodeHelper.GetCallerMethod(i).DeclaringType) != Assembly.GetCallingAssembly())
                    {
                        var type1 = CodeHelper.GetCallerMethod(i).DeclaringType;
                        if (type1 != null && !type1.FullName.StartsWith("Library"))
                            if (!Assembly.GetAssembly(CodeHelper.GetCallerMethod(i).DeclaringType).GlobalAssemblyCache)
                                return i;
                    }
                i++;
            }
            return -1;
        }

        internal static string GenerateLogFileSpec() { return Path.Combine(DefaultDirectory.FullName, string.Concat(ApplicationHelper.ProductTitle, ".log")); }
    }
}