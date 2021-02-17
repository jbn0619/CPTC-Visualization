using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDCEventManager: EventManager
{
    #region Fields

    private List<UptimeChartData> uptimeCharts;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of all the uptime charts in this scene.
    /// </summary>
    public List<UptimeChartData> UptimeCharts
    {
        get
        {
            return uptimeCharts;
        }
    }

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
        foreach (TeamData team in CCDCManager.Instance.TeamManager.Teams)
        {
            if (!team.Queue.IsEmpty) // team.Alerts.Count > 0
            {
                team.ReadNextAlert();
            }
        }
    }
}
