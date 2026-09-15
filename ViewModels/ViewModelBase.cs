using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace FullYearProject.ViewModels;

/// <summary>
///     Base class for view models.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    /// <summary>
    ///     The logger for the view model.
    /// </summary>
    protected ILogger Logger => field ??= Logging.Logger.Create(GetType());
}