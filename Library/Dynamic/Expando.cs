using System.ComponentModel;
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

    public virtual object? this[string index]
    {
        get => this.Properties.ContainsKey(index) ? this.Properties[index] : null;
        set
        {
            if (this.Properties.ContainsKey(index))
            {
                if (this.Properties[index] == value)
                {
                    return;
                }

                this.Properties[index] = value;
            }
            else
            {
                this.Properties.Add(index, value);
            }

            this.OnPropertyChanged(index);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

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
        foreach (var kvp in this.Properties)
        {
            info?.AddValue(kvp.Key, kvp.Value);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
