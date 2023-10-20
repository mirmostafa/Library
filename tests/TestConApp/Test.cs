using System;
using System.Linq;
using System.Threading;

namespace Test.Dtos
{
    public sealed class PersonDto
    {
        private string _lastName;
        public PersonDto(string name, int age)
        {
            this.Name = name;
            this._age = age;
        }

        public string Name
        {
            get;
            set;
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
        }

        public PersonDto SetAge(int age)
        {
            this._age = age;
            return this;
        }
    }
}