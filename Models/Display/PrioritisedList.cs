using System;
using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Display;

/// <summary>
///     A list that allows prioritising items. Items are returned in the order of their priority, and items with the same
///     priority are returned in a random order.
/// </summary>
/// <typeparam name="TPriority">The type of the possible priority values.</typeparam>
/// <typeparam name="TValue">The type of the list's items.</typeparam>
public class PrioritisedList<TPriority, TValue> where TPriority : struct
{
    private readonly List<TValue>[] _lists;

    /// <summary>
    ///     Creates a new PrioritisedList using the provided allowed priorities and reset priority.
    /// </summary>
    /// <param name="allowedPriorities">A list of priorities that can be assigned to items in the list.</param>
    /// <param name="resetOnAllPriority">
    ///     <inheritdoc cref="ResetOnAllPriority" path="/summary/node()" />
    /// </param>
    public PrioritisedList(IEnumerable<TPriority> allowedPriorities, TPriority? resetOnAllPriority)
    {
        AllowedPriorities = allowedPriorities.ToArray();
        AllowedPriorities.Sort();

        ResetOnAllPriority = resetOnAllPriority;

        // Create a list for each priority to store items of that priority
        _lists = new List<TValue>[AllowedPriorities.Length];
        for (var i = 0; i < _lists.Length; i++)
        {
            _lists[i] = [];
        }
    }

    /// <summary>
    ///     The possible priorities for items in the list.
    /// </summary>
    public TPriority[] AllowedPriorities { get; }

    /// <summary>
    ///     If not null, the list will reset if all items have a priority equal to this value.
    /// </summary>
    public TPriority? ResetOnAllPriority { get; set; }

    /// <summary>
    ///     Creates a new PrioritisedList using the values of an enum as the possible priorities.
    /// </summary>
    /// <param name="resetOnAllPriority">
    ///     <inheritdoc cref="ResetOnAllPriority" path="/summary/node()" />
    /// </param>
    /// <typeparam name="TEnum">The type of the enum used for possible priorities.</typeparam>
    /// <returns>The generated <see cref="PrioritisedList{TPriority,TValue}" /> instance</returns>
    public static PrioritisedList<TEnum, TValue> FromEnum<TEnum>(TEnum? resetOnAllPriority = null)
        where TEnum : struct, Enum
    {
        return new(Enum.GetValues<TEnum>(), resetOnAllPriority);
    }

    /// <summary>
    ///     Adds an item to the list with the given priority.
    /// </summary>
    /// <param name="value">The item to add to the list.</param>
    /// <param name="priority">The priority to give the added item.</param>
    public void Add(TValue value, TPriority priority = default)
    {
        var idx = AllowedPriorities.IndexOf(priority);
        _lists[idx].Add(value);
    }

    /// <summary>
    ///     Adds the elements of a specified collection to the list with the given priority.
    /// </summary>
    /// <param name="values">The items to add to the list.</param>
    /// <param name="priority">The priority to give the added items.</param>
    public void AddRange(IEnumerable<TValue> values, TPriority priority = default)
    {
        var idx = AllowedPriorities.IndexOf(priority);
        _lists[idx].AddRange(values);
    }

    /// <summary>
    ///     Removes the first occurrence of a specific object from the list.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the item was successfully removed; otherwise False.</returns>
    public bool Remove(TValue value)
    {
        return _lists.Any(list => list.Remove(value));
    }

    /// <summary>
    ///     Determines whether the list contains a specific value.
    /// </summary>
    /// <param name="value">The value to find in the list.</param>
    /// <returns>True if the value was found in the list; otherwise false.</returns>
    public bool Contains(TValue value)
    {
        return _lists.Any(list => list.Contains(value));
    }

    /// <summary>
    ///     Gets the priority of an item in the list.
    /// </summary>
    /// <param name="value">The item to get the priority of.</param>
    /// <returns>The priority of the item in <paramref name="value" />.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the item provided in <paramref name="value" /> does not exist in
    ///     the list.
    /// </exception>
    public TPriority GetPriority(TValue value)
    {
        for (var i = 0; i < AllowedPriorities.Length; i++)
        {
            if (_lists[i].Contains(value))
            {
                return AllowedPriorities[i];
            }
        }

        throw new InvalidOperationException("Value not found in list.");
    }

