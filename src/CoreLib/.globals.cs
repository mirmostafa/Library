global using System.Diagnostics.CodeAnalysis;
global using System.Text;

global using Library.Coding;
global using Library.Helpers;

global using static Library.Coding.CodeHelper;

global using Checker = Library.Validations.Check;
global using IMsLogger = Microsoft.Extensions.Logging.ILogger;
global using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Library.Wpf")]
[assembly: InternalsVisibleTo("Library.Web")]
[assembly: InternalsVisibleTo("LibraryTest")]
[assembly: InternalsVisibleTo("Library.Cqrs")]

//x [ModuleInitializer]
public static class CoreLibModule
{
}