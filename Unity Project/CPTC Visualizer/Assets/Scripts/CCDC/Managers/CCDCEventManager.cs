using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CCDCEventManager: EventManager
{
    #region Fields

    [Header("Game Object Prefabs")]
    [SerializeField]
    private NotificationButton bannerGO;
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
}
