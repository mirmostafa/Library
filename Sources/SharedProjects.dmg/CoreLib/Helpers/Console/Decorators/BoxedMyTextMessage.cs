namespace Mohammad.Helpers.Console.Decorators
{
    internal class BoxedMyTextMessage : ITextDisplay
    {
        // This is basically a hook to the unaltered object.
        private readonly ITextDisplay _ItfTextMsg;
        public BoxedMyTextMessage(ITextDisplay oldMsg) { this._ItfTextMsg = oldMsg; }

        public void Display()
        {
            // Add customization and use ref to object.    
            "****************************".WriteLine();
            this._ItfTextMsg.Display();
            "****************************".WriteLine();
        }
    }
}