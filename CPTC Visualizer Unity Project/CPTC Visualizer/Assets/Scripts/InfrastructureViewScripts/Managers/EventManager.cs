using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

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
        
    }

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
    /// Reads the information in the most recent event and changes the correspodning infrastructure as-needed.
    /// </summary>
    public void ReadEvent()
    {
        // Parse-out the event's information to figure-out where it happened.
        UpdateDataPacket packet = events[0];
        TeamData affectedTeam = GameManager.Instance.TeamManager.Teams[packet.TeamID];
        NodeData affectedNode = affectedTeam.InfraCopy.AllNodes[packet.NodeID];

        // Make changes to the scene based-on what happens in the event.
        ProcessEvent(affectedTeam, affectedNode);

        // At the very end, delete the used packet from the list of events.
        events.RemoveAt(0);
    }

    /// <summary>
    /// Changes the team's infrastructure according to what happens in the event.
    /// </summary>
    /// <param name="team">The affected team.</param>
    /// <param name="node">The node where this event occured.</param>
    public void ProcessEvent(TeamData team, NodeData node)
    {

    }

    /*
    /// <summary>
    /// Spawns an attack button into the world when called (if the times match-up).
    /// </summary>
    public void SpawnAttack()
    {
        foreach(UpdateDataPacket packet in events)
        {
            DateTime delayedTime = DateTime.Now.AddMinutes(-1 * attackDelay);
            if (packet.StartTime == delayedTime)
            {
                // Go-through each node affected and pull-out its address.
                foreach (char a in packet.HostName)
                {
                    // Begin by finding-out which team we're attacking.
                    int recipient = FindTeamInIP(a);

                    if (recipient == -1) break;

                    TeamData recievingTeam = GameManager.Instance.TeamManager.CCDCTeams[recipient];

                    // Next, find the id of the node we're attacking.
                    int nodeIndex = 0;
                    for (int i = 0; i < recievingTeam.InfraCopy.AllNodes.Count; i++)
                    {
                        if (recievingTeam.InfraCopy.AllNodes[i].Ip == a)
                        {
                            nodeIndex = i;
                            break;
                        }
                    }

                    // Spawn a notification marker in the proper spot.
                    NotificationButton newMarker = Instantiate(markerGO, notificationCanvas.transform);
                    Enum.TryParse(attack.AttackType, out CCDCAttackType myAttack);
                    Vector3 newPos = recievingTeam.InfraCopy.AllNodes[nodeIndex].gameObject.transform.position + new Vector3(0, .3f, -3);
                    newMarker.transform.position = newPos;
                    newMarker.AttackType = myAttack;
                    newMarker.AffectedNodeID = nodeIndex;
                    newMarker.AffectedTeamID = recipient;

                    recievingTeam.NotifMarkers.Add(newMarker);

                    // Disable this marker so that it can be properly-revealed later-on.
                    newMarker.gameObject.SetActive(false);

                    // Spawn-in a notification banner under this team's button.
                    GameObject newBanner = Instantiate(bannerGO);

                    TeamViewButton currentButton = GameManager.Instance.TeamManager.TeamViewButtons[recipient];
                    newBanner.transform.SetParent(currentButton.transform, true);
                    newBanner.transform.position = currentButton.transform.position + new Vector3(-50 + (recievingTeam.NotifBanners.Count * 25), -75, 0);
                    recievingTeam.NotifBanners.Add(newBanner);

                    // Pass this banner's reference to the marker for later-destruction.
                    newMarker.CorrespondingBanner = newBanner;
                }
            }
        }
    }
    */

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
}
