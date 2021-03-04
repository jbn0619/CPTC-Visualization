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
    [SerializeField]
    private float timeBeforePulls;
    private DateTime lastGrab;

    private float pullTimer;
    private bool hasStarted;

    private string pullLocation;
    private string saveLocation;
    private int cMinuteIndex;
    private double delay;
    #endregion Fields
    
    #region Properties
    public float TimeBetweenPulls
    {
        get { return timeBeforePulls; }
    }

    public double Delay
    {
        get { return delay; }
        set { delay = value; }
    }
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        pullTimer = timeBeforePulls;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            if (pullTimer < 0)
            {
                pullTimer = timeBeforePulls;

                SaveData(FormatData(PullData()));
                
            }
        }
    }

    private string FormatData(string data)
    {
        try
        {
            string formattedData = data;
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    private void SaveData(string data)
    {

    }

    private string PullData()
    {
        try
        {
            string response = "";

            using (WebClient client = new WebClient())
            {
                response = client.DownloadString(pullLocation);
            }

            return response;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    public string GrabData()
    {
        try
        {
            DateTime target = DateTime.Now.AddMinutes(-delay);
            if (target.Minute == lastGrab.Minute)
            {
                cMinuteIndex++;
            }
            else
            {
                cMinuteIndex = 0;
            }

            lastGrab = target;

            string data = "";

            using (StreamReader r = File.OpenText(saveLocation + "\\" + target.Hour + "-" + target.Minute + "-" + cMinuteIndex))
            {
                data = r.ReadToEnd();
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
