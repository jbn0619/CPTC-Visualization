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
        pullLocation = "C:\\ProgramData\\CSEC Visualizer\\PullData.txt";
        lastGrab = DateTime.Now;
        lastPull = DateTime.Now;
        //creds = new NetworkCredential("dataviz", "Jq7stJ&7zL35sHuxV2zp");
    
        hosts = new Dictionary<int, HostData>();
        int index = 0;
        
        // team 1
        hosts.Add(index++, new HostData("ssh-genovia-dns-team01", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team01", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team01", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team01", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team01", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team01", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team01", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team01", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team01", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team01", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team01", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team01", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team01", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team01", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team01", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team01", true));//

        // team 2
        hosts.Add(index++, new HostData("ssh-genovia-dns-team02", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team02", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team02", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team02", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team02", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team02", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team02", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team02", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team02", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team02", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team02", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team02", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team02", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team02", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team02", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team02", true));//

        // team 3
        hosts.Add(index++, new HostData("ssh-genovia-dns-team03", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team03", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team03", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team03", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team03", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team03", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team03", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team03", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team03", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team03", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team03", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team03", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team03", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team03", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team03", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team03", true));//

        // team 4
        hosts.Add(index++, new HostData("ssh-genovia-dns-team04", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team04", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team04", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team04", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team04", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team04", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team04", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team04", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team04", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team04", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team04", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team04", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team04", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team04", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team04", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team04", true));//

        // team 5
        hosts.Add(index++, new HostData("ssh-genovia-dns-team05", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team05", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team05", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team05", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team05", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team05", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team05", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team05", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team05", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team05", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team05", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team05", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team05", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team05", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team05", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team05", true));//

        // team 6
        hosts.Add(index++, new HostData("ssh-genovia-dns-team06", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team06", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team06", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team06", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team06", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team06", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team06", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team06", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team06", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team06", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team06", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team06", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team06", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team06", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team06", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team06", true));//

        // team 7
        hosts.Add(index++, new HostData("ssh-genovia-dns-team07", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team07", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team07", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team07", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team07", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team07", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team07", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team07", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team07", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team07", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team07", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team07", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team07", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team07", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team07", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team07", true));//

        // team 1
        hosts.Add(index++, new HostData("ssh-genovia-dns-team08", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team08", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team08", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team08", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team08", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team08", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team08", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team08", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team08", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team08", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team08", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team08", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team08", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team08", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team08", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team08", true));//

        // team 9
        hosts.Add(index++, new HostData("ssh-genovia-dns-team09", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team09", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team09", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team09", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team09", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team09", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team09", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team09", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team09", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team09", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team09", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team09", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team09", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team09", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team09", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team09", true));//

        // team 10
        hosts.Add(index++, new HostData("ssh-genovia-dns-team10", true));//
        hosts.Add(index++, new HostData("ldap-dc-corp-team10", true));//
        hosts.Add(index++, new HostData("ssh-genovia-openvpn-team10", true));//
        hosts.Add(index++, new HostData("ssh-genovia-eedms-team10", true));//
        hosts.Add(index++, new HostData("ssh-certificate-authority-team10", true));//
        hosts.Add(index++, new HostData("https_tor-sdapp-corp-team10", true));//
        hosts.Add(index++, new HostData("ssh-genovia-sdm-team10", true));//
        hosts.Add(index++, new HostData("https_tor-www-genovia-cloud-team10", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-mail-team10", true));//
        hosts.Add(index++, new HostData("winrm-fs-genovia-corp-team10", true));//
        hosts.Add(index++, new HostData("rdp-ws1-genovia-corp-team10", true));//
        hosts.Add(index++, new HostData("ssh-genovia-corp-wsit-team10", true));//

        hosts.Add(index++, new HostData("smtp-shangri-la-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-fs-shangri-la-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-ws1-shangri-la-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-ws2-shangri-la-corp-team10", true));//
        hosts.Add(index++, new HostData("smtp-gilead-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-fs-gilead-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-ws1-gilead-corp-team10", true));//
        hosts.Add(index++, new HostData("winrm-ws2-gilead-corp-team10", true));//
        hosts.Add(index++, new HostData("ssh-gilead-corp-wsit-team10", true));//
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the comp has started
        if (hasStarted)
        {
            // ticks down the timer every frame
            pullTimer -= UnityEngine.Time.deltaTime;

            for (int i = 0; i < hosts.Count; i++)
            {
                if (UnityEngine.Random.Range(0.0f, 100.0f) < .001)
                {
                    hosts[i].state = !hosts[i].state;
                }
            }

            // checks if it's time to pull data
            if (pullTimer < 0)
            {
                // resets the pull timer
                pullTimer = timeBeforePulls;

                // gets, formats, then saves the data
                Debug.Log(PullData(true));
                FormatData(PullData(true));
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
            data = data.Replace('\n', ' ');
            data = data.Trim();
            string[] formatData = data.Split('\r');
            
            for (int i = 0; i < formatData.Length; i++)
            {
                string[] lineData = formatData[i].Split(':');
                lineData[0] = lineData[0].Trim();

                dataContainer.AddHost(new HostData(lineData[0], bool.Parse(lineData[1])));
            }


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
            // string response
            string response = "";

            // gets the data
            using (StreamReader reader = File.OpenText(pullLocation))
            {
                response = reader.ReadToEnd();
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

    private string PullData(bool spoofed)
    {
        string response = "";

        for (int i = 0; i < hosts.Count; i++)
        {
            if (i != hosts.Count - 1)
                response += hosts[i].name + ":" + hosts[i].state + "\n\r";
            else
                response += hosts[i].name + ":" + hosts[i].state;
        }

        return response;
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
    public string name;
    public bool state; // up = true

    public HostData(string _name, bool _state)
    {
        name = _name;
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
