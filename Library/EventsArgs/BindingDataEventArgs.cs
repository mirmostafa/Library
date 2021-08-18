namespace Library.CodeGeneration.UI.EventsArgs
{
    public sealed class BindingDataEventArgs : EventArgs
    {
        public BindingDataEventArgs(bool isFirstDataRebind) => this.IsFirstDataRebind = isFirstDataRebind;
        public bool IsFirstDataRebind { get; }
    }
}
