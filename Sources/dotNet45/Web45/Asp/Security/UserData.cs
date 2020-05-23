using System;

namespace Mohammad.Web.Asp.Security {
    [Serializable]
    public class UserData
    {
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public bool IsAdmin { get; }
        public DateTime SignInTime { get; }

        public UserData(string firstName, string lastName, string userName, bool? isAdmin, DateTime signInTime)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserName = userName;
            this.IsAdmin = isAdmin ?? false;
            this.SignInTime = signInTime;
        }

        public UserData(string firstName, string lastName, string userName, bool? isAdmin = false)
            : this(firstName, lastName, userName, isAdmin, DateTime.Now) { }
    }
}