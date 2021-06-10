using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// the ID number for this node
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    protected int id;
    /// <summary>
    /// The IP address of this node's access to the simulation
    /// </summary>
    [SerializeField]
    protected string ip;
    /// <summary>
    /// The type of simulated computer system this node is
    /// </summary>
    [SerializeField]
    protected NodeTypes type;
    /// <summary>
    /// The current level of functionality of this node
    /// </summary>
    [SerializeField]
    protected NodeState state;
    /// <summary>
    /// A list of ID numbers for teams currently accessing this node
    /// </summary>
    [SerializeField]
    protected List<int> teamIDs;
    /// <summary>
    /// A list of ID numbers for adjacent Nodes
    /// </summary>
    [SerializeField]
    protected List<int> connections;
    /// <summary>
    /// A boolean to track if this node is hidden in the system
    /// </summary>
    [SerializeField]
    protected bool isHidden;

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

    // Legacy Fields
    private UptimeChartData uptimeChart;

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
    /// Get and sets a list of node ids this node has connections to.
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
    }

    // Update is called once per frame
    void Update()
    {
        SplitSprite();
    }

    /// <summary>
    /// Changes this object's sprite based-on the new NodeType read-in.
    /// </summary>
    private void ChangeSprite()
    {
        //Sprite newSprite = GeneralResources.Instance.NodeSprites[(int)type];
        Sprite newSprite = Resources.Load<Sprite>(type.ToString() + "_Icon");
        nodeSprite.sprite = newSprite;
        nodeSprite.transform.localScale = new Vector3(.15f, .15f, 1);
    }    

    /// <summary>
    /// Set all variables within the node with data passed from the server
    /// </summary>
    /// <param name="_id">An integer Id for the node</param>
    /// <param name="_ip"> the IP address of the node</param>
    /// <param name="_isHidden"> determines of the node is visible</param>
    /// <param name="_type"> tracks what type of system the node is</param>
    /// <param name="_state"> tracks what state the node is currently experiencing</param>
    /// <param name="_connections"> tracks the interger IDs of adjecent Nodes</param>
    /// <param name="_teamIDs">list of all teams accessing this node</param>
    public void SetData(int _id, string _ip, bool _isHidden, NodeTypes _type, 
        NodeState _state, List<int> _connections, List<int> _teamIDs)
    {
        this.id             = _id;
        this.ip             = _ip;
        this.type           = _type;
        this.state          = _state;
        this.isHidden       = _isHidden;
        this.connections    = _connections;
        this.teamIDs          = _teamIDs;
    }

    /// <summary>
    /// Use the data from the FileReader to add references to newly instanced GameObjects
    /// </summary>
    public void InstanceData()
    {
        foreach(int teamID in this.teamIDs)
        {
            this.teams.Add(GameManager.Instance.MainInfra.FindTeamByID(teamID));
        }
    }

    /// <summary>
    /// Create a new NodeData with all of the data from this node
    /// </summary>
    /// <returns></returns>
    public NodeData Clone()
    {
        NodeData newNode = new NodeData();
        newNode.id = this.id;
        newNode.ip = this.ip;
        newNode.type = this.type;
        newNode.state = this.state;
        newNode.isHidden = this.isHidden;
        newNode.connections = this.connections;
        newNode.connectionGOS = this.connectionGOS;
        newNode.nodeSprite = this.nodeSprite;

        return newNode;
    }

    /// <summary>
    /// Divide the sprite into colored pieces based on the number of teams at the node, and color those pieces to the designated colors of the corresponding teams.
    /// </summary>
    public void SplitSprite()
    {
        // ** Some sort of variable to hold the sprite and its segments **

        /*
         * for (int i = 0; i < teams.count; i++)
         * {
         *       ** Adds teams[i].teamColor to the sprite Segment **
         * }
         * 
         */
    }
}
