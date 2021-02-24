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
    #endregion Properties
    
    // Constructor for injuects
    public Inject(string _name, string _description, string _timestamp)
    {
        name = _name;
        description = _description;
        timestamp = _timestamp;
    }
}
