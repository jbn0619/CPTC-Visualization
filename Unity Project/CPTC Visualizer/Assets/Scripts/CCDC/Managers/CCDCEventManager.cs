using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CCDCEventManager: EventManager
{
    #region Fields

    [SerializeField]
    int secondPeriodIndex;
    [SerializeField]
    Canvas notificationCanvas;

    private List<Assets.Scripts.CCDCAttackData> attacks;

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
        compType = CompetitionType.CCDC;
        attacks = new List<Assets.Scripts.CCDCAttackData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public override void RunAlerts()
    {
        foreach (CCDCTeamData team in CCDCManager.Instance.TeamManager.Teams)
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
        string payload = CCDCDataFormatter.Instance.GrabData();

        HostDataContainer newBatch = JsonUtility.FromJson<HostDataContainer>(payload);
        int[] deltas = new int[10];

        foreach (HostData h in newBatch.Hosts)
        {
            // Find the proper node by its IP address.
            foreach (CCDCTeamData team in CCDCManager.Instance.TeamManager.CCDCTeams)
            {
                foreach (CCDCNodeData n in team.InfraCopy.AllNodes)
                {
                    // If the IP addresses match, then update the uptime chart.
                    if (n.Ip == h.IP)
                    {
                        n.UptimeChart.UpdateData(h.state);

                        if (h.state != n.IsActive)
                        { 
                            deltas[team.TeamId] += 1;
                            n.IsActive = h.state;
                            //n.UptimeChart.StateChanged();
                        }

                        if (!n.IsActive)
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
    /// Spawns an attack button into the world when called (if the times match-up).
    /// </summary>
    public void SpawnAttack()
    {
        foreach(Assets.Scripts.CCDCAttackData attack in attacks)
        {
            DateTime delayedTime = DateTime.Now.AddMinutes(-1 * attackDelay);
            if (attack.StartTime == delayedTime.ToShortTimeString())
            {
                // Go-through each node affected and pull-out its address.
                foreach (string a in attack.NodesAffected)
                {
                    // Begin by finding-out which team we're attacking.
                    int recipient = FindTeamInIP(a);

                    if (recipient == -1) break;

                    CCDCTeamData recievingTeam = CCDCManager.Instance.TeamManager.CCDCTeams[recipient];

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

                    TeamViewButton currentButton = CCDCManager.Instance.TeamManager.TeamViewButtons[recipient];
                    newBanner.transform.SetParent(currentButton.transform, true);
                    newBanner.transform.position = currentButton.transform.position + new Vector3(-50 + (recievingTeam.NotifBanners.Count * 25), -75, 0);
                    recievingTeam.NotifBanners.Add(newBanner);

                    // Pass this banner's reference to the marker for later-destruction.
                    newMarker.CorrespondingBanner = newBanner;
                }
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

        Assets.Scripts.CCDCCompiledAttacks payload = JsonUtility.FromJson<Assets.Scripts.CCDCCompiledAttacks>(input);

        foreach (Assets.Scripts.CCDCAttackData attack in payload.attacks)
        {
            attacks.Add(attack);
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
