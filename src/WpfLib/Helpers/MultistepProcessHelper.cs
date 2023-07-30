using Library.EventsArgs;
using Library.Threading.MultistepProgress;

namespace Library.Wpf.Helpers;

public static class MultistepProcessHelper
{
    public static void ShowProgress(this IProgressReport process, in string caption,
        in string instruction,
        in string footerText,
        bool isCancellable = false,
        bool runInTask = false,
        in CancellationToken cancellationToken = default)
    {
        reset(process);

        process.Reported += process_Reported;
        process.Ended += process_Ended;

        void process_Reported(object? sender, ItemActedEventArgs<ProgressData> e)
        {
        }
        void process_Ended(object? sender, ItemActedEventArgs<ProgressData?> e) 
            => reset((IProgressReport)sender!);

        void reset(IProgressReport process)
        {
            process.Reported -= process_Reported;
            process.Ended -= process_Ended;
        }
    }
}