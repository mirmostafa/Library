using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logging;
public interface ILoggerContainer
{
    ILogger Logger { get; }
}
