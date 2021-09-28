namespace Library.Ioc;

public interface ILibServiceInstaller
{
    int? Order { get; set; }
    void Install();
}
