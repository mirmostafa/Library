namespace Library.Ioc;

public interface IServiceInstaller
{
    int? Order { get; set; }

    void Install();
}