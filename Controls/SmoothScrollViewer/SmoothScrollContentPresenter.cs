using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace FullYearProject.Controls.SmoothScrollViewer;

/// <summary>
///     A <see cref="ScrollContentPresenter" /> that smoothly scrolls to the target offset.
/// </summary>
public class SmoothScrollContentPresenter : ScrollContentPresenter
{
    /// <summary>
    ///     Identifies the <see cref="OffsetTarget" /> dependency property.
    /// </summary>
    public static readonly DirectProperty<SmoothScrollContentPresenter, Vector> OffsetTargetProperty =
        AvaloniaProperty.RegisterDirect<SmoothScrollContentPresenter, Vector>(nameof(OffsetTarget),
            obj => obj.OffsetTarget);

    /// <summary>
    ///     Identifies the <see cref="ScrollStepSize" /> dependency property.
    /// </summary>
    public static readonly StyledProperty<double> ScrollStepSizeProperty =
        AvaloniaProperty.Register<SmoothScrollContentPresenter, double>(nameof(ScrollStepSize), 40);

    // The scrollviewer that owns this presenter.
    private ScrollViewer? _scrollViewer;

    /// <summary>
    ///     The target offset to scroll to.
    /// </summary>
    public Vector OffsetTarget
    {
        get;
        private set => SetAndRaise(OffsetTargetProperty, ref field, value);
    }

    /// <summary>
    ///     The number of device-independent units to scroll per wheel event.
    /// </summary>
    public double ScrollStepSize
    {
        get => GetValue(ScrollStepSizeProperty);
        set => SetValue(ScrollStepSizeProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        // Find the ScrollViewer that owns this presenter.
        _scrollViewer = TemplatedParent as ScrollViewer;
        _scrollViewer?.PropertyChanged += OnOwnerPropertyChanged;

        base.OnAttachedToVisualTree(e);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _scrollViewer?.PropertyChanged -= OnOwnerPropertyChanged;

        base.OnDetachedFromVisualTree(e);
    }

    // Called when a property on the ScrollViewer changes.
    private void OnOwnerPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // If the scroll offset changes from the ScrollViewer, update the OffsetTarget property.
        if (e.Property == ScrollViewer.OffsetProperty)
        {
            var newVal = e.GetNewValue<Vector>();
            if (newVal - Offset != Vector.Zero)
            {
                // Disable transitions while updating OffsetTarget.
                var transitions = Transitions;
                Transitions = null;

                OffsetTarget = newVal;
                SetCurrentValue(OffsetProperty, newVal);

                Transitions = transitions;
            }
        }
    }


    /// <inheritdoc />
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        // Do nothing if the content is not scrollable.
        if (!(Extent.Height > Viewport.Height) && !(Extent.Width > Viewport.Width))
        {
            return;
        }

        var scrollable = Child as ILogicalScrollable;
        var isLogical = scrollable?.IsLogicalScrollEnabled == true;

        var x = OffsetTarget.X;
        var y = OffsetTarget.Y;
        var delta = e.Delta;

        // If the shift key is pressed, treat the wheel as a horizontal scroll.
        if (e.KeyModifiers == KeyModifiers.Shift && delta.X < 1f)
        {
            delta = new(delta.Y, delta.X);
        }
        else
        {
            delta = FlowDirection == FlowDirection.RightToLeft ? delta.WithX(-delta.X) : delta;
        }

        // Scroll content vertically if it is bigger than the viewport.
        if (Extent.Height > Viewport.Height)
        {
            var height = isLogical ? scrollable!.ScrollSize.Height : ScrollStepSize;
            y += -delta.Y * height;
            y = Math.Max(y, 0);
            y = Math.Min(y, Extent.Height - Viewport.Height);
        }

        // Do the same for horizontal scrolling.
        if (Extent.Width > Viewport.Width)
        {
            var width = isLogical ? scrollable!.ScrollSize.Width : ScrollStepSize;
            x += -delta.X * width;
            x = Math.Max(x, 0);
            x = Math.Min(x, Extent.Width - Viewport.Width);
        }

        // Calculate the new offset and apply it to the OffsetTarget property.
        Vector newOffset = new(x, y);

        var offsetChanged = newOffset != OffsetTarget;
        OffsetTarget = newOffset;

        e.Handled = !IsScrollChainingEnabled || offsetChanged;
    }
}