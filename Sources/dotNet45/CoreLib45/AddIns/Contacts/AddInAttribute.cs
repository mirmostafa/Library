#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.Primitives;

namespace Mohammad.AddIns.Contacts
{
    //public class AddIn<TBaseClass>
    //    where TBaseClass : class
    //{
    //    public string FilePath { get; set; }
    //    public TBaseClass Instance { get; set; }

    //    public override string ToString() { return this.Instance?.ToString() ?? typeof(TBaseClass).FullName; }
    //}

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AddInAttribute : Attribute
    {
    }

    public interface IAddInAppContext
    {
        void Initialize(IApplicationInjector injector);
    }

    public interface IAddInAction
    {
        string Id { get; }
    }
}