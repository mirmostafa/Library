namespace Library.Serialization;
public static class XmlSerializer
{
    public static void Serialize<T>(T? o, string path!!, bool indent = true)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var writer = new StreamWriter(path);
        using var xmlWriter = System.Xml.XmlWriter.Create(writer, new System.Xml.XmlWriterSettings { Indent = indent });
        serializer.Serialize(xmlWriter, o);
    }

    public static T? Desrialize<T>(string path!!)
    {
        using var file = new FileStream(path, FileMode.Open);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var result = serializer.Deserialize(file);
        return result is null ? default : (T)result;
    }
}
