using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.EventsArgs;
using Library.Threading.MultistepProgress;
using Library.Wpf.Dialogs;

namespace Library.Wpf.Helpers;
public static class MultistepProcessHelper
{
    public static void ShowProgress(this IMultistepProcess process, in string caption,
        in string instruction,
        in string footerText,
        bool isCancallable = false,
        bool runInTask = false,
        in CancellationToken cancellationToken = default)
    {
        dispose(process);

        process.Reported += process_Reported;
        process.Ended += process_Ended;

        void process_Reported(object? sender, ItemActedEventArgs<(int Max, int Current, string? Description)> e)
        {
        }
        void process_Ended(object? sender, ItemActedEventArgs<string?> e)
        {
            dispose((IMultistepProcess)sender!);
        }

        void dispose(IMultistepProcess process)
        {
            process.Reported -= process_Reported;
            process.Ended -= process_Ended;
        }
    }
}
