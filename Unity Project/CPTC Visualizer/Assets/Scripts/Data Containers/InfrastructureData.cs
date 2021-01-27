using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureData: MonoBehaviour
{
    #region Fields

    private List<NetworkData> networks;
    private List<NodeData> allNodes;

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

    /// <summary>
    /// Gets a list of ALL nodes in all networks in this infrastructure.
    /// </summary>
    public List<NodeData> AllNodes
    {
        get
        {
            return allNodes;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // Initailize fields.
        networks = new List<NetworkData>();
        allNodes = new List<NodeData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
