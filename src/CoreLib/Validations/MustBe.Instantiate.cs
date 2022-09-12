using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public sealed class MustBe<TObject>
{
    private readonly string _argName;
    private readonly TObject _obj;
    private readonly Result<TObject> _result;

    public MustBe(TObject obj, [CallerArgumentExpression("obj")] string? argName = null)
        => (this._obj, this._argName, this._result) = (obj, argName!, Result<TObject>.CreateSuccess(this._obj));

    public MustBe<TObject> ArgumentNotNull(Func<string>? getMessage = null)
        => this.InnerNullCheck(() => getMessage is null ? new ArgumentNullException(this._argName) : new ArgumentNullException(getMessage(), (Exception?)default));

    public MustBe<TObject> Clear()
    {
        this._result.Errors.Clear();
        return this;
    }

    public MustBe<TObject> IsValid(bool valid, Func<Exception> getExceptionOnNotValid)
    {
        if (!valid)
        {
            this._result.Errors.Add((this._obj, getExceptionOnNotValid()));
        }

        return this;
    }

    public MustBe<TObject> NotNull()
            => this.InnerNullCheck(() => new NullValueValidationException(this._argName));

    public MustBe<TObject> NotNull(Func<string> getMessage)
        => this.InnerNullCheck(() => new NullValueValidationException(getMessage(), default));

    public MustBe<TObject> NotNull(Func<Exception> getMessage)
        => this.InnerNullCheck(getMessage);

    public Result<TObject> Result()
            => this._result;

    public Result<TObject> ThrowOnFail(object? owner = null, string? instruction = null)
        => this.Result().ThrowOnFail(owner, instruction);

    private MustBe<TObject> InnerNullCheck(Func<Exception> getexception)
    {
        (bool IsValid, Func<Exception> GetException) result;
        switch (this._obj)
        {
            case string s when s.IsNullOrEmpty():
                result.GetException = getexception;
                result.IsValid = false;
                break;

            case null:
                result.GetException = getexception;
                result.IsValid = false;
                break;

            default:
                result.GetException = null!;
                result.IsValid = true;
                break;
        };
        return this.IsValid(result.IsValid, result.GetException);
    }
}

public sealed class Check<TObject>
{
    private readonly string _argName;
    private readonly TObject _obj;

    public Check(TObject obj, [CallerArgumentExpression("obj")] string? argName = null)
        => (this._obj, this._argName) = (obj, argName!);

    public Check<TObject> ArgumentNotNull(object? owner = null, string? instruction = null)
    {
        _ = this._obj.MustBe(this._argName).ArgumentNotNull().ThrowOnFail(owner, instruction);
        return this;
    }

    public Check<TObject> ArgumentNotNull(Func<string>? getMessage = null, object? owner = null, string? instruction = null)
    {
        _ = this._obj.MustBe(this._argName).ArgumentNotNull(getMessage).ThrowOnFail(owner, instruction);
        return this;
    }
}