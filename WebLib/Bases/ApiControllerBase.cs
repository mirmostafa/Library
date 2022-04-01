#define USE_ASPNETCORE
using System.Collections;
using System.Diagnostics;
using System.Net;
using Library.Cqrs.Models.Commands;
using Library.Cqrs.Models.Queries;
using Library.Helpers;
using Library.Web.Results;
using Microsoft.AspNetCore.Mvc;

#if !USE_ASPNETCORE
using System;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
#endif

namespace Library.Web.Bases;

public abstract class ApiControllerBase
#if USE_ASPNETCORE
        : ControllerBase
#endif

{
    private ICommandProcessor? _commandProcessor;
    private IQueryProcessor? _queryProcessor;
#if !USE_ASPNETCORE
    #region .NET Core ControllerBase
        private ControllerContext? _controllerContext;
        private IUrlHelper? _url;

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.ControllerContext.
        /// </summary>
        /// <value>
        /// The controller context.
        /// </value>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <remarks>
        ///     Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator activates this property
        ///     while activating controllers. If user code directly instantiates a controller,
        ///     the getter returns an empty Microsoft.AspNetCore.Mvc.ControllerContext.
        /// </remarks>
        [ControllerContext]
        protected ControllerContext ControllerContext
        {
            get
            {
                if (this._controllerContext == null)
                {
                    this._controllerContext = new ControllerContext();
                }

                return this._controllerContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._controllerContext = value;
            }
        }
        
        // <summary>
        /// ets or sets the Microsoft.AspNetCore.Mvc.IUrlHelper.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        /// <exception cref="ArgumentNullException">value</exception>
        public IUrlHelper? Url
        {
            get
            {
                if (this._url == null)
                {
                    this._url = this.GetService<IUrlHelperFactory>()?.GetUrlHelper(this.ControllerContext);
                }

                return this._url;
            }
            set
            {
                this._url = value ?? throw new ArgumentNullException("value");
                ;
            }
        }

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpContext for the executing action.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        protected HttpContext HttpContext => this.ControllerContext.HttpContext;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpRequest for the executing action.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        protected HttpRequest Request => this.HttpContext.Request;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpResponse for the executing action.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        protected HttpResponse Response => this.HttpContext.Response;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary that contains the state of the model and of model-binding validation.
        /// </summary>
        /// <value>
        /// The state of the model.
        /// </value>
        protected ModelStateDictionary ModelState => this.ControllerContext.ModelState;

        /// <summary>
        /// Gets or sets the raw query string used to create the query collection in Request.Query.
        /// </summary>
        /// <value>
        /// The raw query string.
        /// </value>
        protected QueryString QueryString => this.HttpContext.Request.QueryString;

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        protected IHeaderDictionary Headers => this.HttpContext.Request.Headers;

        /// <summary>
        /// Gets or sets the RequestBody Stream.
        /// </summary>
        /// <value>
        /// The RequestBody Stream.
        /// </value>
        protected Stream Body => this.HttpContext.Request.Body;

        /// <summary>
        /// Gets the collection of Cookies for this request.
        /// </summary>
        /// <value>
        /// The collection of Cookies for this request.
        /// </value>
        protected IRequestCookieCollection Cookies => this.HttpContext.Request.Cookies;

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object that redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found)
        ///     to the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentNullException">url</exception>
        protected virtual RedirectResult Redirect(in string url)
            => !string.IsNullOrEmpty(url) ? new RedirectResult(url) : throw new ArgumentNullException(nameof(url));

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent
        ///     set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently)
        ///     using the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentNullException">url</exception>
        protected virtual RedirectResult RedirectPermanent(in string url)
            => !string.IsNullOrEmpty(url) ? new RedirectResult(url, permanent: true) : throw new ArgumentNullException(nameof(url));

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object that redirects
        ///     (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified local
        ///     localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</returns>
        /// <exception cref="ArgumentNullException">localUrl</exception>
        protected virtual LocalRedirectResult LocalRedirect(in string localUrl)
            => !string.IsNullOrEmpty(localUrl) ? new LocalRedirectResult(localUrl) : throw new ArgumentNullException(nameof(localUrl));

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        ///     set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently)
        ///     using the specified localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</returns>
        /// <exception cref="ArgumentNullException">localUrl</exception>
        protected virtual LocalRedirectResult LocalRedirectPermanent(in string localUrl)
        {
            return !string.IsNullOrEmpty(localUrl)
                ? new LocalRedirectResult(localUrl, permanent: true)
                : throw new ArgumentNullException(nameof(localUrl));
        }

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to an action
        //     with the same name as current one. The 'controller' and 'action' names are retrieved
        //     from the ambient values of the current request.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to an action
        ///     with the same name as current one. The 'controller' and 'action' names are retrieved
        ///     from the ambient values of the current request.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction() => this.RedirectToAction(null);

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        ///     action using the actionName.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction(in string actionName) => this.RedirectToAction(actionName, null);

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        ///     action using the actionName and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction(in string actionName, in object? routeValues)
            => this.RedirectToAction(actionName, null, routeValues);

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        ///     action using the actionName and the controllerName.
        /// </summary>
        /// <param name="actionName">The name of the action..</param>
        /// <param name="controllerName">The name of the controller..</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction(in string actionName, in string? controllerName)
            => this.RedirectToAction(actionName, controllerName, null);

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        ///     action using the specified actionName, controllerName, and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The parameters for a route..</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction(in string actionName, in string? controllerName, in object? routeValues)
            => this.RedirectToAction(actionName, controllerName, routeValues, null);

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        ///     action using the specified actionName, controllerName, and fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="fragment">TThe fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        protected virtual RedirectToActionResult RedirectToAction(in string actionName, in string? controllerName, in string? fragment)
            => this.RedirectToAction(actionName, controllerName, null, fragment);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the specified actionName, controllerName, routeValues, and fragment.
        //
        // Parameters:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        protected virtual RedirectToActionResult RedirectToAction(in string actionName,
            in string? controllerName,
            in object? routeValues,
            in string? fragment)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        protected virtual RedirectToRouteResult RedirectToRoute(in string routeName) => this.RedirectToRoute(routeName, null);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeValues.
        //
        // Parameters:
        //   routeValues:
        //     The parameters for a route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoute(in object routeValues) => this.RedirectToRoute(null, routeValues);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName and routeValues.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoute(string? routeName, object? routeValues)
            => this.RedirectToRoute(routeName, routeValues, null);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName and fragment.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoute(in string routeName, string? fragment)
            => this.RedirectToRoute(routeName, null, fragment);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName, routeValues, and fragment.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoute(string? routeName, object? routeValues, string? fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName)
            => this.RedirectToRoutePermanent(routeName, null);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeValues.
        //
        // Parameters:
        //   routeValues:
        //     The parameters for a route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoutePermanent(object routeValues) => this.RedirectToRoutePermanent(null, routeValues);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName and routeValues.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues) => this.RedirectToRoutePermanent(routeName, routeValues, null);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName and fragment.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, string? fragment) => this.RedirectToRoutePermanent(routeName, null, fragment);

        //
        // Summary:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName, routeValues, and fragment.
        //
        // Parameters:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.

        protected virtual RedirectToRouteResult RedirectToRoutePermanent(in string routeName, in object? routeValues, in string? fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedResult that produces an Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        //     response.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedResult for the response.

        protected virtual UnauthorizedResult Unauthorized() => new UnauthorizedResult();

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized response.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response.

        protected virtual UnauthorizedObjectResult Unauthorized(object value) => new UnauthorizedObjectResult(value);

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundResult for the response.

        protected virtual NotFoundResult NotFound() => new NotFoundResult();

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundObjectResult for the response.

        protected virtual NotFoundObjectResult NotFound(object value) => new NotFoundObjectResult(value);

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Parameters:
        //   modelState:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors
        //     to be returned to the client.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.

        protected virtual BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException("modelState");
            }

            return new BadRequestObjectResult(modelState);
        }

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.ConflictResult for the response.

        protected virtual ConflictResult Conflict() => new ConflictResult();

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Parameters:
        //   error:
        //     Contains errors to be returned to the client.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.

        protected virtual ConflictObjectResult Conflict(object error) => new ConflictObjectResult(error);

        //
        // Summary:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Parameters:
        //   modelState:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors
        //     to be returned to the client.
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.

        protected virtual ConflictObjectResult Conflict(ModelStateDictionary modelState) => new ConflictObjectResult(modelState);
    #endregion
