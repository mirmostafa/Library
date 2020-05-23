using System;
using System.Windows.Forms;

namespace Mohammad.Win.Settings
{
    [Serializable]
    public class FormSettings : FormSettings<Form>
    {
        public string Name { get; set; }
    }
}