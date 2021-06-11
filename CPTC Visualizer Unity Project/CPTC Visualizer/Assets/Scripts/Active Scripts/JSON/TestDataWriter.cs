using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Author: Justin Neft
/// Function: Generates phony event and team data to be used for testing. Can easily be slotted into any scene and used on its own.
/// </summary>
public class TestDataWriter: MonoBehaviour
{
    #region Fields

    [Header("Inputs")]
    [SerializeField]
    private KeyCode writeEvents = KeyCode.LeftBracket;
    [SerializeField] KeyCode writeTeams = KeyCode.RightBracket;
    [SerializeField]
    private KeyCode writeInfra = KeyCode.I;

    [SerializeField]
    private FileManager fileManager;

    [Header("Event Data Params")]
    [SerializeField]
    private uint eventCount = 1;
    
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
        if (Input.GetKeyDown(writeTeams)) WriteTeamData();
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
    /// Writes phony team data for use in testing.
    /// </summary>
    public void WriteTeamData()
    {
        // TODO STUFF HERE
    }

    /// <summary>
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
    }
}
