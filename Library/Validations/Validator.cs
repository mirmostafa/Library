using System.Diagnostics.CodeAnalysis;
using Library.Data.Markers;

namespace Library.Validations;

public static class Validator
{
    public static TEntity Validate<TEntity>([DisallowNull]this TEntity entity)
        where TEntity : notnull, IEntity
    {
        if(entity == null)
        {
            throw new Exceptions.Validations.NullValueValidationException(nameof(entity));
        }
        var props = entity.GetType().GetProperties();
        foreach (var prop in props)
        {
            InnerValidate(prop);
        }
        return entity;
    }

    private static void InnerValidate(System.Reflection.PropertyInfo prop)
    {

    }
}
