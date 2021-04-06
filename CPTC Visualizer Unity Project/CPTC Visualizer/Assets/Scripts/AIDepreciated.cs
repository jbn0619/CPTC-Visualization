using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldAI: MonoBehaviour
{
    #region Fields
    // for keeping count of data being sent
    private int[] numSent;
    private int teamCount;
    
    // list of teams and events
    private List<TeamData> teams;
    private List<IPriorityEvent> events;

    // object references
    private GameObject infraManager;
    
    // timers for tracking when things need to happen
    private float pullTimer;
    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // sets up the fields
        infraManager = GameObject.Find("Infrastructure Manager");
        //teamCount = imScript.Teams.Count;
        //teams = imScript.Teams;
        numSent = new int[teamCount];

        // formats them all to be 0 to start
        for (int i = 0; i < teamCount; i++)
        {
            numSent[i] = 0;
        }

        // timer between pulling data
        pullTimer = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        // update timer
        pullTimer -= Time.deltaTime;

        // check if ready to pull
        if (pullTimer < 0)
        {
            // reset timer
            pullTimer = 15f;

            // loop that gets all of the data
            for (int i = 0; i < teams.Count; i++)
            {
                // adds data to the list of these events
                AddData(teams[i].Queue.Peek);
            }

            // This will eventually be used, not sure where
            // because we need to send it to the alerts??
            SendData();
        }
    }

    /// <summary>
    /// Adds data in an ordered fashion so that the
    /// events can be sent in a streamlined version
    /// </summary>
    /// <param name="_data">the event to be added</param>
    public void AddData(IPriorityEvent _data)
    {
        // first checks if the list is empty
        if (events.Count == 0)
        {
            // adds to the defualt spot
            events.Add(_data);
        }
        else // only going to send the top five spots
        {
            // loop that places the events in the proper order
            for (int i = 0; i < events.Count; i++)
            {
                // checks to make sure that the data belongs in
                // front of element i
                if (numSent[_data.Team] <= numSent[events[i].Team] && _data.Resultant >= events[i].Resultant)
                {
                    events.Insert(i, _data);
                    return;
                }
            }

            // adds at the end cause it doesn't
            // go before other elements
            events.Add(_data);
        }

    }

    /// <summary>
    /// Sends the important events to the 
    /// alert system to be shown to casters
    /// </summary>
    /// <returns>A list of up to 5 events</returns>
    public List<IPriorityEvent> SendData()
    {
        // removes anything that isn't
        // going to be sent (not more than 5)
        int count = events.Count;
        for (int i = 5; i < count; i++)
        {
            // removes the events passed index 5
            events.RemoveAt(5);
        }

        // adds to the numSent count
        // and pops sent data
        foreach (IPriorityEvent e in events)
        {
            numSent[e.Team]++;
            teams[e.Team].Queue.Pop();
        }

        // returns the leftover list
        return events;
    }
}
