#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Runtime.CompilerServices;

namespace Mohammad.Logging
{
    public interface ISimpleLogger
    {
        void Log(object                  text,                           object                    moreInfo         = null, object sender = null,
                 LogLevel                level          = LogLevel.Info, [CallerMemberName] string memberName       = "",
                 [CallerFilePath] string sourceFilePath = "",            [CallerLineNumber] int    sourceLineNumber = 0);
    }
}