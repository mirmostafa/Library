using System;

namespace Mohammad.ProgressiveOperations
{
    public abstract class MultiStepOperationStepBase : IMultiStepOperationStep
    {
        protected MultiStepOperationStepBase(MultiStepOperation operation, string description = null, int priorityId = -1)
            => this.InitializeComponents(operation, description, priorityId);

        public Action<MultiStepOperation> Step { get; set; }
        public MultiStepOperation Operation { get; private set; }
        public string Description { get; private set; }
        public int PriorityId { get; private set; }
        public long Id { get; internal set; }

        protected abstract void OnStep(MultiStepOperation op);

        private void InitializeComponents(MultiStepOperation operation, string description, int priorityId)
        {
            this.Operation = operation;
            this.Step = this.OnStep;
            this.Description = description;
            this.PriorityId = priorityId;
        }
    }
}