#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.Helpers.Console.Decorators
{
    internal class BoxedMyTextMessage : ITextDisplay
    {
        #region Fields

        // This is basically a hook to the unaltered object.
        private readonly ITextDisplay _ItfTextMsg;

        #endregion

        public BoxedMyTextMessage(ITextDisplay oldMsg) => this._ItfTextMsg = oldMsg;

        public void Display()
        {
            // Add customization and use ref to object.    
            "****************************".WriteLine();
            this._ItfTextMsg.Display();
            "****************************".WriteLine();
        }
    }
}