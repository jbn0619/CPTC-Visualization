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

    // This is in alerts per team.
    [SerializeField]
    private int maxAlerts;

    private int teamCount;
    private int nodeCount;

    // This is in alerts per team.
    private int alertCount;
    
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
        // Set up how much of each data type there will be.
        alertCount = Random.Range(1, maxAlerts);
        teamCount = Random.Range(1, maxTeams);
        nodeCount = Random.Range(1, maxNodes);

        // First, check if we're randomizing the node infrastructure or not.
        if (randomizeInfrastructure == false)
        {
            // Generate a standard infrastructure of 5 nodes.
            nodeCount = 5;


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
