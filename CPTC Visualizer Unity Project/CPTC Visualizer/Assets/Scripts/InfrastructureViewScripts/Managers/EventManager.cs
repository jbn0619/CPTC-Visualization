using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// Author: Justin Neft
/// Function: Reads events from a JSON file, then changes the corresponding infrastructures accordingly.
/// </summary>
public class EventManager: MonoBehaviour
{
    #region Fields

    [SerializeField]
    int secondPeriodIndex;
    [SerializeField]
    Canvas notificationCanvas;

    private List<UpdateDataPacket> events;

    [SerializeField]
    private double attackDelay;

    [Header("Game Object Prefabs")]
    [SerializeField]
    private GameObject bannerGO;
    [SerializeField]
    private NotificationButton markerGO;
    [SerializeField]
    private FileManager fileManager;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the delay given to attacks' times.
    /// </summary>
    public double AttackDelay
    {
        get
        {
            return attackDelay;
        }
        set
        {
            attackDelay = value;
        }
    }

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        events = new List<UpdateDataPacket>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            LoadEventsFromJSON();
        }
    }

    /// <summary>
    /// Loads the events from events.json (if it exists), then deletes the file.
    /// </summary>
    public void LoadEventsFromJSON()
    {
        UpdateDataPacket updateData = fileManager.CreateDataFromJSON("events.json", "\\Infrastructure\\Database\\");

        Debug.Log("Data packet succesfully retrieved.");

        events.Add(updateData);
        ReadEvent();
    }

    /// <summary>
    /// Reads the information in the most recent event and changes the correspodning infrastructure as-needed.
    /// </summary>
    public void ReadEvent()
    {
        // Parse-out the event's information to figure-out where it happened.
        UpdateDataPacket packet = events[0];
        //TeamData affectedTeam = GameManager.Instance.TeamManager.Teams[packet.TeamID];
        //NodeData affectedNode = affectedTeam.InfraCopy.AllNodes[packet.NodeID];

        // Make changes to the scene based-on what happens in the event.
        ProcessEvent(packet.TeamID, packet.NodeID, packet.Type);

        // At the very end, delete the used packet from the list of events.
        events.RemoveAt(0);
    }

    /// <summary>
    /// Changes the team's infrastructure according to what happens in the event.
    /// </summary>
    /// <param name="team">The affected team.</param>
    /// <param name="node">The node where this event occured.</param>
    public void ProcessEvent(int teamID, int nodeID, String type)
    {
        Enum.TryParse(type, out CPTCEvents eventType);
        switch (eventType)
        {
            case CPTCEvents.Discovery:
                break;
            case CPTCEvents.Exploit:
                break;
            case CPTCEvents.NetworkScan:
                break;
            case CPTCEvents.ShutDown:
                ShutDownNode(nodeID);
                break;
            case CPTCEvents.StartUp:
                StartUpNode(nodeID);
                break;
            default:
                break;
        }
    }

    public void ShutDownNode(int nodeID)
    {
        GameManager.Instance.MainInfra.AllNodes[nodeID].NodeSprite.color = Color.red;
    }

    public void StartUpNode(int nodeID)
    {
        GameManager.Instance.MainInfra.AllNodes[nodeID].NodeSprite.color = Color.cyan;
    }

    // DEPRECATED METHODS
    /*
    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public void RunAlerts()
    {
        foreach (TeamData team in GameManager.Instance.TeamManager.Teams)
        {
            if (!team.Queue.IsEmpty) // team.Alerts.Count > 0
            {

            }
        }
    }

    /// <summary>
    /// Reads-in a json that summarizes all attacks logged by the red team.
    /// </summary>
    public void ReadAttacksJSON()
    {
        notificationCanvas.gameObject.SetActive(true);
        StreamReader reader = new StreamReader("C:\\ProgramData\\CSEC Visualizer\\attacks.json");
        string input = reader.ReadToEnd();
        reader.Close();

        List<UpdateDataPacket> payload = JsonUtility.FromJson<List<UpdateDataPacket>>(input);

        foreach (UpdateDataPacket packet in payload)
        {
            events.Add(packet);
        }
    }

    /// <summary>
    /// Reads a JSON of uptime-data for ever node and changes node colors/uptime charts accordingly.
    /// </summary>
    public void ReadNodeStateJSON()
    {
        string payload = DataFormatter.Instance.GrabData();

        HostDataContainer newBatch = JsonUtility.FromJson<HostDataContainer>(payload);
        int[] deltas = new int[10];

        foreach (HostData h in newBatch.Hosts)
        {
            // Find the proper node by its IP address.
            foreach (TeamData team in GameManager.Instance.TeamManager.CCDCTeams)
            {
                foreach (NodeData n in team.InfraCopy.AllNodes)
                {
                    // If the IP addresses match, then update the uptime chart.
                    if (n.Ip == h.name)
                    {
                        n.UptimeChart.UpdateData(h.state);

                        if (h.state != n.IsActive)
                        {
                            deltas[team.TeamId] += 1;
                            n.IsActive = h.state;
                            //n.UptimeChart.StateChanged();
                        }

                        if (n.IsActive)
                            n.NodeSprite.color = new Color(0.3137255f, 0.3333333f, 0.9098039f);
                        else
                            n.NodeSprite.color = new Color(0.9098039f, 0.3137255f, 0.3137255f);
                    }
                }
            }
        }

        TeamViewAI.Instance.UpdateDeltas(deltas);
    }

    /// <summary>
    /// When given an IP address string, parses-out what team that IP address is from.
    /// </summary>
    /// <param name="ip">The given IP-address as a string.</param>
    /// <returns>The IP's team as an int.</returns>
    private int FindTeamInIP(string ip)
    {
        if (ip.Length > 0)
        {
            // Cut out the beginning of the string, as it doesn't matter.
            string teamToAttack = ip.Substring(secondPeriodIndex, 3);

            // If the final character is a period, then truncate it. Otherwise, keep the character.
            if (teamToAttack[1] == '.' || teamToAttack[2] == '.')
            {
                teamToAttack = teamToAttack.Substring(0, 2);
            }
            teamToAttack = teamToAttack.Substring(0, teamToAttack.Length - 1);

            int.TryParse(teamToAttack, out int recipient);

            return recipient - 1;
        }
        return -1;
    }
    */
}
