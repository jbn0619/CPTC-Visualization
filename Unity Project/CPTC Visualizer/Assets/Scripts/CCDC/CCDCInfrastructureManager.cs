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
    private UptimeChartData uptimeChartGO;

    #endregion Fields
    
    #region Properties

    
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();
    }

    /// <summary>
    /// Read in data from a JSON file and convert it into Data containers split-up into gameObjects.
    /// </summary>
    public override void ReadJson()
    {
        StreamReader reader = new StreamReader("Assets/Data/data.json");
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
            TeamData newTeam = Instantiate(teamGO, CCDCManager.Instance.TeamManager.gameObject.transform);
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

            CCDCManager.Instance.TeamManager.Teams.Add(newTeam);
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
                NodeData newNode = Instantiate(nodeGO, newNet.transform);
                newNode.Id = payload.infrastructure.networks[i].nodes[k].id;
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
        foreach(TeamData team in CCDCManager.Instance.TeamManager.Teams)
        {
            // Instantiate a copy of the infrastructure, and make it a child of the team's gameObject.
            InfrastructureData newInfra = Instantiate(infrastructure, team.gameObject.transform);
            newInfra.gameObject.transform.position = infrastructure.gameObject.transform.position;
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

        // Place each network first, then place nodes around them.
        for(int i = 0; i < infrastructure.Networks.Count; i++)
        {
            float radius = infrastructure.Networks.Count / 1.5f;
            float angle = i * Mathf.PI * 2f / infrastructure.Networks.Count;

            // Move the network to another position based-on a..radial position?
            infrastructure.Networks[i].gameObject.transform.position = infrastructure.gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            infrastructure.Networks[i].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

            // Edit the network's lineRenderer to re-size it to encompase the node sprites.
            float nodeRadius = infrastructure.Networks.Count / (radius * 2);
            GenerateNetworkOutline(infrastructure.Networks[i], nodeRadius + 0.5f);

            // Place each of the netowrk's nodes around in a circle.
            for(int j = 0; j < infrastructure.Networks[i].Nodes.Count; j++)
            {
                radius = nodeRadius;
                angle = j * Mathf.PI * 2f / infrastructure.Networks[i].Nodes.Count;

                // Move the node to another position based-on a radial position.
                infrastructure.Networks[i].Nodes[j].gameObject.transform.position = infrastructure.Networks[i].gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + new Vector3(-0.15f, 0, 0);
                infrastructure.Networks[i].Nodes[j].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

                // Next, place an uptime chart that corresponds to this node.
                UptimeChartData newChart = Instantiate(uptimeChartGO, UIManager.Instance.SceneCanvases[1].gameObject.transform);
                newChart.gameObject.transform.position = infrastructure.Networks[i].Nodes[j].gameObject.transform.position + new Vector3(0.35f, 0, 0);
                newChart.gameObject.transform.localScale = new Vector2(0.002f, 0.008f);
                newChart.ID = infrastructure.Networks[i].Nodes[j].Id;
                CCDCManager.Instance.EventManager.UptimeCharts.Add(newChart);

                // Next, check their state to edit their color.
                switch (infrastructure.Networks[i].Nodes[j].State)
                {
                    case NodeState.Off:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = Color.gray;
                        break;
                    case NodeState.On:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = Color.cyan;
                        break;
                    case NodeState.NotWorking:
                        infrastructure.Networks[i].Nodes[j].NodeSprite.color = Color.red;
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
}