using System.ComponentModel;
using Mohammad.Logging.Entities;

namespace Mohammad.Logging.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWriter<in TLogEntity>
        where TLogEntity : LogEntity
    {
        bool ShowInDebuggerTracer { get; set; }
        void Write(TLogEntity logEntity);
        void LoadLastLog();
    }
}