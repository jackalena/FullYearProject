using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace FullYearProject.Controls.SmoothScrollViewer;

public class SmoothScrollContentPresenter : ScrollContentPresenter
{
    public static readonly DirectProperty<SmoothScrollContentPresenter, Vector> OffsetTargetProperty =
        AvaloniaProperty.RegisterDirect<SmoothScrollContentPresenter, Vector>(nameof(OffsetTarget),
            obj => obj.OffsetTarget);

    public static readonly StyledProperty<double> ScrollStepSizeProperty =
        AvaloniaProperty.Register<SmoothScrollContentPresenter, double>(nameof(ScrollStepSize), 40);

    private ScrollViewer? _scrollViewer;

    public Vector OffsetTarget
    {
        get;
        private set => SetAndRaise(OffsetTargetProperty, ref field, value);
    }

    public double ScrollStepSize
    {
        get => GetValue(ScrollStepSizeProperty);
        set => SetValue(ScrollStepSizeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _scrollViewer = TemplatedParent as ScrollViewer;
        _scrollViewer?.PropertyChanged += OnOwnerPropertyChanged;

        base.OnAttachedToVisualTree(e);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _scrollViewer?.PropertyChanged -= OnOwnerPropertyChanged;

        base.OnDetachedFromVisualTree(e);
    }

    private void OnOwnerPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == ScrollViewer.OffsetProperty)
        {
            var newVal = e.GetNewValue<Vector>();
            if (newVal - Offset != Vector.Zero)
            {
                var transitions = Transitions;
                Transitions = null;

                OffsetTarget = newVal;
                SetCurrentValue(OffsetProperty, newVal);

                Transitions = transitions;
            }
        }
    }


    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        if (Extent.Height > Viewport.Height || Extent.Width > Viewport.Width)
        {
            var scrollable = Child as ILogicalScrollable;
            var isLogical = scrollable?.IsLogicalScrollEnabled == true;

            var x = OffsetTarget.X;
            var y = OffsetTarget.Y;
            var delta = e.Delta;

            if (e.KeyModifiers == KeyModifiers.Shift && delta.X < 1f)
            {
                delta = new(delta.Y, delta.X);
            }
            else
            {
                delta = AdjustDeltaForFlowDirection(delta, FlowDirection);
            }

            if (Extent.Height > Viewport.Height)
            {
                var height = isLogical ? scrollable!.ScrollSize.Height : ScrollStepSize;
                y += -delta.Y * height;
                y = Math.Max(y, 0);
                y = Math.Min(y, Extent.Height - Viewport.Height);
            }

            if (Extent.Width > Viewport.Width)
            {
                var width = isLogical ? scrollable!.ScrollSize.Width : ScrollStepSize;
                x += -delta.X * width;
                x = Math.Max(x, 0);
                x = Math.Min(x, Extent.Width - Viewport.Width);
            }

            Vector newOffset = new(x, y);

            var offsetChanged = newOffset != OffsetTarget;
            OffsetTarget = newOffset;

            e.Handled = !IsScrollChainingEnabled || offsetChanged;
        }
    }

    private static Vector AdjustDeltaForFlowDirection(Vector delta, FlowDirection flowDirection)
    {
        return flowDirection == FlowDirection.RightToLeft ? delta.WithX(-delta.X) : delta;
    }
}