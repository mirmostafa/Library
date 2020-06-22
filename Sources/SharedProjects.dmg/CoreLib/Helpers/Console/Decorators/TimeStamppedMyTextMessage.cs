using System;

namespace Mohammad.Helpers.Console.Decorators
{
    internal class TimeStamppedMyTextMessage : ITextDisplay
    {
        private readonly ITextDisplay _ItfTextMsg;
        public TimeStamppedMyTextMessage(ITextDisplay oldMsg) { this._ItfTextMsg = oldMsg; }

        public void Display()
        {
            this._ItfTextMsg.Display();
            string.Format("-> Message sent at {0}", DateTime.Now).WriteLine();
        }
    }
}