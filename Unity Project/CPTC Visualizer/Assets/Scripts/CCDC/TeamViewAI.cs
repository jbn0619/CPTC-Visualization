using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TeamViewAI: Singleton<TeamViewAI>
{
    #region Fields
    // other references
    private List<Inject> injects; // list of injects for the comp

    // team references
    [SerializeField]
    private int numOfTeams; // should be ten for now
    private float timeBeforeChange; // should be 60
    private int previousTeam; // keeps track to not display twice in a row
    private int[] teamTracker; // keeps track of number of displays
    [SerializeField]
    private int[] teamDeltas; // keeps track of the changes in infra
    private bool hasStarted; // checks if the AI has started
    private float testTimer;
    #endregion Fields

    #region Properties
    /// <summary>
    /// List of all the Injects in the competition
    /// </summary>
    public List<Inject> Injects
    {
        get { return injects; }
    }
    #endregion Properties

    // Update is called once per frame
    void LateUpdate()
    {
        // checks if the AI has started
        if (hasStarted)
        {
            // updates timer
            timeBeforeChange -= Time.deltaTime;
            testTimer -= Time.deltaTime;

            // randomizes the deltas, only for testing
            if (testTimer <= 0)
            {
                // gives random deltas
                RandomizeDeltas();

                testTimer = 15f;
            }

            // checks if its time 
            if (timeBeforeChange <= 0)
            {
                // temp vars to test if it's time ofr an inject
                bool injectTime = false;

                // loop that goes through injects
                for (int i = 0; i < injects.Count; i++)
                {
                    // checks each inject
                    if (CheckInject(injects[i]))
                    {
                        // sets temp vars
                        injectTime = true;
                    }
                }

                // checks if it's time
                if (injectTime)
                {
                    // show the inject and such
                    Debug.Log("STOP; INJECT TIME");
                }
                else
                {
                    // sets the previous team so that it doesn't
                    // show the same team twice in a row
                    previousTeam = Prioritize();
                    
                    // resets the delta of chosen team
                    ResetChanges();

                    // switches to the correct team view
                    CCDCManager.Instance.TeamManager.SelectTeamView(previousTeam);
                }
            }
        }
    }

    /// <summary>
    /// Checsk if it's the time to display an inject
    /// </summary>
    /// <returns>true/false</returns>
    public bool CheckInject(Inject inject)
    {
        return DateTime.Now.ToShortTimeString() == inject.Timestamp ? true : false;
    }

    /// <summary>
    /// Reads in and creates the injects for the
    /// competition screens
    /// </summary>
    public void ReadInjects()
    {
        // path of injects file
        string path = "Assets\\Data\\injects.txt";

        // checks if the file exists
        if (File.Exists(path))
        {
            // opens a stream reader
            using (StreamReader r = File.OpenText(path))
            {
                // try just in case of error
                try
                {
                    // temp vars for reading in proper data
                    string line = r.ReadLine();
                    int pos = 0;
                    string time = "";
                    string name = "";
                    string description = "";
                    float estTime = 0;

                    // loop that reads in all the data
                    while (line != null && line != "")
                    {
                        // skips the comments/blank lines
                        if (line[0] == '#')
                        {
                            line = r.ReadLine();
                            continue;
                        }

                        // First in the inject, name
                        if (pos == 0)
                        {
                            name = line;
                            pos++;
                        }
                        // second in the inject, description
                        else if (pos == 1)
                        {
                            description = line;
                            pos++;
                        }
                        // third, timestamp
                        else if (pos == 2)
                        {
                            time = line;
                            pos++;
                        }
                        // forth, duration, then resets the vars
                        else if (pos == 3)
                        {
                            estTime = float.Parse(line);
                            injects.Add(new Inject(name, description, time, estTime));
                            Debug.Log("Added new inject: " + name);
                            time = "";
                            name = "";
                            description = "";
                            estTime = 0;
                            pos = 0;
                        }
                        else
                        {
                            // uh oh, this shouldn't happen :<
                            Debug.LogError("ERROR: Reading in injects failed. Check the file format!");
                        }

                        // gets the next line
                        line = r.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    // displays the error
                    Debug.LogError(e.Message);
                }
            }
        }
        else
        {
            // Why isn't the file there???
            Debug.LogError("ERROR: Reading in injects failed. The file does not exist!");
        }
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

    /// <summary>
    /// Used for testing random deltas to make sure the 
    /// views are sending properly
    /// </summary>
    public void RandomizeDeltas()
    {
        // loop that spoofs data
        for (int i = 0; i < numOfTeams; i++)
        {
            teamDeltas[i] += UnityEngine.Random.Range(0, 3);
        }
    }

    /// <summary>
    /// Starts the ai when the competition starts
    /// </summary>
    public void BeginComp()
    {
        // starting references
        teamTracker = new int[numOfTeams];
        teamDeltas = new int[numOfTeams];
        previousTeam = -1; // starting value

        // injects
        injects = new List<Inject>();
        ReadInjects(); // reads in the injects.txt
        hasStarted = true;
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
}
