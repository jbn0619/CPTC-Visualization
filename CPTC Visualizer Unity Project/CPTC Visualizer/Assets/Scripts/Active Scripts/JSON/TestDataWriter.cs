using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel - Summer 2021
/// Function: Generates phony event and team data to be used for testing. Can easily be slotted into any scene and used on its own.
/// </summary>
public class TestDataWriter: MonoBehaviour
{
    #region Fields

    [Header("Inputs")]
    [SerializeField]
    private KeyCode writeEvents = KeyCode.LeftBracket;
    [SerializeField] KeyCode writeAlerts = KeyCode.RightBracket;
    [SerializeField]
    private KeyCode writeInfra = KeyCode.I;

    [SerializeField]
    private FileManager fileManager;

    [Header("Event Data Params")]
    [SerializeField]
    private uint eventCount = 1;

    [Header("Alert Data Params")]
    [SerializeField]
    private string splunkFileName = "test_FromSplunk.json";
    [SerializeField]
    private string ControllerToScenesFileName = "test_controllerToInfraScene.json";
    [SerializeField]
    private int membersPerTeam = 6;
    [SerializeField]
    [Range(1,5)]
    private int numberOfAlertTypes = 5;

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
        if (Input.GetKeyDown(writeEvents)) WriteEventData();
        if (Input.GetKeyDown(writeAlerts)) WriteAlertData();
        if (Input.GetKeyDown(writeInfra)) WriteInfrastructure();
    }

    /// <summary>
    /// Writes test data for events to a json file.
    /// </summary>
    public void WriteEventData()
    {
        // First, figure out how many teams we have and how many nodes are in an infrastructure (total).
        //uint teamCount = (uint)GameManager.Instance.TeamManager.Teams.Count;
        uint teamCount = 10;
        //uint nodeCount = (uint)GameManager.Instance.MainInfra.AllNodes.Count;
        uint nodeCount = 10;
        List<UpdateDataPacket> packets = new List<UpdateDataPacket>();

        for (int i = 0; i < eventCount; i++)
        {
            UpdateDataPacket newPacket = new UpdateDataPacket();

            // Set where (team and node) this packet is ocurring.
            int affectedTeam = (int)UnityEngine.Random.Range(0, teamCount);
            int affectedNode = (int)UnityEngine.Random.Range(0, nodeCount);
            newPacket.TeamID = affectedTeam;
            newPacket.NodeID = affectedNode;

            // Set the current time for this packet.
            DateTime currentTime = DateTime.Now;
            newPacket.StartTime = currentTime.ToShortTimeString();

            // Set a dummy IP address and Host Name
            newPacket.HostName = "Host " + (i + 1);
            newPacket.IpAddress = "192.67." + (i + 1) + ".31";

            // Set this packet's event type.
            int eIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CPTCEvents)).Length);
            CPTCEvents e = (CPTCEvents)eIndex;
            newPacket.Type = e.ToString();

            packets.Add(newPacket);
        }

        fileManager.SaveToJSON("events.json", packets);
    }

    public void WriteInfrastructure()
    {
        Debug.Log("Writing Infrastructure to JSON");
        fileManager.SaveToJSON("infraDraft.JSON", GameManager.Instance.MainInfra);
    }

    /// <summary>
    /// Writes phony alert data for use in testing. Based on the values from Main Infra
    /// </summary>
    public void WriteAlertData()
    {
        // Check if the infrastructure exists
        if(GameManager.Instance.MainInfra != null)
        {
            InfrastructureData infra = GameManager.Instance.MainInfra;
            int nodeCount = infra.Nodes.Count - 1;
            List<AlertData> alerts = new List<AlertData>();

            // store a list of all nodes the team has not visited this round
            List<NodeData> unvisitedNodes = new List<NodeData>(infra.Nodes);

            // create an alert for every member of each team in the competition
            foreach(TeamData team in infra.Teams)
            {
                // loop through for each member of the team ...
                for(int i = 0; i < membersPerTeam; i++)
                {
                    // grab a random int to determine node this member is visiting
                    int rand = (int)UnityEngine.Random.Range(0, nodeCount);
                    // add the newly created alert data to the list for the file
                    alerts.Add(new AlertData(
                        (CPTCEvents)UnityEngine.Random.Range(0, numberOfAlertTypes),
                        unvisitedNodes[rand].Ip,
                        team.ID,
                        DateTime.Now));
                    // remove the node they visited from the list of available nodes to visit
                    unvisitedNodes.RemoveAt(rand);
                    nodeCount--;
                }
                // reset the list of available nodes to be visited
                unvisitedNodes = new List<NodeData>(infra.Nodes);
                nodeCount = infra.Nodes.Count - 1;
            }
            // TODO: make the alerts only activate in nodes the teams can accsess
            // save an alert list to a new JSON file
            fileManager.SaveToJSON(splunkFileName, alerts);
            Debug.Log($"New Dummy Splunk Data Generated\n   Number Of Alert Types: {numberOfAlertTypes}\n   Number of Teams: {infra.Teams}");
        }
    }

    /*This method is not called, and is more within the functions of FileReader
     * /// <summary>
    /// Writes update data packets to a json format.
    /// </summary>
    /// <param name="fileName">The new file's name.</param>
    /// <param name="data">The data to convert to json format.</param>
    private void SaveToJSON(string fileName, List<UpdateDataPacket> data)
    {
         string filePath = "C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\" + fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\");
        }

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                foreach (UpdateDataPacket packet in data)
                {
                    sw.WriteLine(packet.ConvertToJSON());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }*/
}
