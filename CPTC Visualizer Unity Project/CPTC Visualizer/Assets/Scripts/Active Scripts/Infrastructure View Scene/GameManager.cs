using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Justin Neft & Kevin Laporte
///     Ben Wetzel - Summer 2021
/// Function: Controls the entire infrastructure scene, and contains references to all components within. This class is a singleton, so it can be freely referenced anywhere with GameManager.Instance.
/// </summary>
public class GameManager: Singleton<GameManager>
{
    #region Fields

    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private InfrastructureData mainInfra;
    [SerializeField]
    private GameObject prefabNode;
    [SerializeField]
    private GameObject prefabNetwork;
    [SerializeField]
    private GameObject prefabInfrastructure;

    [Header("Manager GameObjects")]
    [SerializeField]
    private InputManager inputManager;
    // [SerializeField]
    // private AIManager aiManager;
    [SerializeField]
    private TeamManager teamManager;
    [SerializeField]
    private EventManager eventManager;
    [SerializeField]
    private VideoManager videoManager;
    [SerializeField]
    private FileManager fileManager;
    
    public GameObject notificationControls;

    [Header("Time fields")]

    [SerializeField]
    private System.DateTime startOfComp;
    [SerializeField]
    private System.DateTime startOfVisualizer;
    [SerializeField]
    private double timeDelay;

    [SerializeField]
    private float stateCheckTime;
    private float stateCheckCount;

    [SerializeField]
    private float attackCheckTime;
    private float attackCheckCount;

    private bool readDateStarted;
    private bool compStarted;

    [Header("Config File Test Vars")]
    [SerializeField]
    private float configUpdateCount;
    [SerializeField]
    private float configUpdateTime;
    [SerializeField]
    private float dataReadInterval;
    [SerializeField]
    private Text dataText;
    
    #endregion Fields
    
    #region Manager Properties

    /// <summary>
    /// Gets a reference to this scene's team manager if it exists.
    /// </summary>
    public TeamManager TeamManager
    {
        get
        {
            return teamManager;
        }
    }

    /// <summary>
    /// Gets a reference to this scene's event manager if it exists.
    /// </summary>
    public EventManager EventManager
    {
        get
        {
            return eventManager;
        }
    }

    /// <summary>
    /// Gets a reference to th is scene's video manager if it exists.
    /// </summary>
    public VideoManager VideoManager
    {
        get
        {
            return videoManager;
        }
    }

    /// <summary>
    /// Gets a reference to this scene's file manager if it exists.
    /// </summary>
    public FileManager FileManager
    {
        get
        {
            return fileManager;
        }
    }

    /// <summary>
    /// Gets the template infrastructure, to be passed-into the teams.
    /// </summary>
    public InfrastructureData MainInfra
    {
        get
        {
            return mainInfra;
        }
    }

    /// <summary>
    /// A prefab Game Object used to create Node objects
    /// </summary>
    public GameObject NodePrefab
    {
        get
        {
            return this.prefabNode;
        }
    }

    /// <summary>
    /// A prefab Game Object used to create Network objects
    /// </summary>
    public GameObject NetworkPrefab
    {
        get
        {
            return this.prefabNetwork;
        }
    }

    /// <summary>
    /// A prefab Game Object used to create Infrastructure objects
    /// </summary>
    public GameObject InfraPrefab
    {
        get
        {
            return this.prefabInfrastructure;
        }
    }

    #endregion Manager Properties

    #region Competition Properties

    /// <summary>
    /// Gets the main camera of this scene.
    /// </summary>
    public Camera MainCam 
    { 
        get 
        {
            return mainCam;
        } 
    }

    /// <summary>
    /// Gets if the reading date has started yet.
    /// </summary>
    public bool ReadDateStarted
    {
        get
        {
            return readDateStarted;
        }
        set
        {
            readDateStarted = value;
        }
    }

    public bool CompStarted { get; set; }

    public DateTime StartOfComp { get; set; }

    public DateTime StartOfVisualizer { get; set; }

    public double TimeDelay { get; set; }

    #endregion Competition Properties

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        readDateStarted = false;
        compStarted = false;
        stateCheckCount = 0.0f;
        attackCheckCount = 0.0f;
        configUpdateCount = 0.0f;
        configUpdateTime = 5;

        BuildInfrastructure();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to check for a config update
        configUpdateCount += Time.deltaTime;
        if(configUpdateCount >= configUpdateTime)
        {
            configUpdateCount = 0.0f;

            UpdateConfigFile();
            eventManager.LoadEventsFromJSON();

            Debug.Log("DataReadInterval now set to: " + dataReadInterval);
        }

        // Check if we need to read node states.
        stateCheckCount += Time.deltaTime;

        if(readDateStarted)
        {
            // Update nodes
            if (stateCheckCount >= stateCheckTime)
            {
                stateCheckCount = 0.0f;
                eventManager.LoadEventsFromJSON();
            }
        }

