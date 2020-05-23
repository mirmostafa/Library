#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Threading;
using Library40.Win.Settings;

namespace Library40.Win.Helpers
{
	public partial class ControlHelper
	{
		public static DialogResult ShowDialog<TForm>(Func<TForm> creator) where TForm : Form
		{
			TForm form;
			return ShowDialog(creator, out form);
		}

		public static DialogResult ShowDialog<TForm>(Func<TForm> creator, out TForm form) where TForm : Form
		{
			return ShowDialog(creator, null, out form);
		}

		public static DialogResult ShowDialog<TForm>(Func<TForm> creator, FormSettings<TForm> formSettings, out TForm form) where TForm : Form
		{
			form = CreateInstance(creator, formSettings);
			return form.ShowDialog();
		}

		public static TForm Show<TForm>() where TForm : Form, new()
		{
			return Show(() => new TForm());
		}

		public static TForm Show<TForm>(Func<TForm> creator) where TForm : Form
		{
			return Show(creator, null);
		}

		public static TForm Show<TForm>(Func<TForm> creator, FormSettings<TForm> formSettings) where TForm : Form
		{
			var result = CreateInstance(creator, formSettings);
			result.Show();
			return result;
		}

		public static TForm CreateInstance<TForm>() where TForm : Form, new()
		{
			return CreateInstance<TForm>(null);
		}

		public static TForm CreateInstance<TForm>(FormSettings<TForm> formSettings) where TForm : Form, new()
		{
			return CreateInstance(() => new TForm(), formSettings);
		}

		public static TForm CreateInstance<TForm>(Func<TForm> creator, FormSettings<TForm> formSettings) where TForm : Form
		{
			var form = creator();
			if (formSettings != null)
				formSettings.SetForm(form);
			return form;
		}

		public static Form CreateInstance(Type formType)
		{
			return ObjectHelper.CreateInstance<Form>(formType);
		}

		public static TForm ShowSingleton<TForm>() where TForm : Form, new()
		{
			return ShowSingleton(() => new TForm());
		}

		public static TForm ShowSingleton<TForm>(Func<TForm> creator) where TForm : Form
		{
			return ShowSingleton(creator, null);
		}

		public static TForm ShowSingleton<TForm>(FormSettings<TForm> formSettings) where TForm : Form, new()
		{
			return ShowSingleton(() => new TForm(), formSettings);
		}

		public static TForm ShowSingleton<TForm>(Func<TForm> creator, FormSettings<TForm> formSettings) where TForm : Form
		{
			return ShowSingleton(creator, formSettings, null);
		}

		public static TForm ShowSingleton<TForm>(Func<TForm> creator, FormSettings<TForm> formSettings, Predicate<TForm> predicate) where TForm : Form
		{
			var form = predicate == null
				? Application.OpenForms.Cast<Form>().Where(EqualsTo<TForm>).FirstOrDefault() as TForm
				: Application.OpenForms.Cast<Form>().FirstOrDefault(frm => EqualsTo<TForm>(frm) && predicate(frm as TForm)) as TForm;
			if (form == null)
				return Show(creator, formSettings);
			form.BringToFront();
			if (form.WindowState == FormWindowState.Minimized)
				form.WindowState = FormWindowState.Normal;
			return form;
		}

		public static void FadeOut(this Form form)
		{
			FadeIn(form, 500);
		}

		public static void FadeIn(this Form form, int milliseconds)
		{
			FadeIn(form, TimeSpan.FromMilliseconds(milliseconds));
		}

		public static void FadeOut(this Form form, int milliseconds)
		{
			FadeIn(form, TimeSpan.FromMilliseconds(milliseconds));
		}

		public static void FadeIn(this Form form, TimeSpan timeout)
		{
			for (double i = 0; i <= 1; i += .1)
			{
				form.Opacity = i;
				form.Update();
				Async.Sleep((timeout.TotalMilliseconds / (1 / .1)).ToInt());
			}
		}

		public static void FadeOut(this Form form, TimeSpan timeout)
		{
			for (double i = 1; i > 0; i -= .1)
			{
				form.Opacity = i;
				form.Update();
				Async.Sleep((timeout.TotalMilliseconds / (1 / .1)).ToInt());
			}
		}
	}
}