﻿namespace Library.CodeGeneration.Models;

public record struct GenerateCodeResult(in Code? Main, in Code? Partial);
public record struct MethodArgument(in string Type, in string Name);
public record struct GenerateCodesParameters(in bool GenerateMainCode = false, in bool GeneratePartialCode = true, in bool GenerateUiCode = false);
public record struct ConstrcutorArgument(in string Type, in string Name, in string? DataMemberName = null, in bool IsPropery = false);
public record struct PropertyAccessor(in bool Has = true, in bool? IsPrivate = null, in string? Code = null);