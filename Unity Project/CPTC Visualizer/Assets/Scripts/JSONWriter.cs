using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private int maxTeams;

    [SerializeField]
    private int maxNodes;

    // This is in alerts per team.
    [SerializeField]
    private int maxAlerts;

    private int teamCount;
    private int nodeCount;

    // This is in alerts per team.
    private int alertCount;
    
    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // Alerts hold event types

        // Teams hold discovered node IDs and alerts

        // Entire infrastructure holds nodes

        // Nodes hold node IDs, type, and connections.

        List<Alert> alerts = new List<Alert>();
        alerts.Add(new Alert(CPTCEvents.Discovery));
        alerts.Add(new Alert(CPTCEvents.ShutDown));

        List<int> c = new List<int>();
        c.Add(1);
        c.Add(2);
        List<Node> nodes = new List<Node>();
        nodes.Add(new Node(0, NodeTypes.Host, c));
        Team myTeam = new Team(4, alerts, nodes);

        Debug.Log(myTeam.ConvertToJSON());

        // Make an infrastructure
        List<Assets.Scripts.Network> networks = new List<Assets.Scripts.Network>();
        List<int> netC = new List<int>();
        netC.Add(1);
        netC.Add(3);
        netC.Add(2);
        Assets.Scripts.Network net = new Assets.Scripts.Network(2, nodes, netC);
        networks.Add(net);

        Infrastructure infra = new Infrastructure(networks);

        Debug.Log(infra.ConvertToJSON());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates Json-formatted data based-on the fields for this script.
    /// </summary>
    public void GenerateData()
    {
        // Set up how much of each data type there will be.
        alertCount = Random.Range(1, maxAlerts);
        teamCount = Random.Range(1, maxTeams);
        nodeCount = Random.Range(1, maxNodes);

        // First, check if we're randomizing the node infrastructure or not.
        if (randomizeInfrastructure == false)
        {
            // Generate a standard infrastructure of 5 nodes.
            nodeCount = 5;

            
            // Generate nodes.
            for (int i = 0; i < nodeCount; i++)
            {

            }
        }
        else
        {

        }
    }

    /// <summary>
    /// Savees any generated data to a Json file.
    /// </summary>
    private void SaveToJson()
    {

    }
}
