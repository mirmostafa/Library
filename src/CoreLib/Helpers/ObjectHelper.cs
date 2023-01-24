using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Creational;
using Library.DesignPatterns.Creational.Exceptions;
using Library.Exceptions;
using Library.Interfaces;
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
    /// <param name="o">           The o.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="converter">   The converter.</param>
    /// <returns></returns>
    public static T CheckDbNull<T>(in object o, in T defaultValue, in Func<object, T> converter)
        => IsDbNull(o) ? defaultValue : converter.Invoke(o);

    /// <summary>
    /// Composes the specified objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static T ComposeByType<T>(this IEnumerable<T> values)
            where T : IMergable<T>, IEmpty<T>
    {
        var result = T.Empty;
        foreach (var value in values)
        {
            result = value.Merge(result);
        }
        return result;
    }

    /// <summary>
    /// Generates the lazy singleton instance.
    /// </summary>
    /// <typeparam name="TSingleton">The type of the singleton.</typeparam>
    /// <param name="createInstance">    The create instance.</param>
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
    /// <param name="createInstance">    A delegate to be used for getting an instance of</param>
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
    public static TSingleton? GenerateSingletonInstance<TSingleton>(Func<TSingleton>? createInstance = null, Action<TSingleton>? initializeInstance = null)
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
                EnumerableHelper.EmptyArray<ParameterModifier>());
            if (constructor is null)
            {
                throw new SingletonException(
                    $"The class must have a static method: \"{typeof(TSingleton)} CreateInstance()\" or a private/protected parameter-less constructor.");
            }

            result = constructor.Invoke(EnumerableHelper.EmptyArray<object>()) as TSingleton;

            //! Just to make sure that the code will work.
            if (result is null)
            {
                return null;
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
    /// <param name="value">       The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="inherited">   if set to <c>true</c> [inherited].</param>
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
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="property">The property.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">property</exception>
    public static TAttribute? GetAttribute<TAttribute>(in PropertyInfo property)
        where TAttribute : Attribute =>
        property is null
                ? throw new ArgumentNullException(nameof(property))
                : property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault().As<TAttribute>();

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="type">        The type.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="inherited">   if set to <c>true</c> [inherited].</param>
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
    /// <param name="inherited">   if set to <c>true</c> [inherited].</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TType, TAttribute>(in TAttribute? defaultValue, in bool inherited)
        where TAttribute : Attribute
    {
        var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), inherited);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TType">The type of the type.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TType, TAttribute>()
        where TAttribute : Attribute
    {
        var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), false);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="value">    The value.</param>
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
    /// <param name="obj">      The object.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static TFieldType GetField<TFieldType>(in object? obj, string fieldName)
    {
        var field = obj?.GetType().GetFields().FirstOrDefault(fld => string.Compare(fld.Name, fieldName, StringComparison.Ordinal) == 0);
        return field switch
        {
            not null => field.GetValue(obj)!.To<TFieldType>(),
            null => throw new ObjectNotFoundException("Field not found")
        };
    }

    public static int GetHashCode(object o, params object[] properties) =>
            properties.Aggregate(o.GetHashCode(), (hash, property) => hash ^ property.GetHashCode());

    /// <summary>
    /// Gets the method.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="obj">         The object.</param>
    /// <param name="name">        The name.</param>
    /// <param name="bindingFlags">The binding flags.</param>
    /// <returns></returns>
    public static TDelegate? GetMethod<TDelegate>([DisallowNull] in object obj,
        [DisallowNull] in string name,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        where TDelegate : class
    {
        var methodInfo = obj.ArgumentNotNull(nameof(obj)).GetType().GetMethod(name, bindingFlags);
        return methodInfo is not null
            ? Cast.As<TDelegate>(Delegate.CreateDelegate(typeof(TDelegate), obj, methodInfo))
            : null;
    }

    /// <summary>
    /// Gets the method.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="objType">     Type of the object.</param>
    /// <param name="name">        The name.</param>
    /// <param name="bindingFlags">The binding flags.</param>
    /// <returns></returns>
    public static TDelegate? GetMethod<TDelegate>(in Type objType,
        in string name,
        in BindingFlags bindingFlags = BindingFlags.Public)
        where TDelegate : class
    {
        var methodInfo = objType.GetMethod(name, bindingFlags);
        return methodInfo is not null
            ? Cast.As<TDelegate>(Delegate.CreateDelegate(typeof(TDelegate), null, methodInfo))
            : null;
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    /// <param name="obj">           The object.</param>
    /// <param name="propName">      Name of the property.</param>
    /// <param name="eventNoDefault">The event no default.</param>
    /// <returns></returns>
    public static TPropertyType? GetProp<TPropertyType>([DisallowNull] in object obj, string propName, in int eventNoDefault)
    {
        _ = obj.ArgumentNotNull(nameof(obj));

        if (eventNoDefault != 0)
        {
            return GetProp<TPropertyType>(obj, propName);
        }

        var property = obj.GetType()
            .GetProperties()
            .FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
        return property is not null ? (TPropertyType?)property.GetValue(obj, null) : default;
    }

    public static PropertyInfo? GetProp<TDeclaringType>(string propName)
        => typeof(TDeclaringType).GetProperty(propName);

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    /// <param name="obj">           The object.</param>
    /// <param name="propName">      Name of the property.</param>
    /// <param name="searchPrivates">if set to <c>true</c> [search privates].</param>
    /// <returns></returns>
    public static TPropertyType? GetProp<TPropertyType>([DisallowNull] in object obj, [DisallowNull] string propName, bool searchPrivates = false)
    {
        var type = obj.ArgumentNotNull(nameof(obj)).GetType();
        var properties = type.GetProperties();
        if (!properties.Any())
        {
            properties = type.GetProperties(searchPrivates
                ? BindingFlags.Instance | BindingFlags.Static |
                  BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Default);
        }

        var property = properties.FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
        return property is not null ? (TPropertyType?)property.GetValue(obj, null) : default;
    }

    public static object? GetProp([DisallowNull] in object obj, in string propName, bool searchPrivates = false)
        => GetProp<object>(obj, propName, searchPrivates);

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

    public static bool HasAttribute<TType, TAttribute>() where TAttribute : Attribute
            => HasAttribute<TAttribute>(typeof(Type));

    public static bool HasAttribute<TAttribute>(Type type)
        where TAttribute : Attribute
    {
        var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
        return attributes.Length > 0;
    }

    public static bool Implements([DisallowNull] in object obj, Type type)
        => obj.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(property => property.PropertyType.FindInterfaces((m, filterCriteria) => m.FullName == type.FullName, null).Any());

    /// <summary>
    /// Get the index of range.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="item"> The item.</param>
    /// <param name="range">The range.</param>
    /// <returns></returns>
    public static int? IndexOf<TSource>(in TSource item, params TSource[] range)
    {
        var result = Array.IndexOf(range, item);
        return result == -1 ? default(int?) : result;
    }

    /// <summary>
    /// Determines whether [is database null] [the specified object].
    /// </summary>
    /// <param name="o">The o.</param>
    /// <returns><c>true</c> if [is database null] [the specified o]; otherwise, <c>false</c>.</returns>
    public static bool IsDbNull(in object? o)
        => o switch
        {
            null => true,
            DBNull => true,
            _ => false
        };

    /// <summary>
    /// Determines whether the specified value is default (null or zero or ...).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the specified value is default; otherwise, <c>false</c>.</returns>
    public static bool IsDefault<T>(in T value)
        => value?.Equals(default(T)) ?? true;

    /// <summary>
    /// Determines whether the specified item is in range.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="item"> The item.</param>
    /// <param name="range">The range.</param>
    /// <returns><c>true</c> if the specified item is in; otherwise, <c>false</c>.</returns>
    public static bool IsIn<TSource>(in TSource item, params TSource[] range)
        => range.Contains(item);

    /// <summary>
    /// Determines whether the specified value is null.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the specified value is null; otherwise, <c>false</c>.</returns>
    public static bool IsNull([NotNullWhen(false)] in object? value)
        => value is null;

    public static bool IsNull<TStruct>(this TStruct @struct) where TStruct : struct
        => @struct.Equals(default(TStruct));

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid? guid)
        => guid is null || guid == Guid.Empty;

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid guid)
        => guid == Guid.Empty;

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Id id)
        => id == Guid.Empty;

    /// <summary>
    /// Determines whether [is null or empty string] [the specified value].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if [is null or empty string] [the specified value]; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmptyString([NotNullWhen(false)] in object value)
        => string.IsNullOrEmpty(Cast.ToString(value));

    public static dynamic props(this object o)
        => _propsExpando.GetOrCreateValue(o);

    public static IEnumerable<T> Repeat<T>(T value, int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return value;
        }
    }

    public static void SetField([DisallowNull] in object obj, in string fieldName, in object value)
    {
        Check.IfArgumentNotNull(obj);
        obj.GetType().GetField(fieldName)?.SetValue(obj, value);
    }

    public static void SetProperty([DisallowNull] in object obj, in string propertyName, in object value)
    {
        var property = obj?.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value, null);
    }
}