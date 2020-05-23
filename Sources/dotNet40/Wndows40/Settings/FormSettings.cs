#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;

namespace Library40.Win.Settings
{
	[Serializable]
	public class FormSettings : FormSettings<Form>
	{
		public string Name { get; set; }
	}
}