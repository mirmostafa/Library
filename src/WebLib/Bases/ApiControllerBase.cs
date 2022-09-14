using System.Diagnostics;
using System.Net;

using Library.Cqrs.Models.Commands;
using Library.Cqrs.Models.Queries;
using Library.Validations;
using Library.Web.Results;

using Microsoft.AspNetCore.Mvc;

using StandardResult = Library.Results.Result;

namespace Library.Web.Bases;

public abstract class ApiControllerBase : ControllerBase
{
    private ICommandProcessor? _commandProcessor;
    private IQueryProcessor? _queryProcessor;

    protected virtual IQueryProcessor QueryProcessor => this._queryProcessor ??= this.GetService<IQueryProcessor>();
    protected virtual ICommandProcessor CommandProcessor => this._commandProcessor ??= this.GetService<ICommandProcessor>();

    protected virtual IApiResult Succees()
        => ApiResult.New(HttpStatusCode.OK.ToInt());

    protected virtual IApiResult<CommandResult> Succees(CommandResult commandResult)
        => ApiResult<CommandResult>.Ok(commandResult)!;

    protected virtual IApiResult<TResponseDto?> Succees<TResponseDto>(TResponseDto? result)
        => ApiResult<TResponseDto?>.Ok(result);

    protected virtual IApiResult<TResult?> Succees<TResult>(CommandResult<TResult> commandResult)
        => ApiResult<TResult?>.Ok(commandResult.NotNull().Result);

    protected virtual IApiResult<TResult?> NoCotent<TResult>(TResult? result)
        => this.Fail<TResult>(HttpStatusCode.NoContent.ToInt(), "آیتمی یافت نشد.");

    protected new virtual IApiResult BadRequest()
        => this.Fail(statusCode: HttpStatusCode.BadRequest.ToInt());

    protected virtual IApiResult BadRequest(string message)
        => this.Fail(HttpStatusCode.BadRequest.ToInt(), message);

    protected virtual IApiResult Fail(int? statusCode, string? message = null)
        => ApiResult.New(statusCode ?? HttpStatusCode.BadRequest.ToInt(), message);

    protected virtual IApiResult<TResult?> Fail<TResult>(int? statusCode, string? message = null, TResult? result = default)
        => new ApiResult<TResult?>(statusCode, message, result);

    [DebuggerStepThrough]
    protected virtual T GetService<T>()
        where T : notnull
        => this.HttpContext.RequestServices.GetRequiredService<T>();

    [DebuggerStepThrough]
    protected async Task<IApiResult<TResult?>> EnqueryAsync<TQueryResult, TResult>(IQuery<TQueryResult> queryParameter)
        where TQueryResult : IQueryResult<TResult>
    {
        var cqResult = await this.QueryProcessor.ExecuteAsync(queryParameter);
        return this.ProcessResult(cqResult.Result);
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

    protected virtual IApiResult Result(in string? message = null, in int statusCode = (int)HttpStatusCode.OK)
        => ApiResult.New(statusCode, message);

    protected virtual IApiResult<TResult?> Result<TResult>(in TResult? result, in string? message = null, in int statusCode = (int)HttpStatusCode.OK)
        => ApiResult<TResult>.New(statusCode, message, result);

    protected virtual IApiResult<TResult?> ProcessResult<TResult>(in TResult result)
        => result switch
        {
            null => this.NoCotent(result),
            //x IEnumerable items when !items.Any() => this.NoCotent(result),
            StandardResult { IsSucceed: true } res => this.Succees(result),
            StandardResult { IsFailure: true } res => this.Fail(res.Status?.ToInt(), res.Message, result),
            _ => this.Succees(result),
        };
}