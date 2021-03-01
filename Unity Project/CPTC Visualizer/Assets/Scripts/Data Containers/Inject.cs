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
    /// <summary>
    /// Name of the inject
    /// </summary>
    public string Name
    {
        get { return name; }
    }

    /// <summary>
    /// String description for the inject
    /// </summary>
    public string Description
    {
        get { return description; }
    }

    /// <summary>
    /// Time that the inject starts
    /// </summary>
    public string Timestamp
    {
        get { return timestamp; }
    }

    /// <summary>
    /// Time that the inject lasts
    /// </summary>
    public float Duration
    {
        get { return duration; }
    }
    #endregion Properties
    
    // Constructor for injects
    public Inject(string _name, string _description, string _timestamp, float _estTime)
    {
        name = _name;
        description = _description;
        timestamp = _timestamp;
        duration = _estTime;
    }
}
