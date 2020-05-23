using System;

namespace Mohammad.Wpf.Interfaces
{
    public interface ISettingsEnabledElement
    {
        void LoadSettings();
        void SaveSettings();
        //event EventHandler<EventsArgs.ItemActingEventArgs<WindowSettings<TForm>>> LoadingSettings;
        event EventHandler LoadingSettings;
        event EventHandler SavingSettings;
    }
}