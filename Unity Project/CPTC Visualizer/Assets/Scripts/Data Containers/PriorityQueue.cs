using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    #region Fields
    private List<IPriorityEvent> queue;

    #endregion Fields

    #region Properties
    /// <summary>
    /// Returns the first event in the queue
    /// </summary>
    public IPriorityEvent Peek
    {
        get { return queue[0]; }
    }

    /// <summary>
    /// Returns the number of elements in the queue
    /// </summary>
    public int Size
    {
        get { return queue.Count; }
    }

    /// <summary>
    /// Returns a boolean whether the queue is empty
    /// </summary>
    public bool IsEmpty
    {
        get { return Size > 0 ? true : false; }
    }
    #endregion Properties

    /// <summary>
    /// Default Constrctor for the queue
    /// </summary>
    public PriorityQueue()
    {
        queue = new List<IPriorityEvent>();
    }

    /// <summary>
    /// Removes the first element in the queue
    /// </summary>
    /// <returns>The first element in the queue</returns>
    public IPriorityEvent Pop()
    {
        IPriorityEvent temp = queue[0];

        queue.Remove(temp);

        return temp;
    }

    /// <summary>
    /// Adds new data to the list prioritized by resultant
    /// </summary>
    /// <param name="_data">New Data</param>
    public void Push(IPriorityEvent _data)
    {
        int count = queue.Count;
        for (int i = 0; i < count; i++)
        {
            if (_data.Resultant > queue[i].Resultant)
            {
                queue.Insert(i, _data);
                return;
            }
        }
    }

    /// <summary>
    /// Removes data that becomes obsolete
    /// </summary>
    public void Sanitize()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            // insert delete code here
        }
    }
}
