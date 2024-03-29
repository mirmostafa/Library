﻿using System.Collections.ObjectModel;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepOperationStepCollection : Collection<IMultiStepOperationStep>
{
    public MultiStepOperationStepCollection(IEnumerable<IMultiStepOperationStep> steps)
        : base(steps.ToList()) { }

    public MultiStepOperationStepCollection()
    { }

    public MultiStepOperationStepCollection(IList<IMultiStepOperationStep> step)
        : base(step) { }

    public void Add(MultiStepOperation operation, Action action, string? description = null, int priorityId = -1)
        => this.Add(new MultiStepOperationStep(operation, action, description, priorityId));

    public void Add<TArgument>(MultiStepOperation operation, Action<TArgument> action, TArgument argument, string? description = null, int priorityId = -1)
        => this.Add(new MultiStepOperationStep<TArgument>(operation, action, argument, description, priorityId));
}