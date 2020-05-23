#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Bcl.Hierarchy
{
	public class Node<T>
	{
		public Node(T parent, T current)
		{
			this.Current = current;
			this.Parent = parent;
		}

		public Node(T current)
		{
			this.Current = current;
		}

		public T Current { get; set; }
		public T Parent { get; set; }

		public override string ToString()
		{
			return this.Current.ToString();
		}
	}
}