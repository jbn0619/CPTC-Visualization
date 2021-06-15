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
    /// <summary>
    /// This Network's ID number
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    private int id;
    /// <summary>
    /// List of ID numbers of the nodes within this Network
    /// </summary>
    [SerializeField]
    private List<int> nodeIDs;
    /// <summary>
    /// List of ID numbers of adjacent Networks
    /// </summary>
    [SerializeField]
    private List<int> connections;

    /// <summary>
    /// List of the Node GameObjects childed to this object
    /// </summary>
    [Header("Script References")]
    [SerializeField]
    protected List<GameObject> nodeObjects;
    /// <summary>
    /// List of the NodeData Components of the nodes within this network
    /// </summary>
    [SerializeField]
    protected List<NodeData> nodes;
    /// <summary>
    /// List of the Linerenderers used to draw the connections between this network and other networks
    /// </summary>
    [SerializeField]
    protected List<LineRenderer> connectionGOS;

    // Legacy Fields
    private bool isActive;
    private float scanTime;
    private bool scanActive;
    private float scanCount;
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
    public List<int> NodeIDs
    {
        get
        {
            return nodeIDs;
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
    /// Get a list of the data of all nodes within the simulated Network
    /// </summary>
    public List<NodeData> Nodes
    {
        get
        {
            return this.nodes;
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
    /// <param name="_nodes"> list of nodes within this network</param>
    /// <param name="_connections">list of int IDs this network is connected to</param>
    public void SetData(int _id, List<int> _nodes, List<int> _connections)
    {
        this.id = _id;
        this.nodeIDs = _nodes;
        this.connections = _connections;
    }

    public void AddNodeData(NodeData _node)
    {
        int searchID = _node.Id;
        foreach (int nodeID in this.nodeIDs)
        {
            if (nodeID == searchID)
            {
                nodes.Add(_node);
                break;
            }
        }
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
            foreach (int nodeID in this.nodeIDs)
            {
                if (nodeID == searchID)
                {
                    nodeObjects.Add(_node);
                    break;
                }
            }
        }
        else
        {
            //is not a Node object
        }
    }

    /* Phased out because we are using an in-between class to transfer data from the fileReader to the JSON files 
     * /// <summary>
    /// Stores the data of the refernced objects within the Network, rather than the instances of the objects
    /// </summary>
    /// <returns>Sting of JSON formatted data</returns>
    public string ConvertToJSON()
    {
        string dataString = $"{{\"id\":{this.id},";
        dataString += "\"nodes\":[";
        for (int i = 0; i < this.nodes.Count; i++)
        {
            dataString += $"{JsonUtility.ToJson(this.nodes[i])}";
            if (i < this.nodes.Count - 1) { dataString += ","; }
        }
        dataString += "],\"connections\":[";
        dataString += JsonUtility.ToJson(this.connections);
        dataString += "]}";
        Debug.Log(dataString);
        return dataString;
    }*/
}
