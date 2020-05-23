using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;

namespace Mohammad.AddIns
{
    public static class Composer
    {
        public static IEnumerable<TContract> Compose<TContract, TContractAttribute>(Assembly asm, Func<Type, TContract> instanceCreator = null,
            ExceptionHandling exceptionHandling = null) where TContractAttribute : Attribute where TContract : class
        {
            foreach (var type in asm.GetTypes())
            {
                TContract plugin = null;
                try
                {
                    if (!(type.GetCustomAttribute(typeof(TContractAttribute)) is TContractAttribute))
                        continue;
                    if (instanceCreator != null)
                    {
                        plugin = instanceCreator(type);
                    }
                    else
                    {
                        var constructor = type.GetConstructor(new Type[] {});
                        if (constructor == null)
                            continue;
                        plugin = constructor.Invoke(null) as TContract;
                        if (plugin == null)
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    if (exceptionHandling != null)
                        exceptionHandling.HandleException(ex);
                    else
                        throw;
                }
                if (plugin != null)
                    yield return plugin;
            }
        }

        public static IEnumerable<TContract> Compose<TContract, TContractAttribute>(string dll, Func<Type, TContract> instanceCreator = null,
            ExceptionHandling exceptionHandling = null) where TContractAttribute : Attribute where TContract : class
        {
            Assembly asm = null;
            try
            {
                asm = Assembly.LoadFrom(dll);
            }
            catch (Exception ex)
            {
                if (exceptionHandling != null)
                    exceptionHandling.HandleException(ex);
                else
                    throw;
            }
            return asm != null ? Compose<TContract, TContractAttribute>(asm, instanceCreator, exceptionHandling) : null;
        }

        public static IEnumerable<TContract> Compose<TContract, TContractAttribute>(IEnumerable<string> dlls, Func<Type, TContract> instanceCreator = null,
            ExceptionHandling exceptionHandling = null) where TContractAttribute : Attribute where TContract : class
            => dlls.SelectMany(dll => Compose<TContract, TContractAttribute>(dll, instanceCreator, exceptionHandling));

        public static bool HasAddIn<TContract, TContractAttribute>(string file) where TContractAttribute : Attribute where TContract : class
        {
            try
            {
                return
                    Assembly.LoadFrom(file)
                        .GetTypes()
                        .Select(type => new {type, attribute = type.GetCustomAttribute<TContractAttribute>()})
                        .Where(t => t.attribute != null)
                        .Select(t => t.type)
                        .Any(type => type.IsSubclassOf(typeof(TContract)) || type.GetInterface(typeof(TContract).Name) != null);
            }
            catch
            {
                return false;
            }
        }
    }
}