namespace Library.Web;

public record struct ApiInfo(in string? AreaName, in string? ControllerName, in string? ActionName)
{
    public static implicit operator (string? AreaName, string? ControllerName, string? ActionName)(ApiInfo value) =>
        (value.AreaName, value.ControllerName, value.ActionName);
    public static implicit operator ApiInfo((string? AreaName, string? ControllerName, string? ActionName) value) =>
        new(value.AreaName, value.ControllerName, value.ActionName);
}