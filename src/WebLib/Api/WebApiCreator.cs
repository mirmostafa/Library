using Library.Helpers;
using Library.Validations;

namespace Library.Web.Api;
public sealed class WebApiCreator
{
    public WebApiCreator(WebApplication? app = null, IEnumerable<string>? args = null) =>
        this.App = GetApp(app) ?? WebApplication.CreateBuilder(args?.ToArray() ?? Array.Empty<string>()).Build();

    public static WebApiCreator New(WebApplication? app = null, params string[] urls) =>
        new WebApiCreator(app).AddUrl(urls);
    public static WebApiCreator New(params string[] urls) =>
        new WebApiCreator(null).AddUrl(urls);

    public WebApplication App { get; }
    public ICollection<string> Urls => this.App.Urls;

    public static (WebApiCreator Creator, WebApplication App) Create(in WebApplication? app, in HttpMethod method, in string route, in Delegate body)
    {
        var creator = New(app).Create(method, route, body);
        return (creator, creator.App);
    }

    private static WebApplication GetApp(WebApplication? app) =>
        app ?? WebApplication.CreateBuilder().Build();

    public WebApiCreator Create(in HttpMethod method, in string route, in Delegate body)
    {
        Check.MustBeArgumentNotNull(route);
        Check.MustBeArgumentNotNull(body);

        _ = method switch
        {
            HttpMethod.None => null,
            HttpMethod.Get => this.App.MapGet(route, body),
            HttpMethod.Post => this.App.MapPost(route, body),
            HttpMethod.Put => this.App.MapPut(route, body),
            HttpMethod.Patch => throw new NotSupportedException(),
            HttpMethod.Delete => this.App.MapDelete(route, body),
            _ => throw new NotSupportedException(),
        };
        return this;
    }

    public WebApiCreator MapDelete(string route, Delegate body) =>
        this.Create(HttpMethod.Delete, route, body);
    public WebApiCreator MapPut(string route, Delegate body) =>
        this.Create(HttpMethod.Put, route, body);
    public WebApiCreator MapPost(string route, Delegate body) =>
        this.Create(HttpMethod.Post, route, body);
    public WebApiCreator MapGet(string route, Delegate body) =>
        this.Create(HttpMethod.Get, route, body);

    public void Deconstruct(out WebApiCreator creator, out WebApplication app) =>
        (creator, app) = (this, this.App);

    public WebApiCreator AddUrl(params string[] url) =>
        this.Fluent(() => this.Urls.AddRange(url));
    public Task<WebApiCreator> RunAsync() =>
        this.Fluent().Async(async () => await this.App.RunAsync());
}

public enum HttpMethod
{
    None = 0,
    Get = 1,
    Post = 2,
    Put = 3,
    Patch = 4,
    Delete = 5,
}
