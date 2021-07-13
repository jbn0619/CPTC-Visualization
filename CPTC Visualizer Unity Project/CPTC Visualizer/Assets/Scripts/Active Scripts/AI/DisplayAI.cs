using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAI: Singleton<DisplayAI>
{
    #region Fields
    [SerializeField] private float timeBeforeChange; // should be 60
    private int numOfTeams;
    private int previousTeam; // keeps track to not display twice in a row
    private int[] teamTracker; // keeps track of number of displays
    [SerializeField] private int[] teamDeltas; // keeps track of the changes in infra
    private bool hasStarted; // checks if the AI has started
    private float delay;

    #endregion Fields

    #region Properties
    public float Delay { get { return delay; } set { delay = value; } }

    public bool HasStarted { get { return hasStarted; } set { hasStarted = value; } }
    #endregion Properties

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as DisplayAI;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        numOfTeams = GameManager.Instance.TeamManager.Teams.Count;
        teamDeltas = new int[numOfTeams];
        teamTracker = new int[numOfTeams];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Old AI Code
    /// <summary>
    /// Used to get the team number that should be prioritized next on
    /// screen for the visualizer.
    /// </summary>
    /// <returns>Number of next team shown</returns>
    public int Prioritize()
    {
        int teamNum = -1;

        for (int i = 0; i < numOfTeams; i++)
        {
            // if checking agaisnt itself, don't
            if (i == teamNum || i == previousTeam)
            {
                continue;
            }

            // checks if it's the first time
            if (teamNum == -1)
            {
                teamNum = i;
            }

            // checks if the current team has higher deltas
            if (teamDeltas[i] > teamDeltas[teamNum])
            {
                if (teamTracker[i] - teamTracker[teamNum] > 4)
                {
                    continue;
                }
                teamNum = i;
            }
            else if (teamTracker[teamNum] - teamTracker[i] > 4)
            {
                teamNum = i;
            }

        }

        // sets the variables for the tracker
        Debug.Log("Sending to team " + teamNum);
        teamTracker[teamNum]++;
        return teamNum;
    }

    /// <summary>
    /// Sets all of the deltas back to zero and
    /// resets the timer for the next change
    /// </summary>
    public void ResetChanges()
    {
        // resets all of the changes from past team
        teamDeltas[previousTeam] = 0;

        // resets the timer
        timeBeforeChange = 30f;
    }

    /// <summary>
    /// Used to update the TeamViewAI's data for
    /// services that have gone up and down
    /// </summary>
    public void UpdateDeltas(int[] array)
    {
        // throws error if the array is not the same size or null
        if (array.Length != teamDeltas.Length)
        {
            throw new Exception("An error has occured: Given array does not match the size of the number of teams...");
        }
        else if (array == null)
        {
            throw new Exception("An error has occured: Given array is NULL...");
        }

        // loop that adds the deltas to the AI's tracking of them
        for (int i = 0; i < numOfTeams; i++)
        {
            teamDeltas[i] += array[i];
        }
    }
    #endregion Old AI Code
}
