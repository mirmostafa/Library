using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;
using Library.Results;

namespace UnitTest;

//[TestClass]
//public class ValidationTest
//{
//    [TestMethod]
//    public void _01_ValidationResult_BeNotNullNotNullTest()
//    {
//        var arg = "Test";

//        arg = arg.If().NotNull().ThrowOnFail();
//    }

//    [TestMethod]
//    [ExpectedException(typeof(NullValueValidationException))]
//    public void _02_ValidationCheck_BeNotNullNullTest()
//    {
//        string? arg = null;

//        arg = arg.Should().BeNotNull();
//    }

//    [TestMethod]
//    public void _03_ValidationResult_ArgumentBeNotNullNotNullTest()
//    {
//        var arg = "Test";

//        arg = arg.If().ArgumentIsNotNull().ThrowOnFail();
//    }

//    [TestMethod]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void _04_ValidationCheck_ArgumentBeNotNullNullTest()
//    {
//        string? arg = null;

//        arg = arg.If().ArgumentIsNotNull().Build();
//    }
//}

public static class Validation
{
    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, [CallerArgumentExpression(nameof(value))] string argName = null!) 
        => new(value, argName);
}

public sealed class ValidationResultSet<TValue>
{
    private readonly List<(Func<TValue, bool> IsValid, Func<Exception> OnError)> _rules;
    private readonly string ValueName;

    public ValidationResultSet(TValue value, string valueName)
        => (this.Value, this.ValueName, this._rules) = (value, valueName, new());

    public IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> Rules 
        => this._rules.ToEnumerable();

    public TValue Value { get; }

    public Result<TValue> Build()
    {
        var result = new Result<TValue>(this.Value);
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                result += Result<TValue>.CreateFail(onError(), this.Value);
            }
        }
        return result;
    }

    public ValidationResultSet<TValue> ArgumentNotNull(Func<Exception>? onError = null)
        => this.AddRule(x => x, x => this.Value is not null, onError, () => new ArgumentNullException(this.ValueName));

    public ValidationResultSet<TValue> IsNotBiggerThan(Expression<Func<TValue, int>> propertyExpression, int max, Func<Exception>? onError = null)
            => this.AddRule(propertyExpression, x => !(x > max), onError, x => new ValidationException(x));

    public ValidationResultSet<TValue> IsNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullReferenceException(x));

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<Exception> onError)
    {
        this._rules.Add((isValid, onError));
        return this;
    }

    public TValue ThrowOnFail()
    {
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                throw onError();
            }
        }
        return this.Value;
    }

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value)
        => propertyExpression.Compile().Invoke(value);

    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<string, Exception> onErrorAlternative)
    {
        this._rules.Add((x => isValid(Invoke(propertyExpression, x)), onError ?? this.GetOnError(propertyExpression, onErrorAlternative)));
        return this;
    }

    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<Exception> onErrorAlternative)
    {
        this._rules.Add((x => isValid(Invoke(propertyExpression, x)), onError ?? onErrorAlternative));
        return this;
    }

    private Func<Exception> GetOnError<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError) 
        => () => onError(this.GetPropertyName(propertyExpression));

    private string GetPropertyName<TType>(Expression<Func<TValue, TType>> propertyExpression)
        => ObjectHelper.GetPropertyInfo(this.Value, propertyExpression).Name;
}

[TestClass]
public class ValidationTest
{
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _01_BasicTest()
    {
        var arg = new Person("", 0);
        arg = arg.Check().RuleFor(x => !x.Name.IsNullOrEmpty(), () => new NullReferenceException()).ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _02_IsNullOrEmptyStringTest()
    {
        var arg = new Person("", 0);
        arg = arg.Check().IsNullOrEmpty(x => x.Name).Build().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void _03_IsBiggerThanIntTest()
    {
        var arg = new Person("", 20);
        arg = arg.Check().IsNotBiggerThan(x => x.Age, 10).Build().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void _04_IsArgumentNullNullTest()
    {
        Person? arg = null;
        _ = arg.Check().ArgumentNotNull().ThrowOnFail();
    }

    [TestMethod]
    public void _05_IsArgumentNullNotNullTest()
    {
        var arg = new Person("", 20);
        _ = arg.Check().ArgumentNotNull().ThrowOnFail();
    }
}

file record Person(string Name, int Age);