using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CPTCEvents { NetworkScan, Discovery, Exploit, ShutDown, StartUp }

[Serializable]
public class UpdateDataPacket
{
    #region Fields

    private string hostName;
    private string ipAddress;
    private int nodeID;
    private int teamID;
    private CPTCEvents type;
    private string startTime;

    #endregion Fields

    #region Properties

    public string HostName {get; set;}

    public string IpAddress { get; set; }

    public int NodeID { get; set; }

    public int TeamID { get; set; }

    public CPTCEvents Type { get; set; }

    public string StartTime { get; set; }
    
    #endregion Properties

    /// <summary>
    /// Constructor for this object.
    /// </summary>
    public UpdateDataPacket()
    {

    }
}
