using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDCTeamData: TeamData
{
    #region Fields

    private List<UptimeChartData> uptimeCharts;

    private List<NotificationButton> notifMarkers;
    private List<GameObject> notifBanners;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of all the uptime charts in this scene.
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

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Dynamically moves all of this team's infrastructure into the scene.
    /// </summary>
    public override void BuildTeamGraph()
    {
        // Place each network first, then place nodes around them.
        for (int i = 0; i < infraCopy.Networks.Count; i++)
        {
            float radius = infraCopy.Networks.Count / 1.5f;
            float angle = i * Mathf.PI * 2f / infraCopy.Networks.Count;

            infraCopy.Networks[i].gameObject.transform.position = infraCopy.gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            infraCopy.Networks[i].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

            float nodeRadius = infraCopy.Networks.Count / (radius * 2);

            // Place each of the netowrk's nodes around in a circle.
            for (int j = 0; j < infraCopy.Networks[i].Nodes.Count; j++)
            {
                radius = nodeRadius;
                angle = j * Mathf.PI * 2f / infraCopy.Networks[i].Nodes.Count;

                // Move the node to another position based-on a radial position.
                infraCopy.Networks[i].Nodes[j].gameObject.transform.position = infraCopy.Networks[i].gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + new Vector3(-0.15f, 0, 0);
                infraCopy.Networks[i].Nodes[j].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

                // If the node gets shut down, then disable it (for now).
                infraCopy.Networks[i].Nodes[j].gameObject.SetActive(infraCopy.Networks[i].Nodes[j].IsActive);

                // See if we can display the node based-on if this team has discovered it or not.
                infraCopy.Networks[i].Nodes[j].gameObject.SetActive(discoveredNodeIds.Contains(infraCopy.Networks[i].Nodes[j].Id));

                // Check what connections need to be turned-off or left on.
                for (int k = 0; k < infraCopy.Networks[i].Nodes[j].Connections.Count; k++)
                {
                    if (discoveredNodeIds.Contains(infraCopy.Networks[i].Nodes[j].Connections[k]) == false)
                    {
                        infraCopy.Networks[i].Nodes[j].ConnectionGOS[k].gameObject.SetActive(false);
                    }
                    else
                    {
                        infraCopy.Networks[i].Nodes[j].ConnectionGOS[k].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
