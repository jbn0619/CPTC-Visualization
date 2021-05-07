using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CPTCEvents { NetworkScan, Discovery, Exploit, ShutDown, StartUp }

/// <summary>
/// Author: Justin Neft
/// Function: A data container that represents the important information of an "event", or something happening. Is read by the event Manager.
/// </summary>
[Serializable]
public class UpdateDataPacket
{
    #region Fields

    public string hostName;
    public string ipAddress;
    public int nodeID;
    public int teamID;
    public string type;
    public string startTime;

    #endregion Fields

    #region Properties

    public string HostName 
    {
        get
        {
            return hostName;
        }
        set
        {
            hostName = value;
        }
    }

    public string IpAddress 
    {
        get
        {
            return ipAddress;
        }
        set
        {
            ipAddress = value;
        }
    }

    public int NodeID 
    {
        get
        {
            return nodeID;
        }
        set
        {
            nodeID = value;
        }
    }

    public int TeamID 
    {
        get
        {
            return teamID;
        }
        set
        {
            teamID = value;
        }
    }

    public String Type 
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    public string StartTime 
    {
        get
        {
            return startTime;
        }
        set
        {
            startTime = value;
        }
    }
    
    #endregion Properties

    /// <summary>
    /// Constructor for this object.
    /// </summary>
    public UpdateDataPacket()
    {

    }

    /// <summary>
    /// Converts this UpdateDataPacket object and all data within it into a json-formatted string.
    /// </summary>
    /// <returns>A large string in a JSON format.</returns>
    public string ConvertToJSON()
    {
        string cptcInfo = "";
        cptcInfo = JsonUtility.ToJson(this);
        Debug.Log(cptcInfo);
        return cptcInfo;
    }
}
