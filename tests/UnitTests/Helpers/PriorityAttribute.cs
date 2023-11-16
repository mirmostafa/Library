[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PriorityAttribute(int priority) : Attribute
{
    public int Priority { get; private set; } = priority;
}