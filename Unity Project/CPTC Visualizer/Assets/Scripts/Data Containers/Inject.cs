using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Inject
{
    #region Fields
    private string timestamp;
    private string name;
    private string description;
    private float duration;
    #endregion Fields

    #region Properties
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public string Timestamp
    {
        get { return timestamp; }
    }

    public float Duration
    {
        get { return duration; }
    }
    #endregion Properties
    
    // Constructor for injuects
    public Inject(string _name, string _description, string _timestamp, float _estTime)
    {
        name = _name;
        description = _description;
        timestamp = _timestamp;
        duration = _estTime;
    }
}
