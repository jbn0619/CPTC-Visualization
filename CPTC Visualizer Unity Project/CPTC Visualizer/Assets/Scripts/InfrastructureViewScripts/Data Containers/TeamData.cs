using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamData: MonoBehaviour
{
    #region Fields

    protected int teamId;

    [SerializeField]
    protected string teamName;
    [SerializeField]
    protected Color teamColor;

    [SerializeField]
    protected List<AlertData> alerts;

    [SerializeField]
    protected PriorityQueue queue;

    [SerializeField]
    protected List<int> discoveredNodeIds;

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
    /// Gets a list of nodes this team has discovered.
    /// </summary>
    public List<int> DiscoveredNodeIds
    {
        get
        {
            return discoveredNodeIds;
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

    public string TeamName
    {
        get { return teamName; }
        set { teamName = value; }
    }

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
}