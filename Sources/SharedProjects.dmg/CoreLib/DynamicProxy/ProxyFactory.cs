using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Mohammad.DesignPatterns.Creational;

namespace Mohammad.DynamicProxy
{
    /// <summary>
    /// </summary>
    public class ProxyFactory : Singleton<ProxyFactory>
    {
        private readonly Hashtable _TypeMap = Hashtable.Synchronized(new Hashtable());
        private const string PROXY_SUFFIX = "Proxy";
        private const string ASSEMBLY_NAME = "ProxyAssembly";
        private const string MODULE_NAME = "ProxyModule";
        private const string HANDLER_NAME = "handler";
        private static readonly Hashtable _OpCodeTypeMapper = new Hashtable();
        // Initialize the value type mapper.  This is needed for methods with intrinsic 
        // return types, used in the Emit process.
        static ProxyFactory()
        {
            _OpCodeTypeMapper.Add(typeof(bool), OpCodes.Ldind_I1);
            _OpCodeTypeMapper.Add(typeof(short), OpCodes.Ldind_I2);
            _OpCodeTypeMapper.Add(typeof(int), OpCodes.Ldind_I4);
            _OpCodeTypeMapper.Add(typeof(long), OpCodes.Ldind_I8);
            _OpCodeTypeMapper.Add(typeof(double), OpCodes.Ldind_R8);
            _OpCodeTypeMapper.Add(typeof(float), OpCodes.Ldind_R4);
            _OpCodeTypeMapper.Add(typeof(ushort), OpCodes.Ldind_U2);
            _OpCodeTypeMapper.Add(typeof(uint), OpCodes.Ldind_U4);
        }

        private ProxyFactory() { }

        public object Create(IProxyInvocationHandler handler, Type objType, bool isObjInterface)
        {
            var typeName = objType.FullName + PROXY_SUFFIX;
            var type = (Type) this._TypeMap[typeName];

            // check to see if the type was in the cache.  If the type was not cached, then
            // create a new instance of the dynamic type and add it to the cache.
            if (type == null)
            {
                if (isObjInterface)
                    type = this.CreateType(handler, new[] {objType}, typeName);
                else
                    type = this.CreateType(handler, objType.GetInterfaces(), typeName);

                this._TypeMap.Add(typeName, type);
            }

            // return a new instance of the type.
            return Activator.CreateInstance(type, handler);
        }

        public object Create(IProxyInvocationHandler handler, Type objType) { return this.Create(handler, objType, false); }

        private Type CreateType(IProxyInvocationHandler handler, Type[] interfaces, string dynamicTypeName)
        {
            Type retVal = null;

            if (handler != null && interfaces != null)
            {
                var objType = typeof(object);
                var handlerType = typeof(IProxyInvocationHandler);

                var domain = Thread.GetDomain();
                var assemblyName = new AssemblyName {Name = ASSEMBLY_NAME, Version = new Version(1, 0, 0, 0)};

                // create a new assembly for this proxy, one that isn't presisted on the file system
                var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                // create a new module for this proxy
                var moduleBuilder = assemblyBuilder.DefineDynamicModule(MODULE_NAME);

                // Set the class to be public and sealed
                const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed;

                // Gather up the proxy information and create a new type builder.  One that
                // inherits from Object and implements the interface passed in
                var typeBuilder = moduleBuilder.DefineType(dynamicTypeName, typeAttributes, objType, interfaces);

                // Define a member variable to hold the delegate
                var handlerField = typeBuilder.DefineField(HANDLER_NAME, handlerType, FieldAttributes.Private);

                // build a constructor that takes the delegate object as the only argument
                //ConstructorInfo defaultObjConstructor = objType.GetConstructor( new Type[0] );
                var superConstructor = objType.GetConstructor(new Type[0]);
                var delegateConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] {handlerType});

                #region( "Constructor IL Code" )

                var constructorIl = delegateConstructor.GetILGenerator();

                // Load "this"
                constructorIl.Emit(OpCodes.Ldarg_0);
                // Load first constructor parameter
                constructorIl.Emit(OpCodes.Ldarg_1);
                // Set the first parameter into the handler field
                constructorIl.Emit(OpCodes.Stfld, handlerField);
                // Load "this"
                constructorIl.Emit(OpCodes.Ldarg_0);
                // Call the super constructor
                constructorIl.Emit(OpCodes.Call, superConstructor);
                // Constructor return
                constructorIl.Emit(OpCodes.Ret);

                #endregion

                // for every method that the interfaces define, build a corresponding 
                // method in the dynamic type that calls the handlers invoke method.  
                foreach (var interfaceType in interfaces)
                    this.GenerateMethod(interfaceType, handlerField, typeBuilder);

                retVal = typeBuilder.CreateType();
            }

