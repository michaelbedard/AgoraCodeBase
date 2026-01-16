using System.Collections;

namespace Domain.Utility;

public class ConcurrentList<T> : IEnumerable<T>
{
    private readonly List<T> _list;
    private readonly object _lock = new object();

    public ConcurrentList()
    {
        _list = new List<T>();
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            _list.Add(item);
        }
    }

    public bool Remove(T item)
    {
        lock (_lock)
        {
            return _list.Remove(item);
        }
    }

    public T this[int index]
    {
        get
        {
            lock (_lock)
            {
                return _list[index];
            }
        }
        set
        {
            lock (_lock)
            {
                _list[index] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }
    }

    public bool Contains(T item)
    {
        lock (_lock)
        {
            return _list.Contains(item);
        }
    }

    public T[] ToArray()
    {
        lock (_lock)
        {
            return _list.ToArray();
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _list.Clear();
        }
    }

    public IEnumerable<T> GetAll()
    {
        lock (_lock)
        {
            return _list.ToList(); // Return a copy of the list to avoid modification outside the lock
        }
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        lock (_lock)
        {
            // Return a copy of the list's enumerator to avoid modification issues during iteration
            return _list.ToList().GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
