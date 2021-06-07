using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel - Summer 2021
/// Function: A data container that is tied to a game object in the unity scene. Contains information for a single "node" within an infrastructure.
/// </summary>
public class NodeData: MonoBehaviour
{
    #region Fields

    [Header("Node Fields")]
    [SerializeField]
    protected int id;
    [SerializeField]
    protected string ip;
    [SerializeField]
    protected NodeTypes type;
    protected NodeState state;
    [SerializeField]
    protected bool isHidden;

    [SerializeField]
    protected List<int> connections;

    [SerializeField]
    protected List<LineRenderer> connectionGOS;

    [SerializeField]
    protected SpriteRenderer nodeSprite;

    [SerializeField]
    protected List<TeamData> teams; // Tracks the teams with current access to this node

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    /// Set all variables within the node with data from the server admins
    /// </summary>
    /// <param name="_ID"></param>
    /// <param name="_IP"></param>
    /// <param name="_IsActive"></param>
    /// <param name="_IsHidden"></param>
    /// <param name="_Type"></param>
    /// <param name="_State"></param>
    /// <param name="_Connections"></param>
    /// <param name="_ConnectionGOS"></param>
    public void SetData(int _ID, string _IP, bool _IsHidden, NodeTypes _Type, 
        NodeState _State, List<int> _Connections, List<LineRenderer> _ConnectionGOS)
    {
        this.id             = _ID;
        this.ip             = _IP;
        this.type           = _Type;
        this.state          = _State;
        this.isHidden       = _IsHidden;
        this.connections    = _Connections;
        this.connectionGOS  = _ConnectionGOS;
        /* Example Call of SetNode
         * This would proably be in a Start() or loading method, called to set up the node objects.
        foreach (NodeData n in allNodes)
        {
            n.SetData(index, PARSED_DATA.ip, true, false, PARSED_DATA.type, PARSED DATA.state, PARSED_DATA.connections, PARSED_DATA.lineConnections)
        }
         */
    }
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

    public void SplitSprite()
    {
        // number of teams accessing = teamIDS
        List<double> spriteColors = new List<double>();

        /*
         * for (int i = 0; i < teamIDS; i++)
         * {
         *      if (Team is accessing node)
         *      {
         *          spriteColors.Add(*hexidecimal of team's color*);
         *      }
         * }
         * 
         * ** Add the change to the sprite with the divisions and colors **
         */
    }
}
