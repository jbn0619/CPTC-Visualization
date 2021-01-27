using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkData: MonoBehaviour
{
    #region Fields

    private int id;
    private List<NodeData> nodes;
    private List<int> connections;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets or sets this network's id (seperate from node Ids).
    /// </summary>
    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            if (value >= 0)
            {
                id = value;
            }
        }
    }

    /// <summary>
    /// Gets a list of nodes within this network.
    /// </summary>
    public List<NodeData> Nodes
    {
        get
        {
            return nodes;
        }
    }

    /// <summary>
    /// Gets a list of connections between this network and other networks.
    /// </summary>
    public List<int> Connections
    {
        get
        {
            return connections;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize fields.
        nodes = new List<NodeData>();
        connections = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
