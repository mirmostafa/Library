﻿global using System.Text;
global using Library.Helpers;
global using System.Diagnostics.CodeAnalysis;
global using static Library.Coding.CodeHelper;
global using static Library.Coding.Functional;

global using IMsLogger = Microsoft.Extensions.Logging.ILogger;
global using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;


using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Library.Wpf")]
[assembly: InternalsVisibleTo("Library.Web")]
[assembly: InternalsVisibleTo("LibraryTest")]
[assembly: InternalsVisibleTo("Library.Cqrs")]
