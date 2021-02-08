using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamData: MonoBehaviour
{
    #region Fields

    private int teamId;

    [SerializeField]
    private List<AlertData> alerts;

    [SerializeField]
    private PriorityQueue queue;

    [SerializeField]
    private List<int> discoveredNodeIds;

    private InfrastructureData infraCopy;
    
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
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // queue = new PriorityQueue();
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
    /// Reads the first alert in the alerts list and changes the infrastructure based-on that.
    /// </summary>
    public void ReadNextAlert()
    {
        if (!queue.IsEmpty) // alerts.Count > 0
        {
            AlertData newAlert = (AlertData)queue.Pop(); // alerts[0];
            //alerts.RemoveAt(0);

            switch (newAlert.Type)
            {
                // This is a low-priority event.
                case CPTCEvents.Discovery:
                    // Grab a random discovered-node, then "discover" one of its connections.
                    int baseNode = discoveredNodeIds[Random.Range(0, discoveredNodeIds.Count)];
                    if (infraCopy.AllNodes[baseNode].Connections.Count == 0)
                    {
                        break;
                    }

                    // Determine what node will be discovered, and activate the corresponding connection gameObject.
                    int randIndex = Random.Range(0, infraCopy.AllNodes[baseNode].Connections.Count);
                    int newNode = infraCopy.AllNodes[baseNode].Connections[randIndex];
                    infraCopy.AllNodes[baseNode].ConnectionGOS[randIndex].gameObject.SetActive(true);

                    discoveredNodeIds.Add(newNode);
                    break;
                // This is a high-priority event.
                case CPTCEvents.Exploit:
                    int randNodeIndex = Random.Range(0, discoveredNodeIds.Count);
                    int randNodeID = discoveredNodeIds[randNodeIndex];

                    infraCopy.AllNodes[randNodeID].NodeSprite.color = Color.magenta;
                    break;
                // This is a high-priority event.
                case CPTCEvents.ShutDown:
                    foreach (int n in newAlert.AffectedNodes)
                    {
                        // Make sure we're not shutting-down a node that's already been shut down.
                        if (infraCopy.ShutDownNodes.Contains(n) == false)
                        {
                            // Disable the node's gameObject, then add its ID to shutDownNodes.
                            infraCopy.ShutDownNodes.Add(n);
                            //infraCopy.AllNodes[n].gameObject.SetActive(false);
                            infraCopy.AllNodes[n].NodeSprite.enabled = false;
                        }
                    }
                    break;
                // This is a low-priority event.
                case CPTCEvents.StartUp:
                    // Grab a random node that has been shut down, and start it up again.
                    if (infraCopy.ShutDownNodes.Count > 0)
                    {
                        int startUpIndex = Random.Range(0, infraCopy.ShutDownNodes.Count);
                        startUpIndex = infraCopy.ShutDownNodes[startUpIndex];
                        //infraCopy.AllNodes[startUpIndex].gameObject.SetActive(true);
                        infraCopy.AllNodes[startUpIndex].NodeSprite.enabled = true;
                        infraCopy.ShutDownNodes.Remove(startUpIndex);
                    }
                    break;
                case CPTCEvents.NetworkScan:
                    // Get a random network to scan.
                    int netIndex = Random.Range(0, infraCopy.Networks.Count);
                    infraCopy.Networks[netIndex].ScanActive = true;
                    break;
                default:
                    break;
            }

            Debug.Log("ALERT: Team " + teamId + " attempted " + newAlert.Type);
        }
        else
        {
            Debug.Log("Team " + teamId + " has done NOTHING");
        }

        // After changes have been made, update the team's visual graph.
        BuildTeamGraph();
    }

    /// <summary>
    /// Dynamically moves all of this team's infrastructure into the scene.
    /// </summary>
    public void BuildTeamGraph()
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

                infraCopy.Networks[i].Nodes[j].gameObject.transform.position = infraCopy.Networks[i].gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                infraCopy.Networks[i].Nodes[j].gameObject.transform.localScale = new Vector2(0.15f, 0.15f);

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
