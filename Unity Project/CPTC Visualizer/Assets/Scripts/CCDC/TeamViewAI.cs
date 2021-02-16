using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamViewAI: MonoBehaviour
{
    #region Fields
    private int[] teamTracker;
    [SerializeField]
    private int[] teamDeltas;

    [SerializeField]
    private Text UpdateText;

    [SerializeField]
    private int numOfTeams;
    private float timeBeforeChange;
    private int previousTeam;
    #endregion Fields

    #region Properties

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        teamTracker = new int[numOfTeams];
        teamDeltas = new int[numOfTeams];
        previousTeam = -1;
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeChange -= Time.deltaTime;
        
        if (timeBeforeChange <= 0)
        {
            ResetChanges();

            RandomizeDeltas();

            previousTeam = Prioritize();

            UpdateText.text = "Team " + previousTeam;
            Debug.Log("Showing team " + previousTeam + " with " + teamTracker[previousTeam] + " times shown");

        }

    }

    /// <summary>
    /// Used to update the TeamViewAI's data for
    /// services that have gone up and down
    /// </summary>
    public void UpdateDeltas()
    {

    }

    public void RandomizeDeltas()
    {
        for (int i = 0; i < numOfTeams; i++)
        {
            teamDeltas[i] += Random.Range(0, 9);
        }
    }

    /// <summary>
    /// Sets all of the deltas back to zero and
    /// resets the timer for the next change
    /// </summary>
    public void ResetChanges()
    {
        for (int i = 0; i < numOfTeams; i++)
        {
            teamDeltas[i] = 0;
        }

        timeBeforeChange = 1f;
    }

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

        teamTracker[teamNum]++;
        return teamNum;
    }
}