#endif

    protected virtual IQueryProcessor QueryProcessor => this._queryProcessor ??= this.GetService<IQueryProcessor>();
    protected virtual ICommandProcessor CommandProcessor => this._commandProcessor ??= this.GetService<ICommandProcessor>();

    protected virtual IApiResult Result(in string? message = null, in int statusCode = (int)HttpStatusCode.OK)
        => ApiResult.New(statusCode, message);

    protected virtual IApiResult<TResult?> Result<TResult>(in TResult? result,
        in string? message = null,
        in int statusCode = (int)HttpStatusCode.OK,
        in bool isSucceed = true)
        => ApiResult<TResult>.New(statusCode, message, isSucceed, result);

    protected virtual IApiResult Succees() =>
        ApiResult.New(HttpStatusCode.OK.ToInt());

    protected virtual IApiResult<CommandResult> Succees(CommandResult commandResult)
        => ApiResult<CommandResult>.Ok(commandResult)!;

    protected virtual IApiResult<TResponseDto?> Succees<TResponseDto>(TResponseDto? result)
        => ApiResult<TResponseDto?>.Ok(result);

    protected virtual IApiResult<TResult?> Succees<TResult>(CommandResult<TResult> commandResult!!)
        => ApiResult<TResult?>.Ok(commandResult.Result);

    protected virtual IApiResult<TResult?> NoCotent<TResult>(TResult? result)
        => ApiResult<TResult>.New(HttpStatusCode.NoContent, "آیتمی یافت نشد.", false, default);

    protected new virtual IApiResult BadRequest()
        => ApiResult.New(statusCode: HttpStatusCode.BadRequest.ToInt());

    protected virtual IApiResult BadRequest(string message)
        => ApiResult.New(HttpStatusCode.BadRequest.ToInt(), message);

    [DebuggerStepThrough]
    protected virtual T GetService<T>()
        where T : notnull => this.HttpContext.RequestServices.GetRequiredService<T>();

    [DebuggerStepThrough]
    protected async Task<IApiResult<TResult?>> EnqueryAsync<TQueryResult, TResult>(IQuery<TQueryResult> queryParameter)
        where TQueryResult : IQueryResult<TResult>
    {
        var cqResult = await this.QueryProcessor.ExecuteAsync(queryParameter);
        var result = cqResult.Result;
        return this.ProcessResult(result);
    }

    [DebuggerStepThrough]
    protected async Task<IApiResult<TResult?>> EnqueryAsync<TQueryParameter, TQueryResult, TResult>()
           where TQueryParameter : IQuery<TQueryResult>, new()
           where TQueryResult : IQueryResult<TResult>
    {
        var cqResult = await this.QueryProcessor.ExecuteAsync(new TQueryParameter());
        return this.ProcessResult(cqResult.Result);
    }

    [DebuggerStepThrough]
    protected virtual async Task<IApiResult<bool>> ExecuteAsync<TCommand, TCommandResult>(TCommand parameter)
    {
        _ = await this.CommandProcessor.ExecuteAsync<TCommand, TCommandResult>(parameter);
        return this.Succees(true);
    }

    [DebuggerStepThrough]
    protected virtual async Task<IApiResult<bool>> ExecuteAsync<TCommand, TCommandResult>()
        where TCommand : new()
    {
        _ = await this.CommandProcessor.ExecuteAsync<TCommand, TCommandResult>(new());
        return this.Succees(true);
    }

    protected virtual IApiResult<TResult?> ProcessResult<TResult>(TResult result)
    {
        switch (result)
        {
            case null:
            case IEnumerable items when !items.Any():
                return this.NoCotent(result);
            default:
                return this.Succees(result);
        }
    }

}

#if false
Can use as MVC or POCO. In POCO-mode HttpContext was null.
#endif