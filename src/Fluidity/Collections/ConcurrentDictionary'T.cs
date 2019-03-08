// <copyright file="ConcurrentDictionary'T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace Fluidity.Collections
{
    // Wrapper around the standard ConcurrentDictionary<,> that explicityly implements IReadOnlyDictonary<,> interface
    // This is implemented in .NET 6.4, but Umbraco currently targets .NET 4.5.2 so we implement it our selves for now
    public class ConcurrentDictionary<TKey, TValue> : System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => base[key];

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => base.Count;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => base.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => base.Values;

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return base.ContainsKey(key);
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return base.TryGetValue(key, out value);
        }
    }
}
