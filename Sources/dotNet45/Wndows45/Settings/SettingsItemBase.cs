using System;
using System.Linq;

namespace Mohammad.Win.Settings
{
    [Serializable]
    public class SettingsItemBase
    {
        public virtual void Reset()
        {
            foreach (var item in this.GetType().GetProperties().Where(item => item.CanWrite))
                try
                {
                    item.SetValue(this, null, null);
                }
                catch
                {
                    try
                    {
                        item.SetValue(this, 0, null);
                    }
                    catch {}
                }
        }
    }
}