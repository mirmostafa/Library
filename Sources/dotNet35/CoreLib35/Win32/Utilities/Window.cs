#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library35.Helpers;
using Library35.Win32.Natives;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library35.Win32.Utilities
{
	public enum WindowState
	{
		Normal,
		Minimize,
		Maximize
	}

	public class Window
	{
		protected Window(IntPtr hwnd)
		{
			this.Hwnd = hwnd;
		}

		public WindowState WindowState
		{
			get { return this.WindowState = WindowState.Normal; }
			set
			{
				switch (value)
				{
					case WindowState.Normal:
						break;
					case WindowState.Minimize:
						Api.CloseWindow(this.Hwnd);
						break;
					case WindowState.Maximize:
						break;
					default:
						throw new ArgumentOutOfRangeException("value");
				}
			}
		}

		public IntPtr Hwnd { get; protected set; }

		public string Text
		{
			get { return GetWindowText(this.Hwnd); }
			set { Api.SetWindowText(this.Hwnd, value); }
		}

		public Window Parent
		{
			get { return new Window(Api.GetParent(this.Hwnd)); }
		}

		public bool Enabled
		{
			get { return Api.IsWindowEnabled(this.Hwnd); }
			set { Api.EnableWindow(this.Hwnd, value); }
		}

		public string ModuleName
		{
			get
			{
				var fileName = new StringBuilder(2000);
				Api.GetWindowModuleFileName(this.Hwnd, fileName, 2000);
				return fileName.ToString();
			}
		}

		public bool Visible
		{
			get { return Api.IsWindowVisible(this.Hwnd); }
			set { Api.ShowWindow(this.Hwnd, value.ToInt()); }
		}

		public static Window FindByText(string text)
		{
			var hwnd = Api.FindWindow(null, text);
			return hwnd != IntPtr.Zero ? new Window(hwnd) : null;
		}

		public void BringToTop()
		{
			Api.BringWindowToTop(this.Hwnd);
		}

		public IEnumerable<Window> GetChildern()
		{
			IList<Window> childern = new List<Window>();
			Api.WNDENUMPROC enumChildProc = delegate(IntPtr hwnd, IntPtr param)
			                                {
				                                childern.Add(new Window(hwnd));
				                                return true;
			                                };
			Api.EnumChildWindows(this.Hwnd, enumChildProc, IntPtr.Zero);
			return childern.AsEnumerable();
		}

		public void Close()
		{
			Api.DestroyWindow(this.Hwnd);
		}

		/// <summary>
		///     Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
		/// </returns>
		/// <param name="other"> An object to compare with this object. </param>
		public bool Equals(Window other)
		{
			if (ReferenceEquals(null, other))
				return false;
			return ReferenceEquals(this, other) || other.Hwnd.Equals(this.Hwnd);
		}

		/// <summary>
		///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
		///     <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" /> ;
		///     otherwise, false.
		/// </returns>
		/// <param name="obj">
		///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" /> .
		/// </param>
		/// <exception cref="T:System.NullReferenceException">
		///     The
		///     <paramref name="obj" />
		///     parameter is null.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			return obj.GetType() == typeof (Window) && this.Equals((Window)obj);
		}

		/// <summary>
		///     Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///     A hash code for the current <see cref="T:System.Object" /> .
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.Hwnd.GetHashCode();
		}

		public static bool operator ==(Window left, Window right)
		{
			return left.Hwnd == right.Hwnd;
		}

		public static bool operator !=(Window left, Window right)
		{
			return !(left == right);
		}

		public static IEnumerable<Window> GetAll()
		{
			var result = new List<Window>();
			Api.EnumDesktopWindowsDelegate callback = delegate(IntPtr wnd, int param)
			                                          {
				                                          result.Add(new Window(wnd));
				                                          return true;
			                                          };
			Api.EnumDesktopWindows(IntPtr.Zero, callback, IntPtr.Zero);
			return result;
		}

		public override string ToString()
		{
			return this.Text;
		}

		/// <summary>
		///     Returns the caption of a windows by given HWND identifier.
		/// </summary>
		public static string GetWindowText(IntPtr hWnd)
		{
			var title = new StringBuilder(1024);
			var titleLength = Api.GetWindowText(hWnd, title, title.Capacity + 1);
			title.Length = titleLength;

			return title.ToString();
		}

		public void BringWindowToTop()
		{
			Api.BringWindowToTop(this.Hwnd);
		}

		public void AnimateWindow(int time, AnimateWindowFlags flags)
		{
			Api.AnimateWindow(this.Hwnd, time, flags);
		}

		public void Minimize()
		{
			Api.CloseWindow(this.Hwnd);
		}

		public static Window GetActiveWindow()
		{
			return new Window(Api.GetActiveWindow());
		}

		public void Flash(bool invert)
		{
			Api.FlashWindow(this.Hwnd, invert);
		}
	}

	public delegate Boolean EnumChildProc(IntPtr hwnd, IntPtr lParam);
}