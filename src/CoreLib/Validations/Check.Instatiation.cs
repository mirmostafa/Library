using System.Runtime.CompilerServices;

using Library.Results;

namespace Library.Validations;

public sealed class MustBe<T>
{
    private readonly string? _argName;

    private readonly T? _obj;

    public MustBe([AllowNull] T? obj, [CallerArgumentExpression("obj")] string? argName = null)
    {
        this._obj = obj;
        this._argName = argName;
    }

    [return: NotNull]
    public Result<T> ArgumentNotNull()
        => this._obj switch
        {
            string s => !s.IsNullOrEmpty() 
                        ? Result<T>.CreateSuccess(this._obj) 
                        : Result<T>.CreateFail(value: this._obj, error: new ArgumentNullException(this._argName))!,
            _ => this._obj is not null ? Result<T>.CreateSuccess(this._obj) : Result<T>.CreateFail(value: this._obj)!
        };
}