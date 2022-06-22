namespace Library.Data.Models;

public record struct DataColumnBindingInfo(string? Title, string? BindingPath, DataColumnBindingType DataType = DataColumnBindingType.None) : IDataColumnBindingInfo
{
    public void Deconstruct(out string? title, out string? bindingPath) =>
        (title, bindingPath) = (this.Title, this.BindingPath);

    public static implicit operator (string? Title, string? BindingPath)(DataColumnBindingInfo value) =>
        (value.Title, value.BindingPath);
    public static implicit operator DataColumnBindingInfo((string? Title, string? BindingPath) value) =>
        new(value.Title, value.BindingPath);
}

public enum DataColumnBindingType
{
    None,
    Text,
}