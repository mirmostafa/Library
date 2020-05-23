#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35
{
	public class LazyInit<T>
		where T : class
	{
		private T _Value;

		public LazyInit(Func<T> creator, LazyInitMode mode = LazyInitMode.FirstCall)
		{
			this.Creator = creator;
			this.Mode = mode;
			if (this.Mode == LazyInitMode.Immediately)
				this.Value = creator();
		}

		public Func<T> Creator { get; set; }

		public LazyInitMode Mode { get; set; }

		public T Value
		{
			get
			{
				switch (this.Mode)
				{
					case LazyInitMode.FirstCall:
						return this._Value ?? (this._Value = this.Creator());
					case LazyInitMode.Immediately:
						return this._Value;
					case LazyInitMode.NewPerCall:
						return this.Creator();
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set { this._Value = value; }
		}

		public static implicit operator T(LazyInit<T> lazyInit)
		{
			return lazyInit.Value;
		}
	}

	public enum LazyInitMode
	{
		FirstCall,
		Immediately,
		NewPerCall
	}
}