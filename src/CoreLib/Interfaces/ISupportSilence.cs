using System.Diagnostics.Contracts;

namespace Library.Interfaces;

public interface ISupportSilence
{
    bool IsEnabledRaisingEvents { get; set; }
}

public static class SupportSilenceHelper
{
    [Pure]
    [return: NotNullIfNotNull(nameof(obj))]
    [return: MaybeNull]
    public static TSupportSilence SetEnableRaisingEvents<TSupportSilence>(this TSupportSilence obj, bool value)
        where TSupportSilence : ISupportSilence
    {
        if (obj != null)
        {
            obj.IsEnabledRaisingEvents = value;
        }

        return obj;
    }
}