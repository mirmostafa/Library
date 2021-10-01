﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Library.Dynamic;

[Serializable]
public class Expando : DynamicObject, ISerializable, INotifyPropertyChanged
{
    protected Dictionary<string, object?> Properties { get; } = new();

    public Expando()
    {
    }

    protected Expando(SerializationInfo info, StreamingContext context)
    {
        var buffer = info?.GetValue("Properties", typeof(Dictionary<string, object?>));
        if (buffer is Dictionary<string, object?> dic)
        {
            this.Properties = dic;
        }
    }

    public virtual object? this[[DisallowNull]string propName]
    {
        get => this.Properties.ContainsKey(propName) ? this.Properties[propName] : null;
        set
        {
            if (this.Properties.ContainsKey(propName))
            {
                if (this.Properties[propName] == value)
                {
                    return;
                }

                this.Properties[propName] = value;
            }
            else
            {
                this.Properties.Add(propName, value);
            }

            this.OnPropertyChanged(propName);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Check.IfArgumentNotNull(info, nameof(info));

        this.FillByProperties(info, context);
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = binder is null ? null : this[binder.Name];
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (binder is null)
        {
            return false;
        }

        this[binder.Name] = value;
        return true;
    }

    protected virtual void FillByProperties(SerializationInfo info, StreamingContext context)
    {
        foreach (var (key, value) in this.Properties)
        {
            info?.AddValue(key, value);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
