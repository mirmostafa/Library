namespace Library.Coding.ProgressiveOperations;

public interface IMultiStepOperationStep
{
    Action<MultiStepOperation> Step { get; set; }
    MultiStepOperation Operation { get; }
    string? Description { get; }
    int PriorityId { get; }
}
