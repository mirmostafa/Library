using Mohammad.Logging;

namespace Mohammad
{
    public static class LibrarySupervisor
    {
        private static ILogger _Logger;

        public static ILogger Logger
        {
            get => _Logger ?? Logging.Logger.Empty;
            set => _Logger = value;
        }
    }
}