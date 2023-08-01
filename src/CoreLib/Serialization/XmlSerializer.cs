using Library.Validations;

namespace Library.Serialization;

public static class XmlSerializer
{
    /// <summary>
    /// Deserializes the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    public static T? DeserializeFile<T>([DisallowNull] string path)
    {
        using var file = new FileStream(path.ArgumentNotNull(), FileMode.Open);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var result = serializer.Deserialize(file);
        return result is null ? default : (T)result;
    }

    /// <summary>
    /// Serializes the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o">The o.</param>
    /// <param name="path">The path.</param>
    /// <param name="indent">if set to <c>true</c> [indent].</param>
    public static void SerializeFile<T>(T? o, [DisallowNull] string path, bool indent = true)
    {
        Check.MustBeArgumentNotNull(path);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var writer = new StreamWriter(path);
        using var xmlWriter = System.Xml.XmlWriter.Create(writer, new System.Xml.XmlWriterSettings { Indent = indent });
        serializer.Serialize(xmlWriter, o);
    }
}