#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Web.UI;

namespace Library40.Web.Helpers
{
	public static class ControlHelper
	{
		public static IEnumerable<TControl> GetControls<TControl>(this Control control) where TControl : class
		{
			foreach (Control childControl in control.Controls)
			{
				TControl buffer;
				if ((buffer = childControl as TControl) != null)
					yield return buffer;

				if (childControl.Controls.Count > 0)
					foreach (var t in GetControls<TControl>(childControl))
						yield return t;
			}
		}
	}
}