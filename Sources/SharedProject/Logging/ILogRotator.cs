namespace Mohammad.Logging
{
    public interface ILogRotator
    {
        bool IsLogRotationEnabled { get; set; }
    }
}