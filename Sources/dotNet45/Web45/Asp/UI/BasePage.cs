using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;
using System.Web.UI;
using Mohammad.EventsArgs;

namespace Mohammad.Web.Asp.UI
{
    public abstract class BasePage : Page
    {
        protected object DataContext { get; set; }
        protected BasePage() { this.MaintainScrollPositionOnPostBack = true; }
        protected virtual void OnLoading(ActingEventArgs e) { }
        protected virtual void OnPageIsNotPostBack(ActingEventArgs e) { }
        protected virtual void OnPageIsPostBack() { }

        protected virtual void LoadState()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            var vs = properties.Where(p => p.GetCustomAttribute(typeof(ViewStateAttribute)) != null);
            foreach (var prop in vs)
                prop.SetValue(this, this.ViewState[prop.Name]);
        }

        protected virtual void SaveState()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            var vs = properties.Where(p => p.GetCustomAttribute(typeof(ViewStateAttribute)) != null);
            foreach (var prop in vs)
                this.ViewState[prop.Name] = prop.GetValue(this);
        }
    }
}