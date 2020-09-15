namespace Mohammad.Logging
{
    namespace EventsArgs
    {
        //public abstract class LogEventArgs<TLogEntity> : EventArgs
        //    where TLogEntity : LogEntity
        //{
        //    internal LogEventArgs(TLogEntity logEntity)
        //    {
        //        this.LogEntity = logEntity;
        //    }

        //    public TLogEntity LogEntity { get; private set; }
        //}

        //public class LoggingEventArgs<TLogEntity> : LogEventArgs<TLogEntity>
        //    where TLogEntity : LogEntity
        //{
        //    internal LoggingEventArgs(TLogEntity logEntity)
        //        : base(logEntity)
        //    {
        //    }

        //    public bool Handled { get; set; }
        //}

        //public class LoggedEventArgs<TLogEntity> : LogEventArgs<TLogEntity>
        //    where TLogEntity : LogEntity
        //{
        //    internal LoggedEventArgs(TLogEntity logEntity)
        //        : base(logEntity)
        //    {
        //    }
        //}
    }

    namespace Configurations
    {
        public enum LoggingSeverity
        {
            High,
            Normal,
            Never
        }
    }
}