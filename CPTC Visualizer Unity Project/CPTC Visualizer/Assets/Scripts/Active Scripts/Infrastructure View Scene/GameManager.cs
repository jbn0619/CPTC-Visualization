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
    [Header("Data from Controller Scene")]
    [SerializeField]
    private string infraFile = "test_controllerToInfraScene.json";
    [SerializeField]
    private string alertsFile = "test_controllerToAlertsScene.json";
    [SerializeField]
    private TestDataWriter dataWriter;

    [Header("Essential GameObjects")]
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
    [SerializeField]
    private List<Sprite> osSprites;
    [SerializeField]
    private Button toControllerButton;

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
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private Canvas fluidCanvas;
    [SerializeField]
    private GameObject notificationControls;

    [Header("Timer Fields")]
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
    [SerializeField]
    public float loadDataFromControllerTime;
    [SerializeField]
    private float loadDataFromControllerCount;

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
    #endregion Manager Properties
    #region General Properties
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
    /// <summary>
    /// A list of all sprites used to show the different types of nodes
    /// </summary>
    public List<Sprite> OsSprites
    {
        get { return osSprites; }
    }
    /// <summary>
    /// Gets a string for the name of the file used to load Alert data into the system
    /// </summary>
    public string AlertFileName
    {
        get { return alertsFile; }
    }
    /// <summary>
    /// Gets a string for the name of the file used to laod Infrastructure data into the system
    /// </summary>
    public string InfraFileName
    {
        get { return infraFile; }
    }
    #endregion General Properties
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

    public double TimeDelay 
    {
        get { return timeDelay; }
        set { timeDelay = value; }
    }

    #endregion Competition Properties

    // Start is called before the first frame update
    void Start()
    {
        toControllerButton.onClick.AddListener(delegate {  GoToControllerScene(); });
        mainCam = Camera.main;
        readDateStarted = false;
        compStarted = false;
        stateCheckCount = 0.0f;
        attackCheckCount = 0.0f;
        configUpdateCount = 0.0f;
        loadDataFromControllerCount = 0.0f;
        configUpdateTime = 5;

        BuildInfrastructure();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if new alert files should be looked for
        loadDataFromControllerCount += Time.deltaTime;
        if(loadDataFromControllerCount >= loadDataFromControllerTime)
        {
            loadDataFromControllerCount = 0.0f;
            eventManager.LoadAlerts(alertsFile, timeDelay);
        }
        
        /*// Check if we need to check for a config update
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
        }*/

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
    }

    private void UpdateConfigFile()
    {
        List<string> fileData = fileManager.ReadFile("Config_Infrastructure.txt", "Infrastructure\\");

        fileData[1] = fileData[1].Remove(0, 18);

        dataReadInterval = int.Parse(fileData[1]);

        dataText.text = ("Data Read Interval : " + dataReadInterval);

        Debug.Log("Infrastructure Config file successfully updated.");
    }
    /// <summary>
    /// Method for the Scene transfer button to execute when it's pressed
    /// </summary>
    private void GoToControllerScene()
    {
        // Make new dummy event data
        dataWriter.WriteAlertData();
        // switch scenes to the Controller Scene
        SceneManager.LoadScene(sceneName: "Controller");
    }

    /// <summary>
    /// Instance all Infrastructure Game Objects if they are not instanced, and assign their children's positions
    /// </summary>
    public void BuildInfrastructure()
    {
        if(mainInfra == null)
        {
            // Instantiate an Infrastructure prefab for the mainInfra
            GameObject mainInfraObject = Instantiate(prefabInfrastructure);
            // set canvas as parent of the infrastructure
            mainInfraObject.transform.SetParent(fluidCanvas.gameObject.transform, false);
            mainInfraObject.name = "Main Infrastructure View";
            // grab a ref to its prefabed infra data
            mainInfra = mainInfraObject.GetComponent<InfrastructureData>();

            // Grab Data from all Infrastructures
            List<InfrastructureData> infras = fileManager.CreateInfrasFromJSON(infraFile, "Infrastructure\\Database\\");
            // set the object's infra data to the data from the JSON file to the first Infrastructure in the list.
            mainInfra.SetData(infras[0].Networks, infras[0].AllNodes, infras[0].Teams);
            // instantiate the child objects with the data
            mainInfra.InstanceChildren();

            // Instance an empty to hold all of the team Infrastructure objects in the Heirarchy
            GameObject emptyHolder = new GameObject();
            emptyHolder.transform.SetParent(fluidCanvas.gameObject.transform, false);
            emptyHolder.name = "Team Infrastructures";

            // Instance the team Infrastructure Objects childed to the empty object and instance all of their children within them
            List<GameObject> infraObjects = new List<GameObject>();
            List<int> teamIds = new List<int>();
            for(int i = 0; i < infras.Count; i++)
            {
                infraObjects.Add(Instantiate(prefabInfrastructure));
                infraObjects[i].transform.SetParent(emptyHolder.transform, false);
                infraObjects[i].GetComponent<InfrastructureData>().SetData(infras[i].Networks, infras[i].AllNodes, infras[i].Teams);
                infraObjects[i].GetComponent<InfrastructureData>().InstanceChildren();
                infraObjects[i].SetActive(false);
                teamIds.Add(i);
            }

            // Instance the Team Objects and pass their data to them
            teamManager.InstanceTeams(teamIds, infraObjects);
        }

        // Set positions of main infrastructure
        mainInfra.PositionNetworks();
        mainInfra.PositionNodes();
        mainInfra.DrawAllConnections();

        // set positions of all team infrastructures
        foreach(GameObject team in teamManager.TeamObjects)
        {
            team.GetComponent<TeamData>().Infra.PositionNetworks();
            team.GetComponent<TeamData>().Infra.PositionNodes();
            team.GetComponent<TeamData>().Infra.DrawAllConnections();
        }
        // Temporary Positioning until main infra works
        mainInfra.gameObject.SetActive(false);
        teamManager.TeamObjects[0].GetComponent<TeamData>().InfraObject.SetActive(true);
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
