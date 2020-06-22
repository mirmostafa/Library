using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Mohammad.Properties;

namespace Security
{
    [Serializable]
    public class AuthenticationInfo : INotifyPropertyChanged
    {
        private string _Domain;

        private string _Password;

        private string _UserName;
        public static IEqualityComparer<AuthenticationInfo> UserNameDomainComparer { get; } = new UserNameDomainEqualityComparer();

        public string UserName
        {
            get { return this._UserName; }
            set
            {
                if (value == this._UserName)
                    return;
                this._UserName = value;
                this.OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return this._Password; }
            set
            {
                if (value == this._Password)
                    return;
                this._Password = value;
                this.OnPropertyChanged();
            }
        }

        public string Domain
        {
            get { return this._Domain; }
            set
            {
                if (value == this._Domain)
                    return;
                this._Domain = value;
                this.OnPropertyChanged();
            }
        }

        public override string ToString() => $"{nameof(this.UserName)}: {this.UserName}, {nameof(this.Domain)}: {this.Domain}";
        public void GetObjectData(SerializationInfo info, StreamingContext context) { throw new NotImplementedException(); }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private sealed class UserNameDomainEqualityComparer : IEqualityComparer<AuthenticationInfo>
        {
            public bool Equals(AuthenticationInfo x, AuthenticationInfo y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (ReferenceEquals(x, null))
                    return false;
                if (ReferenceEquals(y, null))
                    return false;
                if (x.GetType() != y.GetType())
                    return false;
                return string.Equals(x.UserName, y.UserName) && string.Equals(x.Domain, y.Domain);
            }

            public int GetHashCode(AuthenticationInfo obj)
            {
                unchecked
                {
                    return ((obj.UserName?.GetHashCode() ?? 0) * 397) ^ (obj.Domain?.GetHashCode() ?? 0);
                }
            }
        }
    }

    public class FtpAuthenticationInfo : AuthenticationInfo
    {
        private Uri _Uri;

        public Uri Uri
        {
            get { return this._Uri; }
            set
            {
                if (Equals(value, this._Uri))
                    return;
                this._Uri = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Uri));
            }
        }

        public string UriAddress
        {
            get { return this.Uri?.ToString(); }
            set
            {
                if (this.UriAddress == value)
                    return;
                this.Uri = new Uri(value);
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.UriAddress));
            }
        }

        public override string ToString() => $"{base.ToString()}, {nameof(this.UriAddress)}: {this.UriAddress}";
    }
}