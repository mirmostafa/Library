namespace Library.Data.Models;

public interface IDataColumnBindingInfo
{
    string? BindingPath { get; }
    DataColumnBindingType DataType { get; }
    string? Title { get; }

    static IDataColumnBindingInfo New(string? title, string? bindingPath, DataColumnBindingType dataType = DataColumnBindingType.None) =>
        new DataColumnBindingInfo(title, bindingPath, dataType);
}