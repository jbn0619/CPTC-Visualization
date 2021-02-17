using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts;

public enum CPTCEvents { Exploit, ShutDown, StartUp, Discovery, NetworkScan}

public enum NodeTypes { Host, Router}

public enum NodeState { Off, On, NotWorking}

public enum InfraGenType { Random, CCDC, CPTC}

public class JSONWriter: MonoBehaviour
{
    #region Fields

    [Header("File Options")]

    [SerializeField]
    private string fileName;

    [Header("Data Options")]

    [SerializeField]
    private InfraGenType genType;

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
        Infrastructure infra0 = null;
        switch (genType)
        {
            case InfraGenType.CCDC:
                infra0 = FixedCCDCGenerator();
                break;
            case InfraGenType.CPTC:
                infra0 = FixedCPTCGenerator();
                break;
            case InfraGenType.Random:
                infra0 = RandomGenerator();
                break;
            default:
                infra0 = RandomGenerator();
                break;
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

                int time = j * UnityEngine.Random.Range(1, 10);

                int p = 5;
                switch (e)
                {
                    case CPTCEvents.Discovery:
                        p = 3;
                        break;

                    case CPTCEvents.Exploit:
                        p = 1;
                        break;

                    case CPTCEvents.ShutDown:
                        p = 2;
                        break;

                    case CPTCEvents.StartUp:
                        p = 5;
                        break;
                }

                // List-out affected nodes.
                List<int> affectedNodes = new List<int>();
                int affectNum = UnityEngine.Random.Range(1, 5);
                for (int k = 0; k < affectNum; k++)
                {
                    affectedNodes.Add(UnityEngine.Random.Range(0, nodeCount * networkCount));
                }

                Alert a = new Alert(e, affectedNodes, p, time);

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
    /// Generates a hand-crafted infrastructure to mimic what the CCDC competition will have.
    /// </summary>
    /// <returns>The CCDC infrastructure.</returns>
    private Infrastructure FixedCCDCGenerator()
    {
        // Generates an infrastructure of 4 networks, each wtih 5 to 7 nodes.

        // Generate the core network.
        Node n0 = new Node(0, NodeTypes.Host, NodeState.On, null);
        Node n1 = new Node(1, NodeTypes.Host, NodeState.On, null);
        Node n2 = new Node(2, NodeTypes.Host, NodeState.On, null);
        Node n3 = new Node(3, NodeTypes.Host, NodeState.On, null);
        Node n4 = new Node(4, NodeTypes.Host, NodeState.On, null);
        Node n5 = new Node(5, NodeTypes.Host, NodeState.On, null);
        List<Node> coreNodes = new List<Node>();
        coreNodes.Add(n0);
        coreNodes.Add(n1);
        coreNodes.Add(n2);
        coreNodes.Add(n3);
        coreNodes.Add(n4);
        coreNodes.Add(n5);
        Assets.Scripts.Network core = new Assets.Scripts.Network(0, coreNodes, null);

        // Generate the Genovia network.
        Node n6 = new Node(6, NodeTypes.Host, NodeState.On, null);
        Node n7 = new Node(7, NodeTypes.Host, NodeState.On, null);
        Node n8 = new Node(8, NodeTypes.Host, NodeState.On, null);
        Node n9 = new Node(9, NodeTypes.Host, NodeState.On, null);
        Node n10 = new Node(10, NodeTypes.Host, NodeState.On, null);
        Node n11 = new Node(11, NodeTypes.Host, NodeState.On, null);
        Node n12 = new Node(12, NodeTypes.Host, NodeState.On, null);
        List<Node> genoviaNodes = new List<Node>();
        genoviaNodes.Add(n6);
        genoviaNodes.Add(n7);
        genoviaNodes.Add(n8);
        genoviaNodes.Add(n9);
        genoviaNodes.Add(n10);
        genoviaNodes.Add(n11);
        genoviaNodes.Add(n12);
        Assets.Scripts.Network genovia = new Assets.Scripts.Network(0, genoviaNodes, null);

        // Generate the ShangriLa network.
        
        Node n13 = new Node(13, NodeTypes.Host, NodeState.On, null);
        Node n14 = new Node(14, NodeTypes.Host, NodeState.On, null);
        Node n15 = new Node(15, NodeTypes.Host, NodeState.On, null);
        Node n16 = new Node(16, NodeTypes.Host, NodeState.On, null);
        Node n17 = new Node(17, NodeTypes.Host, NodeState.On, null);
        List<Node> shangNodes = new List<Node>();
        shangNodes.Add(n13);
        shangNodes.Add(n14);
        shangNodes.Add(n15);
        shangNodes.Add(n16);
        shangNodes.Add(n17);
        Assets.Scripts.Network shangriLa = new Assets.Scripts.Network(0, shangNodes, null);

        // Generate the Gildead network.
        Node n18 = new Node(18, NodeTypes.Host, NodeState.On, null);
        Node n19 = new Node(19, NodeTypes.Host, NodeState.On, null);
        Node n20 = new Node(20, NodeTypes.Host, NodeState.On, null);
        Node n21 = new Node(21, NodeTypes.Host, NodeState.On, null);
        Node n22 = new Node(22, NodeTypes.Host, NodeState.On, null);
        Node n23 = new Node(23, NodeTypes.Host, NodeState.On, null);
        List<Node> gileadNodes = new List<Node>();
        gileadNodes.Add(n18);
        gileadNodes.Add(n19);
        gileadNodes.Add(n20);
        gileadNodes.Add(n21);
        gileadNodes.Add(n22);
        gileadNodes.Add(n23);
        Assets.Scripts.Network gilead = new Assets.Scripts.Network(0, gileadNodes, null);

        List<Assets.Scripts.Network> networks = new List<Assets.Scripts.Network>();
        networks.Add(core);
        networks.Add(genovia);
        networks.Add(shangriLa);
        networks.Add(gilead);
        Infrastructure newInfra = new Infrastructure(networks);
        return newInfra;
    }

    /// <summary>
    /// Generates a hand-crafted infrastructure to mimic what the CPTC competition will have.
    /// </summary>
    /// <returns>The CPTC infrastructure.</returns>
    private Infrastructure FixedCPTCGenerator()
    {
        // Generate a standard infrastructure of 1 network with 5 nodes.
        nodeCount = 5;

        List<int> nc0 = new List<int>();
        nc0.Add(1);
        nc0.Add(3);
        nc0.Add(4);
        Node n0 = new Node(0, NodeTypes.Host, NodeState.On, nc0);

        List<int> nc1 = new List<int>();
        nc1.Add(0);
        nc1.Add(2);
        Node n1 = new Node(1, NodeTypes.Host, NodeState.On, nc1);

        List<int> nc2 = new List<int>();
        nc2.Add(1);
        nc2.Add(3);
        Node n2 = new Node(2, NodeTypes.Host, NodeState.On, nc2);

        List<int> nc3 = new List<int>();
        nc3.Add(0);
        nc3.Add(2);
        Node n3 = new Node(3, NodeTypes.Host, NodeState.On, nc3);

        List<int> nc4 = new List<int>();
        nc4.Add(0);
        Node n4 = new Node(4, NodeTypes.Host, NodeState.On, nc4);

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
        Infrastructure newInfra = new Infrastructure(networks);
        return newInfra;
    }

    /// <summary>
    /// Randomly generates an infrastructure with given-parameters.
    /// </summary>
    /// <returns>A random infrastructure.</returns>
    private Infrastructure RandomGenerator()
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
                int sIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(NodeState)).Length);
                NodeState s = (NodeState)sIndex;
                bool hidden = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

                // Add the new node to nodes.
                Node n = new Node(id, t, s, nodeConnections, hidden);
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
        Infrastructure newInfra = new Infrastructure(networks);
        return newInfra;
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
