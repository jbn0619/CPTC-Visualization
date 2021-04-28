using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDataPacket : MonoBehaviour
{
    #region Fields

    private string hostName;
    private string ipAddress;
    private int nodeID;
    private int teamID;
    private CPTCEvents type;

    #endregion Fields

    #region Properties

    public string HostName {get; set;}

    public string IpAddress { get; set; }

    public int NodeID { get; set; }

    public int TeamID { get; set; }

    public CPTCEvents Type { get; set; }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
