using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Authors: Garret Paradis and Ben Wetzel
/// Purpose: Alerts are the results of queries made by the Splunk program searching for teams' interactions with the network. They are used to track the teams' progress through the network.
/// </summary>
public class AlertData
{
    #region Fields
    /// <summary>
    /// This is the type of Miter attack this alert is catagorized as.
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    private CPTCEvents type;
    /// <summary>
    /// This is the IP address of the node the team is working on
    /// </summary>
    [SerializeField]
    private string nodeIP;
    /// <summary>
    /// This is the ID number of the team working on the node
    /// </summary>
    [SerializeField]
    private int teamID;
    /// <summary>
    /// This is the time the alert was triggered in the Splunk + the delay added to lessen the impact of the information on the competitors.
    /// </summary>
    [SerializeField]
    private DateTime timeStamp;

    #endregion Fields
    #region Properties
    /// <summary>
    /// Get the type of Miter Attack this alert is labeled as
    /// </summary>
    public CPTCEvents Type
    {
        get { return this.type; }
    }
    /// <summary>
    /// Get the IP address of the affected Node
    /// </summary>
    public string NodeIP
    {
        get { return this.nodeIP; }
    }
    /// <summary>
    /// Get the ID number of the team which triggered the alert
    /// </summary>
    public int TeamID
    {
        get { return this.teamID; }
    }
    /// <summary>
    /// Get the time (modified with a delay) of when the team triggered the alert.
    /// </summary>
    public DateTime TimeStamp
    {
        get { return this.timeStamp; }
    }
    #endregion Properties


    public AlertData(CPTCEvents _type, string _nodeIP, int _teamID, DateTime _timeStamp)
    {
        type = _type;
        nodeIP = _nodeIP;
        teamID = _teamID;
        timeStamp = _timeStamp;
    }


    /*This isn't necessary because the constructor already fulfills this purpose
     * public void SetData(string _type, List<int> _nodes, int _priority, int _timestamp)
    {
        type = _type;
        nodeIP = _nodes;
        teamID = _priority;
        timeStamp = _timestamp;
    }*/
}