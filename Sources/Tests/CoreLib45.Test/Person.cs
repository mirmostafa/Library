#region Code Identifications

// Created on     2018/03/10
// Last update on 2018/03/10 by Mohammad Mir mostafa 

#endregion

using System;

namespace CoreLib45.Test
{
    public class Person : MarshalByRefObject
    {
        public int Age { get; set; }
        public string Name { get; }
        public Person(string name) => this.Name = name ?? throw new NullReferenceException(nameof(name));
        public string Address { get; set; }
        /// <inheritdoc />
        public override int GetHashCode() => this.Name?.GetHashCode() ?? 0;
    }

    public class Student : Person
    {
        public string Major { get; set; }

        /// <inheritdoc />
        public Student(string name)
            : base(name) { }
    }

}