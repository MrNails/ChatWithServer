using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SimpleChatServer.DataStructures;

public sealed class ReadOnlyHashSet<T> :
    IReadOnlySet<T>
{
    private readonly HashSet<T> m_hashSet;

    public ReadOnlyHashSet(HashSet<T> hashSet)
    {
        if (hashSet == null)
            throw new ArgumentNullException(nameof(hashSet));

        m_hashSet = hashSet;
    }

    public int Count => m_hashSet.Count;

    public IEnumerator<T> GetEnumerator()
    {
        return m_hashSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Contains(T item) => m_hashSet.Contains(item);

    public bool IsProperSubsetOf(IEnumerable<T> other) => m_hashSet.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => m_hashSet.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => m_hashSet.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => m_hashSet.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => m_hashSet.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => m_hashSet.SetEquals(other);
}