        // Check if we need to read attacks.
        attackCheckCount += Time.deltaTime;
        if (attackCheckCount >= attackCheckTime)
        {
            attackCheckCount = 0.0f;
            //eventManager.ReadEvent();
        }

        if(Input.GetKeyDown(KeyCode.Pause))
        {
            TeamViewAI.Instance.HasStarted = !(TeamViewAI.instance.HasStarted);
            notificationControls.SetActive(!notificationControls.activeSelf);

            foreach(TeamViewButton button in teamManager.TeamViewButtons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(!button.gameObject.activeSelf);
                }
            }
        }
        // Generates team names and colors
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    //teamManager.GenerateTeamNames();
        //    //teamManager.ReadTeams();
        //    //injectNotifManager.CreateTestInject();
        //}
    }

    private void UpdateConfigFile()
    {
        List<string> fileData = fileManager.ReadFile("Config_Infrastructure.txt", "Infrastructure\\");

        fileData[1] = fileData[1].Remove(0, 18);

        dataReadInterval = int.Parse(fileData[1]);

        dataText.text = ("Data Read Interval : " + dataReadInterval);

        Debug.Log("Infrastructure Config file successfully updated.");
    }

    private void BuildInfrastructure()
    {
        if(mainInfra == null)
        {
            // this is really wonky right now and I just want to get it working before I worry about making it function well. - BW
            // make the object exist in the scene
            GameObject mainInfraObject = Instantiate(prefabInfrastructure);
            // grab a ref to its infra data
            this.mainInfra = mainInfraObject.GetComponent<InfrastructureData>();
            // set the object's infra data to the data from the JSON file
            fileManager.CreateInfraFromJSON("infraDraft.JSON", "Infrastructure\\Database\\");
            // instantiate the child objects with the data
            mainInfra.InstanceChildren();
        }
    }

    /*
    /// <summary>
    /// Takes this script's infrastructure and duplicates it. It then sends those copies to each team so each team can edit their own infrastructures with their alerts and whatnot.
    /// </summary>
    public void DuplicateInfrastructure()
    {
        for (int i = 0; i < GameManager.Instance.TeamManager.CCDCTeams.Count; i++)
        {
            TeamData team = GameManager.Instance.TeamManager.CCDCTeams[i];
            // Instantiate a copy of the infrastructure, and make it a child of the team's gameObject.
            InfrastructureData newInfra = Instantiate(infrastructure);
            newInfra.transform.parent = team.gameObject.transform;
            newInfra.gameObject.transform.position = infrastructure.gameObject.transform.position;

            // Make an empty gameObject to clean up the uptime charts scene heirarchy.
            GameObject emptyObj = new GameObject();
            emptyObj.transform.parent = UIManager.Instance.SceneCanvases[1].gameObject.transform;
            emptyObj.name = "Team " + (i + 1).ToString() + " Uptime Charts";

            // Make sure the data of individual nodes is properly-copied.
            for (int k = 0; k < newInfra.AllNodes.Count; k++)
            {
                // Assign the node's values to a new NodeData object.
                NodeData currentNode = newInfra.AllNodes[k];
                NodeData oldNode = infrastructure.AllNodes[k];
                currentNode.Ip = oldNode.Ip;
                currentNode.Id = oldNode.Id;
                currentNode.IsHidden = oldNode.IsHidden;
                currentNode.Type = oldNode.Type;
                currentNode.State = oldNode.State;
                foreach (int c in oldNode.Connections)
                {
                    currentNode.Connections.Add(c);
                }
                foreach (LineRenderer c in oldNode.ConnectionGOS)
                {
                    LineRenderer copy = Instantiate(c, currentNode.gameObject.transform);
                    currentNode.ConnectionGOS.Add(copy);
                }

                if (currentNode is NodeData)
                {
                    // Instantiate new uptime charts for each node.
                    UptimeChartData newChart = Instantiate(uptimeChartGO, emptyObj.transform);

                    ((NodeData)currentNode).UptimeChart = newChart;
                    newChart.gameObject.transform.position = newInfra.AllNodes[k].gameObject.transform.position + new Vector3(0.5f, 0, 0);
                    newChart.gameObject.transform.localScale = new Vector2(0.002f, 0.008f);
                    newChart.NodeID = newInfra.AllNodes[k].Id;
                    newChart.TeamID = team.TeamId;

                    team.UptimeCharts.Add(newChart);
                    newChart.gameObject.SetActive(false);
                }
            }

            // appends the teams at the end of the names
            for (int k = 0; k < newInfra.AllNodes.Count; k++)
            {
                NodeData current = newInfra.AllNodes[k];
                string name = current.Ip;
                string append = "";

                if (i < 9)
                {
                    append = "-team0" + (i + 1);
                }
                else
                {
                    append = "-team" + (i + 1);
                }

                current.Ip = name + append;
            }

            team.InfraCopy = newInfra;

            // Create the team's graph, then hide it for later.
            team.BuildTeamGraph();
            team.InfraCopy.gameObject.SetActive(false);
        }
    }
    */
}
