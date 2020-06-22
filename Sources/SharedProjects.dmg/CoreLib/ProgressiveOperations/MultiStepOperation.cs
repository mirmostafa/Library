using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.Annotations;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Interfaces;
using Mohammad.Logging;
using Mohammad.Threading.Tasks;

namespace Mohammad.MultistepOperations
{
    public abstract class MultiStepOperation : LoggerContainer, IDisposable, IExceptionHandlerContainer, ISupportSilence, INotifyPropertyChanged
    {
        private readonly MultiStepOperationStepCollection _Steps;
        private CancellationTokenSource _CancellationTokenSource;
        private bool _ContinueOnException;
        private double _CurrentStepIndex;
        private double _CurrentStepsCount;
        private ExceptionHandling _ExceptionHandling;
        private bool _IsDisposed;
        private bool _IsOperating;
        private double _MainStepIndex;
        private long _MainStepsCount;
        private TaskScheduler _Scheduler;
        public bool IsCancellationRequested => this._CancellationTokenSource.IsCancellationRequested;

        protected Task Task { get; private set; }

        public long MainStepsCount
        {
            get { return this._MainStepsCount; }
            private set
            {
                if (value == this._MainStepsCount)
                    return;
                this._MainStepsCount = value;
                this.OnPropertyChanged();
            }
        }

        public double MainStepIndex
        {
            get { return this._MainStepIndex; }
            private set
            {
                if (value.Equals(this._MainStepIndex))
                    return;
                this._MainStepIndex = value;
                this.OnPropertyChanged();
            }
        }

        public double CurrentStepsCount
        {
            get { return this._CurrentStepsCount; }
            protected set
            {
                if (value.Equals(this._CurrentStepsCount))
                    return;
                this._CurrentStepsCount = value;
                this.OnPropertyChanged();
            }
        }

        public double CurrentStepIndex
        {
            get { return this._CurrentStepIndex; }
            protected set
            {
                if (value.Equals(this._CurrentStepIndex))
                    return;
                this._CurrentStepIndex = value;
                this.OnPropertyChanged();
            }
        }

