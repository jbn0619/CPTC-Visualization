using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPTCEventManager : EventManager
{
    #region Fields



    #endregion Fields

    #region Properties

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        compType = CompetitionType.CPTC;
        notificationManager = CPTCManager.Instance.NotifManager;
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
        foreach (TeamData team in CPTCManager.Instance.TeamManager.Teams)
        {
            if (!team.Queue.IsEmpty) // team.Alerts.Count > 0
            {
                notificationManager.CreateNotification(team.TeamId, ((AlertData)(team.Queue.Peek)).Type); // team.Alerts[0].Type
                team.ReadNextAlert();
            }
        }
    }
}
