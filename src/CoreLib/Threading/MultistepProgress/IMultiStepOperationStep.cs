namespace Library.Threading.MultistepProgress;

public interface IMultiStepOperationStep
{
    string? Description { get; }
    MultiStepOperation Operation { get; }
    int PriorityId { get; }
    Action<MultiStepOperation> Step { get; set; }
}