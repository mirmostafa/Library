﻿namespace Library.Coding.ProgressiveOperations;

public class MultiStepOperationStep : MultiStepOperationStepBase
{
    private Action Action { get; }
    private Action<MultiStepOperation> Action2 { get; }

    public MultiStepOperationStep(MultiStepOperation operation, Action<MultiStepOperation> action, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId) => this.Action2 = action;

    public MultiStepOperationStep(MultiStepOperation operation, Action action, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId) => this.Action = action;

    protected override void OnStep(MultiStepOperation op)
    {
        if (this.Action != null)
        {
            this.Action();
        }
        else
        {
            this.Action2(op);
        }
    }
}
