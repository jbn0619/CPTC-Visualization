using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TeamViewAI: MonoBehaviour
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
    #endregion Fields

    #region Properties
    public List<Inject> Injects
    {
        get { return injects; }
    }
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        // starting references
        teamTracker = new int[numOfTeams];
        teamDeltas = new int[numOfTeams];
        previousTeam = -1;

        // injects
        injects = new List<Inject>();
        ReadInjects();
    }

    // Update is called once per frame
    void Update()
    {
        // updates timer
        timeBeforeChange -= Time.deltaTime;
        
        // checks if its time 
        if (timeBeforeChange <= 0)
        {
            // temp vars to test if it's time ofr an inject
            bool injectTime = false;
            
            // loop that goes through injects
            for (int i = 0; i < injects.Count; i++)
            {
                // checks each inject
                if (CheckInjects(injects[i]))
                { 
                    // sets temp vars
                    injectTime = true;
                }
            }

            // resets the deltas
            ResetChanges();

            // checks if it's time
            if (injectTime)
            {
                // show the inject and such
                Debug.Log("STOP; INJECT TIME");
            }
            else
            {
                // gives random deltas
                RandomizeDeltas();

                // sets the previous team so that it doesn't
                // show the same team twice in a row
                previousTeam = Prioritize();

                // switches to the correct team view
                CCDCManager.Instance.TeamManager.SelectTeamView(previousTeam);
            }
        }
    }

    /// <summary>
    /// Checsk if it's the time to display an inject
    /// </summary>
    /// <returns>true/false</returns>
    public bool CheckInjects(Inject inject)
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
    public void UpdateDeltas()
    {

    }

    /// <summary>
    /// Used for testing random deltas to make sure the 
    /// views are sending properly
    /// </summary>
    public void RandomizeDeltas()
    {
        for (int i = 0; i < numOfTeams; i++)
        {
            teamDeltas[i] += UnityEngine.Random.Range(0, 9);
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

        Debug.Log("Sending to team " + teamNum);
        teamTracker[teamNum]++;
        return teamNum;
    }
}
