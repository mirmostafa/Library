using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging
{
    public class Logger<TWriter> : Logger<TWriter, LogEntity>
        where TWriter : class, IWriter<LogEntity>
    {
        public Logger(TWriter writer, bool raiseEventOnly = false)
            : base(writer, raiseEventOnly) { }

        protected Logger() { }
    }
}