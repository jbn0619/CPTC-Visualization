using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CCDCAlertData: AlertData
{
    #region Fields

    private CCDCAttackType attackType;
    private DateTime startTime;
    
    #endregion Fields
    
    #region Properties

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
