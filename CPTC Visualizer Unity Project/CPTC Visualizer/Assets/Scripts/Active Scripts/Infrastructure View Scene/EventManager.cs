using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// Author: Justin Neft
///     Ben Wetzel - Summer 2021
/// Function: Reads events from a JSON file, then changes the corresponding infrastructures accordingly.
/// </summary>
public class EventManager: MonoBehaviour
{
    #region Fields

    [SerializeField]
    int secondPeriodIndex;
    [SerializeField]
    Canvas mainCanvas;
    // [SerializeField]
    // private List<UpdateDataPacket> events;
    /// <summary>
    /// All alerts loaded from the file.
    /// </summary>
    [SerializeField]
    private List<AlertData> loadedEvents;
    /// <summary>
    /// Alerts selected by the AI or the Production team to be displayed by the Infrastructure Scene
    /// </summary>
    [SerializeField]
    private List<AlertData> selectedEvents;
    /// <summary>
    /// List of buttons which are currently displaying an alert to the stream
    /// </summary>
    [SerializeField]
    private List<NotificationButton> displayedEvents;

    [Header("Game Object Prefabs")]
    [SerializeField]
    private GameObject bannerPrefab;
    [SerializeField]
    private NotificationButton markerPrefab;

    [Header("Manager References")]
    [SerializeField]
    private FileManager fileManager;
    [SerializeField]
    private TeamManager teamManager;

    #endregion Fields

    #region Properties

