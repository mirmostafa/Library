using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Creational;
using Library.DesignPatterns.Creational.Exceptions;
using Library.Exceptions;
using Library.Interfaces;
using Library.Results;
using Library.Types;
using Library.Validations;

namespace Library.Helpers;

public static class ObjectHelper
{
    private static readonly ConditionalWeakTable<object, Dynamic.Expando> _propsExpando = new();

    /// <summary>
    /// Checks the database null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o">The o.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="converter">The converter.</param>
    /// <returns></returns>
    public static T CheckDbNull<T>(in object? o, in T defaultValue, in Func<object, T> converter)
        => IsDbNull(o) ? defaultValue : converter.Invoke(o);

    /// <summary>
    /// Determines whether an object contains properties that implement a specified interface.
    /// </summary>
    /// <param name="obj">The object to check for properties implementing the interface.</param>
    /// <param name="type">The interface type to check for.</param>
    /// <returns>
    /// True if the object has properties implementing the specified interface, otherwise false.
    /// </returns>
    public static bool Contains(in object? obj, Type type)
        => obj is not null && obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(property =>
                    property.PropertyType.FindInterfaces(
                        (m, filterCriteria) => m.FullName == type.FullName, // Check if the interface's full name matches the specified type's full name.
                        null)
                    .Any()); // Check if any interface implementation matches the specified type.

    /// <summary>
    /// Generates the lazy singleton instance.
    /// </summary>
    /// <typeparam name="TSingleton">The type of the singleton.</typeparam>
    /// <param name="createInstance">The create instance.</param>
    /// <param name="initializeInstance">The initialize instance.</param>
    /// <returns></returns>
    public static Lazy<TSingleton> GenerateLazySingletonInstance<TSingleton>(
        Func<TSingleton>? createInstance = null,
        Action<TSingleton>? initializeInstance = null)
        where TSingleton : class, ISingleton<TSingleton>
        => new(() => GenerateSingletonInstance<TSingleton>());

    /// <summary>
    /// Generates a singleton instance of a class (Must be cached by the owner class).
    /// </summary>
    /// <typeparam name="TSingleton">The type of class which is being instantiated.</typeparam>
    /// <param name="createInstance">A delegate to be used for getting an instance of</param>
    /// <param name="initializeInstance">The instance initializer.</param>
    /// <returns></returns>
    /// <exception cref="SingletonException">
    /// The class must have a static method: "TSingleton CreateInstance()" or a private/protected constructor.
    /// </exception>
    /// <remarks>
    /// Before generating instance, GenerateSingletonInstance searches for a static method:
    /// "TSingleton CreateInstance()". If found, GenerateSingletonInstance calls it and retrieves an
    /// instance. Otherwise, a private/protected parameter-less constructor is required to construct
    /// an instance of TSingleton./&gt; After generating instance, searches for a method named:
    /// "InitializeComponents". If found will be called.
    /// </remarks>
    [return: NotNull]
    public static TSingleton GenerateSingletonInstance<TSingleton>(Func<TSingleton>? createInstance = null, Action<TSingleton>? initializeInstance = null)
        where TSingleton : class, ISingleton<TSingleton>
    {
        //! If (T) has implemented CreateInstance as a static method, use it to create an instance
        var ci = createInstance ?? GetMethod<Func<TSingleton>>(typeof(TSingleton),
            "CreateInstance",
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static);
        var result = ci?.Invoke();

        //! if not, try to find a non-public constructor instead. A non-public constructor is mandatory.
        if (result is null)
        {
            var constructor = typeof(TSingleton).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                EnumerableHelper.EmptyArray<Type>(),
                EnumerableHelper.EmptyArray<ParameterModifier>()) ?? throw new SingletonException($"""The class must have a static method: "{typeof(TSingleton)} CreateInstance()" or a private/protected parameter-less constructor.""");
            result = constructor.Invoke(EnumerableHelper.EmptyArray<object>()) as TSingleton;

            //! Just to make sure that the code will work.
            if (result is null)
            {
                throw new SingletonException("Couldn't create instance.");
            }
        }

        //! If (T) has implemented CreateInstance as an instantiate method, use it to initialize the instance.
        var initialize = GetMethod<Action>(result, "InitializeComponents");
        initialize?.Invoke();

        //! if(T) has initialized b"InitializeInstance" delegate, call it to initialize the instance.
        initializeInstance?.Invoke(result);
        return result;
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="inherited">if set to <c>true</c> [inherited].</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TAttribute>(in object value,
        in TAttribute? defaultValue = null,
        bool inherited = true)
        where TAttribute : Attribute
    {
        var attributes = value.GetType().GetCustomAttributes(typeof(TAttribute), inherited);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
    }

