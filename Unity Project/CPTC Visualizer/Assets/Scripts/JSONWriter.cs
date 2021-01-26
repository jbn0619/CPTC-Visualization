using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum CPTCEvents { Exploit, ShutDown, StartUp, Discovery}

public enum NodeTypes { Host, Router}

public class JSONWriter: MonoBehaviour
{
    #region Fields

    [Header("File Options")]

    [SerializeField]
    private string fileName;

    [Header("Data Options")]

    [SerializeField]
    private bool randomizeInfrastructure;

    [SerializeField]
    private int maxTeams;

    [SerializeField]
    private int maxNodes;

    [SerializeField]
    private int maxAlerts;
    
    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // Alerts hold event types
        
        // Teams hold discovered node IDs and alerts

        // Entire infrastructure holds nodes

        // Nodes hold node IDs, type, and connections.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates Json-formatted data based-on the fields for this script.
    /// </summary>
    public void GenerateData()
    {
        // First, check if we're randomizing the node infrastructure or not.
        if (randomizeInfrastructure == false)
        {
            // Generate a standard infrastructure of 5 nodes.

        }
        else
        {

        }
    }

    /// <summary>
    /// Savees any generated data to a Json file.
    /// </summary>
    private void SaveToJson()
    {

    }
}
