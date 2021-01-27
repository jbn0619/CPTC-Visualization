using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureData: MonoBehaviour
{
    #region Fields

    private List<NetworkData> networks;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of all networks within this infrastructure.
    /// </summary>
    public List<NetworkData> Networks
    {
        get
        {
            return networks;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // Initailize fields.
        networks = new List<NetworkData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