    /*Delay Time is tracked by the game manager. cut down to one variable
     * /// <summary>
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
    }*/

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        // events = new List<UpdateDataPacket>();

    }

    // Update is called once per frame
    void Update()
    {
        // display selected alerts every loop of a timer during AI runtime
        // display selected alerts every input from production team - Using Config files
    }

    #region Alert Methods
    /// <summary>
    /// Look for alerts file from Controller scene and load them into the system if they are not already loaded
    /// </summary>
    /// <param name="_alertsFile">Name of the file Alerts are stored in</param>
    /// <param name="_delay">delay in minutes the Alerts' DateTime is to be shifted forward</param>
    public void LoadAlerts(string _alertsFile, double _delay)
    {
        // grab a reference to the mainInfrastructure
        InfrastructureData mainInfra = GameManager.Instance.MainInfra;
        // pull a list of new alerts from the controler's alerts file
        List<AlertData> newAlerts = fileManager.CreateAlertsFromJSON(_alertsFile, "Alerts\\");
        // add the competition delay to new Events
        foreach(AlertData alert in newAlerts)
        {
            alert.TimeStamp = alert.TimeStamp.AddMinutes(_delay);
        }

        if (loadedEvents == null) 
        {
            loadedEvents = new List<AlertData>();
        }

        // check if new alerts should be loaded into the system
        if( loadedEvents.Count <= 0 || // have alerts not been loaded in yet this runtime? , or 
            newAlerts[newAlerts.Count - 1].TimeStamp != loadedEvents[loadedEvents.Count - 1].TimeStamp)// are the timestamps of the last alerts in the list of loaded alerts and the list of new alerts the same? (So as not to duplicate alerts which have already been loaded into the system)
        {
            // for each alert to be loaded in ...
            for (int i = 0; i < newAlerts.Count; i++)
            {
                // set a reference to the version of the node within the main architecture
                newAlerts[i].MainNode = teamManager.Teams[0].Infra.FindNodeObjectByIP(newAlerts[i].NodeIP).GetComponent<NodeData>(); // switch from the main infrastructure until infrastructure's deep cpoy is completed
                // add the team's ID number to the node's list of team ID numbers
                newAlerts[i].MainNode.TeamIDs.Add(newAlerts[i].TeamID);
                // add the team to the node's list of teams 
                newAlerts[i].MainNode.Teams.Add(teamManager.Teams[newAlerts[i].TeamID]);

                // set the alert's team to a reference of that team in the teamManager
                newAlerts[i].Team = teamManager.Teams[newAlerts[i].TeamID];
                // add the alert to the list of alerts its team is tracking
                newAlerts[i].Team.Alerts.Add(newAlerts[i]);
                // add the node to the team's list of current nodes
                newAlerts[i].Team.NodeIPs.Add(newAlerts[i].NodeIP);

                // set a reference to the version of the node within the team's architecture
                newAlerts[i].TeamNode = newAlerts[i].Team.Infra.FindNodeObjectByIP(newAlerts[i].NodeIP).GetComponent<NodeData>();
                // add the team's ID number to the node's list of team ID numbers
                newAlerts[i].TeamNode.TeamIDs.Add(newAlerts[i].TeamID);
                // add a reference to the team in the version of the node within the team's infrastructure
                newAlerts[i].TeamNode.Teams.Add(newAlerts[i].Team);

                // add the new alert to the list of alerts loaded into the system
                loadedEvents.Add(newAlerts[i]);
            }
            Debug.Log($"New Events Loaded from {_alertsFile}");
            DisplaySelectedAlerts(); // Move later once manual controls are set up between controller and infra scenes
        }
    }
    public void LoadPriorityAlerts()
    {
        // move alerts from loaded list to display based on if the AI or Human Operators indicated to do so, depending on if the stream is in off-ari mode or on-air mode
    }
    /// <summary>
    /// Display alert notifications chosen by Production team (on-air time) or AI (off-air time) at their nodes
    /// </summary>
    public void DisplaySelectedAlerts()
    {
        // for now, just set all alerts to be selected. In the future this would pull from the AI or the Human's selections
        selectedEvents = loadedEvents;
        // take list of Alerts and create a notification for each of them. 
        foreach(AlertData alert in selectedEvents)
        {
            // instance a new notification button
            displayedEvents.Add(Instantiate(markerPrefab, transform));
            // make a simplified reference to the notification component of the instanced object
            NotificationButton notif = displayedEvents[displayedEvents.Count - 1].GetComponent<NotificationButton>();
            notif.Team = alert.Team;
            displayedEvents[displayedEvents.Count - 1].GetComponent<SpriteRenderer>().color = alert.Team.TeamColor;
            notif.Node = alert.MainNode;
            // display the node's updated pie chart
            notif.Node.SplitSprite();
            // add the button to its team's list of buttons
            alert.Team.NotifMarkers.Add(notif);
            // set the notification's name
            notif.name = $"{alert.Team.name} at {alert.MainNode.name}";
            // add this alert to the notification button
            notif.Alert = alert;
            // Set the position of the notification to be the position of the node.
            notif.gameObject.transform.position = notif.Node.gameObject.transform.position;
        }
    }
    #endregion Alert Methods

    /* Used old Event system. Outdated data structures
     * /// <summary>
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
    /// <param name="_teamID">The affected team.</param>
    /// <param name="_nodeID">The node where this event occured.</param>
    public void ProcessEvent(int _teamID, int _nodeID, String _type)
    {
        Enum.TryParse(_type, out CPTCEvents eventType);
        switch (eventType)
        {
            case CPTCEvents.Discovery:
                break;
            case CPTCEvents.Exploit:
                break;
            case CPTCEvents.NetworkScan:
                break;
            case CPTCEvents.ShutDown:
                ShutDownNode(_nodeID);
                break;
            case CPTCEvents.StartUp:
                StartUpNode(_nodeID);
                break;
            default:
                break;
        }
    }

    public void ShutDownNode(int _nodeID)
    {
        GameManager.Instance.MainInfra.FindNodeDataByID(_nodeID).NodeSprite.color = Color.red;
    }

    public void StartUpNode(int _nodeID)
    {
        GameManager.Instance.MainInfra.FindNodeDataByID(_nodeID).NodeSprite.color = Color.cyan;
    }*/

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
