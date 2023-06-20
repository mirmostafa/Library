namespace Library.Threading.MultistepProgress;

public abstract class MultiStepOperationStepBase : IMultiStepOperationStep
{
    protected MultiStepOperationStepBase(MultiStepOperation operation, string? description = null, int priorityId = -1)
    {
        this.Operation = operation;
        this.Step = this.OnStep;
        this.Description = description;
        this.PriorityId = priorityId;
    }

    public string? Description { get; }

    public MultiStepOperation Operation { get; }

    public int PriorityId { get; private set; }

    public Action<MultiStepOperation> Step { get; set; }

    protected abstract void OnStep(MultiStepOperation op);
}