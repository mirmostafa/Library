#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Helpers.Console.Decorators
{
    internal class TimeStamppedMyTextMessage : ITextDisplay
    {
        #region Fields

        private readonly ITextDisplay _ItfTextMsg;

        #endregion

        public TimeStamppedMyTextMessage(ITextDisplay oldMsg) => this._ItfTextMsg = oldMsg;

        public void Display()
        {
            this._ItfTextMsg.Display();
            string.Format("-> Message sent at {0}", DateTime.Now).WriteLine();
        }
    }
}