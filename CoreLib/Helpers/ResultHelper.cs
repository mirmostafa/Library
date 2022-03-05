using Library.Coding;
using Library.Results;
using System.Xml;
using System.Xml.Serialization;

namespace Library.Helpers;
public static class ResultHelper
{
    public static Result<Stream> ToFile(this Result<Stream> result, string filePath!!, FileMode fileMode = FileMode.Create)
    {
        var stream = result.Value;
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);

        return result;
    }

    public static Result<string> ToText(this Result<Stream> result)
    {
        var stream = result.Value;
        using var reader = new StreamReader(stream);
        return new(reader.ReadToEnd());
    }

    public static Result<StreamWriter> ToStreamWriter(this Result<Stream> result) =>
        new(new(result.Value));

    public static Result<XmlWriter> ToXmlWriter(this Result<Stream> result, bool indent = true) =>
        new(XmlWriter.Create(result.ToStreamWriter(), new XmlWriterSettings { Indent = indent }));

    public static Result<Stream> SerializeToXmlFile<T>(this Result<Stream> result, string filePath!!) =>
        result.Fluent(() => new XmlSerializer(typeof(T)).Serialize(result.Value, filePath));
}
