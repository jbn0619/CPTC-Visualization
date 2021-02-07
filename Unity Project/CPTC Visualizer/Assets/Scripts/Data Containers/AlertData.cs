using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Used to contain data from the alerts read in
/// Uses an interface that allows the data to be stored into the priority queues
/// </summary>
public class AlertData : MonoBehaviour, IPriorityEvent
{
    #region Fields

    private CPTCEvents type;

    [SerializeField]
    private List<int> affectedNodes;

    private int team;
    private int timestamp;
    private int priority;
    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets what kind of event this alert is.
    /// </summary>
    public CPTCEvents Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    /// <summary>
    /// Gets a list of all nodes this alert/event has influenced.
    /// </summary>
    public List<int> AffectedNodes
    {
        get
        {
            return affectedNodes;
        }
    }

    public int Resultant
    {
        get { return timestamp / (priority * 5); }
    }

    public int Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    // probably going to be deleted
    public int EventID => throw new NotImplementedException();

    public int Team
    {
        get { return team; }
        set { team = value; }
    }

    public int Timestamp
    {
        get { return timestamp; }
        set { timestamp = value; }
    }

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        // How to convert from string to enum:
        //Enum.TryParse(string, out CPTCEvents newEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
