namespace Library.Threading.MultistepProgress;

public sealed class MultiStepOperationStep<TArgument> : MultiStepOperationStepBase
{
    public MultiStepOperationStep(MultiStepOperation operation, Action<MultiStepOperation, TArgument> action, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId) => this.Action2 = action;

    public MultiStepOperationStep(MultiStepOperation operation, Action<TArgument> action, TArgument argument, string? description = null, int priorityId = -1)
        : base(operation, description, priorityId) => (this.Argument, this.Action) = (argument, action);

    public TArgument Argument { get; set; }
    private Action<TArgument> Action { get; }
    private Action<MultiStepOperation, TArgument> Action2 { get; }

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