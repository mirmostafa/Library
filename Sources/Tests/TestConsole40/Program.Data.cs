#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;

namespace TestConsole40
{
	internal partial class Program
	{
		private static IEnumerable<dynamic> Prepare()
		{
			yield return new
			             {
				             Age = 20,
				             Name = "Ali"
			             };
			yield return new
			             {
				             Age = 32,
				             Name = "Mohammad"
			             };
			yield return new
			             {
				             Age = 29,
				             Name = "Maryam"
			             };
		}

		private static IEnumerable<Person> GetPeople()
		{
			return Prepare().Select(a => new Person
			                             {
				                             Age = a.Age,
				                             Name = a.Name
			                             });
		}
	}

	internal class Person : IHuman
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public override string ToString()
		{
			return string.Format("{0} is {1} years old.", this.Name, this.Age);
		}
	}

	public interface IHuman
	{
		string Name { get; set; }
		int Age { get; set; }
	}
}