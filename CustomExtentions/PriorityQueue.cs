using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    public List<PriorityQueueElement<T>> queue;
    public int Count { get { return queue.Count; } }

    public PriorityQueue()
    {
        queue = new List<PriorityQueueElement<T>>();
    }

    public void Insert(T item, float priority)
    {
        queue.Add(new PriorityQueueElement<T>(item, priority));
        queue.Sort((x, y) => { return x.priority.CompareTo(y.priority); });
    }

    public bool Remove(T item)
    {
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].item.Equals(item))
            {
                queue.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public T GetFirstItemInQueue()
    {
        return queue[0].item;
    }

    public T RetrieveFirstItemInQueue()
    {
        T ret = queue[0].item;
        queue.RemoveAt(0);
        return ret;
    }

    public delegate bool ItemCriteria(T item);
    public T[] GetItemsWith(ItemCriteria check)
    {
        List<T> l = new List<T>();
        for (int i = 0; i < queue.Count; i++)
        {
            if (check(queue[i].item))
            {
                l.Add(queue[i].item);
            }
        }
        return l.ToArray();
    }

    public T[] RetrieveItemsWith(ItemCriteria check)
    {
        T[] a = GetItemsWith(check);
        for (int i = 0; i < a.Length; i++)
        {
            Remove(a[i]);
        }
        return a;
    }
}

public class PriorityQueueElement<T>
{
    public T item;
    public float priority;
    public PriorityQueueElement(T item, float priority)
    {
        this.item = item;
        this.priority = priority;
    }
}
