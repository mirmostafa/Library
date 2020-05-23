using Mohammad.Wpf.Windows.Settings;

namespace Mohammad.Wpf.Windows.Intenals
{
    public class AppSettings : AppSettings<AppSettings>
    {
        private WindowSettings _LibraryMainWindow;

        public WindowSettings LibraryMainWindow
        {
            get { return this._LibraryMainWindow ?? (this._LibraryMainWindow = new WindowSettings()); }
            set
            {
                this._LibraryMainWindow = value;
                this.OnPropertyChanged();
            }
        }
    }
}