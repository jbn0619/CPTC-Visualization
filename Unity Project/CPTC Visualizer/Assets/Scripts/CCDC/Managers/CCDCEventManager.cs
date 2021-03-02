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

    [Header("Game Object Prefabs")]
    [SerializeField]
    private GameObject bannerGO;
    [SerializeField]
    private NotificationButton markerGO;

    #endregion Fields

    #region Properties

    

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        compType = CompetitionType.CCDC;
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        if (Input.GetKeyDown(KeyCode.P))
        {
            ReadAttacksJSON();

            UIManager.Instance.SceneCanvases[2].gameObject.SetActive(true);
        }
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
        StreamReader reader = new StreamReader("Assets/Data/nodeState.json");
        string input = reader.ReadToEnd();
        reader.Close();

        // THIS IS PSEUDO-CODE MEANT TO OUTLINE WHAT NODE-STATE READING WILL LOOK LIKE
        /*
        foreach (CCDCTeamData team in CCDCManager.Instance.TeamManager.Teams)
        {
            for (int i = 0; i < payload.Nodes.Count; i++) 
            {
                team.UptimeCharts[i].UpdateData(payload.Nodes[i].IsOn);
            }
        }
        */

        // READ NODE STATES HERE
        throw new System.Exception("TODO");
    }

    /// <summary>
    /// Reads-in a json that summarizes all attacks logged by the red team.
    /// </summary>
    public void ReadAttacksJSON()
    {
        StreamReader reader = new StreamReader("Assets/Data/attacks.json");
        string input = reader.ReadToEnd();
        reader.Close();

        Assets.Scripts.CCDCCompiledAttacks payload = JsonUtility.FromJson<Assets.Scripts.CCDCCompiledAttacks>(input);

        foreach (Assets.Scripts.CCDCAttackData attack in payload.attacks)
        {
            // Go-through each node affected and pull-out its address.
            foreach (string a in attack.NodesAffected)
            {
                // Begin by finding-out which team we're attacking.
                int recipient = FindTeamInIP(a);

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
                Vector3 newPos = recievingTeam.InfraCopy.AllNodes[nodeIndex].gameObject.transform.position + new Vector3(0, 0.25f, 0);
                newMarker.transform.position = newPos;
                newMarker.transform.localScale = new Vector3(0.25f, 0.25f, 1);
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
                newBanner.transform.position = currentButton.transform.position + new Vector3(-50 + (recievingTeam.NotifBanners.Count * 15), -75, 0);
                newBanner.transform.localScale = new Vector3(10, 10, 1);
                recievingTeam.NotifBanners.Add(newBanner);

                // Pass this banner's reference to the marker for later-destruction.
                newMarker.CorrespondingBanner = newBanner;
            }
        }
    }

    /// <summary>
    /// When given an IP address string, parses-out what team that IP address is from.
    /// </summary>
    /// <param name="ip">The given IP-address as a string.</param>
    /// <returns>The IP's team as an int.</returns>
    private int FindTeamInIP(string ip)
    {
        // Cut out the beginning of the string, as it doesn't matter.
        string teamToAttack = ip.Substring(secondPeriodIndex, 3);

        // If the final character is a period, then truncate it. Otherwise, keep the character.
        if (teamToAttack[2] == '.')
        {
            teamToAttack = teamToAttack.Substring(0, 2);
        }
        teamToAttack = teamToAttack.Substring(1, teamToAttack.Length - 1);

        int.TryParse(teamToAttack, out int recipient);

        return recipient;
    }
}
