using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlertData: MonoBehaviour
{
    #region Fields

    private CPTCEvents type;

    [SerializeField]
    private List<int> affectedNodes;
    
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
