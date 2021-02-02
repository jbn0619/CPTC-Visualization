using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeData: MonoBehaviour
{
    #region Fields

    private int id;
    private bool isActive;
    private NodeTypes type;

    [SerializeField]
    private List<int> connections;

    [SerializeField]
    private List<LineRenderer> connectionGOS;
    
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
    /// Gets or sets if this node has been shut down or not.
    /// </summary>
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
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

    /// <summary>
    /// Gets a list of gameObjects that represent this node's connections.
    /// </summary>
    public List<LineRenderer> ConnectionGOS
    {
        get
        {
            return connectionGOS;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // How to convert from string to enum:
        //Enum.TryParse(string, out NodeTypes newType);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}