using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MITREEvent: IChainable
{
    #region Fields
    private DateTime timestamp;
    private string location;
    private int team;
    private string eventName;
    #endregion Fields

    #region Properties
    public DateTime Timestamp { get { return timestamp; } }

    public string Location { get { return location; } }

    public int Team { get { return team; } }

    public string EventName { get { return eventName; } }
    #endregion Properties

    public MITREEvent(string name, string eventLocation, int teamNum, DateTime time)
    {
        eventName = name;
        location = eventLocation;
        team = teamNum;
        timestamp = time;
    } 
}
