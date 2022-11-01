using Library;
using Library.Threading.MultistepProgress;

namespace Library.Threading.MultistepProgress;

public interface IMultiStepOperationStep
{
    Action<MultiStepOperation> Step { get; set; }
    MultiStepOperation Operation { get; }
    string? Description { get; }
    int PriorityId { get; }
}
