#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Helpers;

namespace Library35.Data.Linq.Security
{
	public class User
	{
		// Methods

		// Properties
		public string Address { get; set; }

		public static User Current
		{
			get { return new User(); }
		}

		public string Email { get; set; }

		public string Family { get; set; }

		public bool IsAdmin { get; set; }

		public bool IsApproved { get; set; }

		public DateTime LastLoginDate { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public string Tel { get; set; }

		public string UserName { get; set; }

		public void CheckPermission(string premissionKey)
		{
			if (!premissionKey.IsNullOrEmpty() && Current.HasAccessTo(premissionKey))
			{
			}
		}

		public bool HasAccessTo(string rulename)
		{
			return true;
		}
	}
}