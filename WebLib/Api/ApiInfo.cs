using System.Reflection;
using Library.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Library.Web.Api;

public record struct ApiInfo(in string? AreaName, in string? ControllerName, in string? ActionName)
{
    public ApiInfo(ControllerActionDescriptor cad)
        : this(cad.ArgumentNotNull().MethodInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue ?? "NoArea", cad.ControllerName, cad.ActionName) { }

    public void Deconstruct(out string? areaName, out string? controllerName, out string? actionName)
        => (areaName, controllerName, actionName) = (this.AreaName, this.ControllerName, this.ActionName);
    public static implicit operator (string? AreaName, string? ControllerName, string? ActionName)(ApiInfo value) =>
        (value.AreaName, value.ControllerName, value.ActionName);
    public static implicit operator ApiInfo((string? AreaName, string? ControllerName, string? ActionName) value) =>
        new(value.AreaName, value.ControllerName, value.ActionName);
    public (string? AreaName, string? ControllerName, string? ActionName) ToValueTuple() =>
        (this.AreaName, this.ControllerName, this.ActionName);
    public static ApiInfo ToApiInfo(in string? areaName, in string? controllerName, in string? actionName) =>
        new(areaName, controllerName, actionName);
}