using CommunityToolkit.Mvvm.ComponentModel;

namespace FullYearProject.ViewModels;

public partial class SelectableItemViewModel<T> : ViewModelBase, ISelectableItem
{
    /// <summary>
    ///     The item this class holds.
    /// </summary>
    [ObservableProperty]
    public partial T Item { get; set; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="SelectableItemViewModel{T}" /> class using the specified item.
    ///     The item will be initialised with IsSelected = false.
    /// </summary>
    /// <param name="item">The item this class holds.</param>
    public SelectableItemViewModel(T item)
    {
        Item = item;
    }

    /// <inheritdoc />
    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}

/// <summary>
///     Represents an item that can be selected.
/// </summary>
public interface ISelectableItem
{
    /// <summary>
    ///     Whether the item is selected.
    /// </summary>
    bool IsSelected { get; }
}