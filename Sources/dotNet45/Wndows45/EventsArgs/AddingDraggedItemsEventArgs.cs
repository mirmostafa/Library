using System;
using System.Collections.Generic;

namespace Mohammad.Win.EventsArgs
{
    public class AddingDraggedItemsEventArgs : EventArgs
    {
        public IEnumerable<string> Data { get; private set; }
        public AddingDraggedItemsEventArgs(IEnumerable<string> data) { this.Data = data; }
    }
}