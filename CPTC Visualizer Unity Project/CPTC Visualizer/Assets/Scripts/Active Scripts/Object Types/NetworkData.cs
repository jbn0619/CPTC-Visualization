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
    /// This Network's name in the system
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    private string networkName;
    /// <summary>
    /// This network's IP address on the simulation
    /// </summary>
    private string ip;
    /// <summary>
    /// List of the NodeData Components of the nodes within this network
    /// </summary>
    [SerializeField]
    protected List<NodeData> nodes;
    /// <summary>
    /// Determines if this network is connected to the base Node(VDI) or not
    /// </summary>
    [SerializeField]
    protected bool vdi;

    /// <summary>
    /// Index of this network within the Infra.Networks list
    /// </summary>
    [Header("References")]
    [SerializeField]
    private int index;
    /// <summary>
    /// List of Infra.Networks Indecies of adjacent Networks
    /// </summary>
    [SerializeField]
    private List<int> connections;
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
    /// Gets the string passed by the JSON storing the network's name in the system
    /// </summary>
    public string NetworkName
    {
        get
        {
            return this.networkName;
        }
    }
    /// <summary>
    /// Gets this network's IP address within the simulation
    /// </summary>
    public string Ip
    {
        get { return this.ip; }
    }
    /// <summary>
    /// Gets if this network has a connection to the VDI Network or not
    /// </summary>
    public bool VDI
    {
        get { return this.vdi; }
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
    /// Gets and Sets the Index of this netwrok object within the Infra's list of networks.
    /// </summary>
    public int Index
    {
        get { return this.index; }
        set { this.index = value; }
    }
    /// <summary>
    /// Gets a list of the Infra.Networks index of networks connected to this network
    /// </summary>
    public List<int> Connections
    {
        get
        {
            return connections;
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
    /// <param name="_name">This network's name in the simulation</param>
    /// <param name="_nodes"> list of nodes within this network</param>
    /// <param name="_visibleToStart">determines if the Network is visible to the VDI network</param>
    public void SetData(string _name, string _ip, List<NodeData> _nodes, bool _visibleToStart)
    {
        this.networkName = _name;
        this.ip = _ip;
        this.nodes = _nodes;
        this.vdi = _visibleToStart;
        // this.connections = _connections;
    }
    public NetworkData DeepCopy()
    {
        NetworkData returnNet = new NetworkData();
        List<NodeData> nodeCopies = new List<NodeData>();
        foreach(NodeData node in nodes)
        {
            nodeCopies.Add(node.DeepCopy());
        }
        string vdiString = vdi.ToString();
        returnNet.SetData(String.Copy(networkName),String.Copy(ip),nodeCopies, bool.Parse(vdiString));
        return returnNet;
    }

    /// <summary>
    /// Replaces the NodeData at the given index
    /// </summary>
    /// <param name="_node">NodeData being passed in</param>
    /// <param name="_index">index of the NetworkData.Nodes list to replace</param>
    public void AddNodeData(NodeData _node, int _index)
    {
        nodes[_index] = _node;
    }

    /// <summary>
    /// Determine if this node will have a connection with the passed node
    /// </summary>
    /// <param name="_net">Node being checked for adjacency with the current node</param>
    /// <returns>True = network is connected / False = network not connected</returns>
    public bool IsAdjacentTo(NetworkData _net)
    {
        // Check If both networks are open to the VDI
        if(vdi && _net.vdi)
        {
            return true;
        } // Check If neither network is the VDI network
        else if(networkName!="vdi" && _net.NetworkName != "vdi")
        {
            return true;
        }
        return false;
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
