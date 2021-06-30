using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel & Garrett Paradis - Summer 2021
/// Function: A data container that is tied to a game object in the unity scene. Contains information for a single "node" within an infrastructure.
/// </summary>
[Serializable]
public class NodeData: MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The IP address of this node's access to the simulation
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    protected string ip;
    /// <summary>
    /// Name of this node's Machine on the Infrastructure
    /// </summary>
    [SerializeField]
    protected string hostName;
    /// <summary>
    /// description of the host's purpose on the Infrastructure
    /// </summary>
    [SerializeField]
    protected string hostDescription;
    /// <summary>
    /// Store enumeration of the node's host Operating System
    /// </summary>
    [SerializeField]
    protected OperatingSystems os;
    /// <summary>
    /// index of this node within Infra.AllNodes
    /// </summary>
    [SerializeField]
    protected int index;
    /// <summary>
    /// A list of Infra.AllNodes indecies for adjacent Nodes
    /// </summary>
    [SerializeField]
    protected List<int> connections;
    /// <summary>
    /// Linerenderers used to draw connections between adjacent Nodes
    /// </summary>
    [Header("Script References")]
    [SerializeField]
    protected List<LineRenderer> connectionGOS;
    /// <summary>
    /// reference to this object's SpriteRenderer
    /// </summary>
    [SerializeField]
    protected SpriteRenderer nodeSprite;
    /// <summary>
    /// A list of the teams with currently accessing this node
    /// </summary>
    [SerializeField]
    protected List<TeamData> teams;
    private float value;
    public List<Color> wedgeColors;
    public Image wedgePrefab;
    protected List<Image> wedges;
    protected Vector3 pos;
    /// <summary>
    /// A list of Infrastructure.teams indexes for teams currently accessing this node
    /// </summary>
    [SerializeField]
    protected List<int> teamIDs;

    // Legacy Fields
    private UptimeChartData uptimeChart;
    /// <summary>
    /// The type of simulated computer system this node is
    /// </summary>
    protected NodeTypes type;
    /// <summary>
    /// The current level of functionality of this node
    /// </summary>
    protected NodeState state;
    /// <summary>
    /// A boolean to track if this node is hidden in the system
    /// </summary>
    protected bool isHidden;
    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets this node's index within the Infra.AllNodes list
    /// </summary>
    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            if (value >= 0)
            {
                index = value;
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
    /// Gets the name of this node's host
    /// </summary>
    public string HostName
    {
        get { return this.hostName; }
    }
    /// <summary>
    /// Gets a short description of what this node does within the overall system
    /// </summary>
    public string HostDescription
    {
        get { return this.hostDescription; }
    }
    /// <summary>
    /// Gets this node's Operating System
    /// </summary>
    public OperatingSystems OS
    {
        get { return this.os; }
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
            ChangeSprite();
        }
    }

    /// <summary>
    /// Gets or sets what this node's state is. This will probably be called once per Tick. Make sure calls are only referencing the nodes you need.
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
    /// Get and sets a list of node indecies this node has connections to within Infra.AllNodes
    /// </summary>
    public List<int> Connections
    {
        get
        {
            return connections;
        }
        set
        {
            connections = value;
        }
    }

    /// <summary>
    /// Gets and Sets a list of id numbers for the teams currently at this node.
    /// </summary>
    public List<int> TeamIDs
    {
        get
        {
            return this.teamIDs;
        }
        set
        {
            this.teamIDs = value;
        }
    }

    /// <summary>
    /// Gets and sets a list of team IDs currently accessing this node
    /// </summary>
    public List<TeamData> Teams
    {
        get
        {
            return teams;
        }
        set
        {
            teams = value;
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
        set
        {
            connectionGOS = value;
        }
    }

    /// <summary>
    /// Gets or sets this node's sprite renderer.
    /// </summary>
    public SpriteRenderer NodeSprite
    {
        get
        {
            return nodeSprite;
        }
        set
        {
            nodeSprite = value;
        }
    }

    /// <summary>
    /// Gets or sets this node's uptime chart.
    /// </summary>
    public UptimeChartData UptimeChart
    {
        get
        {
            return uptimeChart;
        }
        set
        {
            uptimeChart = value;
        }
    }
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        nodeSprite = this.GetComponent<SpriteRenderer>();
        pos = Camera.main.WorldToScreenPoint(this.transform.position);
        SplitSprite();
    }

    // Update is called once per frame
    void Update()
    {
        //SplitSprite();
        if (wedges != null)
        {
            for (int i = 0; i < wedges.Count; i++)
            {
                wedges[i].transform.position = pos;
            }
        }
    }

    /// <summary>
    /// Set all variables within the node with data passed from the server
    /// </summary>
    /// <param name="_ip"> the IP address of the node</param>
    /// <param name="_hostname">The name of this node's Host on the network</param>
    /// <param name="_hostDescription">A short description of what the host does on the network</param>
    /// <param name="_os">The Operating System of the node's Host</param>
    /// <param name="_isHidden"> determines of the node is visible</param>
    /// <param name="_type"> tracks what type of system the node is</param>
    /// <param name="_state"> tracks what state the node is currently experiencing</param>
    /// <param name="_connections"> tracks the interger IDs of adjecent Nodes</param>
    /// <param name="_teamIDs">list of all teams accessing this node</param>
    public void SetData(string _ip, string _hostname, string _hostDescription, OperatingSystems _os)
    {
        this.ip             = _ip;
        this.hostName = _hostname;
        this.hostDescription = _hostDescription;
        this.os = _os;
        // this.type           = _type;
        // this.state          = _state;
        // this.isHidden       = _isHidden;
        // this.connections    = _connections;
        // this.teamIDs          = _teamIDs;
    }

    /// <summary>
    /// Use the data from the FileReader to add references to newly instanced GameObjects
    /// </summary>
    /// <param name="_index">Index of the Node within Infra.AllNodes</param>
    public void InstanceData(int _index)
    {
        name = hostName;
        index = _index;
        ChangeSprite();
    }

    /// <summary>
    /// Changes this object's sprite based-on the new NodeType read-in.
    /// </summary>
    private void ChangeSprite()
    {
        //Sprite newSprite = GeneralResources.Instance.NodeSprites[(int)type];
        Sprite newSprite = GameManager.Instance.OsSprites[(int)os];
        nodeSprite.sprite = newSprite;
        Vector3 temp = nodeSprite.transform.localScale;
        temp.x *= .15f;
        temp.y *= .15f;
        nodeSprite.transform.localScale = temp;
    } 

    /// <summary>
    /// Divide the sprite into colored pieces based on the number of teams at the node, and color those pieces to the designated colors of the corresponding teams.
    /// </summary>
    public void SplitSprite()
    {
        // Size of each segment
        //float value = 1 / teams.Count;

        float zRotation = 0f;

        /// <summary>
        /// Loops through all the teams accessing the node and makes that many circle segments
        /// ** Will run if data is added to teams list **
        /// </summary>
  
        
        for (int i = 0; i < teams.Count; i++)
        {
            value = 1 / teams.Count;
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = teams[i].TeamColor;
            newWedge.fillAmount = value;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }

        /// <summary>
        /// Test data for above loop that takes data given in unity interface and converts that to a pie graph
        /// </summary>
        zRotation = 0f;
        float testValue = .25f; // for testing the sprite color

        for (int i = 0; i < wedgeColors.Count; i++)
        {
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = testValue;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            newWedge.name = $"{this.name} Wedge {i}";
            zRotation -= newWedge.fillAmount * 360f;
            //wedges.Add(newWedge);
        }
    }



    /*Redundant code. We Won't be cloning nodes in the forseable future. - Ben 
     * /// <summary>
    /// Create a new NodeData with all of the data from this node
    /// </summary>
    /// <returns></returns>
    public NodeData Clone()
    {
        NodeData newNode = new NodeData();
        newNode.index = this.index;
        newNode.ip = this.ip;
        newNode.type = this.type;
        newNode.state = this.state;
        newNode.isHidden = this.isHidden;
        newNode.connections = this.connections;
        newNode.connectionGOS = this.connectionGOS;
        newNode.nodeSprite = this.nodeSprite;

        return newNode;
    }*/
}
