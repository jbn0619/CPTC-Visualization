using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts;

public enum CPTCEvents { Exploit, ShutDown, StartUp, Discovery}

public enum NodeTypes { Host, Router}

public class JSONWriter: MonoBehaviour
{
    #region Fields

    [Header("File Options")]

    [SerializeField]
    private string fileName;

    [Header("Data Options")]

    [SerializeField]
    private bool randomizeInfrastructure;

    // This is in alerts per team.
    [SerializeField]
    [Range(1, 100)]
    private int alertCount;

    [SerializeField]
    [Range(1, 100)]
    private int teamCount;

    // This is in nodes per network.
    [SerializeField]
    [Range(1, 100)]
    private int nodeCount;

    [SerializeField]
    [Range(1, 100)]
    private int networkCount;

    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets if the writer will randomize the infrastructure or not.
    /// </summary>
    public bool RandomizeInfrastructure
    {
        get
        {
            return randomizeInfrastructure;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateData();
        }
    }

    /// <summary>
    /// Generates Json-formatted data based-on the fields for this script.
    /// </summary>
    public void GenerateData()
    {
        // Create an infrastructure either randomly or standardly.
        Infrastructure infra0;
        if (randomizeInfrastructure == false)
        {
            // Generate a standard infrastructure of 1 network with 5 nodes.
            nodeCount = 5;

            List<int> nc0 = new List<int>();
            nc0.Add(1);
            nc0.Add(3);
            nc0.Add(4);
            Node n0 = new Node(0, NodeTypes.Host, nc0);

            List<int> nc1 = new List<int>();
            nc1.Add(0);
            nc1.Add(2);
            Node n1 = new Node(1, NodeTypes.Host, nc1);

            List<int> nc2 = new List<int>();
            nc2.Add(1);
            nc2.Add(3);
            Node n2 = new Node(2, NodeTypes.Host, nc2);

            List<int> nc3 = new List<int>();
            nc3.Add(0);
            nc3.Add(2);
            Node n3 = new Node(3, NodeTypes.Host, nc3);

            List<int> nc4 = new List<int>();
            nc4.Add(0);
            Node n4 = new Node(4, NodeTypes.Host, nc4);

            List<Node> nodes = new List<Node>();
            nodes.Add(n0);
            nodes.Add(n1);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);

            List<int> netC0 = new List<int>();
            Assets.Scripts.Network net1 = new Assets.Scripts.Network(0, nodes, netC0);
            List<Assets.Scripts.Network> networks = new List<Assets.Scripts.Network>();
            networks.Add(net1);
            infra0 = new Infrastructure(networks);
        }
        else
        {
            // Create each network first before making the full infrastructure object.
            int globalNodeCount = 0;
            List<Assets.Scripts.Network> networks = new List<Assets.Scripts.Network>();
            for (int i = 0; i < networkCount; i++)
            {
                // Create the nodes to be used in this network.
                List<Node> nodes = new List<Node>();
                for (int j = 0; j < nodeCount; j++)
                {
                    int id = globalNodeCount;
                    globalNodeCount++;

                    // Randomly determine how many connections this node will have, then generate them.
                    int conNum = UnityEngine.Random.Range(1, 5);
                    List<int> nodeConnections = new List<int>();
                    for (int k = 0; k < conNum; k++)
                    {
                        // There are a few pieces of criteria for a connection to get added:
                        // 1) The connection is not to itself.
                        // 2) The connection is not a repeat.
                        int nextCon = UnityEngine.Random.Range((i * nodeCount), nodeCount * (i + 1));

                        if (nextCon != id && nodeConnections.Contains(nextCon) == false)
                        {
                            nodeConnections.Add(nextCon);
                        }
                    }

                    // Randomly determine this node's type.
                    int tIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(NodeTypes)).Length);
                    NodeTypes t = (NodeTypes)tIndex;

                    // Add the new node to nodes.
                    Node n = new Node(id, t, nodeConnections);
                    nodes.Add(n);
                }

                // Generate network connections.
                int netConNum = UnityEngine.Random.Range(1, 5);
                List<int> netCon = new List<int>();
                for (int l = 0; l < netConNum; l++)
                {
                    int newCon = UnityEngine.Random.Range(0, networkCount);

                    if (newCon != i && netCon.Contains(newCon) == false)
                    {
                        netCon.Add(newCon);
                    }
                }

                // Add the new network to networks.
                Assets.Scripts.Network net = new Assets.Scripts.Network(i, nodes, netCon);
                networks.Add(net);
            }

            // Create the infrastructure.
            infra0 = new Infrastructure(networks);
        }

        // Next create each team and their alerts.
        List<Team> teams = new List<Team>();
        for (int i = 0; i < teamCount; i++)
        {
            // Create any IDs for nodes this team has discovered.
            List<int> dNodes = new List<int>();
            dNodes.Add(0);

            // Create the alerts here.
            List<Alert> alerts = new List<Alert>();
            for (int j = 0; j < alertCount; j++)
            {
                // Grabs a random value from the CPTCEvents enum type.
                int eIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CPTCEvents)).Length);
                CPTCEvents e = (CPTCEvents)eIndex;
                Alert a = new Alert(e);

                alerts.Add(a);
            }

            // Create each team object with their alerts and discoveredNodes information
            Team t = new Team(i, alerts, dNodes);
            teams.Add(t);
        }

        CPTCData finalData = new CPTCData(infra0, teams);

        SaveToJson(finalData);
    }

    /// <summary>
    /// Saves any generated data to a Json file.
    /// </summary>
    private void SaveToJson(CPTCData data)
    {
        string filePath = "Assets/Data/" + fileName;

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(data.ConvertToJSON());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #region UI Methods

    /// <summary>
    /// Flips the randomizeInfrastructure bool when the toggle button is clicked.
    /// </summary>
    public void FlipRandomInfra()
    {
        randomizeInfrastructure = !randomizeInfrastructure;
    }

    /// <summary>
    /// Edits the number of alerts per team based on its corresponding inputField.
    /// </summary>
    /// <param name="sender">The inputField that was changed.</param>
    public void EditAlertCount(InputField sender)
    {
        int.TryParse(sender.text, out alertCount);
    }

    /// <summary>
    /// Edits the number of teams based on its corresponding inputField.
    /// </summary>
    /// <param name="sender">The inputField that was changed.</param>
    public void EditTeamCount(InputField sender)
    {
        int.TryParse(sender.text, out teamCount);
    }

    /// <summary>
    /// Edits the number of nodes per network based on its corresponding inputField.
    /// </summary>
    /// <param name="sender">The inputField that was changed.</param>
    public void EditNodeCount(InputField sender)
    {
        int.TryParse(sender.text, out nodeCount);
    }

    /// <summary>
    /// Edits the number of networks based on its corresponding inputField.
    /// </summary>
    /// <param name="sender">The inputField that was changed.</param>
    public void EditNetworkCount(InputField sender)
    {
        int.TryParse(sender.text, out networkCount);
    }

    #endregion UI Methods
}
