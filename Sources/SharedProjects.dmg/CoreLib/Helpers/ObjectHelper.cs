using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary>
        ///     Copies the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="ignoreExceptions"></param>
        public static void ReflectionCopy<TSource, TDestination>(TSource source, TDestination destination, bool ignoreExceptions)
        {
            var props = from sourceProperty in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        join destProperty in typeof(TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public) on
                        sourceProperty.Name equals destProperty.Name
                        where sourceProperty.CanRead && destProperty.CanWrite
                        select new {Value = sourceProperty.GetValue(source, null), Destination = destProperty};

            Action<dynamic> action;
            if (ignoreExceptions)
                action = p => { Catch(() => p.Destination.SetValue(destination, p.Value, null)); };
            else
                action = p => p.Destination.SetValue(destination, p.Value, null);
            foreach (var p in props)
                action(p);
            //props.ForEach(p => p.Destination.SetValue(destination, p.Value, null));
        }

        /// <summary>
        ///     Copies the specified source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="ignoreExceptions">
        ///     if set to &lt;c&gt;true&lt;/c&gt; any exception will be ignored while copying.
        /// </param>
        /// <summary>
        ///     Copies the specified source.
        /// </summary>
        /// <typeparam name="T">The type of the destination.</typeparam>
        /// <returns></returns>
        public static T ReflectionCopy<T>(this T source, bool ignoreExceptions)
            where T : new()
        {
            var result = new T();
            ReflectionCopy(source, result, ignoreExceptions);
            return result;
        }

        public static T StreamCopy<T>(T item)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, item);
                return (T) bf.Deserialize(ms);
            }
        }

        /// <summary>
        ///     Creates a new instance of TType.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns></returns>
        public static TType CreateInstance<TType>()
        {
            var ctor = typeof(TType).GetConstructor(new Type[] { });
            return ctor != null ? (TType) ctor.Invoke(null) : default(TType);
        }

        /// <summary>
        ///     Creates a new instance of TType.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TType CreateInstance<TType>(Type type) => (TType) type.GetConstructor(new Type[] { })?.Invoke(null);

        /// <summary>
        ///     Creates an new instance of TType.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="types">The types.</param>
        /// <param name="args">The constructor's arguments.</param>
        /// <returns></returns>
        public static TType CreateInstance<TType>(Type[] types, object[] args)
        {
            var constructorInfo = typeof(TType).GetConstructor(types);
            return constructorInfo != null ? (TType) constructorInfo.Invoke(args) : default(TType);
        }

        public static TAttribute GetAttribute<TAttribute>(object value, TAttribute defaultValue = null, bool inherited = true)
            where TAttribute : Attribute
        {
            var attributes = value.GetType().GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute) attributes[0] : defaultValue;
        }

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo property)
            where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault().As<TAttribute>();

        public static TAttribute GetAttribute<TAttribute>(Type type, TAttribute defaultValue = null, bool inherited = true)
            where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute) attributes[0] : defaultValue;
        }

        public static TAttribute GetAttribute<TType, TAttribute>(TAttribute defaultValue, bool inherited)
            where TAttribute : Attribute
        {
            var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), inherited);
            return attributes.Length > 0 ? (TAttribute) attributes[0] : defaultValue;
        }

        public static TAttribute GetAttribute<TType, TAttribute>()
            where TAttribute : Attribute
        {
            var attributes = typeof(TType).GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Length > 0 ? (TAttribute) attributes[0] : default(TAttribute);
        }

        public static TAttribute GetAttribute<TAttribute>(object value, bool inherited)
            where TAttribute : Attribute => GetAttribute<TAttribute>(value, null, inherited);

        public static string GetDescription(object classInstance) => GetAttribute<DescriptionAttribute>(classInstance)?.Description;

        public static TFieldType GetField<TFieldType>(object obj, string fieldName)
        {
            var field = obj.GetType().GetFields().FirstOrDefault(fld => string.Compare(fld.Name, fieldName, StringComparison.Ordinal) == 0);
            return field != null ? (TFieldType) field.GetValue(obj) : default(TFieldType);
        }

        public static IEnumerable<string> GetPropertiesName<TType>()
        {
            var properties = typeof(TType).IsGenericType
                ? typeof(TType).GetGenericArguments()[0].GetProperties()
                : typeof(TType).GetProperties();
            return properties.Select(property => property.Name);
        }

        public static TPropertyType GetProp<TPropertyType>(object obj, string propName, int eventNoDefault)
        {
            if (eventNoDefault != 0)
                return GetProp<TPropertyType>(obj, propName);
            var property = obj.GetType().GetProperties()
                .FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);
            return property != null ? (TPropertyType) property.GetValue(obj, null) : default(TPropertyType);
        }

        public static TPropertyType GetProp<TPropertyType>(object obj, string propName, bool serachPrivates = false)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();
            if (!properties.Any())
                properties = type.GetProperties(serachPrivates
                    ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                    : BindingFlags.Default);
            var property = properties.FirstOrDefault(prop => string.Compare(prop.Name, propName, StringComparison.Ordinal) == 0);

            return property != null ? (TPropertyType) property.GetValue(obj, null) : default(TPropertyType);
        }

        public static object GetProp(object obj, string propName, bool serachPrivates = false) => GetProp<object>(obj,
            propName,
            serachPrivates);

        public static TResult Iif<TResult>(this bool booleanExpr, TResult trueResult, TResult falseResult) => booleanExpr
            ? trueResult
            : falseResult;

        public static int? IndexOf<TSource>(TSource item, params TSource[] range)
        {
            var result = Array.IndexOf(range, item);
            return result == -1 ? default(int?) : result;
        }

        public static bool IsIn<TSource>(TSource item, params TSource[] range) => range.Contains(item);
        public static T NotNull<T>(this T a, T defaultValue) => a.NotNull(o => o, () => defaultValue);

        public static TResult NotNull<T, TResult>(this T a, TResult defaultValue) => a.NotNull(
            o => typeof(TResult) == typeof(string) ? o.ToString().To<TResult>() : o.To<TResult>(),
            () => defaultValue);

        public static TResult NotNull<T, TResult>(this T a, Func<T, TResult> whenNotNull, Func<TResult> whenNull = null)
        {
            if (Equals(a, default(T)))
                return whenNull == null ? default(TResult) : whenNull();
            if (a is string && string.IsNullOrEmpty(a as string))
                return whenNull == null ? default(TResult) : whenNull();
            return whenNotNull(a);
        }

        public static TResult NotNull<T, TResult>(this T a, Func<TResult> whenNull = null)
        {
            return a.NotNull(o => o.To<TResult>(), whenNull);
        }

        public static Dictionary<string, object> ReflectFields(object obj, bool privateFields = false)
        {
            var result = new Dictionary<string, object>();
            foreach (var field in obj.GetType().GetFields(privateFields
                ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                Catch(() => result.Add(field.Name, field.GetValue(obj)));
            return result;
        }

        public static Dictionary<string, object> ReflectProperties(object obj, bool privateProperties = false)
        {
            var result = new Dictionary<string, object>();
            foreach (var property in obj.GetType().GetProperties(privateProperties
                ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                Catch(() => result.Add(property.Name, property.GetValue(obj, null)));
            return result;
        }

        public static bool Implements(object obj, Type type) => obj.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(property => property.PropertyType
                .FindInterfaces((m, filterCriteria) => m.FullName == "System.IDisposable", null).Any());

        public static Collection<string> ReflectProperties(Type type, bool privateProperties = false)
        {
            var result = new Collection<string>();
            foreach (var property in type.GetProperties(privateProperties
                ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                Catch(() => result.Add(property.Name));
            return result;
        }

        public static void SetField(object obj, string fieldName, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(obj, value);
        }

        public static void SetProperty(object obj, string propertyName, object value)
        {
            var property = obj?.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
                property.SetValue(obj, value, null);
        }

        public static IEnumerable<dynamic> AsEnumerable(dynamic dynObj)
        {
            var cursor = dynObj.GetEnumerator();
            while (cursor.MoveNext())
                yield return cursor.Current;
        }

        public static TType As<TType>(this object obj)
            where TType : class => obj as TType;

        public static TType To<TType>(this object obj) => (TType) obj;

        public static TDelegate GetMethod<TDelegate>(object obj, string name,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
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
        public static TDelegate GetMethod<TDelegate>(Type objType, string name, BindingFlags bindingFlags = BindingFlags.Public)
            where TDelegate : class
        {
            var methodInfo = objType.GetMethod(name, bindingFlags);
            return methodInfo != null ? As<TDelegate>(Delegate.CreateDelegate(typeof(TDelegate), null, methodInfo)) : null;
        }

        public static IEnumerable<T> AsEnumerable<T>(T obj) { yield return obj; }

        public static string ToString(object value, string defaultValue = "") => (value ?? defaultValue).ToString();

        public static void Dispose<TDisposable>(this TDisposable disposable, Action<TDisposable> action = null)
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

        public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, TResult result)
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

        public static Lazy<TSingletone> GenerateLazySingletonInstance<TSingletone>(Func<TSingletone> createInstance = null,
            Action<TSingletone> initializeInstance = null)
            where TSingletone : class, ISingleton<TSingletone> => new Lazy<TSingletone>(() => GenerateSingletonInstance<TSingletone>());

        /// <summary>
        ///     Generates a singleton instance of a class (Must be cached by the owner class).
        /// </summary>
        /// <typeparam name="TSingletone">The type of class which is being instantiated.</typeparam>
        /// <param name="createInstance">A delegate to be used for getting an instance of </param>
        /// <param name="initializeInstance">The instance initializer.</param>
        /// <returns></returns>
        /// <exception cref="SingletonException">
        ///     The class must have a static method: "TSingletone CreateInstance()" or a private/protected constructor.
        /// </exception>
        /// <remarks>
        ///     Before generating instance, GenerateSingletonInstance searches for a static method: "TSingletone CreateInstance()".
        ///     If found, GenerateSingletonInstance calls it and retrieves an instance. Otherwise, a private/protected
        ///     parameterless constructor is required to construct an instance of TSingleton./>
        ///     After generating instance, searches for a method named: "InitializeComponents". If found will be called.
        /// </remarks>
        public static TSingletone GenerateSingletonInstance<TSingletone>(Func<TSingletone> createInstance = null,
            Action<TSingletone> initializeInstance = null)
            where TSingletone : class, ISingleton<TSingletone>
        {
            //! If (T) has implemented CreateInstance as a static method, use it to create an instance
            var ci = createInstance ?? GetMethod<Func<TSingletone>>(typeof(TSingletone),
                         "CreateInstance",
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var result = ci?.Invoke();

            //! if not, try to find a non-public constructor instead. A non-public constructor is mandatory.
            if (result == null)
            {
                var constructor = typeof(TSingletone).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[0],
                    new ParameterModifier[0]);
                if (constructor == null)
                    throw new SingletonException(
                        $"The class must have a static method: \"{typeof(TSingletone)} CreateInstance()\" or a private/protected parameterless constructor.");
                result = constructor.Invoke(new object[0]) as TSingletone;

                //! Just to make sure that the code will work.
                if (result == null)
                    return null;
            }

            //! If (T) has implemented CreateInstance as an instantiate method, use it to initialize the instance.
            var initialize = GetMethod<Action>(result, "InitializeComponents");
            initialize?.Invoke();

            //! if(T) has initialized b"InitializeInstance" delegate, call it to initialize the instance.
            initializeInstance?.Invoke(result);
            return result;
        }

        public static bool IsDbNull(object o) => o == null || DBNull.Value.Equals(o);

        public static T CheckDbNull<T>(object o, T defaultValue, Func<object, T> converter) => IsDbNull(o) ? defaultValue : converter(o);

        public static T ToNotNull<T>(this T? t, T defaultValue = default(T))
            where T : struct => t ?? defaultValue;

        public static bool IsNullOrEmptyString(object value) => string.IsNullOrEmpty(ObjectHelper.ToString(value));

        public static bool IsNull(object value) => value == null;

        public static bool IsDefault<T>(T value) => value?.Equals(default(T)) ?? true;

        public static TStruct IfDefault<TStruct>(this TStruct t, TStruct value, TStruct defaultValue = default(TStruct))
            where TStruct : struct => Equals(t, defaultValue) ? value : t;

        public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> ifTrue)
        {
            return obj.If(o => o == null, _ => default(TResult), ifTrue);
        }
    }
}