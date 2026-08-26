using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FullYearProject.Helpers;

public class ReflectionHelpers
{
    public static Func<TType, TField> CreateFieldGetter<TType, TField>(string fieldName)
    {
        var type = typeof(TType);

        var fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (fieldInfo == null)
        {
            throw new ArgumentException($"Field '{fieldName}' not found on {type.Name}");
        }

        var paramExpr = Expression.Parameter(type, "instance");
        var fieldExpr = Expression.Field(paramExpr, fieldInfo);

        LambdaExpression lambda = Expression.Lambda<Func<TType, TField>>(fieldExpr, paramExpr);

        return (Func<TType, TField>) lambda.Compile();
    }
}