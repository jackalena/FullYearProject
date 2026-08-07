using System;
using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Responses;

public class PrioritisedList<TPriority, TValue> where TPriority : struct
{
    private readonly Dictionary<TPriority, List<TValue>> _lists = new();

    public PrioritisedList(IEnumerable<TPriority> alowedPriorities, TPriority? resetOnAllKey)
    {
        AlowedPriorities = alowedPriorities.ToArray();
        AlowedPriorities.Sort();

        ResetOnAllKey = resetOnAllKey;

        foreach (var priority in AlowedPriorities)
        {
            _lists[priority] = [];
        }
    }

    public TPriority[] AlowedPriorities { get; }
    public TPriority? ResetOnAllKey { get; set; }

    public void Add(TValue value, TPriority priority = default)
    {
        _lists[priority].Add(value);
    }

    public TValue GetNext()
    {
        var orderedKeys = _lists.Keys
            .Order().Select(k => (TPriority?)k).ToArray();

        // Find the key of the first list with items, starting with the lowest priority index
        var key = orderedKeys.FirstOrDefault(k => _lists[k!.Value].Count > 0, null);

        // There were no lists with any items; ie. the whole PrioritisedList is empty.
        if (key == null)
        {
            throw new InvalidOperationException("List is empty.");
        }

        // If the selected list is the one in ResetOnAllKey, reset the list and return the next item.
        if (key.Value.Equals(ResetOnAllKey))
        {
            Reset();
            return GetNext();
        }

        var list = _lists[key.Value];
        var newListKey = orderedKeys.SkipWhile(k => !k.Equals(key.Value)).ElementAtOrDefault(1);

        var idx = Random.Shared.Next(list.Count);
        var value = list[idx];

        if (newListKey != null)
        {
            list.RemoveAt(idx);
            _lists[newListKey.Value].Add();
        }

        return value;
    }

    public void Reset(TPriority newPriority = default)
    {
        _lists[newPriority] ??= [];

        foreach (var key in _lists.Keys)
        {
            if (newPriority.Equals(key))
            {
                continue;
            }

            var oldList = _lists[key];
            _lists[newPriority].AddRange(oldList);
            oldList.Clear();
        }
    }
}