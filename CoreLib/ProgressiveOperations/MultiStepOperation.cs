using Library.Coding;
using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Exceptions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.ProgressiveOperations;

public abstract class MultiStepOperation : IDisposable, IExceptionHandlerContainer, INotifyPropertyChanged
{
    private readonly MultiStepOperationStepCollection _steps;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _continueOnException;
    private double _currentStepIndex;
    private double _currentStepsCount;
    private ExceptionHandling _exceptionHandling;
    private bool _isDisposed;
    private bool _isOperating;
    private double _mainStepIndex;
    private long _mainStepsCount;
    //private TaskScheduler _scheduler;

    public bool IsCancellationRequested => this._cancellationTokenSource.IsCancellationRequested;

    protected Task Task { get; private set; }

    public long MainStepsCount
    {
        get => this._mainStepsCount;
        private set
        {
            if (value == this._mainStepsCount)
            {
                return;
            }

            this._mainStepsCount = value;
            this.OnPropertyChanged();
        }
    }

    public double MainStepIndex
    {
        get => this._mainStepIndex;
        private set
        {
            if (value.Equals(this._mainStepIndex))
            {
                return;
            }

            this._mainStepIndex = value;
            this.OnPropertyChanged();
        }
    }

    public double CurrentStepsCount
    {
        get => this._currentStepsCount;
        protected set
        {
            if (value.Equals(this._currentStepsCount))
            {
                return;
            }

            this._currentStepsCount = value;
            this.OnPropertyChanged();
        }
    }

    public double CurrentStepIndex
    {
        get => this._currentStepIndex;
        protected set
        {
            if (value.Equals(this._currentStepIndex))
            {
                return;
            }

            this._currentStepIndex = value;
            this.OnPropertyChanged();
        }
    }

    public bool ContinueOnException
    {
        get => this._continueOnException;
        set
        {
            if (value == this._continueOnException)
            {
                return;
            }

            this._continueOnException = value;
            this.OnPropertyChanged();
        }
    }

    public bool IsOperating
    {
        get => this._isOperating;
        set
        {
            if (value == this._isOperating)
            {
                return;
            }

            this._isOperating = value;
            this.OnPropertyChanged();
        }
    }

    protected MultiStepOperation(MultiStepOperationStepCollection? steps = null)
        => this._steps = steps ?? new MultiStepOperationStepCollection();

    ~MultiStepOperation() { this.Dispose(false); }

    private void Dispose(bool disposing)
    {
        if (this._isDisposed)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        this._cancellationTokenSource?.Cancel();
        this._cancellationTokenSource?.Dispose();
        this.Task?.Dispose();
        this._isDisposed = true;
    }

