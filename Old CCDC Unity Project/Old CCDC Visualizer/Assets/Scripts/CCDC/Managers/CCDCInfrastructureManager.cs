using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CCDCInfrastructureManager : InfrastructureManager
{
    #region Fields

    [SerializeField]
    private JSONWriter writer;

    [SerializeField]
    private UptimeChartData uptimeChartGO;
    [SerializeField]
    private CCDCNodeData ccdcNodeGO;

    private List<UptimeChartData> uptimeCharts;

    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets a list of the main infrastructure's uptime charts.
    /// </summary>
    public List<UptimeChartData> UptimeCharts
    {
        get
        {
            return uptimeCharts;
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
    /// Read in data from a JSON file and convert it into Data containers split-up into gameObjects.
    /// </summary>
    public override void ReadJson()
    {
        StreamReader reader = new StreamReader("C:\\ProgramData\\CSEC Visualizer\\data.json");
        string input = reader.ReadToEnd();
        reader.Close();
        CPTCData payload = JsonUtility.FromJson<CPTCData>(input);

        // Clean up our objects.
        if (infrastructure != null)
        {
            GameObject.Destroy(infrastructure.gameObject);
            foreach (TeamData t in CCDCManager.Instance.TeamManager.Teams)
            {
                GameObject.Destroy(t.gameObject);
            }
            CCDCManager.Instance.TeamManager.Teams.Clear();
        }
        
        // Collects the team data first.
        for(int i = 0; i < payload.teams.Count; i++)
        {
            CCDCTeamData newTeam = Instantiate((CCDCTeamData)teamGO, CCDCManager.Instance.TeamManager.gameObject.transform);
            newTeam.SetupQueue();
            newTeam.TeamId = payload.teams[i].teamId;
            // Move all discovered node-IDs into newTeam.
            foreach (int n in payload.teams[i].discoveredNodeIds)
            {
                newTeam.DiscoveredNodeIds.Add(n);
            }

            // Convert all alerts into AlertData gameObjects.
            foreach (Alert a in payload.teams[i].alerts)
            {
                AlertData newAlert = Instantiate(alertGO, newTeam.transform);
                Enum.TryParse(a.type, out CPTCEvents newEvent);
                newAlert.Type = newEvent;
                newAlert.Priority = a.priority;
                newAlert.Timestamp = a.timestamp;

                // Transfer over our affected nodes for this alert.
                foreach(int n in a.affectedNodes)
                {
                    newAlert.AffectedNodes.Add(n);
                }
                
                newTeam.Queue.Push(newAlert); // .Alerts.Add(newAlert);
            }

            CCDCManager.Instance.TeamManager.CCDCTeams.Add(newTeam);
        }

        // Instantiate our infrastructure and populate it.
        infrastructure = Instantiate(infraGO);

        for(int i = 0; i < payload.infrastructure.networks.Count; i++)
        {
            NetworkData newNet = Instantiate(networkGO, infrastructure.transform);
            newNet.Id = payload.infrastructure.networks[i].networkId;

            // Move all network connection-IDs into newNet.
            foreach (int n in payload.infrastructure.networks[i].networkConnections)
            {
                newNet.Connections.Add(n);
            }

            // Move all the nodes into this network.
            for (int k = 0; k < payload.infrastructure.networks[i].nodes.Count; k++)
            {
                CCDCNodeData newNode = Instantiate(ccdcNodeGO, newNet.transform);
                newNode.Id = payload.infrastructure.networks[i].nodes[k].id;
                newNode.Ip = payload.infrastructure.networks[i].nodes[k].ip;
                newNode.IsActive = true;
                Enum.TryParse(payload.infrastructure.networks[i].nodes[k].type, out NodeTypes newType);
                newNode.Type = newType;
                Enum.TryParse(payload.infrastructure.networks[i].nodes[k].state, out NodeState newState);
                newNode.State = newState;

                newNode.IsHidden = payload.infrastructure.networks[i].nodes[k].isHidden;

                // Move all the node's connection-IDs into newNode.
                foreach (int c in payload.infrastructure.networks[i].nodes[k].connections)
                {
                    newNode.Connections.Add(c);
                }

                // Pass this node's reference to its network and the infrastructure.
                newNet.Nodes.Add(newNode);
                infrastructure.AllNodes.Add(newNode);
            }

            infrastructure.Networks.Add(newNet);
        }

        infrastructure.gameObject.transform.position = this.gameObject.transform.position;

        GenerateGraph();

        DuplicateInfrastructure();
    }

    /// <summary>
    /// Takes this script's infrastructure and duplicates it. It then sends those copies to each team so each team can edit their own infrastructures with their alerts and whatnot.
    /// </summary>
    public override void DuplicateInfrastructure()
    {
        for (int i = 0; i < CCDCManager.Instance.TeamManager.CCDCTeams.Count; i++)
        {
            CCDCTeamData team = CCDCManager.Instance.TeamManager.CCDCTeams[i];
            // Instantiate a copy of the infrastructure, and make it a child of the team's gameObject.
            InfrastructureData newInfra = Instantiate(infrastructure);
            newInfra.transform.parent = team.gameObject.transform;
            newInfra.gameObject.transform.position = infrastructure.gameObject.transform.position;

            // Make an empty gameObject to clean up the uptime charts scene heirarchy.
            GameObject emptyObj = new GameObject();
            emptyObj.transform.parent = UIManager.Instance.SceneCanvases[1].gameObject.transform;
            emptyObj.name = "Team " + (i + 1).ToString() + " Uptime Charts";

            // Make sure the data of individual nodes is properly-copied.
            for (int k = 0; k < newInfra.AllNodes.Count; k++)
            {
                // Assign the node's values to a new NodeData object.
                NodeData currentNode = newInfra.AllNodes[k];
                NodeData oldNode = infrastructure.AllNodes[k];
                currentNode.Ip = oldNode.Ip;
                currentNode.Id = oldNode.Id;
                currentNode.IsHidden = oldNode.IsHidden;
                currentNode.Type = oldNode.Type;
                currentNode.State = oldNode.State;
                foreach (int c in oldNode.Connections)
                {
                    currentNode.Connections.Add(c);
                }
                foreach (LineRenderer c in oldNode.ConnectionGOS)
                {
                    LineRenderer copy = Instantiate(c, currentNode.gameObject.transform);
                    currentNode.ConnectionGOS.Add(copy);
                }

                if (currentNode is CCDCNodeData)
                {
                    // Instantiate new uptime charts for each node.
                    UptimeChartData newChart = Instantiate(uptimeChartGO, emptyObj.transform);

                    ((CCDCNodeData)currentNode).UptimeChart = newChart;
                    newChart.gameObject.transform.position = newInfra.AllNodes[k].gameObject.transform.position + new Vector3(0.5f, 0, 0);
                    newChart.gameObject.transform.localScale = new Vector2(0.002f, 0.008f);
                    newChart.NodeID = newInfra.AllNodes[k].Id;
                    newChart.TeamID = team.TeamId;

                    team.UptimeCharts.Add(newChart);
                    newChart.gameObject.SetActive(false);
                }
            }

            // appends the teams at the end of the names
            for (int k = 0; k < newInfra.AllNodes.Count; k++)
            {
                NodeData current = newInfra.AllNodes[k];
                string name = current.Ip;
                string append = "";
                
                if (i < 9)
                {
                    append = "-team0" + (i + 1);
                }
                else
                {
                    append = "-team" + (i + 1);
                }

                current.Ip = name + append;
            }

            /// <summary>
            /// OLD LOOP FOR TEAMS WITH IPS
            /// Update the node IPs with this team's number.
            ///for (int k = 0; k < newInfra.AllNodes.Count; k++)
            ///{
            ///    NodeData currentNode = newInfra.AllNodes[k];
            ///    string ip = currentNode.Ip;
            ///    string teamNum = (i + 1).ToString();
            ///    int xIndex = ip.IndexOf('X');
            ///
            ///    // Check if we meet the conditions to change the number behind the X-character (if one exists).
            ///    if (i == 9 && ip[xIndex - 1] != '.')
            ///    {
            ///        char previousNum = ip[xIndex - 1];
            ///        string oldNum = previousNum.ToString() + "0";
            ///        int.TryParse(oldNum, out int num);
            ///        int totalNum = i + 1 + num;
            ///
            ///        string part1 = ip.Substring(0, xIndex - 1);
            ///        string part2 = totalNum.ToString();
            ///        string part3 = ip.Substring(xIndex + 1, ip.Length - (xIndex + 1));
            ///        string result = part1 + part2 + part3;
            ///
            ///        currentNode.Ip = result;
            ///    }
            ///    else
            ///    {
            ///        string part1 = ip.Substring(0, xIndex);
            ///        string part2 = teamNum;
            ///        string part3 = ip.Substring(xIndex + 1, ip.Length - (xIndex + 1));
            ///        string result = part1 + part2 + part3;
            ///
            ///        currentNode.Ip = result;
            ///    }
            ///}
            /// </summary>

            team.InfraCopy = newInfra;

            // Create the team's graph, then hide it for later.
            team.BuildTeamGraph();
            team.InfraCopy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Generates the graph and connects nodes. Builds an outward circular graph of networks and nodes.
    /// </summary>
    public override void GenerateGraph()
    {
        // Open-up the uptime chart canvas and add charts to it.
        UIManager.Instance.SceneCanvases[1].gameObject.SetActive(true);
        
        if (uptimeCharts == null)
        {
            uptimeCharts = new List<UptimeChartData>();
        }
        else
        {
            uptimeCharts.Clear();
        }

        // Make an empty gameObject to clean up the uptime charts scene heirarchy.
        GameObject emptyObj = new GameObject(); 
        emptyObj.transform.parent = UIManager.Instance.SceneCanvases[1].gameObject.transform;
        emptyObj.name = "Team Infrastructure Uptime Charts";

        // Place each network first, then place nodes around them.
        for (int i = 0; i < infrastructure.Networks.Count; i++)
        {
            float radius = 0;
            if (writer.GenType == InfraGenType.BillStackpole)
            {
                radius = 3;
            }
            else
            {
                radius = infrastructure.Networks.Count / 1.15f;
            }
            float angle = i * Mathf.PI * 2f / infrastructure.Networks.Count;

            // Move the network to another position based-on a..radial position?
            infrastructure.Networks[i].gameObject.transform.position = infrastructure.gameObject.transform.position + new Vector3(Mathf.Cos(angle + 48.06f) * radius, Mathf.Sin(angle + 48.06f) * radius, 0);
            //infrastructure.Networks[i].gameObject.transform.position = infrastructure.gameObject.transform.position + new Vector3((-4) + 3 * i, 0, 0);
            infrastructure.Networks[i].gameObject.transform.localScale = new Vector2(.75f, .75f);

            // Edit the network's lineRenderer to re-size it to encompase the node sprites.
            float nodeRadius = infrastructure.Networks[i].Nodes.Count / (radius * 1.5f);
            GenerateNetworkOutline(infrastructure.Networks[i], nodeRadius + 0.75f);

            // Place each of the netowrk's nodes around in a circle.
            for (int j = 0; j < infrastructure.Networks[i].Nodes.Count; j++)
            {
                radius = nodeRadius;
                angle = j * Mathf.PI * 2f / infrastructure.Networks[i].Nodes.Count;

                // Move the node to another position based-on a radial position.
                infrastructure.Networks[i].Nodes[j].gameObject.transform.position = infrastructure.Networks[i].gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + new Vector3(0, 0, 0);

                // Next, place an uptime chart that corresponds to this node.
                UptimeChartData newChart = Instantiate(uptimeChartGO, emptyObj.transform);
                newChart.gameObject.transform.position = infrastructure.Networks[i].Nodes[j].gameObject.transform.position + new Vector3(.75f, 0, 0);
                newChart.gameObject.transform.localScale = new Vector2(0.004f, 0.016f);
                newChart.NodeID = infrastructure.Networks[i].Nodes[j].Id;
                newChart.TeamID = -1;
                uptimeCharts.Add(newChart);

                // Next, check their state to edit their color.
                switch (infrastructure.Networks[i].Nodes[j].State)
                {
                    case NodeState.Off:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = Color.gray;
                        break;
                    case NodeState.On:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = new Color(0.3137255f, 0.3333333f, 0.9098039f);
                        break;
                    case NodeState.NotWorking:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = new Color(0.9098039f, 0.3137255f, 0.3137255f);
                        break;
                }

                // Disable the node's sprite if it is hidden.
                if (infrastructure.Networks[i].Nodes[j].IsHidden)
                {
                    infrastructure.Networks[i].Nodes[j].NodeSprite.gameObject.SetActive(false);
                }
            }
        }
        GenerateConnections();
    }

    /// <summary>
    /// Generates an outline for the given network with a lineRenderer.
    /// </summary>
    /// <param name="network">The network we need to visualize here.</param>
    /// <param name="radius">How large the network must be in-order to encapsulate all its nodes.</param>
    public override void GenerateNetworkOutline(NetworkData network, float radius)
    {
        // infrastructure.Networks[i].gameObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        Vector3[] newPositions = new Vector3[(network.Nodes.Count * 2) + 2];
        for (int i = 0; i <= (network.Nodes.Count * 2) + 1; i++)
        {
            float angle = i * Mathf.PI / network.Nodes.Count;
            newPositions[i] = network.gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }

        // Setup lineRenderer
        network.Outline.startColor = Color.white;
        network.Outline.endColor = Color.white;
        network.Outline.startWidth = 0.15f;
        network.Outline.endWidth = 0.15f;
        network.Outline.positionCount = (network.Nodes.Count * 2) + 2;
        network.Outline.SetPositions(newPositions);
    }

    /// <summary>
    /// Generate conneciton gameObjects to display as part of the graph.
    /// </summary>
    public override void GenerateConnections()
    {
        // Display node connecitons.
        for (int i = 0; i < infrastructure.AllNodes.Count; i++)
        {
            // Go node-by-node and create LineRenderers for each connection.
            foreach (int c in infrastructure.AllNodes[i].Connections)
            {
                LineRenderer newLine = Instantiate(connectionGO, infrastructure.AllNodes[i].transform);
                Vector3 startPos = infrastructure.AllNodes[i].gameObject.transform.position;
                Vector3 endPos = infrastructure.AllNodes[c].gameObject.transform.position;
                newLine.SetPosition(0, startPos);
                newLine.SetPosition(1, endPos);
                newLine.startWidth = 0.01f;
                newLine.endWidth = 0.01f;

                // Add the new lineRenderer to this node's list of references.
                infrastructure.AllNodes[i].ConnectionGOS.Add(newLine);

                // If showConnections is disabled, then hide all connections after generating them so they're "hidden".
                if (showConnections == false)
                {
                    newLine.gameObject.SetActive(false);
                }
                // Next, check if a connection needs to be hidden because either a node is shut-down or hidden from view.
                else if (showHiddenConnections == false)
                {
                    if (infrastructure.AllNodes[i].IsHidden || infrastructure.AllNodes[i].State == NodeState.Off)
                    {
                        newLine.gameObject.SetActive(false);
                    }
                    else if (infrastructure.AllNodes[c].IsHidden || infrastructure.AllNodes[c].State == NodeState.Off)
                    {
                        newLine.gameObject.SetActive(false);
                    }
                }
            }
        }

        // Display network connections.
        for (int i = 0; i < infrastructure.Networks.Count; i++)
        {
            foreach (int c in infrastructure.Networks[i].Connections)
            {
                LineRenderer newLine = Instantiate(connectionGO, infrastructure.Networks[i].transform);
                Vector3 startPos = infrastructure.Networks[i].gameObject.transform.position;
                Vector3 endPos = infrastructure.Networks[c].gameObject.transform.position;

                // Make the start and end positions line-up with the edges of the network sprite.
                Vector3 dir = endPos - startPos;
                dir.Normalize();
                startPos += dir;
                endPos -= dir;

                // Setup the line renderer to display properly.
                newLine.SetPosition(0, startPos);
                newLine.SetPosition(1, endPos);
                newLine.startWidth = 0.1f;
                newLine.endWidth = 0.1f;

                // Add the new lineRenderer to the network's list of references.
                infrastructure.Networks[i].ConnectionGOS.Add(newLine);

                // If showConnections is disabled, then hide all connections after generating them so they're "hidden".
                if (showConnections == false)
                {
                    newLine.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Disables the main infrastructure (and its dependencies), then switches to the first team.
    /// </summary>
    public void DisableMainView()
    {
        // Hide the main infrastructure, then change the view to the first team.
        infrastructure.gameObject.SetActive(false);
        foreach (UptimeChartData u in uptimeCharts)
        {
            u.gameObject.SetActive(false);
        }

        CCDCManager.Instance.TeamManager.SelectTeamView(0);
    }
}