        public bool ContinueOnException
        {
            get { return this._ContinueOnException; }
            set
            {
                if (value == this._ContinueOnException)
                    return;
                this._ContinueOnException = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsOperating
        {
            get { return this._IsOperating; }
            set
            {
                if (value == this._IsOperating)
                    return;
                this._IsOperating = value;
                this.OnPropertyChanged();
            }
        }

        protected MultiStepOperation(MultiStepOperationStepCollection steps = null) { this._Steps = steps ?? new MultiStepOperationStepCollection(); }

        ~MultiStepOperation() { this.Dispose(false); }

        private void Dispose(bool disposing)
        {
            if (this._IsDisposed)
                return;
            if (!disposing)
                return;
            this._CancellationTokenSource?.Cancel();
            this._CancellationTokenSource?.Dispose();
            this.Task?.Dispose();
            this._IsDisposed = true;
        }

        protected virtual async void OnMultiStepErrorOccurred(MultiStepErrorOccurredEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMultiStepErrorOccurred = this.MultiStepErrorOccurred;
            if (onMultiStepErrorOccurred != null)
                await onMultiStepErrorOccurred.RaiseAsync(this, e, this._Scheduler);
            this.Fatal(e.Log, e.Exception, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        public Task Start()
        {
            this.InitializeMainOperationSteps();

            var noPriorities = this._Steps.Where(s => s.PriorityId == -1).ToList();
            if (noPriorities.Any() && this._Steps.Where(s => s.PriorityId != -1).ToList().Any())
                throw new Exception();
            this.MainStepsCount = this._Steps.Count;
            this._IsDisposed = false;
            this._CancellationTokenSource = new CancellationTokenSource();
            var task = Task.Factory.StartNew(() =>
                {
                    this.IsOperating = true;
                    this.OnMainOperationStarted(new MultiStepStartedLogEventArgs(this.MainStepsCount));
                },
                this._CancellationTokenSource.Token);

            if (noPriorities.Any())
            {
                task = Task.Run(() =>
                {
                    for (var index = 0; index < noPriorities.Count; index++)
                    {
                        if (this.IsCancellationRequested)
                            return;
                        var step = noPriorities[index];
                        this.MainStepIndex = index;
                        this.OnMainOperationStepIncreasing(new MultiStepLogEventArgs(index + 1, step.Description, max: this.MainStepsCount));
                        if (this.IsCancellationRequested)
                            return;
                        if (!this.Catch(step))
                            return;
                        if (this.IsCancellationRequested)
                            return;
                        this.OnMainOperationStepIncreased(new MultiStepLogEventArgs(index + 1, step.Description, max: this.MainStepsCount));
                    }
                });
            }
            else
            {
                var stepGroups = this._Steps.GroupBy(step => step.PriorityId).OrderBy(g => g.Key).ToList();
                for (var i = 0; i < stepGroups.Count; i++)
                {
                    if (this.IsCancellationRequested)
                        return task;
                    var stepGroup = stepGroups[i];
                    var index = i;
                    task = task.ContinueWith(_ =>
                    {
                        this.MainStepIndex = index;
                        stepGroup.FastForEach(step =>
                        {
                            if (this.IsCancellationRequested)
                                return;
                            this.OnMainOperationStepIncreasing(new MultiStepLogEventArgs(index, step.Description));
                            if (this.IsCancellationRequested)
                                return;
                            if (!this.Catch(step))
                                return;
                            if (this.IsCancellationRequested)
                                return;
                            this.OnMainOperationStepIncreased(new MultiStepLogEventArgs(index, step.Description));
                        });
                    });
                }
            }
            this.Task = task.ContinueWith(_ =>
            {
                this.IsOperating = false;
                this.OnMainOperationEnded(new MultiStepEndedLogEventArgs(string.Empty, this.ExceptionHandling.LastException == null, this.IsCancellationRequested));
            });
            return task;
        }

        private void InitializeMainOperationSteps()
        {
            this._Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.OnInitializingMainOperationSteps(this._Steps);
            this.MainStepsCount = this._Steps.Count;
            this.MainStepIndex = 0;
        }

        private bool Catch(IMultiStepOperationStep step)
        {
            try
            {
                step.Step(this);
                return true;
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex);
                if (!this.IsCancellationRequested)
                    this.OnMultiStepErrorOccurred(new MultiStepErrorOccurredEventArgs(ex, step.Description));
                return this.ContinueOnException;
            }
        }

        public void Cancel()
        {
            if (!this._IsDisposed)
                this._CancellationTokenSource.Cancel();
            this.ExceptionHandling.HandleException(new OperationCancelledException("Operation cancelled."));
            this.OnMainOperationCanceled();
        }

        public static MultiStepOperation CreateInstance(params IMultiStepOperationStep[] steps) => CreateInstance(new MultiStepOperationStepCollection(steps));

        public static MultiStepOperation CreateInstance(IEnumerable<MultiStepOperationStep> steps) => CreateInstance(new MultiStepOperationStepCollection(steps));

        public static MultiStepOperation CreateInstance(MultiStepOperationStepCollection steps) => new MultiStepOperationImpl(steps);

        public event EventHandler<MultiStepStartedLogEventArgs> MainOperationStarted;
        public event EventHandler<MultiStepLogEventArgs> MainOperationStepIncreasing;
        public event EventHandler<MultiStepLogEventArgs> MainOperationStepIncreased;
        public event EventHandler<MultiStepEndedLogEventArgs> MainOperationEnded;
        public event EventHandler MainOperationCanceled;
        public event EventHandler<MultiStepErrorOccurredEventArgs> MultiStepErrorOccurred;

        protected virtual async void OnMainOperationStarted(MultiStepStartedLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMainOperationStarted = this.MainOperationStarted;
            if (onMainOperationStarted != null)
                await onMainOperationStarted.RaiseAsync(this, e, this._Scheduler);
            this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual async void OnMainOperationStepIncreasing(MultiStepLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMainOperationStepIncreasing = this.MainOperationStepIncreasing;
            if (onMainOperationStepIncreasing != null)
                await onMainOperationStepIncreasing.RaiseAsync(this, e, this._Scheduler);
            this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual async void OnMainOperationStepIncreased(MultiStepLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMainOperationStepIncreased = this.MainOperationStepIncreased;
            if (onMainOperationStepIncreased != null)
                await onMainOperationStepIncreased.RaiseAsync(this, e, this._Scheduler);
            this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual async void OnMainOperationEnded(MultiStepEndedLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMainOperationEnded = this.MainOperationEnded;
            if (onMainOperationEnded != null)
                await onMainOperationEnded.RaiseAsync(this, e, this._Scheduler);
            this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual void OnMainOperationCanceled()
        {
            if (!this.EnableRaisingEvents)
                return;
            var onMainOperationCanceled = this.MainOperationCanceled;
            onMainOperationCanceled?.RaiseAsync(this, this._Scheduler);
            this.Debug("Main operation canceled.");
        }

        protected virtual void OnInitializingMainOperationSteps(MultiStepOperationStepCollection steps) { }
        public event EventHandler<MultiStepStartedLogEventArgs> CurrentOperationStarted;
        public event EventHandler<MultiStepLogEventArgs> CurrentOperationStepIncreasing;
        public event EventHandler<MultiStepLogEventArgs> CurrentOperationStepIncreased;
        public event EventHandler<MultiStepEndedLogEventArgs> CurrentOperationEnded;
        public event EventHandler CurrentOperationCanceled;

        public void StartCurrentOperation(double max, object description = null, double initialValue = 0)
        {
            this.CurrentStepIndex = 0;
            this.SetCurrentStepsCount(max);
            this.OnCurrentOperationStarted(new MultiStepStartedLogEventArgs(max, description, initialValue: initialValue));
        }

        public async void EndCurrentOperation(object description = null, bool succeed = true)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onCurrentOperationEnded = this.CurrentOperationEnded;
            if (onCurrentOperationEnded != null)
                await onCurrentOperationEnded.RaiseAsync(this, new MultiStepEndedLogEventArgs(description, succeed, this.IsCancellationRequested), this._Scheduler);
            this.Log(description);
        }

        protected void CancelCurrentOperation() { this.CurrentOperationCanceled.Raise(this, this._Scheduler); }

        public virtual void CurrentOperationIncreased(string description = null, object moreInfo = null)
        {
            this.CurrentStepIndex++;
            this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
        }

        protected virtual void CurrentOperationIncreasing(string description = null, object moreInfo = null)
        {
            this.CurrentStepIndex++;
            this.OnCurrentOperationIncreasing(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
        }

        protected virtual async void OnCurrentOperationStarted(MultiStepStartedLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onCurrentOperationStarted = this.CurrentOperationStarted;
            if (onCurrentOperationStarted != null)
                await onCurrentOperationStarted.RaiseAsync(this, e, this._Scheduler);
            this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual async void OnCurrentOperationIncreasing(MultiStepLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onCurrentOperationStepIncreasing = this.CurrentOperationStepIncreasing;
            if (onCurrentOperationStepIncreasing != null)
                await onCurrentOperationStepIncreasing.RaiseAsync(this, new MultiStepLogEventArgs(e.Step, e.Log, e.MoreInfo, e.Sender), this._Scheduler);
            this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual async void OnCurrentOperationIncreased(MultiStepLogEventArgs e)
        {
            if (!this.EnableRaisingEvents)
                return;
            var onCurrentOperationStepIncreased = this.CurrentOperationStepIncreased;
            if (onCurrentOperationStepIncreased != null)
                await onCurrentOperationStepIncreased.RaiseAsync(this, e, this._Scheduler);
            this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        protected virtual void ResetCurrentOperation(string description = null)
        {
            this.CurrentStepIndex = 0;
            this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
        }

        protected virtual void SetCurrentStepsCount(double count, string description = null)
        {
            this.CurrentStepsCount = count;
            this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RunCurrentSteps(IEnumerable<Action> actions, string currentOperationDescriptiondescription = null, TimeSpan timeout = default(TimeSpan))
        {
            this.RunCurrentSteps(actions.ToDictionary<Action, Action, string>(action => action, action => null), currentOperationDescriptiondescription, timeout);
        }

        public void RunCurrentSteps(IEnumerable<KeyValuePair<Action, string>> actions, string currentOperationDescriptiondescription = null,
            TimeSpan timeout = default(TimeSpan))
        {
            var steps = actions.Select(action => (Action) (() => this.DoSteps(action.Key, action.Value, timeout))).ToList();

            this.ResetCurrentOperation(currentOperationDescriptiondescription);
            this.SetCurrentStepsCount(steps.Count);
            steps.RunAllWhile(() => !this.IsCancellationRequested);
            this.EndCurrentOperation();
        }

        protected virtual void DoSteps(Action op, string desc, TimeSpan timeout = default(TimeSpan))
        {
            if (this.IsCancellationRequested)
                return;
            this.CurrentOperationIncreasing(desc);
            Exception ex = null;
            if (timeout == default(TimeSpan))
                ex = CodeHelper.Catch(op);
            else
                Async.RunAndWait(timeout, () => ex = CodeHelper.Catch(op));
            if (ex != null)
                this.ExceptionHandling.HandleException(this, ex);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ExceptionHandling ExceptionHandling
        {
            get { return this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling()); }
            set { this._ExceptionHandling = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private sealed class MultiStepOperationImpl : MultiStepOperation
        {
            public MultiStepOperationImpl(MultiStepOperationStepCollection steps)
                : base(steps) {}
        }
    }
}