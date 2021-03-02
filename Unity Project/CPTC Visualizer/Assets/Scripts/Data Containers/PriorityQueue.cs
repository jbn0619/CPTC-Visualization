using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    #region Fields
    [SerializeField]
    private List<IPriorityEvent> queue;

    private int LatestTime;
    #endregion Fields

    #region Properties
    /// <summary>
    /// Returns the first event in the queue
    /// </summary>
    public IPriorityEvent Peek
    {
        get { return IsEmpty ? null : queue[0]; }
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
        get { return Size > 0 ? false : true; }
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
        // adds to the first spot if empty
        if (Size == 0)
        {
            queue.Add(_data);
            LatestTime = _data.Timestamp;
            return;
        }

        // loop that orders based on resultant
        int count = queue.Count;
        for (int i = 0; i < count; i++)
        {
            // higher resultant means further in queue
            if (_data.Resultant > queue[i].Resultant)
            {
                queue.Insert(i, _data);
                LatestTime = _data.Timestamp;
                return;
            }
        }

        // fallback if it belongs at end
        queue.Add(_data);
        LatestTime = _data.Timestamp;
    }

    /// <summary>
    /// Removes data that becomes obsolete
    /// </summary>
    public void Sanitize()
    {
        // loop that cuts out data that's uneccesary
        for (int i = 0; i < queue.Count; i++)
        {
            // insert delete code here
        }
    }
}
