using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Author: Justin Neft
/// Function: Used in CCDC, this data container represents an alert that has happened in the competition.
/// </summary>
public class AlertData: MonoBehaviour, IPriorityEvent
{
    #region Fields

    protected CPTCEvents type;

    [SerializeField]
    protected List<int> affectedNodes;

    protected int team;
    protected int timestamp;
    protected int priority;

    private CCDCAttackType attackType;
    private DateTime startTime;

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

    /// <summary>
    /// Gets or sets what kind of CCDC attack this alert represents.
    /// </summary>
    public CCDCAttackType AttackType
    {
        get
        {
            return attackType;
        }
        set
        {
            attackType = value;
        }
    }

    /// <summary>
    /// Gets or sets this attack's startTime.
    /// </summary>
    public DateTime StartTime
    {
        get
        {
            return startTime;
        }
        set
        {
            startTime = value;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
