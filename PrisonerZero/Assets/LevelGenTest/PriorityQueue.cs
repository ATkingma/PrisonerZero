using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private SortedDictionary<int, Queue<T>> dict = new SortedDictionary<int, Queue<T>>();
    private int count = 0;

    public int Count => count;

    public void Enqueue(T item, int priority)
    {
        if (!dict.ContainsKey(priority))
        {
            dict[priority] = new Queue<T>();
        }
        dict[priority].Enqueue(item);
        count++;
    }

    public T Dequeue()
    {
        if (count == 0)
            throw new InvalidOperationException("Queue is empty");

        var highestPriorityQueue = dict.First().Value;
        T item = highestPriorityQueue.Dequeue();
        if (highestPriorityQueue.Count == 0)
            dict.Remove(dict.First().Key);
        count--;
        return item;
    }

    public bool Contains(T item)
    {
        foreach (var queue in dict.Values)
        {
            if (queue.Contains(item))
                return true;
        }
        return false;
    }

    public void Clear()
    {
        dict.Clear();
        count = 0;
    }
}
