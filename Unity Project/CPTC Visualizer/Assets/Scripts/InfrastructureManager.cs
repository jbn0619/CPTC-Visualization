using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.IO;
using System;

public class InfrastructureManager: Singleton<InfrastructureManager>
{
    #region Fields

    private InfrastructureData infrastructure;
    private List<TeamData> teams;
    private int currentTeamView;

    public int timeBetweenAlerts = 5;

    public float timer;
    public bool simulationStart = false;

    [SerializeField]
    private bool showConnections;

    [Header("GameObject Prefabs")]
    [SerializeField]
    private NodeData nodeGO;
    [SerializeField]
    private NetworkData networkGO;
    [SerializeField]
    private InfrastructureData infraGO;
    [SerializeField]
    private TeamData teamGO;
    [SerializeField]
    private AlertData alertGO;
    [SerializeField]
    private LineRenderer connectionGO;

    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets a list of all teams currently in the competition.
    /// </summary>
    public List<TeamData> Teams
    {
        get
        {
            return teams;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        teams = new List<TeamData>();
        currentTeamView = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReadJson();
        }

        // Check for a view-switch input
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeTeamView(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeTeamView(-1);
        }

        //Starts or ends the simulation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(simulationStart)
            {
                simulationStart = false;
                return;
            }
            timer = 0;
            simulationStart = true;
        }
        
        // If the simulation is running, then update timer and see if we need to run alerts.
        if(simulationStart)
        {
            timer += Time.deltaTime;

            // If enough time has passed, run alerts and reset the timer.
            if (timer >= timeBetweenAlerts)
            {
                timer = 0;
                RunAlerts();
            }
        }
    }

    /// <summary>
    /// Read in data from a JSON file and convert it into Data containers split-up into gameObjects.
    /// </summary>
    public void ReadJson()
    {
        StreamReader reader = new StreamReader("Assets/Data/data.json");
        string input = reader.ReadToEnd();
        reader.Close();
        CPTCData payload = JsonUtility.FromJson<CPTCData>(input);

        // Clean up our objects.
        if (infrastructure != null)
        {
            GameObject.Destroy(infrastructure.gameObject);
            foreach (TeamData t in teams)
            {
                GameObject.Destroy(t.gameObject);
            }
            teams.Clear();
        }
        
        // Collects the team data first.
        for(int i = 0; i < payload.teams.Count; i++)
        {
            TeamData newTeam = Instantiate(teamGO, Vector3.zero, Quaternion.identity);
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

                // Transfer over our affected nodes for this alert.
                foreach(int n in a.affectedNodes)
                {
                    newAlert.AffectedNodes.Add(n);
                }
                
                newTeam.Alerts.Add(newAlert);
            }

            teams.Add(newTeam);
        }

        // Instantiate our infrastructure and populate it.
        infrastructure = Instantiate(infraGO, Vector3.zero, Quaternion.identity);

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

        GenerateGraph();

        DuplicateInfrastructure();
    }

    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public void RunAlerts()
    {
        foreach(TeamData team in teams)
        {
            team.ReadNextAlert();
        }
    }

    /// <summary>
    /// Takes this script's infrastructure and duplicates it. It then sends those copies to each team so each team can edit their own infrastructures with their alerts and whatnot.
    /// </summary>
    public void DuplicateInfrastructure()
    {
        foreach(TeamData team in teams)
        {
            InfrastructureData newInfra = Instantiate(infrastructure, team.gameObject.transform);
            team.InfraCopy = newInfra;
            team.BuildTeamGraph();
            team.InfraCopy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Generates the graph and connects nodes. Builds an outward circular graph of networks and nodes.
    /// </summary>
    public void GenerateGraph()
    {
        // Place each network first, then place nodes around them.
        for(int i = 0; i < infrastructure.Networks.Count; i++)
        {
            float radius = 3f;
            float angle = i * Mathf.PI * 2f / infrastructure.Networks.Count;

            // Move the network to another position based-on a..radial position?
            infrastructure.Networks[i].gameObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            infrastructure.Networks[i].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

            // Edit the network's child sprite to re-size it to encompase the node sprites.
            SpriteRenderer netSprite = infrastructure.Networks[i].GetComponentInChildren<SpriteRenderer>();
            netSprite.transform.localScale = new Vector2(radius + 1, radius + 1);

            // Place each of the netowrk's nodes around in a circle.
            for(int j = 0; j < infrastructure.Networks[i].Nodes.Count; j++)
            {
                radius = 0.75f;
                angle = j * Mathf.PI * 2f / infrastructure.Networks[i].Nodes.Count;

                // Move the node to another position based-on a radial position.
                infrastructure.Networks[i].Nodes[j].gameObject.transform.position = new Vector3(infrastructure.Networks[i].transform.position.x + Mathf.Cos(angle) * radius, infrastructure.Networks[i].transform.position.y + Mathf.Sin(angle) * radius, 0);
                infrastructure.Networks[i].Nodes[j].gameObject.transform.localScale = new Vector2(0.15f, 0.15f);
            }
        }

        GenerateConnections();
    }

    /// <summary>
    /// Generate conneciton gameObjects to display as part of the graph.
    /// </summary>
    public void GenerateConnections()
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
    /// Changes what infrastructure is currently-displayed in the scene.
    /// </summary>
    public void ChangeTeamView(int deltaIndex)
    {
        // Check what team is currently-viewed and set it to false (hide it)
        if (currentTeamView == -1)
        {
            infrastructure.gameObject.SetActive(false);
        }
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }
        // Update the index based-on what key was hit.
        currentTeamView += deltaIndex;

        // Check what index we're at and take the appropriate actions for either wrapping-through the collection in either direction or seeing if we're in teams or infrastructure.

        // The case when we're not looking at teams, but infrastructure.
        if (currentTeamView == -1)
        {
            infrastructure.gameObject.SetActive(true);
        }
        // The case when we wrap from the bottom to the end of the teams list.
        else if (currentTeamView < -1)
        {
            currentTeamView = teams.Count - 1;
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            teams[currentTeamView].BuildTeamGraph();
        }
        // The case when we wrap from the end of teams list back to infrastructure.
        else if (currentTeamView >= teams.Count)
        {
            currentTeamView = -1;
            infrastructure.gameObject.SetActive(true);
        }
        // The case when we're somewhere within the teams list.
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// This method is called by a button to change the currently-viewed infrastructure to a team at a specific index.
    /// </summary>
    /// <param name="teamIndex">The id of the team to display.</param>
    public void SelectTeamView(int teamIndex)
    {
        // First, disable the currently-active infrastructure.
        if (currentTeamView == -1)
        {
            infrastructure.gameObject.SetActive(false);
        }
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            infrastructure.gameObject.SetActive(true);
        }
        else if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            currentTeamView = teamIndex;
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
        }
    }
}