using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
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
    protected bool isActive;
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
            ChangeSprite();
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

    public NodeData Clone()
    {
        NodeData newNode = new NodeData();
        newNode.id = this.id;
        newNode.ip = this.ip;
        newNode.isActive = this.isActive;
        newNode.type = this.type;
        newNode.state = this.state;
        newNode.isHidden = this.isHidden;
        newNode.connections = this.connections;
        newNode.connectionGOS = this.connectionGOS;
        newNode.nodeSprite = this.nodeSprite;

        return newNode;
    }
}
