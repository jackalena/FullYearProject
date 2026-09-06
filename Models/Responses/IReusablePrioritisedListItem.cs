namespace FullYearProject.Models.Responses;

public interface IReusablePrioritisedListItem
{
    /// <summary>
    ///     The number of times this item can be reused.
    /// </summary>
    public int ReuseCount { get; }

    /// <summary>
    ///     The number of times this item has already been reused.
    /// </summary>
    public int CurrentReuseCount { get; set; }
}