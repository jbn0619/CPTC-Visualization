using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData: MonoBehaviour
{
    #region Fields

    private int id;
    private NodeTypes type;
    private List<int> connections;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets or sets this node's id.
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
    /// Gets or sets what type of node this is.
    /// </summary>
    public NodeTypes Type
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
    /// Gets a list of node ids this node has connections to.
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
        connections = new List<int>();

        // How to convert from string to enum:
        // Enum.TryParse(sring, out myEnumType myType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
