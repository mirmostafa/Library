#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;

namespace Library35.DesignPatterns.Behavioral.Observers
{
	public interface IObservable
	{
		void Register(IObserver anObserver);
		void UnRegister(IObserver anObserver);
	}

	public interface IObserver
	{
		void Notify(object anObject);
	}

	public class ObservableImpl : IObservable
	{
		protected readonly Hashtable ObserverContainer = new Hashtable();

		#region IObservable Members
		public void Register(IObserver anObserver)
		{
			this.ObserverContainer.Add(anObserver, anObserver);
		}

		public void UnRegister(IObserver anObserver)
		{
			this.ObserverContainer.Remove(anObserver);
		}
		#endregion

		public void NotifyObservers(object anObject)
		{
			foreach (IObserver anObserver in this.ObserverContainer.Keys)
				anObserver.Notify(anObject);
		}
	}
}