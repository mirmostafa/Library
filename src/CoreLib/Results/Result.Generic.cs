//using Library.Exceptions;
//using Library.Interfaces;
//using Library.Windows;

//namespace Library.Results;

//public record Result<TValue> : ResultBase
//    , IAdditionOperators<Result<TValue>, Result<TValue>, Result<TValue>>
//    , IEquatable<Result<TValue?>>
//{
//    public Result(in TValue value, in object? status = null, in string? message = null) : base(status, message)
//        => this.Value = value;

//    public Result(in TValue value, object? status, NotificationMessage? fullMessage) : base(status, fullMessage)
//        => this.Value = value;

//    public Result(in TValue value, object? status, [DisallowNull] IException exception) : base(status, exception)
//        => this.Value = value;

//    public static Result<TValue?> Fail
//        => CreateFailure(error: -1);

//    public TValue Value { get; }

//    [return: NotNull]
//    public static Result<TValue?> CreateFailure(in string? message = null, in TValue? value = default, in object? error = null)
//    {
//        Result<TValue?> result = new(value, error ?? -1, message);
//        result.SetIsSucceed(false);
//        return result;
//    }

//    [return: NotNull]
//    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in object? status = null)
//    {
//        Result<TValue> result = new(value, status, message);
//        result.SetIsSucceed(true);
//        return result;
//    }

//    public static Result<TValue> From<TValue1>([DisallowNull] in Result<TValue1> other, in TValue value)
//        => Add(other, new Result<TValue>(value));

//    public static Result<TValue> From([DisallowNull] in Result<TValue> @this, in Result<TValue> other)
//        => Add(@this, other);

//    public static Result<TValue> From([DisallowNull] in Result other, in TValue value)
//        => Add(other, new Result<TValue>(value));

//    public static implicit operator bool(Result<TValue?> result)
//        => result.IsSucceed;

//    public static implicit operator Result(Result<TValue> result)
//        => result.ToResult();

//    public static implicit operator TValue(Result<TValue> result)
//        => result.Value;

//    public static Result<TValue> New(TValue item)
//        => new(item);

//    public static Result<TValue> operator +(Result<TValue> left, Result<TValue> right)
//        => right.With(left);

//    public void Deconstruct(out object? Status, out NotificationMessage? Message, out TValue Value)
//        => (Status, Message, Value) = (this.Status, this.FullMessage, this.Value);

//    public void Deconstruct(out bool isSucceed, out TValue Value)
//        => (isSucceed, Value) = (this.IsSucceed, this.Value);

//    public virtual bool Equals(Result<TValue?>? other)
//        => other is not null && (other.Status, other.IsSucceed) == (this.Status, this.IsSucceed) && (other.Value?.Equals(this.Value) ?? this.Value is null);

//    //public override bool Equals(object? obj)
//    //    => this.Equals(obj as Result<TValue?>);

//    public override int GetHashCode()
//        => this.Value?.GetHashCode() ?? 0;

//    public bool IsValid()
//        => this is not null and { IsSucceed: true } and { Value: not null };

//    public Task<Result<TValue>> ToAsync()
//        => Task.FromResult(this);

//    public Result ToResult()
//        => this.IsSucceed ? Result.CreateSuccess(this.FullMessage, this.Status) : Result.CreateFailure(this.FullMessage, this.Status);

//    public Result<TValue1> ToResult<TValue1>(TValue1 value)
//        => Add(this, new Result<TValue1>(value));

//    public Result<TValue1> ToResult<TValue1>(in Func<Result<TValue>, TValue1> action)
//        => Add(this, new Result<TValue1>(action(this)));

//    public Result<TValue1> With<TValue1>(in TValue1 value1)
//        => Result<TValue1>.From(this, value1);

//    public Result<TValue> With(in Result<TValue> other)
//        => Result<TValue>.From(this, other);

//    public Result<TValue> AddError(object data)
//    {
//        this.Errors.Add((null, data));
//        return this;
//    }
//}