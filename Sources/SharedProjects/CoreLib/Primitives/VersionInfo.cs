using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Properties;

namespace Mohammad.Primitives
{
    [ComVisible(true)]
    [Serializable]
    public class VersionInfo : INotifyPropertyChanged, IEquatable<VersionInfo>, IComparable, IComparable<VersionInfo>, ICloneable
    {
        private int _Build;
        private int _Major;
        private int _Minor;
        private int _Revision;

        [XmlAttribute]
        public int Major
        {
            get { return this._Major; }
            set
            {
                if (value == this._Major)
                    return;
                CheckIfNotNeg(value);
                this._Major = value;
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public int Minor
        {
            get { return this._Minor; }
            set
            {
                if (value == this._Minor)
                    return;
                CheckIfNotNeg(value);
                this._Minor = value;
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public int Build
        {
            get { return this._Build; }
            set
            {
                if (value == this._Build)
                    return;
                CheckIfNotNeg(value);
                this._Build = value;
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public int Revision
        {
            get { return this._Revision; }
            set
            {
                if (value == this._Revision)
                    return;
                CheckIfNotNeg(value);
                this._Revision = value;
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public short MajorRevision => (short) (this._Revision >> 16);

        [XmlAttribute]
        public short MinorRevision => (short) (this._Revision & ushort.MaxValue);

        /// <summary>
        ///     For serialization purposes.
        /// </summary>
        public VersionInfo() {}

        public VersionInfo(VersionInfo version)
            : this(version.Major, version.Minor, version.Build, version.Revision) {}

        public VersionInfo(int major = 0, int minor = 0, int build = 0, int revision = 0)
        {
            this.Build = build;
            this.Major = major;
            this.Minor = minor;
            this.Revision = revision;
        }

        public override string ToString() { return $"{this._Major:000}.{this._Minor:000}.{this._Build:000}.{this._Revision:000}"; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((VersionInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this._Build;
                hashCode = (hashCode * 397) ^ this._Major;
                hashCode = (hashCode * 397) ^ this._Minor;
                hashCode = (hashCode * 397) ^ this._Revision;
                return hashCode;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
        }

        public static bool operator ==(VersionInfo left, VersionInfo right) { return Equals(left, right); }
        public static bool operator !=(VersionInfo left, VersionInfo right) { return !Equals(left, right); }

        public static bool operator <(VersionInfo v1, VersionInfo v2)
        {
            if (v1 == null)
                throw new ArgumentNullException(nameof(v1));
            return v1.CompareTo(v2) < 0;
        }

        public static bool operator >(VersionInfo v1, VersionInfo v2) => v2 < v1;

        public static bool operator <=(VersionInfo v1, VersionInfo v2)
        {
            if (v1 == null)
                throw new ArgumentNullException(nameof(v1));
            return v1.CompareTo(v2) <= 0;
        }

        public static bool operator >=(VersionInfo v1, VersionInfo v2) => v2 <= v1;
        public static implicit operator string(VersionInfo version) => version?.ToString();
        public static implicit operator VersionInfo(string version) => Parse(version);

        public static VersionInfo Parse(string input)
        {
            VersionInfo result;
            if (TryParse(input, out result))
                return result;
            throw new ParseException(input, typeof(VersionInfo));
        }

        public static bool TryParse(string input, out VersionInfo version)
        {
            version = null;
            if (input.IsNullOrEmpty())
                return false;
            var parts = input.Split('.');
            if (parts.Length == 0 || parts.Length > 4)
                return false;
            int buffer;
            if (!int.TryParse(parts[0], out buffer))
                return false;
            version = new VersionInfo(buffer);

            if (parts.Length > 1)
            {
                if (!int.TryParse(parts[1], out buffer))
                    return false;
                version.Minor = buffer;
            }

            if (parts.Length > 2)
            {
                if (!int.TryParse(parts[2], out buffer))
                    return false;
                version.Build = buffer;
            }

            if (parts.Length > 3)
            {
                if (!int.TryParse(parts[3], out buffer))
                    return false;
                version.Revision = buffer;
            }
            return true;
        }

        public VersionInfo AddMajor(int value)
        {
            var resut = new VersionInfo(this);
            resut.Major += value;
            return resut;
        }

        public VersionInfo AddMinor(int value)
        {
            var resut = new VersionInfo(this);
            resut.Minor += value;
            return resut;
        }

        public VersionInfo AddBuid(int value)
        {
            var resut = new VersionInfo(this);
            resut.Build += value;
            return resut;
        }

        public VersionInfo AddRevision(int value)
        {
            var resut = new VersionInfo(this);
            resut.Revision += value;
            return resut;
        }

        private static void CheckIfNotNeg(int value, [CallerMemberName] string propertyName = null)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(propertyName, string.Concat(value, " cannot be negative."));
        }

        public void PerformProperyChanged(string propertyName)
        {
            if (propertyName != null)
                this.OnPropertyChanged(propertyName);
        }

        public object Clone() { return new VersionInfo(this._Build, this._Major, this._Minor, this._Revision); }
        public int CompareTo(object obj) { return this.CompareTo(obj.As<VersionInfo>()); }

        public int CompareTo(VersionInfo other)
        {
            if (other == null)
                return 1;
            if (this._Major != other._Major)
                return this._Major > other._Major ? 1 : -1;
            if (this._Minor != other._Minor)
                return this._Minor > other._Minor ? 1 : -1;
            if (this._Build != other._Build)
                return this._Build > other._Build ? 1 : -1;
            if (this._Revision == other._Revision)
                return 0;
            return this._Revision > other._Revision ? 1 : -1;
        }

        public bool Equals(VersionInfo other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.CompareTo(other) == 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}