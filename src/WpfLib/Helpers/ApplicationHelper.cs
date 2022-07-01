using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Library.Wpf.Helpers;

public static partial class ApplicationHelper
{
    private static string? _Company;

    public static string? ApplicationTitle => CalculatePropertyValue<AssemblyTitleAttribute>("Title");

    public static string? Company
    {
        get => _Company ?? CalculatePropertyValue<AssemblyCompanyAttribute>(nameof(Company));
        set => _Company = value;
    }

    public static string? Copyright => CalculatePropertyValue<AssemblyCopyrightAttribute>(nameof(Copyright));

    public static string? Description => CalculatePropertyValue<AssemblyDescriptionAttribute>(nameof(Description));
    public static string? Guid => CalculatePropertyValue<GuidAttribute>("Value");
    public static string? ProductTitle => CalculatePropertyValue<AssemblyProductAttribute>("Product");

    public static string? Version
    {
        get
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            return version?.ToString();
        }
    }

    public static string GetAboutAppString(string? moreInfo = null)
    {
        var builder = new StringBuilder();
        _ = builder.AppendLine($"Product:    \t{ProductTitle}")
            .AppendLine($"Application:\t{ApplicationTitle}")
            .AppendLine($"Version:    \t{Version}")
            .AppendLine(Description)
            .AppendLine();
        if (!moreInfo.IsNullOrEmpty())
        {
            _ = builder.AppendLine(moreInfo).AppendLine();
        }

        _ = builder.AppendLine(Copyright);
        return builder.ToString();
    }

    private static string? CalculatePropertyValue<T>(string propertyName)
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly is null)
        {
            return string.Empty;
        }

        var attributes = assembly.GetCustomAttributes(typeof(T), false);
        if (attributes.Length <= 0)
        {
            return string.Empty;
        }

        var attrib = (T)attributes[0];
        var property = attrib.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        return property is not null ? property.GetValue(attributes[0], null) as string : string.Empty;
    }

    public static void RunInUiThread(this Application app, Action action)
        => app?.Dispatcher.Invoke(DispatcherPriority.Render, action);

    public static Application DoEvents(this Application app)
    {
        var frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            new DispatcherOperationCallback(
                delegate (object f)
                {
                    ((DispatcherFrame)f).Continue = false;
                    return null;
                }), frame);
        Dispatcher.PushFrame(frame);
        return app;
    }
}
