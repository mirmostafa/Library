// Created on     2018/07/29
// Last update on 2018/08/01 by Mohammad Mir mostafa 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestConsole45.ProgressiveApproach
{
    public class Actor : IDisposable
    {
        private readonly CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        protected CancellationToken CancellationToken => this._CancellationTokenSource.Token;

        protected bool IsCancellationRequested => this.CancellationToken.IsCancellationRequested;

        /// <inheritdoc />
        public void Dispose() => this._CancellationTokenSource?.Dispose();

        public static Actor Run(IEnumerable<IStep> steps)
        {
            var actor = new Actor();
            actor.Start(steps);
            return actor;
        }

        public static Actor Run(params IStep[] steps)
        {
            var actor = new Actor();
            actor.Start(steps);
            return actor;
        }

        public void Cancel() => this._CancellationTokenSource.Cancel();

        public void Start(params IStep[] steps) => this.Start(steps.AsEnumerable());

        public void Start(IEnumerable<IStep> steps)
        {
            this.OnExecutionStarted();

            void ExecuteMainAction(IStep step)
            {
                var action = step?.MainAction;
                if (action == null)
                {
                    return;
                }

                var parameters = action.Method.GetParameters();
                if (!parameters.Any())
                {
                    action.DynamicInvoke();
                    return;
                }

                if (parameters.Length == 1)
                {
                    var parameterInfo = parameters.First();
                    if (parameterInfo.ParameterType == typeof(Actor))
                    {
                        action.DynamicInvoke(this);
                        return;
                    }

                    action.DynamicInvoke(step);
                    return;
                }

                action.DynamicInvoke(this, step);
            }

            var index = 0;

            void ExexuteStep(IStep step, int max)
            {
                this.OnExecutingStep((step, index, max));
                ExecuteMainAction(step);
                if (step?.Children.Any() != true)
                {
                    this.OnExecutedStep((step, index, max));
                    return;
                }

                foreach (var child in step.Children.OrderBy(c => c.Priority))
                {
                    if (this.IsCancellationRequested)
                    {
                        break;
                    }

                    ExexuteStep(child, step.Children.Count());
                }

                this.OnExecutedStep((step, index, max));
            }

            var currSteps = steps as IList<IStep> ?? steps.ToList();
            foreach (var step in currSteps)
            {
                if (this.IsCancellationRequested)
                {
                    break;
                }

                ExexuteStep(step, currSteps.Count);
            }

            this.OnExecutionDone();
        }

        protected virtual void OnExecutionDone()
        {
        }

        protected virtual void OnExecutionStarted()
        {
        }

        protected virtual void OnExecutingStep((IStep Step, int Index, int Max) e) => this.ExecutingStep?.Invoke(this, e);

        protected virtual void OnExecutedStep((IStep Step, int Index, int Max) e) => this.ExecutedStep?.Invoke(this, e);

        public event EventHandler<(IStep Step, int Index, int Max)> ExecutingStep;
        public event EventHandler<(IStep Step, int Index, int Max)> ExecutedStep;
    }
}