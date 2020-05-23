using System;
using System.ComponentModel;
using System.Windows;
using Mohammad.Exceptions;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class ShowWindowCommand : LibCommand
    {
        public Type WindowType { get; set; }

        [DefaultValue(false)]
        public bool IsMultiInstance { get; set; }

        public Window Instance { get; set; }

        protected override void OnExecuted()
        {
            if (!this.IsMultiInstance && this.Instance != null)
            {
                this.Instance.WindowState = WindowState.Normal;
                this.Instance.BringIntoView();
                this.Instance.Focus();
                return;
            }
            if (this.WindowType.IsAssignableFrom(typeof(Window)))
                throw new ParseException(string.Format("Cannot convert '{0}' to 'Window'", this.WindowType));
            var constructor = this.WindowType.GetConstructor(new Type[] {});
            if (constructor == null)
                return;
            this.Instance = (Window) constructor.Invoke(null);
            if (!Equals(this.Instance, this.Parent))
                this.Instance.Owner = this.Parent as Window;
            this.Instance.Closed += (_, __) => this.Instance = null;
            this.Instance.ShowDialog();
        }
    }
}