using System;
using Mohammad.Wpf.Windows.Settings;

namespace TestWpfApp45.Settings
{
    public class AppSettings : AppSettings<AppSettings>
    {
        private DateTime? _LastExecutionTime;
        private WindowSettings _MainWindow;

        public DateTime? LastExecutionTime
        {
            get { return this._LastExecutionTime; }
            set
            {
                if (value.Equals(this._LastExecutionTime))
                    return;
                this._LastExecutionTime = value;
                this.OnPropertyChanged();
            }
        }

        public WindowSettings MainWindow { get { return this._MainWindow ?? (this._MainWindow = new WindowSettings()); } set { this._MainWindow = value; } }
    }
}