    /// <summary>
    /// Gets the specified attribute from the given property.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="property">The property.</param>
    /// <returns>The attribute.</returns>
    public static TAttribute? GetAttribute<TAttribute>(in PropertyInfo property)
        where TAttribute : Attribute =>
        property is null
                ? throw new ArgumentNullException(nameof(property))
                : property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault().Cast().As<TAttribute>();

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="type">The type.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="inherited">if set to <c>true</c> [inherited].</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TAttribute>(in Type type,
        in TAttribute? defaultValue = null,
        bool inherited = true)
        where TAttribute : Attribute
    {
        var attributes = type.GetCustomAttributes(typeof(TAttribute), inherited);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TType">The type of the type.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="inherited">if set to <c>true</c> [inherited].</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TType, TAttribute>(in TAttribute? defaultValue, in bool inherited)
        where TAttribute : Attribute
        => typeof(TType).GetCustomAttributes(typeof(TAttribute), inherited).Cast<TAttribute>().FirstOrDefault();

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TType">The type of the type.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TType, TAttribute>()
        where TAttribute : Attribute
        => typeof(TType).GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>().FirstOrDefault();

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="inherited">if set to <c>true</c> [inherited].</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TAttribute>(in object value, in bool inherited)
        where TAttribute : Attribute
        => GetAttribute<TAttribute>(value, null, inherited);

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="classInstance">The class instance.</param>
    /// <returns></returns>
    public static string? GetDescription(in object classInstance)
        => GetAttribute<DescriptionAttribute>(classInstance)?.Description;

    /// <summary>
    /// Gets the field.
    /// </summary>
    /// <typeparam name="TFieldType">The type of the field type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static TFieldType GetField<TFieldType>(in object? obj, string fieldName)
    {
        var field = obj?.GetType().GetFields().FirstOrDefault(fld => string.Compare(fld.Name, fieldName, StringComparison.Ordinal) == 0);
        return field switch
        {
            not null => field.GetValue(obj)!.Cast().To<TFieldType>(),
            null => throw new ObjectNotFoundException("Field not found")
        };
    }

    /// <summary>
    /// Calculates the hash code of an object and its properties.
    /// </summary>
    public static int GetHashCode(object o, params object[] properties)
        => properties.Aggregate(o.GetHashCode(), (hash, property) => hash ^ property.GetHashCode());

    /// <summary>
    /// Gets a method from an object using the specified name and binding flags.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags.</param>
    /// <returns>The delegate for the method, or null if the method was not found.</returns>
    public static TDelegate? GetMethod<TDelegate>([DisallowNull] in object obj,
        [DisallowNull] in string name,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        where TDelegate : class
    {
        var methodInfo = obj.ArgumentNotNull().GetType().GetMethod(name, bindingFlags);
        return methodInfo is not null
            ? Delegate.CreateDelegate(typeof(TDelegate), obj, methodInfo).Cast().As<TDelegate>()
            : null;
    }

    /// <summary>
    /// Gets the method.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="objType">Type of the object.</param>
    /// <param name="name">The name.</param>
    /// <param name="bindingFlags">The binding flags.</param>
    /// <returns></returns>
    public static TDelegate? GetMethod<TDelegate>(in Type objType,
        in string name,
        in BindingFlags bindingFlags = BindingFlags.Public)
        where TDelegate : class
    {
        var methodInfo = objType.GetMethod(name, bindingFlags);
        return methodInfo is not null
            ? Delegate.CreateDelegate(typeof(TDelegate), null, methodInfo).Cast().As<TDelegate>()
            : null;
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="propName">Name of the property.</param>
    /// <param name="eventNoDefault">The event no default.</param>
    /// <returns></returns>
    public static TPropertyType? GetProp<TPropertyType>([DisallowNull] in object obj, string propName, in int eventNoDefault)
    {
        _ = obj.ArgumentNotNull();

        if (eventNoDefault != 0)
        {
            return GetProp<TPropertyType>(obj, propName);
        }

        var property = obj.GetType()
            .GetProperties()
            .FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
        return property is not null ? (TPropertyType?)property.GetValue(obj, null) : default;
    }

    /// <summary> Gets the value of a property of a given object. </summary> <typeparam
    /// name="TPropertyType">The type of the property.</typeparam> <param name="obj">The
    /// object.</param> <param name="propName">The name of the property.</param> <param
    /// name="searchPrivates">Whether to search
    public static TPropertyType? GetProp<TPropertyType>([DisallowNull] in object obj, [DisallowNull] string propName, bool searchPrivates = false)
    {
        var type = obj.ArgumentNotNull().GetType();
        var properties = type.GetProperties();
        if (properties.Length == 0)
        {
            properties = type.GetProperties(searchPrivates
                ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Default);
        }

        var property = properties.FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
        return property is not null ? (TPropertyType?)property.GetValue(obj, null) : default;
    }

    public static object? GetProp([DisallowNull] in object obj, in string propName, bool searchPrivates = false)
        => GetProp<object>(obj, propName, searchPrivates);

    /// <summary>
    /// Gets the properties of an object as a list of tuples.
    /// </summary>
    /// <param name="obj">The object to get the properties from.</param>
    /// <returns>A read-only list of tuples containing the name and value of each property.</returns>
    public static IEnumerable<(string Name, object Value)> GetProperties(object obj)
    {
        var result = new List<(string, object)>();
        foreach (var property in obj.GetType().GetProperties())
        {
            _ = Catch(() => result.Add((property.Name, property.GetValue(obj, null)!)));
        }

        return result.AsReadOnly();
    }

    /// <summary>
    /// Gets the name of the properties.
    /// </summary>
    /// <typeparam name="TType">The type of the type.</typeparam>
    /// <returns></returns>
    public static IEnumerable<string> GetPropertiesName<TType>()
    {
        var properties = typeof(TType).IsGenericType
            ? typeof(TType).GetGenericArguments()[0].GetProperties()
            : typeof(TType).GetProperties();
        return properties.Select(property => property.Name);
    }

    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
    {
        var type = typeof(TSource);

        if (propertyLambda.Body is not MemberExpression member)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
        }

        var propInfo = member.Member as PropertyInfo;
        return propInfo == null
            ? throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()))
            : type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType)
            ? throw new ArgumentException(string.Format("Expression '{0}' refers to a property that is not from type {1}.", propertyLambda.ToString(), type))
            : propInfo;
    }

    public static bool HasAttribute<TType, TAttribute>(bool inherit)
        where TAttribute : Attribute => HasAttribute<TAttribute>(typeof(Type), inherit);

    /// <summary>
    /// Checks if the given type has an attribute of type TAttribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type has an attribute of type TAttribute, false otherwise.</returns>
    public static bool HasAttribute<TAttribute>(Type type, bool inherit)
        where TAttribute : Attribute => type.GetCustomAttribute<TAttribute>(inherit) != null;

    /// <summary>
    /// Checks if the given type has an attribute of type TAttribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type has an attribute of type TAttribute, false otherwise.</returns>
    public static bool HasAttribute<TAttribute>(Type type)
        where TAttribute : Attribute => type.GetCustomAttribute<TAttribute>() != null;

    /// <summary>
    /// Determines whether [is database null] [the specified o].
    /// </summary>
    /// <param name="o">The o.</param>
    /// <returns><c>true</c> if [is database null] [the specified o]; otherwise, <c>false</c>.</returns>
    public static bool IsDbNull([NotNullWhen(false)] in object? o)
        => o is null or DBNull;
    
    /// <summary>
    /// Checks if a given struct is equal to its default value.
    /// </summary>
    public static bool IsDefault<TStruct>(this TStruct @struct) where TStruct : struct
        => @struct.Equals(default(TStruct));

    /// <summary>
    /// Determines whether an instance of a specified type can be assigned to a variable of the
    /// current type.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="type">The type.</param>
    /// <returns>
    /// <c>true</c> if [is inherited or implemented] [the specified object]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInheritedOrImplemented(in object? obj, [DisallowNull] in Type type)
        => obj != null && type.ArgumentNotNull().IsAssignableFrom(obj.GetType());

    public static bool IsInheritedOrImplemented<T>(in object? obj)
        => IsInheritedOrImplemented(obj, typeof(T));

    /// <summary>
    /// Determines whether the specified value is null.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the specified value is null; otherwise, <c>false</c>.</returns>
    public static bool IsNull([NotNullWhen(false)] in object? value)
        => value is null;

    /// <summary>
    /// Checks if the given Guid is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid? guid)
        => guid is null || guid == Guid.Empty;

    /// <summary>
    /// Checks if the given Guid is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid guid)
        => guid == Guid.Empty;

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Id id) =>
        id.Equals(Guid.Empty) || id.Equals(0);

    public static bool IsSetMethodInit([DisallowNull] this PropertyInfo propertyInfo)
    {
        Check.MustBeArgumentNotNull(propertyInfo?.SetMethod);
        return propertyInfo.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(typeof(IsExternalInit));
    }

    /// <summary>
    /// Parses the specified input string into an object of type TSelf, using the specified format provider.
    /// </summary>
    /// <typeparam name="TSelf">The type of the self.</typeparam>
    /// <param name="input">The input string to parse.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>An object of type TSelf.</returns>
    public static TSelf Parse<TSelf>(this string input, IFormatProvider? formatProvider = null)
        where TSelf : IParsable<TSelf> => TSelf.Parse(input, formatProvider);

    [DebuggerStepThrough]
    public static dynamic props(this object o) =>
        _propsExpando.GetOrCreateValue(o);

    public static void SetField([DisallowNull] in object obj, in string fieldName, in object value)
    {
        Check.MustBeArgumentNotNull(obj);
        obj.GetType().GetField(fieldName)?.SetValue(obj, value);
    }

    public static void SetProperty([DisallowNull] in object obj, in string propertyName, in object value)
    {
        var property = obj?.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value, null);
    }

    /// <summary> Tries to parse the specified input string into an object of type TSelf, which
    /// implements IParsable<TSelf>. </summary> <typeparam name="TSelf">The type of the object to
    /// parse.</typeparam> <param name="input">The input string to parse.</param> <param
    /// name="formatProvider">The format provider to use for parsing.</param> <returns>A
    /// TryMethodResult containing the result of the parse operation.</returns>
    public static TryMethodResult<TSelf> TryParse<TSelf>(this string input, IFormatProvider? formatProvider = null)
        where TSelf : IParsable<TSelf> => TryMethodResult<TSelf>.TryParseResult(TSelf.TryParse(input, formatProvider, out var result), result);
}