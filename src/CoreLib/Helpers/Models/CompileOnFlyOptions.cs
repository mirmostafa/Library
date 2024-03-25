using Library.DesignPatterns.Markers;

namespace Library.Helpers.Models;

[Immutable]
public sealed class CompileOnFlyByFileOptions(string sourceFile, string? outputFile = null) : CompileOnFlyOptionsBase(outputFile)
{
    public string SourceFile { get; init; } = sourceFile;
}

[Immutable]
public sealed class CompileOnFlyBySourceOptions(string source, string? outputFile = null) : CompileOnFlyOptionsBase(outputFile)
{
    public string Source { get; init; } = source;
}

[Immutable]
public abstract class CompileOnFlyOptionsBase(string? outputFile = null)
{
    public ISet<string> FrameworkReferences { get; } = new HashSet<string>();
    public bool IsReleaseMode { get; init; } = true;
    public string? OutputFile { get; init; } = outputFile;
    public string RuntimePath { get; init; } = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\";
    public bool WithOverflowChecks { get; init; } = true;
}