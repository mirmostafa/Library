// Created on     2018/03/10
// Last update on 2018/03/10 by Mohammad Mir mostafa 

using System;

namespace CoreLib45.Test
{
    public class Person : MarshalByRefObject
    {
        public Person(string name) => this.Name = name ?? throw new NullReferenceException(nameof(name));
        public int Age { get; set; }
        public string Name { get; }
        public string Address { get; set; }

        /// <inheritdoc />
        public override int GetHashCode() => this.Name?.GetHashCode() ?? 0;
    }

    public class Student : Person
    {
        /// <inheritdoc />
        public Student(string name)
            : base(name)
        {
        }

        public string Major { get; set; }
    }
}