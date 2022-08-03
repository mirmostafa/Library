using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ProgressiveOperations;

public interface IMultistepProgress<T>
{
    void Report(T value, OperationReport? operation = null, OperationReport? subOperation = null);
}

public record OperationReport(int? Step, int? Max)
{
    public static OperationReport New(int? step, int? max)
        => new OperationReport(step, max);
    public OperationReport With(int? step)
        => New(step, this.Max);
    public void Deconstruct(out int? step, out int? max)
        => (step, max) = (this.Step, this.Max);

    public static implicit operator OperationReport((int? Step, int? Max) data)
        => New(data.Step, data.Max);
}