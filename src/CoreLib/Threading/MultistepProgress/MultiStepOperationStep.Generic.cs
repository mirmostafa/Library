﻿using Library;

using Library.Threading.MultistepProgress;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepOperationStep<TArgument> : MultiStepOperationStepBase
{
    private Action<TArgument> Action { get; }
    private Action<MultiStepOperation, TArgument> Action2 { get; }

    public TArgument Argument { get; set; }

    public MultiStepOperationStep(MultiStepOperation operation, Action<MultiStepOperation, TArgument> action, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId) => this.Action2 = action;

    public MultiStepOperationStep(MultiStepOperation operation, Action<TArgument> action, TArgument argument, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId)
    {
        this.Argument = argument;
        this.Action = action;
    }

    protected override void OnStep(MultiStepOperation op)
    {
        if (this.Action != null)
        {
            this.Action(this.Argument);
        }
        else
        {
            this.Action2(op, this.Argument);
        }
    }
}
