using Avalonia;
using Avalonia.Controls;

namespace FullYearProject.Controls.AutoSelectComboBox;

/// <summary>
///     <inheritdoc />
///     Automatically selects the first item when items are added.
/// </summary>
public class AutoSelectComboBox : ComboBox
{
    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ItemCountProperty)
        {
            // If the number of items changes, and there is no selected item, select the first item.
            if (change.NewValue is > 0 && SelectedIndex == -1)
            {
                Dispatcher.Post(() => SelectedIndex = 0);
            }
        }
    }
}