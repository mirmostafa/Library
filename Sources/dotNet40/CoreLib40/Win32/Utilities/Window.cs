#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library40.Helpers;
using Library40.Win32.Natives;
using Library40.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library40.Win32.Utilities
{
	public enum WindowState
	{
		Normal,
		Minimize,
		Maximize
	}

	public class Window : IEquatable<Window>
	{
		private static readonly IEqualityComparer<Window> _HwndComparerInstance = new HwndEqualityComparer();

		protected Window(IntPtr hwnd)
		{
			this.Hwnd = hwnd;
		}

		public static IEqualityComparer<Window> HwndComparer
		{
			get { return _HwndComparerInstance; }
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
		public IEnumerable<Window> Childern
		{
			get
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
		}

		#region IEquatable<Window> Members
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
			if (ReferenceEquals(this, other))
				return true;
			return this.Hwnd.Equals(other.Hwnd);
		}
		#endregion

		public static Window FindByText(string text)
		{
			var hwnd = Api.FindWindow(null, text);
			return hwnd != IntPtr.Zero ? new Window(hwnd) : null;
		}

		public void BringToTop()
		{
			Api.BringWindowToTop(this.Hwnd);
		}

		public void Close()
		{
			Api.DestroyWindow(this.Hwnd);
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
			if (obj.GetType() != this.GetType())
				return false;
			return this.Equals((Window)obj);
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
			Api.EnumDesktopWindows(IntPtr.Zero,
				delegate(IntPtr wnd1, int param1)
				{
					result.Add(new Window(wnd1));
					return true;
				},
				IntPtr.Zero);
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

		#region Nested type: HwndEqualityComparer
		private sealed class HwndEqualityComparer : IEqualityComparer<Window>
		{
			#region IEqualityComparer<Window> Members
			public bool Equals(Window x, Window y)
			{
				if (ReferenceEquals(x, y))
					return true;
				if (ReferenceEquals(x, null))
					return false;
				if (ReferenceEquals(y, null))
					return false;
				if (x.GetType() != y.GetType())
					return false;
				return x.Hwnd.Equals(y.Hwnd);
			}

			public int GetHashCode(Window obj)
			{
				return obj.Hwnd.GetHashCode();
			}
			#endregion
		}
		#endregion
	}

	public delegate Boolean EnumChildProc(IntPtr hwnd, IntPtr lParam);
}