namespace Library.ProgressiveOperations;

public sealed class ProgressIncreaseEventArgs : EventArgs
{
    public ProgressIncreaseEventArgs(int max, int step, string description)
        : this(max, step) => this.Description = description;

    public ProgressIncreaseEventArgs(int max, int step)
    {
        this.Max = max;
        this.Step = step;
    }

    public int Max { get; private set; }

    public int Step { get; private set; }

    public string Description { get; private set; }
}