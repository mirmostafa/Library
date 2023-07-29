using System.Collections.Immutable;
using System.Diagnostics;

using Library.Validations;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract record ResultBase(in bool? Succeed = null,
    in object? Status = null,
    in string? Message = null,
    in IEnumerable<(object Id, object Error)>? Errors = null,
    in ImmutableDictionary<string, object>? ExtraData = null)
{
    private readonly IEnumerable<(object Id, object Error)>? _error;

    /// <summary>
    /// Checks if the operation was successful by checking the Succeed flag, Status and Errors.
    /// </summary>
    public virtual bool IsSucceed => this.Succeed ?? ((this.Status is null or 0 or 200) && (!this.Errors?.Any() ?? true));

    /// <summary>
    /// Gets a value indicating whether the operation has failed.
    /// </summary>
    public virtual bool IsFailure => !this.IsSucceed;

    public void Deconstruct(out bool isSucceed, out string message) =>
        (isSucceed, message) = (this.IsSucceed, this.Message?.ToString() ?? string.Empty);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public virtual bool Equals(ResultBase? other) =>
        other is not null && this.Status == other.Status;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(this.Status, this.Message);

    public static implicit operator bool(ResultBase result) =>
        result.NotNull().IsSucceed;

    /// <summary>
    /// Generates a string representation of the result object.
    /// </summary>
    /// <returns>A string representation of the result object.</returns>
    public override string ToString()
    {
        var result = new StringBuilder().AppendLine($"IsSucceed: {this.IsSucceed}");
        if (!this.Message.IsNullOrEmpty())
        {
            _ = result.AppendLine(this.Message);
        }
        else if (this.Errors?.Count() == 1)
        {
            _ = result.AppendLine(this.Errors!.First().Error?.ToString() ?? "An error occurred.");
        }
        else if (this.Errors?.Any() ?? false)
        {
            foreach (var errorMessage in this.Errors.Select(x => x.Error?.ToString()).Compact())
            {
                _ = result.AppendLine($"- {errorMessage}");
            }
        }

        return result.ToString();
    }

    private string GetDebuggerDisplay() =>
        this.ToString();

    /// <summary>
    /// Combines multiple ResultBase objects into a single ResultBase object.
    /// </summary>
    /// <param name="results">The ResultBase objects to combine.</param>
    /// <returns>A ResultBase object containing the combined results.</returns>
    protected static (bool? Succeed, object? Status, string? Message, IEnumerable<(object Id, object Error)>? Errors, ImmutableDictionary<string, object>? ExtraData) Combine(params ResultBase[] results)
    {
        bool? isSucceed = results.All(x => x.Succeed == null) ? null : results.All(x => x.IsSucceed);
        var status = results.LastOrDefault(x => x.Status is not null)?.Status;
        var message = results.LastOrDefault(x => !x.Message.IsNullOrEmpty())?.Message;
        var errors = results.SelectMany(x => EnumerableHelper.DefaultIfEmpty(x?.Errors));
        var statusBuffer = results.Where(x => x.Status is not null).Select(x => x.Status).ToList();
        if (statusBuffer.Count > 1)
        {
            errors = errors.AddRangeImmuted(statusBuffer.Select(x => ((object)null!, x!)));
            status = null;
        }
        var extraData = combineExtraData(results);

        return (isSucceed, status, message, errors, extraData.ToImmutableDictionary());

        static IEnumerable<KeyValuePair<string, object>> combineExtraData(in ResultBase[] results)
        {
            var lastData = results
                .Where(x => x?.ExtraData is not null)
                .SelectMany(x => x.ExtraData!)
                .Where(item => !item.IsDefault() && !item.Key.IsNullOrEmpty() && item.Value is not null);
            return !lastData.Any()
                ? results.Select(x => new KeyValuePair<string, object>("Previous Result", x))
                : lastData;
        }
    }
}