using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel - Summer 2021
/// Function: A data container that is tied to a game object. Represents a network within the infrastructure, which is a grouping of node objects.
/// </summary>
[Serializable]
public class NetworkData: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;
    [SerializeField]
    private List<NodeData> nodes;
    [SerializeField]
    private List<int> connections;

    private bool isActive;
    private float scanTime;
    private bool scanActive;
    private float scanCount;

    [SerializeField]
    protected List<GameObject> nodeObjects;
    [SerializeField]
    protected List<LineRenderer> connectionGOS;

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
    /// Gets or sets if this network has been shut down or not.
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
    /// Gets a list of node objects within this network.
    /// </summary>
    public List<GameObject> NodeObjects
    {
        get
        {
            return nodeObjects;
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

    /// <summary>
    /// Gets a list of gameObjects that represent this network's connections.
    /// </summary>
    public List<LineRenderer> ConnectionGOS
    {
        get
        {
            return connectionGOS;
        }
    }

    /// <summary>
    /// Gets or set if this network is being scanned or not.
    /// </summary>
    public bool ScanActive
    {
        get
        {
            return scanActive;
        }
        set
        {
            scanActive = value;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        scanCount = 0;
        scanActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Check what material we need to look-at.
        if (scanActive)
        {
            scanCount += Time.deltaTime;
            if (scanCount >= scanTime)
            {
                scanActive = false;
                scanCount = 0f;
            }
            outline.material = GeneralResources.Instance.NetScan;
        }
        else
        {
            outline.material = GeneralResources.Instance.NetBase;
        }
        */
    }

    /// <summary>
    /// Sets the basic data of the Network 
    /// </summary>
    /// <param name="_id">This network's int ID to determine connections</param>
    /// <param name="_nodes"> liat of nodes within this network</param>
    /// <param name="_connections">list of int IDs this network is connected to</param>
    public void SetData(int _id, List<NodeData> _nodes, List<int> _connections)
    {
        this.id = _id;
        this.nodes = _nodes;
        this.connections = _connections;
    }

    /// <summary>
    /// Adds the node object to the network's list of node objects if the passed object has a NodeData with an ID on the network's list
    /// </summary>
    /// <param name="_node">node gameObject to be added to network's nodeObjects list</param>
    public void AddNodeObject(GameObject _node)
    {
        if(_node.GetComponent<NodeData>())
        {
            int searchID = _node.GetComponent<NodeData>().Id;
            foreach (NodeData n in this.nodes)
            {
                if (n.Id == searchID)
                {
                    nodeObjects.Add(_node);
                }
            }
        }
        else
        {
            //is not a Node object
        }
    }
}
