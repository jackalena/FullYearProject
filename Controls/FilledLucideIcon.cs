using System;
using System.Linq.Expressions;
using System.Reflection;
using Avalonia;
using Avalonia.Media;
using Lucide.Avalonia;

namespace FullYearProject.Controls.SmoothScrollViewer;

public class FilledLucideIcon : LucideIcon
{
    public static readonly StyledProperty<IBrush?> FillProperty = AvaloniaProperty.Register<FilledLucideIcon, IBrush?>(nameof(Fill));

    private static readonly Func<LucideIcon, Geometry?> GetGeometryFunc;
    private static readonly Func<LucideIcon, Pen?> GetStrokeFunc;

    private static readonly Action<LucideIcon, DrawingContext>? PushScalingTransformFunc;

    public IBrush? Fill {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    static FilledLucideIcon()
    {
        GetGeometryFunc = CreateFieldGetter<LucideIcon, Geometry?>("_geometry");
        GetStrokeFunc = CreateFieldGetter<LucideIcon, Pen?>("_stroke");

        PushScalingTransformFunc = CreateCaller<LucideIcon, DrawingContext>("PushScalingTransform");
    }

    private static Func<TInstance, TField> CreateFieldGetter<TInstance, TField>(string fieldName)
    {
        var param = Expression.Parameter(typeof(TInstance), "obj");
        var field = Expression.Field(param, fieldName);

        return Expression.Lambda<Func<TInstance, TField>>(field, param).Compile();
    }

    public static Action<TInstance, TArg1>? CreateCaller<TInstance, TArg1>(string methodName)
    {
        var instanceType = typeof(TInstance);
        var method = instanceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (method == null)
        {
            return null;
        }

        var instanceParam = Expression.Parameter(instanceType, "instance");
        var arg1Param = Expression.Parameter(typeof(TArg1), "arg1");

        var call = Expression.Call(instanceParam, method, arg1Param);

        var lambda = Expression.Lambda<Action<TInstance, TArg1>>(call, instanceParam, arg1Param);

        return lambda.Compile();
    }

    public override void Render(DrawingContext context)
    {
        var geometry = GetGeometryFunc(this);

        if (geometry is null)
        {
            return;
        }

        context.DrawRectangle(Brushes.Transparent, null, new(0, 0, Bounds.Width, Bounds.Height));

        PushScalingTransformFunc?.Invoke(this, context);

        context.DrawGeometry(Fill, GetStrokeFunc(this), geometry);
    }
}