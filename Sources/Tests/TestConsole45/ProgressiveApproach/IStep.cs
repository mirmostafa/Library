using System;
using System.Collections.Generic;

namespace TestConsole45.ProgressiveApproach
{
    public interface IStep
    {
        Delegate MainAction { get; }
        IEnumerable<IStep> Children { get; }
        string Description { get; }
        int Priority { get; }
    }
}