using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mohammad.Properties;

namespace Mohammad.BusinessModel.Classes
{
    internal sealed class ColumnAttribute : Attribute
    {
        public string StructurePropName { get; set; }
    }

    internal sealed class Column<TColumnType> : INotifyPropertyChanged
    {
        private TColumnType _OriginalValue;
        private TColumnType _Value;

        public TColumnType OriginalValue
        {
            get { return this._OriginalValue; }
            internal set
            {
                if (Equals(value, this._OriginalValue))
                    return;
                this._OriginalValue = value;
                this.OnPropertyChanged();
            }
        }

        public TColumnType Value
        {
            get { return this._Value; }
            set
            {
                if (Equals(value, this._Value))
                    return;
                this._Value = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static implicit operator TColumnType(Column<TColumnType> column) { return column.Value; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public sealed class ColumnStructure
    {
        private bool _AllowNull;
        private string _ColumnName;
        private string _ForiegnKeyColumn;
        private string _ForiegnKeyTable;
        private bool _IsForiegnKey;
        private bool _IsIdentity;
        private bool _IsPrimaryKey;

        public bool AllowNull
        {
            get { return this._AllowNull; }
            private set
            {
                if (value.Equals(this._AllowNull))
                    return;
                this._AllowNull = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsPrimaryKey
        {
            get { return this._IsPrimaryKey; }
            private set
            {
                if (value.Equals(this._IsPrimaryKey))
                    return;
                this._IsPrimaryKey = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsForiegnKey
        {
            get { return this._IsForiegnKey; }
            private set
            {
                if (value.Equals(this._IsForiegnKey))
                    return;
                this._IsForiegnKey = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsIdentity
        {
            get { return this._IsIdentity; }
            private set
            {
                if (value.Equals(this._IsIdentity))
                    return;
                this._IsIdentity = value;
                this.OnPropertyChanged();
            }
        }

        public string ColumnName
        {
            get { return this._ColumnName; }
            private set
            {
                if (value == this._ColumnName)
                    return;
                this._ColumnName = value;
                this.OnPropertyChanged();
            }
        }

        public string ForiegnKeyColumn
        {
            get { return this._ForiegnKeyColumn; }
            private set
            {
                if (value == this._ForiegnKeyColumn)
                    return;
                this._ForiegnKeyColumn = value;
                this.OnPropertyChanged();
            }
        }

        public string ForiegnKeyTable
        {
            get { return this._ForiegnKeyTable; }
            private set
            {
                if (value == this._ForiegnKeyTable)
                    return;
                this._ForiegnKeyTable = value;
                this.OnPropertyChanged();
            }
        }

        public ColumnStructure(string columnName, bool allowNull, bool isIdentity, bool isPrimaryKey, bool isForiegnKey, string foriegnKeyTable, string foriegnKeyColumn)
        {
            this.AllowNull = allowNull;
            this.IsPrimaryKey = isPrimaryKey;
            this.IsForiegnKey = isForiegnKey;
            this.IsIdentity = isIdentity;
            this.ColumnName = columnName;
            this.ForiegnKeyColumn = foriegnKeyColumn;
            this.ForiegnKeyTable = foriegnKeyTable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}