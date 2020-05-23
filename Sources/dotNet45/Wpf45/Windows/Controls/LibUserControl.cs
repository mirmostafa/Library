using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Wpf.Internals;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibUserControl : UserControl, INotifyPropertyChanged, ILibControl
    {
        protected LibUserControlDynamicCollection Controls { get; }
        protected LibCommandDynamicCollection Commands { get; }
        public string InnerName { get; set; }
        public TaskScheduler UiTaskScheduler { get; private set; }

        public LibUserControl()
        {
            this.InitializedComponents();
            this.Commands = new LibCommandDynamicCollection(this);
            this.Controls = new LibUserControlDynamicCollection(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set(DependencyProperty dp, object value)
        {
            this.SetValue(dp, value);
            return true;
        }

        public void Connect(int connectionId, object target)
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializedComponents()
        {
            this.UiTaskScheduler = Task.Factory.Scheduler;
            this.OnInitialized(EventArgs.Empty);
        }
    }

    public class LibConentControl : ContentControl, INotifyPropertyChanged, ILibControl
    {
        protected LibUserControlDynamicCollection Controls { get; }
        protected LibCommandDynamicCollection Commands { get; }
        public string InnerName { get; set; }
        public TaskScheduler UiTaskScheduler { get; private set; }

        public LibConentControl()
        {
            this.InitializedComponents();
            this.Commands = new LibCommandDynamicCollection(this);
            this.Controls = new LibUserControlDynamicCollection(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set(DependencyProperty dp, object value)
        {
            this.SetValue(dp, value);
            return true;
        }

        public void Connect(int connectionId, object target)
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializedComponents()
        {
            this.UiTaskScheduler = Task.Factory.Scheduler;
            this.OnInitialized(EventArgs.Empty);
        }
    }
}