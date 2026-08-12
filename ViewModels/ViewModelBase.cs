using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace FullYearProject.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    protected ILogger Logger => field ??= Logging.Logger.Create(GetType());
}