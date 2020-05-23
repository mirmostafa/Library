#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Helpers.Console.Decorators
{
	internal class TimeStamppedMyTextMessage : ITextDisplay
	{
		private readonly ITextDisplay _ItfTextMsg;

		public TimeStamppedMyTextMessage(ITextDisplay oldMsg)
		{
			this._ItfTextMsg = oldMsg;
		}

		#region ITextDisplay Members
		public void Display()
		{
			this._ItfTextMsg.Display();
			string.Format("-> Message sent at {0}", DateTime.Now).WriteLine();
		}
		#endregion
	}
}