    protected virtual void OnMultiStepErrorOccurred(MultiStepErrorOccurredEventArgs e)
        => MultiStepErrorOccurred?.Invoke(this, e);//this.Fatal(e.Log, e.Exception, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    public Task Start()
    {
        this.InitializeMainOperationSteps();

        var noPriorities = this._steps.Where(s => s.PriorityId == -1).ToList();
        if (noPriorities.Any() && this._steps.Where(s => s.PriorityId != -1).ToList().Any())
        {
            throw new Exception();
        }

        this.MainStepsCount = this._steps.Count;
        this._isDisposed = false;
        this._cancellationTokenSource = new CancellationTokenSource();
        var task = Task.Factory.StartNew(() =>
        {
            this.IsOperating = true;
            this.OnMainOperationStarted(new MultiStepStartedLogEventArgs(this.MainStepsCount));
        },
            this._cancellationTokenSource.Token);

        if (noPriorities.Any())
        {
            task = Task.Run(() =>
            {
                for (var index = 0; index < noPriorities.Count; index++)
                {
                    if (this.IsCancellationRequested)
                    {
                        return;
                    }

                    var step = noPriorities[index];
                    this.MainStepIndex = index;
                    this.OnMainOperationStepIncreasing(new MultiStepLogEventArgs(index + 1, step.Description, max: this.MainStepsCount));
                    if (this.IsCancellationRequested)
                    {
                        return;
                    }

                    if (!this.Catch(step))
                    {
                        return;
                    }

                    if (this.IsCancellationRequested)
                    {
                        return;
                    }

                    this.OnMainOperationStepIncreased(new MultiStepLogEventArgs(index + 1, step.Description, max: this.MainStepsCount));
                }
            });
        }
        else
        {
            var stepGroups = this._steps.GroupBy(step => step.PriorityId).OrderBy(g => g.Key).ToList();
            for (var i = 0; i < stepGroups.Count; i++)
            {
                if (this.IsCancellationRequested)
                {
                    return task;
                }

                var stepGroup = stepGroups[i];
                var index = i;
                task = task.ContinueWith(_ =>
                {
                    this.MainStepIndex = index;
                    stepGroup.ForEach(step =>
                    {
                        if (this.IsCancellationRequested)
                        {
                            return;
                        }

                        this.OnMainOperationStepIncreasing(new MultiStepLogEventArgs(index, step.Description));
                        if (this.IsCancellationRequested)
                        {
                            return;
                        }

                        if (!this.Catch(step))
                        {
                            return;
                        }

                        if (this.IsCancellationRequested)
                        {
                            return;
                        }

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
        //this._scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        this.OnInitializingMainOperationSteps(this._steps);
        this.MainStepsCount = this._steps.Count;
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
            {
                this.OnMultiStepErrorOccurred(new MultiStepErrorOccurredEventArgs(ex, step.Description));
            }

            return this.ContinueOnException;
        }
    }

    public void Cancel()
    {
        if (!this._isDisposed)
        {
            this._cancellationTokenSource.Cancel();
        }

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

    protected virtual void OnMainOperationStarted(MultiStepStartedLogEventArgs e) => MainOperationStarted?.Invoke(this, e);//this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void OnMainOperationStepIncreasing(MultiStepLogEventArgs e) => MainOperationStepIncreasing?.Invoke(this, e);//this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void OnMainOperationStepIncreased(MultiStepLogEventArgs e) => MainOperationStepIncreased.Invoke(this, e);//this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void OnMainOperationEnded(MultiStepEndedLogEventArgs e) => MainOperationEnded?.Invoke(this, e);//this.Info(e.Log, e.MoreInfo, e.Sender, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void OnMainOperationCanceled()
    {
        var onMainOperationCanceled = this.MainOperationCanceled;
        onMainOperationCanceled?.Invoke(this, EventArgs.Empty);
        //this.Debug("Main operation canceled.");
    }

    protected virtual void OnInitializingMainOperationSteps(MultiStepOperationStepCollection steps) { }
    public event EventHandler<MultiStepStartedLogEventArgs> CurrentOperationStarted;
    public event EventHandler<MultiStepLogEventArgs> CurrentOperationStepIncreasing;
    public event EventHandler<MultiStepLogEventArgs> CurrentOperationStepIncreased;
    public event EventHandler<MultiStepEndedLogEventArgs> CurrentOperationEnded;
    public event EventHandler CurrentOperationCanceled;

    public void StartCurrentOperation(double max, object? description = null, double initialValue = 0)
    {
        this.CurrentStepIndex = 0;
        this.SetCurrentStepsCount(max);
        this.OnCurrentOperationStarted(new MultiStepStartedLogEventArgs(max, description, initialValue: initialValue));
    }

    public void EndCurrentOperation(object? description = null, bool succeed = true) => CurrentOperationEnded?.Invoke(this, new MultiStepEndedLogEventArgs(description, succeed, this.IsCancellationRequested));//this.Log(description);

    protected void CancelCurrentOperation() => this.CurrentOperationCanceled?.Invoke(this, EventArgs.Empty);

    public virtual void CurrentOperationIncreased(string? description = null, object? moreInfo = null)
    {
        this.CurrentStepIndex++;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
    }

    protected virtual void CurrentOperationIncreasing(string? description = null, object? moreInfo = null)
    {
        this.CurrentStepIndex++;
        this.OnCurrentOperationIncreasing(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
    }

    protected virtual void OnCurrentOperationStarted(MultiStepStartedLogEventArgs e)
        => CurrentOperationStarted.Invoke(this, e);//this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual
        void OnCurrentOperationIncreasing(MultiStepLogEventArgs e)
        => CurrentOperationStepIncreasing?.Invoke(this, new MultiStepLogEventArgs(e.Step, e.Log, e.MoreInfo, e.Sender));//this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void OnCurrentOperationIncreased(MultiStepLogEventArgs e)
        => CurrentOperationStepIncreased?.Invoke(this, e);//this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);

    protected virtual void ResetCurrentOperation(string? description = null)
    {
        this.CurrentStepIndex = 0;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
    }

    protected virtual void SetCurrentStepsCount(double count, string? description = null)
    {
        this.CurrentStepsCount = count;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void RunCurrentSteps(IEnumerable<Action> actions, string? currentOperationDescriptiondescription = null, TimeSpan timeout = default)
        => this.RunCurrentSteps(actions.ToDictionary<Action, Action, string?>(action => action, action => null), currentOperationDescriptiondescription, timeout);

    public void RunCurrentSteps(IEnumerable<KeyValuePair<Action, string?>> actions, string? currentOperationDescriptiondescription = null,
        TimeSpan timeout = default)
    {
        var steps = actions.Select(action => new Action(() => this.DoSteps(action.Key, action.Value, timeout))).ToList();

        this.ResetCurrentOperation(currentOperationDescriptiondescription);
        this.SetCurrentStepsCount(steps.Count);
        steps.RunAllWhile(() => !this.IsCancellationRequested);
        this.EndCurrentOperation();
    }

    protected virtual void DoSteps(Action op, string? desc, TimeSpan timeout = default)
    {
        if (this.IsCancellationRequested)
        {
            return;
        }

        this.CurrentOperationIncreasing(desc);
        Exception? ex = null;
        if (timeout == default)
        {
            ex = CodeHelper.Catch(op);
        }
        else
        {
            ex = Task.Factory.StartNew(() => CodeHelper.Catch(op), this._cancellationTokenSource.Token).Result;
        }

        if (ex != null)
        {
            this.ExceptionHandling.HandleException(this, ex);
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ExceptionHandling ExceptionHandling
    {
        get => this._exceptionHandling ??= new ExceptionHandling();
        set => this._exceptionHandling = value;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private sealed class MultiStepOperationImpl : MultiStepOperation
    {
        public MultiStepOperationImpl(MultiStepOperationStepCollection steps)
            : base(steps) { }
    }
}
