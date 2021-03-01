using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeData: MonoBehaviour
{
    #region Fields

    private int id;
    private string ip;
    private bool isActive;
    private NodeTypes type;
    private NodeState state;
    private bool isHidden;

    [SerializeField]
    private List<int> connections;

    [SerializeField]
    private List<LineRenderer> connectionGOS;

    [SerializeField]
    private SpriteRenderer nodeSprite;
    
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
    /// Gets or sets this node's ip address.
    /// </summary>
    public string Ip
    {
        get
        {
            return ip;
        }
        set
        {
            ip = value;
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
    /// Gets or sets if this node is hidden from view or not.
    /// </summary>
    public bool IsHidden
    {
        get
        {
            return isHidden;
        }
        set
        {
            isHidden = value;
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
    /// Gets or sets what this node's state is.
    /// </summary>
    public NodeState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
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

    /// <summary>
    /// Gets this node's sprite renderer.
    /// </summary>
    public SpriteRenderer NodeSprite
    {
        get
        {
            return nodeSprite;
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
