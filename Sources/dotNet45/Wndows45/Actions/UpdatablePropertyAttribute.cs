using System;

namespace Mohammad.Win.Actions
{
    /// <summary>
    ///     Ensures that the owner is Updatable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UpdatablePropertyAttribute : Attribute {}
}