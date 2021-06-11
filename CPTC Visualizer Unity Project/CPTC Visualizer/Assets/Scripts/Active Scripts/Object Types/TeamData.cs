using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel - Summer 2021
/// Function: Data container that is tied to a game object. Represents a competing team in the CPTC competition, and contains team-specific information as well as a copy of the infrastructure for them to modify.
/// </summary>
public class TeamData: MonoBehaviour
{
    #region Fields
    /// <summary>
    /// Id number of this team
    /// </summary>
    [SerializeField]
    protected int teamId;
    /// <summary>
    /// the name of this team displayed on the screen
    /// </summary>
    [SerializeField]
    protected string teamName;
    /// <summary>
    /// The color used to represent this team on the display
    /// </summary>
    [SerializeField]
    protected Color teamColor;
    /// <summary>
    /// A list of alerts the team has caused durning the competition
    /// </summary>
    [SerializeField]
    protected List<AlertData> alerts;
    /// <summary>
    /// A list of ID numbers of the nodes this team is currently accessing
    /// </summary>
    [SerializeField]
    protected List<int> nodeIDs;

    // Legacy Fields
    protected PriorityQueue queue;
    protected InfrastructureData infraCopy;
    private List<UptimeChartData> uptimeCharts;
    private List<NotificationButton> notifMarkers;
    private List<GameObject> notifBanners;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets what this team's id is.
    /// </summary>
    public int TeamId
    {
        get
        {
            return teamId;
        }
        set
        {
            if (value >= 0)
            {
                teamId = value;
            }
        }
    }

    /// <summary>
    /// Returns a reference to the queue that should allow
    /// other classes to interact with the queue of the team
    /// </summary>
    public PriorityQueue Queue
    {
        get { return queue; }
    }

    /// <summary>
    /// Gets a list of alerts generated by this team.
    /// </summary>
    public List<AlertData> Alerts
    {
        get
        {
            return alerts;
        }
    }

    /// <summary>
    /// Gets a list of ID numbers for nodes this team has discovered.
    /// </summary>
    public List<int> NodeIDs
    {
        get
        {
            return nodeIDs;
        }
    }

    /// <summary>
    /// Gets or sets this team's copy of the infrastructure data.
    /// </summary>
    public InfrastructureData InfraCopy
    {
        get
        {
            return infraCopy;
        }
        set
        {
            infraCopy = value;
        }
    }

    /// <summary>
    /// Gets or sets the Team Name
    /// </summary>
    public string TeamName
    {
        get { return teamName; }
        set { teamName = value; }
    }

    /// <summary>
    /// Gets or sets the team's color
    /// </summary>
    public Color TeamColor
    {
        get { return teamColor; }
        set { teamColor = value; }
    }

    /// <summary>
    /// Gets a list of all the uptime charts in this team's infrastructure.
    /// </summary>
    public List<UptimeChartData> UptimeCharts
    {
        get
        {
            return uptimeCharts;
        }
    }

    /// <summary>
    /// Gets a list of all the notification markers active for this team.
    /// </summary>
    public List<NotificationButton> NotifMarkers
    {
        get
        {
            return notifMarkers;
        }
    }

    /// <summary>
    /// Gets a list of all the notification banners active for this team.
    /// </summary>
    public List<GameObject> NotifBanners
    {
        get
        {
            return notifBanners;
        }
    }

    #endregion Properties

    private void Awake()
    {
        uptimeCharts = new List<UptimeChartData>();
        notifBanners = new List<GameObject>();
        notifMarkers = new List<NotificationButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupQueue()
    {
        queue = new PriorityQueue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Set all variables within the team object with data passed from the server
    /// </summary>
    /// <param name="_id">Team's id number</param>
    /// <param name="_alerts">alerts this team has triggered</param>
    /// <param name="_nodeIDs">nodes this team is currently visiting</param>
    public void SetData(int _id, List<int> _nodeIDs, List<AlertData> _alerts = null)
    {
        this.teamId = _id;
        // this.alerts = _alerts; // alerts are not currently implemented as a data structure.
        this.nodeIDs = _nodeIDs;
    }

    // TODO : make methods to give teams random names and colors from a list of possible combos
}
