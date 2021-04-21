using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkData: MonoBehaviour
{
    #region Fields

    private int id;
    private bool isActive;
    private bool scanActive;
    [SerializeField]
    private float scanTime;
    private float scanCount;

    [SerializeField]
    private List<NodeData> nodes;

    [SerializeField]
    private List<int> connections;

    [SerializeField]
    private List<LineRenderer> connectionGOS;

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
}
