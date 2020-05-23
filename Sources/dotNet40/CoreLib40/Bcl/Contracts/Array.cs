#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Diagnostics.Contracts;

namespace Library40.Bcl.Contracts
{
	// An IArray is an ordered collection of objects.    
	[ContractClass(typeof (ArrayContract))]
	public interface IArray
	{
		// The Item property provides methods to read and edit entries in the array.
		Object this[int index] { get; set; }

		int Count { get; }

		// Adds an item to the list.  
		// The return value is the position the new element was inserted in.
		int Add(Object value);

		// Removes all items from the list.
		void Clear();

		// Inserts value into the array at position index.
		// index must be non-negative and less than or equal to the 
		// number of elements in the array.  If index equals the number
		// of items in the array, then value is appended to the end.
		void Insert(int index, Object value);

		// Removes the item at position index.
		void RemoveAt(int index);
	}

	[ContractClassFor(typeof (IArray))]
	public abstract class ArrayContract : IArray
	{
		#region IArray Members
		int IArray.Add(Object value)
		{
			// Returns the index in which an item was inserted.
			Contract.Ensures(Contract.Result<int>() >= -1);
			Contract.Ensures(Contract.Result<int>() < ((IArray)this).Count);
			return default(int);
		}

		Object IArray.this[int index]
		{
			get
			{
				Contract.Requires(index >= 0);
				Contract.Requires(index < ((IArray)this).Count);
				return default(int);
			}
			set
			{
				Contract.Requires(index >= 0);
				Contract.Requires(index < ((IArray)this).Count);
			}
		}

		public int Count
		{
			get
			{
				Contract.Requires(this.Count >= 0);
				Contract.Requires(this.Count <= ((IArray)this).Count);
				return default(int);
			}
		}

		void IArray.Clear()
		{
			Contract.Ensures(((IArray)this).Count == 0);
		}

		void IArray.Insert(int index, Object value)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(index <= ((IArray)this).Count); // For inserting immediately after the end.
			Contract.Ensures(((IArray)this).Count == Contract.OldValue(((IArray)this).Count) + 1);
		}

		void IArray.RemoveAt(int index)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(index < ((IArray)this).Count);
			Contract.Ensures(((IArray)this).Count == Contract.OldValue(((IArray)this).Count) - 1);
		}
		#endregion
	}
}