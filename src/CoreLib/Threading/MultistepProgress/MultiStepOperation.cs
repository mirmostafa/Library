using System.ComponentModel;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Exceptions;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

public abstract class MultiStepOperation : IDisposable, IExceptionHandlerContainer, INotifyPropertyChanged
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MultiStepOperationStepCollection _steps;
    private bool _continueOnException;
    private double _currentStepIndex;
    private double _currentStepsCount;
    private ExceptionHandling? _exceptionHandling;
    private bool _isDisposed;
    private bool _isOperating;
    private double _mainStepIndex;
    private long _mainStepsCount;
    private TaskScheduler? _scheduler;

    public event EventHandler? CurrentOperationCanceled;

    public event EventHandler<MultiStepEndedLogEventArgs>? CurrentOperationEnded;

    public event EventHandler<MultiStepStartedLogEventArgs>? CurrentOperationStarted;

    public event EventHandler<MultiStepLogEventArgs>? CurrentOperationStepIncreased;

    public event EventHandler<MultiStepLogEventArgs>? CurrentOperationStepIncreasing;

    public event EventHandler? MainOperationCanceled;

    public event EventHandler<MultiStepEndedLogEventArgs>? MainOperationEnded;

    public event EventHandler<MultiStepStartedLogEventArgs>? MainOperationStarted;

    public event EventHandler<MultiStepLogEventArgs>? MainOperationStepIncreased;

    public event EventHandler<MultiStepLogEventArgs>? MainOperationStepIncreasing;

    public event EventHandler<MultiStepErrorOccurredEventArgs>? MultiStepErrorOccurred;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected MultiStepOperation(MultiStepOperationStepCollection? steps = null, CancellationTokenSource? cancellationTokenSource = default)
    {
        this._steps = steps ?? new MultiStepOperationStepCollection();
        this._cancellationTokenSource = cancellationTokenSource ?? new();
    }

    ~MultiStepOperation()
    {
        this.Dispose(false);
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

    public ExceptionHandling ExceptionHandling
    {
        get => this._exceptionHandling ??= new ExceptionHandling();
        set => this._exceptionHandling = value;
    }

    public bool IsCancellationRequested => this._cancellationTokenSource?.IsCancellationRequested ?? false;

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

    protected Task? Task { get; private set; }

    public static MultiStepOperation CreateInstance(params IMultiStepOperationStep[] steps)
        => CreateInstance(new MultiStepOperationStepCollection(steps));

    public static MultiStepOperation CreateInstance(IEnumerable<MultiStepOperationStep> steps)
        => CreateInstance(new MultiStepOperationStepCollection(steps));

    public static MultiStepOperation CreateInstance(MultiStepOperationStepCollection steps)
        => new MultiStepOperationImpl(steps);

    public void Cancel()
    {
        if (!this._isDisposed)
        {
            this._cancellationTokenSource?.Cancel();
        }

        this.ExceptionHandling.HandleException(new OperationCancelledException("Operation canceled."));
        this.OnMainOperationCanceled();
    }

    public virtual void CurrentOperationIncreased(string? description = null, object? moreInfo = null)
    {
        this.CurrentStepIndex++;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void EndCurrentOperation(object? description = null, bool succeed = true)
        => CurrentOperationEnded?.Invoke(this, new MultiStepEndedLogEventArgs(description, succeed, this.IsCancellationRequested));

    public void RunCurrentSteps(IEnumerable<Action> actions, string? currentOperationDescription = null, TimeSpan timeout = default)
        => this.RunCurrentSteps(actions.ToDictionary<Action, Action, string?>(action => action, action => null), currentOperationDescription, timeout);

    public void RunCurrentSteps(IEnumerable<KeyValuePair<Action, string?>> actions, string? currentOperationDescription = null,
        TimeSpan timeout = default)
    {
        var steps = actions.Select(action => new Action(() => this.DoSteps(action.Key, action.Value, timeout))).ToList();

        this.ResetCurrentOperation(currentOperationDescription);
        this.SetCurrentStepsCount(steps.Count);
        steps.RunAllWhile(() => !this.IsCancellationRequested);
        this.EndCurrentOperation();
    }

    public Task Start()
    {
        this.InitializeMainOperationSteps();
        if (this._steps.Count == 0)
        {
            throw new Exception();
        }

        var noPriorities = this._steps.Where(s => s.PriorityId == -1).ToList();

        this.MainStepsCount = this._steps.Count;
        this._isDisposed = false;
        this.IsOperating = true;
        this.OnMainOperationStarted(new MultiStepStartedLogEventArgs(this.MainStepsCount));
        Task task;
        this._scheduler = Task.Factory.Scheduler;
        if (noPriorities.Count != 0)
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
            task = Task.Run(() => { }, this._cancellationTokenSource.Token);
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
                    _ = (Task)stepGroup.Enumerate(step =>
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

    public void StartCurrentOperation(double max, object? description = null, double initialValue = 0)
    {
        this.CurrentStepIndex = 0;
        this.SetCurrentStepsCount(max);
        this.OnCurrentOperationStarted(new MultiStepStartedLogEventArgs(max, description, initialValue: initialValue));
    }

    protected void CancelCurrentOperation() => this.CurrentOperationCanceled?.Invoke(this, EventArgs.Empty);

    protected virtual void CurrentOperationIncreasing(string? description = null, object? moreInfo = null)
    {
        this.CurrentStepIndex++;
        this.OnCurrentOperationIncreasing(new MultiStepLogEventArgs(this.CurrentStepIndex, description, moreInfo, max: this.CurrentStepsCount));
    }

    protected virtual void DoSteps(Action op, string? desc, TimeSpan timeout = default)
    {
        if (this.IsCancellationRequested)
        {
            return;
        }

        this.CurrentOperationIncreasing(desc);
        var ex = timeout == default
            ? CodeHelper.Catch(op)
            : Task.Factory.StartNew(() => CodeHelper.Catch(op), this._cancellationTokenSource!.Token).Result;
        if (ex != null)
        {
            this.ExceptionHandling.HandleException(this, ex);
        }
    }

    protected virtual void OnCurrentOperationIncreased(MultiStepLogEventArgs e)
        => CurrentOperationStepIncreased?.Invoke(this, e);

    protected virtual void OnCurrentOperationIncreasing(MultiStepLogEventArgs e)
        => CurrentOperationStepIncreasing?.Invoke(this, new MultiStepLogEventArgs(e.Step, e.Log, e.MoreInfo, e.Sender));

    protected virtual void OnCurrentOperationStarted(MultiStepStartedLogEventArgs e)
        => CurrentOperationStarted?.Invoke(this, e);

    protected virtual void OnInitializingMainOperationSteps(MultiStepOperationStepCollection steps)
    { }

    protected virtual void OnMainOperationCanceled()
    {
        var onMainOperationCanceled = this.MainOperationCanceled;
        onMainOperationCanceled?.Invoke(this, EventArgs.Empty);
        //this.Debug("Main operation canceled.");
    }

    protected virtual void OnMainOperationEnded(MultiStepEndedLogEventArgs e)
        => MainOperationEnded?.Invoke(this, e);

    protected virtual void OnMainOperationStarted(MultiStepStartedLogEventArgs e)
    {
        if (MainOperationStarted == null)
        {
            return;
        }

        if (this._scheduler != null)
        {
            _ = Task.Factory.StartNew(() => MainOperationStarted(this, e), this._cancellationTokenSource.Token, TaskCreationOptions.None, this._scheduler);
        }
        else
        {
            MainOperationStarted?.Invoke(this, e);
        }
    }

    protected virtual void OnMainOperationStepIncreased(MultiStepLogEventArgs e)
        => MainOperationStepIncreased?.Invoke(this, e);

    protected virtual void OnMainOperationStepIncreasing(MultiStepLogEventArgs e)
        => MainOperationStepIncreasing?.Invoke(this, e);

    protected virtual void OnMultiStepErrorOccurred(MultiStepErrorOccurredEventArgs e)
        => MultiStepErrorOccurred?.Invoke(this, e);

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected virtual void ResetCurrentOperation(string? description = null)
    {
        this.CurrentStepIndex = 0;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
    }

    //this.Log(e.Log, e.MoreInfo, e.Sender, LogLevel.Internal, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
    protected virtual void SetCurrentStepsCount(double count, string? description = null)
    {
        this.CurrentStepsCount = count;
        this.OnCurrentOperationIncreased(new MultiStepLogEventArgs(this.CurrentStepsCount, this.CurrentStepIndex, description, max: this.CurrentStepsCount));
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

    private void Dispose(bool disposing)
    {
        Check.ThrowIfDisposed(this, this._isDisposed);

        if (!disposing)
        {
            return;
        }

        this._cancellationTokenSource?.Cancel();
        this._cancellationTokenSource?.Dispose();
        this.Task?.Dispose();
        this._isDisposed = true;
    }

    private void InitializeMainOperationSteps()
    {
        this.OnInitializingMainOperationSteps(this._steps);
        this.MainStepsCount = this._steps.Count;
        this.MainStepIndex = 0;
    }

    private sealed class MultiStepOperationImpl(MultiStepOperationStepCollection steps) : MultiStepOperation(steps)
    {
    }
}