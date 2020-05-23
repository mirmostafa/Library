using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Mohammad.EventsArgs;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibButtonBase : Button, ILibControl, IBindable, ILibSecured
    {
        private static object CoerceIsEnabledValueCallback(DependencyObject d, object value)
        {
            var control = d as LibButtonBase;
            if (control != null)
            {
                var e = new ItemActingEventArgs<object>(value);
                control.OnIsEnabledValueChanging(e);
                value = e.Item;
                if (e.Handled)
                    return value;
            }

            var element = (UIElement) d;
            var method = typeof(UIElement).GetMethod("CoerceIsEnabled", BindingFlags.NonPublic | BindingFlags.Static);
            var value1 = value;
            value = Catch(() => method?.Invoke(element, new[] {d, value1}));
            return value;
        }

        public static DependencyProperty SecurityCodeProperty;

        static LibButtonBase()
        {
            SecurityCodeProperty = DependencyProperty.Register("SecurityCode", typeof(string), typeof(LibButtonBase));
            IsEnabledProperty.OverrideMetadata(typeof(LibButtonBase), new FrameworkPropertyMetadata {CoerceValueCallback = CoerceIsEnabledValueCallback});
        }

        protected virtual void OnIsEnabledValueChanging(ItemActingEventArgs<object> e) { }

        protected virtual void OnSettingControlUnSecured() { }

        public DependencyProperty BindingFieldProperty => ContentProperty;

        public string SecurityCode { get { return (string) this.GetValue(SecurityCodeProperty); } set { this.SetValue(SecurityCodeProperty, value); } }

        public void SetControlUnSecured()
        {
            this.IsEnabled = false;
            this.OnSettingControlUnSecured();
        }
    }
}