            return retVal;
        }

        private void GenerateMethod(Type interfaceType, FieldBuilder handlerField, TypeBuilder typeBuilder)
        {
            MetaDataFactory.Add(interfaceType);
            var interfaceMethods = interfaceType.GetMethods();
            if (interfaceMethods != null)
                for (var i = 0; i < interfaceMethods.Length; i++)
                {
                    var methodInfo = interfaceMethods[i];
                    // Get the method parameters since we need to create an array
                    // of parameter types                         
                    var methodParams = methodInfo.GetParameters();
                    var numOfParams = methodParams.Length;
                    var methodParameters = new Type[numOfParams];

                    // convert the ParameterInfo objects into Type
                    for (var j = 0; j < numOfParams; j++)
                        methodParameters[j] = methodParams[j].ParameterType;

                    // create a new builder for the method in the interface
                    var methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        CallingConventions.Standard,
                        methodInfo.ReturnType,
                        methodParameters);

                    #region( "Handler Method IL Code" )

                    var methodIl = methodBuilder.GetILGenerator();

                    // Emit a declaration of a local variable if there is a return
                    // type defined
                    if (!(methodInfo.ReturnType == typeof(void)))
                    {
                        methodIl.DeclareLocal(methodInfo.ReturnType);
                        if (methodInfo.ReturnType.IsValueType && !methodInfo.ReturnType.IsPrimitive)
                            methodIl.DeclareLocal(methodInfo.ReturnType);
                    }

                    // if we have any parameters for the method, then declare an 
                    // Object array local var.
                    if (numOfParams > 0)
                        methodIl.DeclareLocal(typeof(object[]));

                    // declare a label for invoking the handler
                    var handlerLabel = methodIl.DefineLabel();
                    // declare a lable for returning from the mething
                    var returnLabel = methodIl.DefineLabel();

                    // load "this"
                    methodIl.Emit(OpCodes.Ldarg_0);
                    // load the handler instance variable
                    methodIl.Emit(OpCodes.Ldfld, handlerField);
                    // jump to the handlerLabel if the handler instance variable is not null
                    methodIl.Emit(OpCodes.Brtrue_S, handlerLabel);
                    // the handler is null, so return null if the return type of
                    // the method is not void, otherwise return nothing
                    if (!(methodInfo.ReturnType == typeof(void)))
                    {
                        if (methodInfo.ReturnType.IsValueType && !methodInfo.ReturnType.IsPrimitive && !methodInfo.ReturnType.IsEnum)
                            methodIl.Emit(OpCodes.Ldloc_1);
                        else // load null onto the stack
                            methodIl.Emit(OpCodes.Ldnull);
                        // store the null return value
                        methodIl.Emit(OpCodes.Stloc_0);
                        // jump to return
                        methodIl.Emit(OpCodes.Br_S, returnLabel);
                    }

                    // the handler is not null, so continue with execution
                    methodIl.MarkLabel(handlerLabel);

                    // load "this"
                    methodIl.Emit(OpCodes.Ldarg_0);
                    // load the handler
                    methodIl.Emit(OpCodes.Ldfld, handlerField);
                    // load "this" since its needed for the call to invoke
                    methodIl.Emit(OpCodes.Ldarg_0);
                    // load the name of the interface, used to get the MethodInfo object
                    // from MetaDataFactory
                    methodIl.Emit(OpCodes.Ldstr, interfaceType.FullName);
                    // load the index, used to get the MethodInfo object 
                    // from MetaDataFactory 
                    methodIl.Emit(OpCodes.Ldc_I4, i);
                    // invoke GetMethod in MetaDataFactory
                    methodIl.Emit(OpCodes.Call, typeof(MetaDataFactory).GetMethod("GetMethod", new[] {typeof(string), typeof(int)}));

                    // load the number of parameters onto the stack
                    methodIl.Emit(OpCodes.Ldc_I4, numOfParams);
                    // create a new array, using the size that was just pused on the stack
                    methodIl.Emit(OpCodes.Newarr, typeof(object));

                    // if we have any parameters, then iterate through and set the values
                    // of each element to the corresponding arguments
                    if (numOfParams > 0)
                    {
                        methodIl.Emit(OpCodes.Stloc_1);
                        for (var j = 0; j < numOfParams; j++)
                        {
                            methodIl.Emit(OpCodes.Ldloc_1);
                            methodIl.Emit(OpCodes.Ldc_I4, j);
                            methodIl.Emit(OpCodes.Ldarg, j + 1);
                            if (methodParameters[j].IsValueType)
                                methodIl.Emit(OpCodes.Box, methodParameters[j]);
                            methodIl.Emit(OpCodes.Stelem_Ref);
                        }
                        methodIl.Emit(OpCodes.Ldloc_1);
                    }

                    // call the Invoke method
                    methodIl.Emit(OpCodes.Callvirt, typeof(IProxyInvocationHandler).GetMethod("Invoke"));

                    if (!(methodInfo.ReturnType == typeof(void)))
                    {
                        // if the return type if a value type, then unbox the return value
                        // so that we don't get junk.
                        if (methodInfo.ReturnType.IsValueType)
                        {
                            methodIl.Emit(OpCodes.Unbox, methodInfo.ReturnType);
                            if (methodInfo.ReturnType.IsEnum)
                                methodIl.Emit(OpCodes.Ldind_I4);
                            else if (!methodInfo.ReturnType.IsPrimitive)
                                methodIl.Emit(OpCodes.Ldobj, methodInfo.ReturnType);
                            else
                                methodIl.Emit((OpCode) _OpCodeTypeMapper[methodInfo.ReturnType]);
                        }

                        // store the result
                        methodIl.Emit(OpCodes.Stloc_0);
                        // jump to the return statement
                        methodIl.Emit(OpCodes.Br_S, returnLabel);
                        // mark the return statement
                        methodIl.MarkLabel(returnLabel);
                        // load the value stored before we return.  This will either be
                        // null (if the handler was null) or the return value from Invoke
                        methodIl.Emit(OpCodes.Ldloc_0);
                    }
                    else
                    {
                        // pop the return value that Invoke returned from the stack since
                        // the method's return type is void. 
                        methodIl.Emit(OpCodes.Pop);
                        //mark the return statement
                        methodIl.MarkLabel(returnLabel);
                    }

                    // Return
                    methodIl.Emit(OpCodes.Ret);

                    #endregion
                }

            // Iterate through the parent interfaces and recursively call this method
            foreach (var parentType in interfaceType.GetInterfaces())
                this.GenerateMethod(parentType, handlerField, typeBuilder);
        }
    }
}