using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            get => this._OriginalValue;
            internal set
            {
                if (Equals(value, this._OriginalValue))
                {
                    return;
                }

                this._OriginalValue = value;
                this.OnPropertyChanged();
            }
        }

        public TColumnType Value
        {
            get => this._Value;
            set
            {
                if (Equals(value, this._Value))
                {
                    return;
                }

                this._Value = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static implicit operator TColumnType(Column<TColumnType> column) => column.Value;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public ColumnStructure(string columnName,
            bool allowNull,
            bool isIdentity,
            bool isPrimaryKey,
            bool isForiegnKey,
            string foriegnKeyTable,
            string foriegnKeyColumn)
        {
            this.AllowNull = allowNull;
            this.IsPrimaryKey = isPrimaryKey;
            this.IsForiegnKey = isForiegnKey;
            this.IsIdentity = isIdentity;
            this.ColumnName = columnName;
            this.ForiegnKeyColumn = foriegnKeyColumn;
            this.ForiegnKeyTable = foriegnKeyTable;
        }

        public bool AllowNull
        {
            get => this._AllowNull;
            private set
            {
                if (value.Equals(this._AllowNull))
                {
                    return;
                }

                this._AllowNull = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsPrimaryKey
        {
            get => this._IsPrimaryKey;
            private set
            {
                if (value.Equals(this._IsPrimaryKey))
                {
                    return;
                }

                this._IsPrimaryKey = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsForiegnKey
        {
            get => this._IsForiegnKey;
            private set
            {
                if (value.Equals(this._IsForiegnKey))
                {
                    return;
                }

                this._IsForiegnKey = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsIdentity
        {
            get => this._IsIdentity;
            private set
            {
                if (value.Equals(this._IsIdentity))
                {
                    return;
                }

                this._IsIdentity = value;
                this.OnPropertyChanged();
            }
        }

        public string ColumnName
        {
            get => this._ColumnName;
            private set
            {
                if (value == this._ColumnName)
                {
                    return;
                }

                this._ColumnName = value;
                this.OnPropertyChanged();
            }
        }

        public string ForiegnKeyColumn
        {
            get => this._ForiegnKeyColumn;
            private set
            {
                if (value == this._ForiegnKeyColumn)
                {
                    return;
                }

                this._ForiegnKeyColumn = value;
                this.OnPropertyChanged();
            }
        }

        public string ForiegnKeyTable
        {
            get => this._ForiegnKeyTable;
            private set
            {
                if (value == this._ForiegnKeyTable)
                {
                    return;
                }

                this._ForiegnKeyTable = value;
                this.OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}