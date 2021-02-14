using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected CompetitionType compType;

    [Header("Simulation Fields")]
    [SerializeField]
    protected int timeBetweenAlerts = 5;
    [SerializeField]
    protected float timer;
    [SerializeField]
    protected bool simulationStart = false;
    [SerializeField]
    protected bool showConnections;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Returns what-kind of competition this is.
    /// </summary>
    public CompetitionType CompType
    {
        get
        {
            return compType;
        }
    }

    protected NotificationManager notificationManager;

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Starts or ends the simulation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (simulationStart) simulationStart = false;
            else
            {
                timer = 0;
                simulationStart = true;
            }
        }

        // If the simulation is running, then update timer and see if we need to run alerts.
        if (simulationStart)
        {
            timer += Time.deltaTime;

            // If enough time has passed, run alerts and reset the timer.
            if (timer >= timeBetweenAlerts)
            {
                timer = 0;
                RunAlerts();
            }
        }
    }

    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public virtual void RunAlerts()
    {
        switch (compType)
        {
            case CompetitionType.CCDC:
                foreach (TeamData team in CCDCManager.Instance.TeamManager.Teams)
                {
                    if (!team.Queue.IsEmpty) // team.Alerts.Count > 0
                    {
                        notificationManager.CreateNotification(team.TeamId, ((AlertData)(team.Queue.Peek)).Type); // team.Alerts[0].Type
                        team.ReadNextAlert();
                    }
                }
                break;
            case CompetitionType.CPTC:
                foreach (TeamData team in CPTCManager.Instance.TeamManager.Teams)
                {
                    if (!team.Queue.IsEmpty) // team.Alerts.Count > 0
                    {
                        notificationManager.CreateNotification(team.TeamId, ((AlertData)(team.Queue.Peek)).Type); // team.Alerts[0].Type
                        team.ReadNextAlert();
                    }
                }
                break;
        }
    }
}
