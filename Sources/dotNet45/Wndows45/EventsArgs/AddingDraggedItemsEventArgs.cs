using System;
using System.Collections.Generic;

namespace Mohammad.Win.EventsArgs
{
    public class AddingDraggedItemsEventArgs : EventArgs
    {
        public AddingDraggedItemsEventArgs(IEnumerable<string> data) => this.Data = data;
        public IEnumerable<string> Data { get; }
    }
}