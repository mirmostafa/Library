﻿using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IField : IMember
{
    const AccessModifier DefaultAccessModifier = AccessModifier.Private | AccessModifier.ReadOnly;

    [Obsolete("Use AccessModifier, instead.", true)]
    bool IsReadOnly { get; }

    TypePath Type { get; }

    static IField New(string name, TypePath type, AccessModifier accessModifier = IField.DefaultAccessModifier)
        => new Field(name, type, accessModifier);
}

[Immutable]
public sealed class Field : Member, IField
{
    public Field(string name, TypePath type, AccessModifier accessModifier = IField.DefaultAccessModifier)
        : base(name)
    {
        this.Type = type;
        this.AccessModifier = accessModifier;
    }

    [Obsolete("Use AccessModifier, instead.", true)]
    public bool IsReadOnly { get; init; }

    public TypePath Type { get; }
}