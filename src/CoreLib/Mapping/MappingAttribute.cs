namespace Library.Mapping;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MappingAttribute : Attribute
{
    public MappingAttribute(string sourceClassName, string sourcePropertyName)
        => this.MapFrom = (sourceClassName, sourcePropertyName);
    public MappingAttribute(string sourcePropertyName)
        => this.MapFrom = (null, sourcePropertyName);
    public (string SourceClassName, string SourcePropertyName) MapFrom { get; set; }
}
