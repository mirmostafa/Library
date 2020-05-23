using Mohammad.Settings;

namespace Mohammad.Win.Settings
{
    public abstract class ProgramSettings<TProgramSettings> : ApplicationSettings<TProgramSettings>
        where TProgramSettings : ProgramSettings<TProgramSettings>, new() {}
}