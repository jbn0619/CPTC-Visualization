using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertData: MonoBehaviour
{
    #region Fields
    [Header("JSON Data Fields")]
    [SerializeField]
    public string type;
    [SerializeField]
    public List<int> nodes;
    [SerializeField]
    public int priority;
    [SerializeField]
    public int timestamp;

    #endregion Fields

    public AlertData()
    {
        
    }

    public AlertData(string _type, List<int> _nodes, int _priority, int _timestamp)
    {
        type = _type;
        nodes = _nodes;
        priority = _priority;
        timestamp = _timestamp;
    }

    public void SetData(string _type, List<int> _nodes, int _priority, int _timestamp)
    {
        type = _type;
        nodes = _nodes;
        priority = _priority;
        timestamp = _timestamp;
    }
}