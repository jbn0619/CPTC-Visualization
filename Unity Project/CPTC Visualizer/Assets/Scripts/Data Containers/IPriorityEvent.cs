using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPriorityEvent
{
    #region Properties
    /// <summary>
    /// The resultant used to determine priority
    /// </summary>
    int Resultant
    {
        get;
    }

    /// <summary>
    /// int that represents priority, lower number is higher priority
    /// </summary>
    int Priority
    {
        get;
    }

    /// <summary>
    /// The id of the event, represents the type
    /// </summary>
    int EventID
    {
        get;
    }

    /// <summary>
    /// Int that represents the team the event has occured to
    /// </summary>
    int Team
    {
        get;
    }

    /// <summary>
    /// Returns an int value representing the time of which
    /// and event has occured
    /// </summary>
    int Timestamp
    {
        get;
    }
    #endregion Properties

    #region Methods

    #endregion Methods
}
