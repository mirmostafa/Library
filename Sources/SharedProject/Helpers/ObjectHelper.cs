#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Mohammad.DesignPatterns.Creational;
using Mohammad.DesignPatterns.Creational.Exceptions;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Helpers
{
    /// <summary>
    /// </summary>
    public static class ObjectHelper
    {
        public static T? As<T>(this object obj)
            where T : class => obj as T;

        public static T CheckDbNull<T>(in object o, in T defaultValue, Func<object, T> converter) => IsDbNull(o) ? defaultValue : converter(o);

        /// <summary>
        ///     Creates a new instance of TType.
        /// </summary>
        /// <typeparam name="T">The type of the type.</typeparam>
        /// <returns></returns>
        public static T? CreateInstance<T>()
            where T : class
        {
            var ctor = typeof(T).GetConstructor(EmptyArray<Type>());
            return ctor != null ? (T)ctor.Invoke(null) : default;
        }

#if NETCORE
        public static T[] EmptyArray<T>() => Array.Empty<T>();
#else
        public static T[] EmptyArray<T>() => new T[0];
#endif




        /// <summary>
        ///     Creates a new instance in object o.
        /// </summary>
        /// <typeparam name="T">The type of the type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T? CreateInstance<T>(in Type type)
            where T : class => (T?)type.GetConstructor(EmptyArray<Type>())?.Invoke(null);

        /// <summary>
        ///     Creates an new instance of TType.
        /// </summary>
        /// <typeparam name="T">The type of the type.</typeparam>
        /// <param name="types">The types.</param>
        /// <param name="args">The constructor's arguments.</param>
        /// <returns></returns>
        public static T? CreateInstance<T>(in Type[] types, in object[] args)
            where T : class
        {
            var constructorInfo = typeof(T).GetConstructor(types);
            return constructorInfo != null ? (T)constructorInfo.Invoke(args) : null;
        }

        public static void Dispose<TDisposable>(this TDisposable disposable, Action<TDisposable>? action = null)
            where TDisposable : IDisposable
        {
            try
            {
                action?.Invoke(disposable);
            }
            finally
            {
                disposable?.Dispose();
            }
        }

        public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, Func<TDisposable, TResult> action)
            where TDisposable : IDisposable
        {
            try
            {
                return action(disposable);
            }
            finally
            {
                disposable?.Dispose();
            }
        }

        public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in TResult result)
            where TDisposable : IDisposable
        {
            try
            {
                return result;
            }
            finally
            {
                disposable?.Dispose();
            }
        }

        public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, Func<TResult> action)
            where TDisposable : IDisposable
        {
            try
            {
                return action();
            }
            finally
            {
                disposable?.Dispose();
            }
        }

        public static Lazy<TSingleton> GenerateLazySingletonInstance<TSingleton>(Func<TSingleton>? createInstance = null, Action<TSingleton>? initializeInstance = null)
            where TSingleton : class, ISingleton<TSingleton>
            => new Lazy<TSingleton>(() => GenerateSingletonInstance<TSingleton>());

        /// <summary>
        ///     Generates a singleton instance of a class (Must be cached by the owner class).
        /// </summary>
        /// <typeparam name="TSingleton">The type of class which is being instantiated.</typeparam>
        /// <param name="createInstance">A delegate to be used for getting an instance of </param>
        /// <param name="initializeInstance">The instance initializer.</param>
        /// <returns></returns>
        /// <exception cref="SingletonException">
        ///     The class must have a static method: "TSingleton CreateInstance()" or a private/protected constructor.
        /// </exception>
        /// <remarks>
        ///     Before generating instance, GenerateSingletonInstance searches for a static method: "TSingleton CreateInstance()".
        ///     If found, GenerateSingletonInstance calls it and retrieves an instance. Otherwise, a private/protected
        ///     parameter-less constructor is required to construct an instance of TSingleton./>
        ///     After generating instance, searches for a method named: "InitializeComponents". If found will be called.
        /// </remarks>
        public static TSingleton? GenerateSingletonInstance<TSingleton>(Func<TSingleton>? createInstance = null, Action<TSingleton>? initializeInstance = null)
            where TSingleton : class, ISingleton<TSingleton>
        {
            //! If (T) has implemented CreateInstance as a static method, use it to create an instance
            var ci = createInstance ?? GetMethod<Func<TSingleton>>(typeof(TSingleton), "CreateInstance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var result = ci?.Invoke();

            //! if not, try to find a non-public constructor instead. A non-public constructor is mandatory.
            if (result == null)
            {
                var constructor = typeof(TSingleton).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    EmptyArray<Type>(),
                    EmptyArray<ParameterModifier>());
                if (constructor == null)
                {
                    throw new SingletonException(
                        $"The class must have a static method: \"{typeof(TSingleton)} CreateInstance()\" or a private/protected parameterless constructor.");
                }

                result = constructor.Invoke(EmptyArray<object>()) as TSingleton;

                //! Just to make sure that the code will work.
                if (result == null)
                {
                    return null;
                }
            }

            //! If (T) has implemented CreateInstance as an instantiate method, use it to initialize the instance.
            var initialize = GetMethod<Action>(result, "InitializeComponents");
            initialize.Invoke();

            //! if(T) has initialized b"InitializeInstance" delegate, call it to initialize the instance.
            initializeInstance?.Invoke(result);
            return result;
        }

        public static TAttribute? GetAttribute<TAttribute>(in object value, in TAttribute? defaultValue = null, bool inherited = true)
            where TAttribute : Attribute
        {
            var attributes = value.GetType().GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
        }

        public static TAttribute? GetAttribute<TAttribute>(in PropertyInfo property) where TAttribute : Attribute
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault().As<TAttribute>();
        }

        public static TAttribute? GetAttribute<TAttribute>(in Type type, in TAttribute? defaultValue = null, bool inherited = true)
            where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
        }

        public static TAttribute? GetAttribute<TType, TAttribute>(in TAttribute? defaultValue, bool inherited)
            where TAttribute : Attribute
        {
            var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
        }

        public static TAttribute? GetAttribute<TType, TAttribute>()
            where TAttribute : Attribute
        {
            var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
        }

        public static TAttribute? GetAttribute<TAttribute>(in object value, bool inherited) where TAttribute : Attribute => GetAttribute<TAttribute>(value, null, inherited);

        public static string? GetDescription(in object classInstance) => GetAttribute<DescriptionAttribute>(classInstance)?.Description;

        public static TFieldType GetField<TFieldType>(in object obj, string fieldName)
        {
            var field = obj.GetType().GetFields().FirstOrDefault(fld => string.Compare(fld.Name, fieldName, StringComparison.Ordinal) == 0);
            return field != null ? (TFieldType)field.GetValue(obj) : default;
        }

        public static TDelegate? GetMethod<TDelegate>(in object obj, in string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
            where TDelegate : class
        {
            var methodInfo = obj.GetType().GetMethod(name, bindingFlags);
            return methodInfo != null ? As<TDelegate>(Delegate.CreateDelegate(typeof(TDelegate), obj, methodInfo)) : null;
        }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="objType">Type of the object.</param>
        /// <param name="name">The name.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns></returns>
        public static TDelegate? GetMethod<TDelegate>(in Type objType, in string name, in BindingFlags bindingFlags = BindingFlags.Public)
            where TDelegate : class
        {
            var methodInfo = objType.GetMethod(name, bindingFlags);
            return methodInfo != null ? As<TDelegate>(Delegate.CreateDelegate(typeof(TDelegate), null, methodInfo)) : null;
        }

        public static TPropertyType GetProp<TPropertyType>(in object obj, string propName, in int eventNoDefault)
        {
            if (eventNoDefault != 0)
            {
                return GetProp<TPropertyType>(obj, propName);
            }

            var property = obj.GetType().GetProperties().FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
            return property != null ? (TPropertyType)property.GetValue(obj, null) : default;
        }

        public static TPropertyType GetProp<TPropertyType>(in object obj, string propName, bool searchPrivates = false)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();
            if (!properties.Any())
            {
                properties = type.GetProperties(searchPrivates
                    ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                    : BindingFlags.Default);
            }

            var property = properties.FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
            return property != null ? (TPropertyType)property.GetValue(obj, null) : default;
        }

        public static object GetProp(in object obj, in string propName, bool searchPrivates = false) => GetProp<object>(obj, propName, searchPrivates);

        public static IEnumerable<string> GetPropertiesName<TType>()
        {
            var properties = typeof(TType).IsGenericType ? typeof(TType).GetGenericArguments()[0].GetProperties() : typeof(TType).GetProperties();
            return properties.Select(property => property.Name);
        }

        public static TStruct IfDefault<TStruct>(this TStruct t, in TStruct value, in TStruct defaultValue = default)
            where TStruct : struct => Equals(t, defaultValue) ? value : t;

        public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> ifTrue) => obj.If(o => o == null, _ => default, ifTrue);

        public static TResult Iif<TResult>(this bool booleanExpr, in TResult trueResult, in TResult falseResult) => booleanExpr ? trueResult : falseResult;

        public static bool Implements(in object obj, in Type type) => obj.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(property => property.PropertyType.FindInterfaces((m, filterCriteria) => m.FullName == "System.IDisposable", null).Any());

        public static int? IndexOf<TSource>(in TSource item, params TSource[] range)
        {
            var result = Array.IndexOf(range, item);
            return result == -1 ? default(int?) : result;
        }

        public static bool IsDbNull(in object o) => o == null || DBNull.Value.Equals(o);

        public static bool IsDefault<T>(in T value) => value?.Equals(default(T)) ?? true;

        public static bool IsIn<TSource>(in TSource item, params TSource[] range) => range.Contains(item);

        public static bool IsNull(in object value) => value == null;

        public static bool IsNullOrEmptyString(in object value) => string.IsNullOrEmpty(ToString(value));

        public static T NotNull<T>(this T a, T defaultValue) => a.NotNull(o => o, () => defaultValue);

        public static TResult? NotNull<T, TResult>(this T a, TResult defaultValue)
            where TResult : class
            => a.NotNull(o => typeof(TResult) == typeof(string) ? o?.ToString().To<TResult>() : o?.To<TResult>(), () => defaultValue);

        public static TResult NotNull<T, TResult>(this T a, Func<T, TResult> whenNotNull, Func<TResult> whenNull = null)
        {
            if (Equals(a, default(T)))
            {
                return whenNull == null ? default : whenNull();
            }

            if (a is string s && string.IsNullOrEmpty(s))
            {
                return whenNull == null ? default : whenNull();
            }

            return whenNotNull(a);
        }

        public static IReadOnlyList<(string Name, object Value)> GetProperties(object obj)
        {
            var result = new List<(string, object)>();
            foreach (var property in obj.GetType().GetProperties())
            {
                _ = Catch(() => result.Add((property.Name, property.GetValue(obj, null))));
            }

            return result.AsReadOnly();
        }

        public static void SetField(in object obj, in string fieldName, in object value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.GetType().GetField(fieldName)?.SetValue(obj, value);
        }

        public static void SetProperty(in object obj, in string propertyName, in object value)
        {
            var property = obj?.GetType().GetProperty(propertyName);
            if (property != null)
            {
                property.SetValue(obj, value, null);
            }
        }

        public static T CopyUsingStream<T>(in T item)
        {
            using var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, item);
            return (T)bf.Deserialize(ms);
        }

        public static T To<T>(this object obj) => (T)obj;

        public static T ToNotNull<T>(this T? t, in T defaultValue = default) where T : struct => t ?? defaultValue;

        public static string ToString(in object value, in string defaultValue = "") => (value ?? defaultValue).ToString();

        public static TDestination MapOnly<TSource, TDestination>(this TSource source, TDestination destination, Func<TDestination, object> onlyProps)
            where TDestination : class
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source is null)
            {
                return default;
            }

            var justProps = onlyProps(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
            var dstProps = typeof(TDestination).GetProperties();
            var result = destination;
            foreach (var prop in dstProps)
            {
                if (justProps.Contains(prop.Name))
                {
                    Copy(source, result, prop);
                }
            }

            return result;
        }

        public static TDestination Map<TDestination>(this object source)
            where TDestination : class, new()
        {
            if (source is null)
            {
                return null;
            }

            var result = new TDestination();
            var dstProps = typeof(TDestination).GetProperties();
            foreach (var prop in dstProps)
            {
                Copy(source, result, prop);
            }

            return result;
        }

        public static IEnumerable<TDestination> Map<TDestination>(this IEnumerable sources)
            where TDestination : class, new()
        {
            var dstProps = typeof(TDestination).GetProperties();
            foreach (var source in sources)
            {
                if (source is null)
                {
                    continue;
                }

                var result = new TDestination();
                foreach (var prop in dstProps)
                {
                    Copy(source, result, prop);
                }

                yield return result;
            }
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> sources, Action<TSource, TDestination> finalize)
            where TDestination : class, new()
        {
            var dstProps = typeof(TDestination).GetProperties();
            foreach (var source in sources)
            {
                if (source is { })
                {
                    var result = new TDestination();
                    foreach (var prop in dstProps)
                    {
                        Copy(source, result, prop);
                        finalize(source, result);
                    }

                    yield return result;
                }
            }
        }

        private static void Copy<TSource, TDestination>(TSource source, TDestination destination, PropertyInfo dstProp)
            where TDestination : class
        {
            if (source!.GetType().GetProperty(dstProp.Name) is { } srcProp)
            {
                _ = Catch(() => dstProp.SetValue(destination, srcProp.GetValue(source)));
            }
        }
    }
}