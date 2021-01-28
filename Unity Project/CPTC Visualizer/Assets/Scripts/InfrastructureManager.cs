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

    [SerializeField]
    List<TeamData> teams;

    [SerializeField]
    List<NetworkData> networks;

    public int timeBetweenAlerts = 5;

    public float timer;
    public bool simulationStart = false;

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

    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        infrastructure = new InfrastructureData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //DestroyChildren();

            ReadJson();
        }

        //Starts or ends the simulation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(simulationStart)
            {
                simulationStart = false;
                return;
            }
            timer = Time.time;
            simulationStart = true;
        }

        if(simulationStart && (Time.time >= timer + timeBetweenAlerts))
        {
            timer = Time.time;
            RunAlerts();
        }
    }

    /// <summary>
    /// Read in data from a JSON file and convert it
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
            networks.Clear();
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
    }

    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public void RunAlerts()
    {
        foreach(TeamData team in teams)
        {
            if(team.Alerts.Count > 0)
            {
                Debug.Log("ALERT: Team " + team.TeamId + " attempted " + team.Alerts[0].Type);
                team.Alerts.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Generates the graph and connects nodes. Builds an outward circular graph of networks and nodes.
    /// </summary>
    /// <param name="infrastructure"></param>
    public void GenerateGraph()
    {
        for(int i = 0; i < infrastructure.Networks.Count; i++)
        {
            float radius = 3f;
            float angle = i * Mathf.PI * 2f / infrastructure.Networks.Count;

            infrastructure.Networks[i].gameObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            infrastructure.Networks[i].gameObject.transform.localScale = new Vector2(0.5f, 0.5f);

            for(int j = 0; j < infrastructure.Networks[i].Nodes.Count; j++)
            {
                radius = 0.75f;
                angle = j * Mathf.PI * 2f / infrastructure.Networks[i].Nodes.Count;

                infrastructure.Networks[i].Nodes[j].gameObject.transform.position = new Vector3(infrastructure.Networks[i].transform.position.x + Mathf.Cos(angle) * radius, infrastructure.Networks[i].transform.position.y + Mathf.Sin(angle) * radius, 0);
                infrastructure.Networks[i].Nodes[j].gameObject.transform.localScale = new Vector2(0.15f, 0.15f);
            }
        }
    }

    public void DestroyChildren()
    {
        if(transform.childCount > 0)
        {
            foreach(Transform child in this.transform)
            {
                DestroyChildren();
            }
        }

        Destroy(gameObject);
    }
}