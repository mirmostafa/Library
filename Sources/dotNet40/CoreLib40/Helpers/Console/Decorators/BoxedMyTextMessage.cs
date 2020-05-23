#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Helpers.Console.Decorators
{
	internal class BoxedMyTextMessage : ITextDisplay
	{
		// This is basically a hook to the unaltered object.
		private readonly ITextDisplay _ItfTextMsg;

		public BoxedMyTextMessage(ITextDisplay oldMsg)
		{
			this._ItfTextMsg = oldMsg;
		}

		#region ITextDisplay Members
		public void Display()
		{
			// Add customization and use ref to object.    
			"****************************".WriteLine();
			this._ItfTextMsg.Display();
			"****************************".WriteLine();
		}
		#endregion
	}
}