    /// <summary>
    ///     Sets the priority of an item in the list.
    /// </summary>
    /// <param name="value">The item to set the priority of.</param>
    /// <param name="priority">The priority to set the item to.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the item provided in <paramref name="value" /> does not exist in
    ///     the list.
    /// </exception>
    public void SetPriority(TValue value, TPriority priority)
    {
        for (var i = 0; i < AllowedPriorities.Length; i++)
        {
            if (_lists[i].Remove(value))
            {
                _lists[AllowedPriorities.IndexOf(priority)].Add(value);

                return;
            }
        }

        throw new InvalidOperationException("Value not found in list.");
    }

    /// <summary>
    ///     Gets the next item in the list and moves it to the next priority level.
    /// </summary>
    /// <returns>The next item.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the list is empty.</exception>
    public PrioritisedListItem<TPriority, TValue> GetNext()
    {
        // Find the key of the first list with items
        var priorityIdx = _lists.TakeWhile(l => l.Count == 0).Count();

        // There were no lists with any items; ie. the whole PrioritisedList is empty.
        if (priorityIdx == _lists.Length)
        {
            throw new InvalidOperationException("List is empty.");
        }

        // If the selected list is the one in ResetOnAllKey, reset the list and return the next item.
        if (AllowedPriorities[priorityIdx].Equals(ResetOnAllPriority))
        {
            Reset();

            return GetNext();
        }

        // Choose a random item from the list
        var list = _lists[priorityIdx];
        var listIdx = Random.Shared.Next(list.Count);
        var item = list[listIdx];

        var listItem = new PrioritisedListItem<TPriority, TValue>(this, item);

        // Once the item is used, move it to the next list.
        listItem.OnCompleted += requestedPriority =>
        {
            // If the item is reusable, increment its reuse counter or move it to the next list if it has been reused enough times.
            if (item is IReusablePrioritisedListItem { ReuseCount: > 0 } reusableItem)
            {
                if (reusableItem.ReuseCount - reusableItem.CurrentReuseCount >= 0)
                {
                    reusableItem.CurrentReuseCount++;
                }
                else
                {
                    MoveItem(requestedPriority);
                    reusableItem.CurrentReuseCount = 0;
                }
            }
            else
            {
                MoveItem(requestedPriority);
            }
        };

        return listItem;

        // Moves the item to the next list
        void MoveItem(TPriority newPriority)
        {
            var newPriorityIdx = AllowedPriorities.IndexOf(newPriority);

            if (newPriorityIdx != priorityIdx)
            {
                list.RemoveAt(listIdx);
                _lists[newPriorityIdx].Add(item);
            }
        }
    }

    /// <summary>
    ///     Resets the list by changing all items to the default priority.
    /// </summary>
    /// <param name="newPriority">The new priority to give all items</param>
    public void Reset(TPriority newPriority = default)
    {
        var newPriorityIdx = AllowedPriorities.IndexOf(newPriority);

        for (var i = 0; i < AllowedPriorities.Length; i++)
        {
            if (AllowedPriorities[i].Equals(newPriorityIdx))
            {
                continue;
            }

            var oldList = _lists[i];
            _lists[newPriorityIdx].AddRange(oldList);
            oldList.Clear();
        }
    }

    /// <summary>
    ///     Removes all items from the list.
    /// </summary>
    public void Clear()
    {
        for (var i = 0; i < AllowedPriorities.Length; i++)
        {
            _lists[i].Clear();
        }
    }
}

/// <summary>
///     Represents an item in a <see cref="PrioritisedList{TPriority,TValue}" />.
/// </summary>
/// <typeparam name="TPriority">The type of the possible priority values.</typeparam>
/// <typeparam name="TValue">The type of the list's items.</typeparam>
public class PrioritisedListItem<TPriority, TValue> where TPriority : struct
{
    internal PrioritisedListItem(PrioritisedList<TPriority, TValue> ownerList, TValue value)
    {
        OwnerList = ownerList;
        Value = value;
    }

    /// <summary>
    ///     The value stored in the item.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    ///     The list that this item belongs to.
    /// </summary>
    public PrioritisedList<TPriority, TValue> OwnerList { get; }

    /// <summary>
    ///     Moves the item to the specified priority.
    /// </summary>
    /// <param name="priority">The priority to move the item to.</param>
    public void MoveToPriority(TPriority priority)
    {
        OnCompleted?.Invoke(priority);
    }

    internal event Action<TPriority>? OnCompleted;
}