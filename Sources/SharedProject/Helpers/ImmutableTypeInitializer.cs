#region Code Identifications

// Created on     2018/07/25
// Last update on 2018/07/25 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Mohammad.Helpers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    ///     <code>
    /// // Full Dynamic
    ///     dynamic a1 = new ImmutableTypeInitializer of Person ();
    ///     a1.Name = "Ali";
    ///     a1.Age = 5;
    ///     a1.Age = 15;
    ///     Person p1 = a1;
    /// 
    /// // Object Initializer and Indexer
    ///     var a2 = new ImmutableTypeInitializer of Person
    ///             {
    ///                ["Name"] = "Ali",
    ///                ["Age"] = 15
    ///             };
    ///     Person p2 = a2;
    /// 
    /// // More Object Oriented
    ///     var p3 = new ImmutableTypeInitializer of Person
    ///             {
    ///                ["Name"] = "Ali",
    ///                ["Age"] = 15
    ///             }.Build();
    /// 
    /// // Method Chain
    ///     var p4 = ImmutableTypeInitializer of Person.New().CtorParam("Name", "Ali").CtorParam("Age", 5).Build();
    /// 
    /// // Functional Programming
    ///     Person p5 = ImmutableTypeInitializer of Person.NewDynamic().SetName("Ali").SetAge(5).Build();
    /// </code>
    /// </example>
    public sealed class ImmutableTypeInitializer<T> : DynamicObject
    {
        #region Fields

        private readonly Dictionary<string, object> _CtorParameters = new Dictionary<string, object>();

        #endregion

        public object this[string ctorParamName]
        {
            set
            {
                var binderName = ctorParamName.ToLower();
                if (this._CtorParameters.ContainsKey(binderName))
                {
                    this._CtorParameters[binderName] = value;
                }
                else
                {
                    this._CtorParameters.Add(binderName, value);
                }
            }
        }

        public static implicit operator T(ImmutableTypeInitializer<T> initializer) => initializer.Build();

        public T Build()
        {
            var target = typeof(T).GetConstructors()
                .FirstOrDefault(ctor => ctor.GetParameters()
                    .Any(parameter =>
                        this._CtorParameters.Keys.Contains(parameter.Name.ToLower())));
            if (target == null)
            {
                throw new Exception();
            }

            var paramInfos = target.GetParameters();
            var seqParams = new object[paramInfos.Length];
            for (var index = 0; index < paramInfos.Length; index++)
            {
                seqParams[index] = this._CtorParameters[paramInfos[index].Name];
            }

            return (T)target.Invoke(seqParams);
        }

        public ImmutableTypeInitializer<T> CtorParam(string name, object value)
        {
            this[name] = value;
            return this;
        }

        public static ImmutableTypeInitializer<T> New() => new ImmutableTypeInitializer<T>();
        public static dynamic NewDynamic() => new ImmutableTypeInitializer<T>();

        /// <inheritdoc />
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var ctorParamName = binder.Name.Substring(3);
            result = this.CtorParam(ctorParamName, args[0]);
            return true;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }
    }
}