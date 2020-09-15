using System;

namespace Mohammad.ProgressiveOperations
{
    public class MultiStepOperationStep : MultiStepOperationStepBase
    {
        public MultiStepOperationStep(MultiStepOperation operation, Action<MultiStepOperation> action, string description = null, int priorityId = -1)
            : base(operation, description, priorityId) => this.Action2 = action;

        public MultiStepOperationStep(MultiStepOperation operation, Action action, string description = null, int priorityId = -1)
            : base(operation, description, priorityId) => this.Action = action;

        private Action Action { get; }
        private Action<MultiStepOperation> Action2 { get; }

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

    public class MultiStepOperationStep<TArgument> : MultiStepOperationStepBase
    {
        public MultiStepOperationStep(MultiStepOperation operation,
            Action<MultiStepOperation, TArgument> action,
            string description = null,
            int priorityId = -1)
            : base(operation, description, priorityId) => this.Action2 = action;

        public MultiStepOperationStep(MultiStepOperation operation,
            Action<TArgument> action,
            TArgument argument,
            string description = null,
            int priorityId = -1)
            : base(operation, description, priorityId)
        {
            this.Argument = argument;
            this.Action = action;
        }

        private Action<TArgument> Action { get; }
        private Action<MultiStepOperation, TArgument> Action2 { get; }

        public TArgument Argument { get; set; }

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
}