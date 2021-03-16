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
    private Dictionary<int, HostData> hosts;

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
    private double delay = 1;

    // credentials
    //public NetworkCredential creds;
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

    public bool HasStart
    {
        set { hasStarted = value; }
    }
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        pMinuteIndex = -1;
        rMinuteIndex = -1;
        saveLocation = "C:\\ProgramData\\CSEC Visualizer\\TestData";
        pullLocation = "https://elasticsearch4.newscrier.org/";
        lastGrab = DateTime.Now;
        lastPull = DateTime.Now;
        //creds = new NetworkCredential("dataviz", "Jq7stJ&7zL35sHuxV2zp");
    //}
        hosts = new Dictionary<int, HostData>();
        
        // team 1
        hosts.Add(0, new HostData("172.17.1.1", true));
        hosts.Add(1, new HostData("172.17.1.2", true));
        hosts.Add(2, new HostData("172.17.1.3", true));
        hosts.Add(3, new HostData("172.16.1.4", true));
        hosts.Add(4, new HostData("172.16.1.5", true));
        hosts.Add(5, new HostData("172.16.1.6", true));
        
        // team 2
        hosts.Add(6, new HostData("172.17.2.1", true));
        hosts.Add(7, new HostData("172.17.2.2", true));
        hosts.Add(8, new HostData("172.17.2.3", true));
        hosts.Add(9, new HostData("172.16.2.4", true));
        hosts.Add(10, new HostData("172.16.2.5", true));
        hosts.Add(11, new HostData("172.16.2.6", true));
        
        // team 3
        hosts.Add(12, new HostData("172.17.3.1", true));
        hosts.Add(13, new HostData("172.17.3.2", true));
        hosts.Add(14, new HostData("172.17.3.3", true));
        hosts.Add(15, new HostData("172.16.3.4", true));
        hosts.Add(16, new HostData("172.16.3.5", true));
        hosts.Add(17, new HostData("172.16.3.6", true));
        
        // team 4
        hosts.Add(18, new HostData("172.17.4.1", true));
        hosts.Add(19, new HostData("172.17.4.2", true));
        hosts.Add(20, new HostData("172.17.4.3", true));
        hosts.Add(21, new HostData("172.16.4.4", true));
        hosts.Add(22, new HostData("172.16.4.5", true));
        hosts.Add(23, new HostData("172.16.4.6", true));
        
        // team 5
        hosts.Add(24, new HostData("172.17.5.1", true));
        hosts.Add(25, new HostData("172.17.5.2", true));
        hosts.Add(26, new HostData("172.17.5.3", true));
        hosts.Add(27, new HostData("172.16.5.4", true));
        hosts.Add(28, new HostData("172.16.5.5", true));
        hosts.Add(29, new HostData("172.16.5.6", true));
        
        // team 6
        hosts.Add(30, new HostData("172.17.6.1", true));
        hosts.Add(31, new HostData("172.17.6.2", true));
        hosts.Add(32, new HostData("172.17.6.3", true));
        hosts.Add(33, new HostData("172.16.6.4", true));
        hosts.Add(34, new HostData("172.16.6.5", true));
        hosts.Add(35, new HostData("172.16.6.6", true));
        
        // team 7
        hosts.Add(36, new HostData("172.17.7.1", true));
        hosts.Add(37, new HostData("172.17.7.2", true));
        hosts.Add(38, new HostData("172.17.7.3", true));
        hosts.Add(39, new HostData("172.16.7.4", true));
        hosts.Add(40, new HostData("172.16.7.5", true));
        hosts.Add(41, new HostData("172.16.7.6", true));
        
        // team 8
        hosts.Add(42, new HostData("172.17.8.1", true));
        hosts.Add(43, new HostData("172.17.8.2", true));
        hosts.Add(44, new HostData("172.17.8.3", true));
        hosts.Add(45, new HostData("172.16.8.4", true));
        hosts.Add(46, new HostData("172.16.8.5", true));
        hosts.Add(47, new HostData("172.16.8.6", true));
        
        // team 9
        hosts.Add(48, new HostData("172.17.9.1", true));
        hosts.Add(49, new HostData("172.17.9.2", true));
        hosts.Add(50, new HostData("172.17.9.3", true));
        hosts.Add(51, new HostData("172.16.9.4", true));
        hosts.Add(52, new HostData("172.16.9.5", true));
        hosts.Add(53, new HostData("172.16.9.6", true));
        
        // team 10
        hosts.Add(54, new HostData("172.17.10.1", true));
        hosts.Add(55, new HostData("172.17.10.2", true));
        hosts.Add(56, new HostData("172.17.10.3", true));
        hosts.Add(57, new HostData("172.16.10.4", true));
        hosts.Add(58, new HostData("172.16.10.5", true));
        hosts.Add(59, new HostData("172.16.10.6", true));
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the comp has started
        if (hasStarted)
        {
            // ticks down the timer every frame
            pullTimer -= UnityEngine.Time.deltaTime;

            // checks if it's time to pull data
            if (pullTimer < 0)
            {
                // resets the pull timer
                pullTimer = timeBeforePulls;

                // gets, formats, then saves the data
                Debug.Log(PullData());
                //FormatData(PullData());
                SaveData(PullData());
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

            // First check if the directory exists, or if we need to make it.
            if (Directory.Exists(saveLocation) == false)
            {
                Directory.CreateDirectory(saveLocation);
            }

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

    private void SaveData(string JSON)
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

            // First check if the directory exists, or if we need to make it.
            if (Directory.Exists(saveLocation) == false)
            {
                Directory.CreateDirectory(saveLocation);
            }

            // defines the path to save the data to
            string path = saveLocation + "\\" + target.Day + "-" + target.Hour + "-" + target.Minute + "-" + pMinuteIndex + ".txt";

            // writes the data into a json and saves it at the location
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(JSON);
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
            
            string response = "";
            
            HostDataContainer container = new HostDataContainer();
            
            for (int i = 0; i < hosts.Count; i++)
            {
                float roll = UnityEngine.Random.Range(0, 1.0f);
            
                if (roll <= .01)
                {
                    hosts[i].state = !hosts[i].state;
                }
            
                container.AddHost(hosts[i]);
            }
            
            response = container.ToJSON();
            
            return response;
            
            // temp var to hold the response
            string respons = "";

            // gets the data from the API

            //ElasticLowLevelClient client = new ElasticLowLevelClient();
             
            //client.Credentials = creds;
                //client.Headers = new WebHeaderCollection();
                //client.Headers.Add(HttpResponseHeader.WwwAuthenticate, "dataviz:Jq7stJ&7zL35sHuxV2zp");

            string creds = "dataviz:Jq7stJ&7zL35sHuxV2zp";
            //client.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", creds);
            //ElasticClient client = new ElasticClient();

            

                // no pull location has been given yet
                //response = client.DownloadString(pullLocation);
            

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

            // First check if the directory exists, or if we need to make it.
            if (Directory.Exists(saveLocation) == false)
            {
                Directory.CreateDirectory(saveLocation);
            }

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
[Serializable]
public class HostData
{
    public string IP;
    public bool state; // up = true

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
[Serializable]
public class HostDataContainer
{
    public List<HostData> Hosts;

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
