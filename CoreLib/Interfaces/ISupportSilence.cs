using Library.Validations;

namespace Library.Interfaces;

public interface ISupportSilence
{
    bool IsEnabledRaisingEvents { get; set; }
}

public static class SupportSilenceHelper
{
    public static Fluency<TSupportSilence> SetEnableRaisingEvents<TSupportSilence>(this TSupportSilence obj, bool value)
        where TSupportSilence : ISupportSilence => obj.NotNull().Fluent(() => obj.IsEnabledRaisingEvents = value);
}