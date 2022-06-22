using Library.Validations;

namespace Library.Serialization;
public static class XmlSerializer
{
    public static void SerializeFile<T>(T? o, [DisallowNull] string path, bool indent = true)
    {
        Check.IfArgumentNotNull(path);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var writer = new StreamWriter(path);
        using var xmlWriter = System.Xml.XmlWriter.Create(writer, new System.Xml.XmlWriterSettings { Indent = indent });
        serializer.Serialize(xmlWriter, o);
    }

    public static T? DesrializeFile<T>([DisallowNull] string path)
    {
        using var file = new FileStream(path.NotNull(), FileMode.Open);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var result = serializer.Deserialize(file);
        return result is null ? default : (T)result;
    }
}
