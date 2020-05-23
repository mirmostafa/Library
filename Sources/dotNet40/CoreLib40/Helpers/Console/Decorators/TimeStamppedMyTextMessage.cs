#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Helpers.Console.Decorators
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