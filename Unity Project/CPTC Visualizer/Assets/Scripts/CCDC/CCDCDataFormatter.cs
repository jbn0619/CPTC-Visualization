using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Http;

public class CCDCDataFormatter: Singleton<CCDCDataFormatter>
{
    #region Fields
    // containers
    private DateTime lastGrab;
    private DateTime lastPull;
    private HostDataContainer dataContainer;

    // timing variables
    [SerializeField]
    private float timeBeforePulls;
    private float pullTimer;
    private bool hasStarted;

    // locations base strings
    private string pullLocation;
    private string saveLocation;
    
    // values to help get/save data properly
    private int rMinuteIndex;
    private int pMinuteIndex;
    private double delay;
    #endregion Fields
    
    #region Properties
    // The time in seconds between each pull of data
    public float TimeBetweenPulls
    {
        get { return timeBeforePulls; }
    }

    /// <summary>
    /// Used to get and set the time of the delay
    /// </summary>
    public double Delay
    {
        get { return delay; }
        set { delay = value; }
    }
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        lastGrab = DateTime.Now;
        lastPull = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the comp has started
        if (hasStarted)
        {
            // ticks down the timer every frame
            pullTimer -= Time.deltaTime;

            // checks if it's time to pull data
            if (pullTimer < 0)
            {
                // resets the pull timer
                pullTimer = timeBeforePulls;

                // gets, formats, then saves the data
                FormatData(PullData());
                SaveData();
            }
        }
    }

    /// <summary>
    /// Formats the data read into the program
    /// by the API so that the data containers
    /// can be sent to text files
    /// </summary>
    /// <param name="data"></param>
    private void FormatData(string data)
    {
        try
        {
            // makes a new daat container so the old don't interfere
            dataContainer = new HostDataContainer();
            string formattedData = data;

            // this will format the data in the HostDataContainer
            // to be called in SaveData
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    /// <summary>
    /// Save all of the formatted data into a text
    /// file that will be pulled later on by the
    /// GrabData() method from outside sources
    /// </summary>
    private void SaveData()
    {
        try
        {
            // temp var so I don't need to keep calling DateTime.Now
            DateTime target = DateTime.Now;

            // checks if it's a new minute from last pull
            if (target.Minute == lastPull.Minute)
            {
                pMinuteIndex++;
            }
            else
            {
                pMinuteIndex = 0;
            }

            // sets now to the previous
            lastPull = target;

            // defines the path to save the data to
            string path = saveLocation + "\\" + target.Day + "-" + target.Hour + "-" + target.Minute + "-" + pMinuteIndex + ".txt";

            // writes the data into a json and saves it at the location
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(dataContainer.ToJSON());
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    /// <summary>
    /// Pulls data from the API that should
    /// give all of the data needed to determine
    /// changes on the infrastructure
    /// </summary>
    /// <returns>The JSON recieved from the API</returns>
    private string PullData()
    {
        try
        {
            // temp var to hold the response
            string response = "";

            // gets the data from the API
            using (WebClient client = new WebClient())
            {
                // no pull location has been given yet
                response = client.DownloadString(pullLocation);
            }

            // returns the JSON response
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    /// <summary>
    /// Returns the data saved in a file by the formatter
    /// based on the time and the times it has been grabbed.
    /// Used in other classes for getting the saved pulled data.
    /// </summary>
    /// <returns>json of saved data</returns>
    public string GrabData()
    {
        try
        {
            // checks gets the target time
            DateTime target = DateTime.Now.AddMinutes(-delay);
            
            // Checks if it's the same minute as previous
            if (target.Minute == lastGrab.Minute)
            {
                rMinuteIndex++;
            }
            else
            {
                rMinuteIndex = 0;
            }

            // sets the previous to this
            lastGrab = target;

            // temp string to hold the data
            string data = "";

            // opens and gets all of the data
            using (StreamReader r = File.OpenText(saveLocation + "\\" + target.Day + "-" + target.Hour + "-" + target.Minute + "-" + rMinuteIndex + ".txt"))
            {
                data = r.ReadToEnd();
            }

            // returns the JSON data
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }
}

/// <summary>
/// Holds the host's ip and up state
/// </summary>
public class HostData
{
    private string IP;
    private bool state; // up = true

    public HostData(string _IP, bool _state)
    {
        IP = _IP;
        state = _state;
    }
}

/// <summary>
/// Container used to send to a JSON for
/// the host data pulled
/// </summary>
public class HostDataContainer
{
    private List<HostData> Hosts;

    public HostDataContainer()
    {
        Hosts = new List<HostData>();
    }

    /// <summary>
    /// Adds a new host to the list
    /// </summary>
    /// <param name="data">the data of the new host</param>
    public void AddHost(HostData data)
    {
        Hosts.Add(data);
    }

    /// <summary>
    /// Returns the JSON string of the list
    /// of the hosts and their states
    /// </summary>
    /// <returns>JSON string of hosts</returns>
    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }
}
