using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertData: MonoBehaviour
{
    #region Fields

    private CPTCEvents type;
    
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
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // How to convert from string to enum:
        // Enum.TryParse(sring, out myEnumType myType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
