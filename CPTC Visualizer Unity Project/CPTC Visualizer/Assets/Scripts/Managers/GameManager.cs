using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: Singleton<GameManager>
{
    #region Fields

    [SerializeField]
    private InfrastructureData mainInfra;

    [Header("Manager GameObjects")]
    [SerializeField]
    private JSONWriter jsonWriter;
    [SerializeField]
    private TeamManager teamManager;
    [SerializeField]
    private EventManager eventManager;
    [SerializeField]
    private InjectNotifManager injectNotifManager;
    [SerializeField]
    private VideoManager videoManager;
    
    [Header("Public Facing Timers")]
    [SerializeField]
    private GameObject showTimerBanner;
    [SerializeField]
    private float timeUntilShow;
    [SerializeField]
    private Text timeUntilShowText;
    [SerializeField]
    private GameObject showNotifBanner;
    private bool showTimerStarted;
    [SerializeField]
    private float elapsedTime;
    [SerializeField]
    private Text elapsedTimeText;


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
    
    #endregion Fields
    
    #region Properties

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

    public InfrastructureData MainInfra
    {
        get
        {
            return mainInfra;
        }
    }

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        readDateStarted = false;
        compStarted = false;
        showTimerStarted = false;
        stateCheckCount = 0.0f;
        attackCheckCount = 0.0f;
        elapsedTime = 0.0f;
        timeUntilShow = 0.0f;
        test.Run();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to read node states.
        stateCheckCount += Time.deltaTime;

        if(readDateStarted)
        {
            // Timer stuff
            elapsedTime += Time.deltaTime;
            elapsedTimeText.text = "Elapsed Time: " + string.Format("{00}:{1:00}:{2:00}",
                Mathf.FloorToInt(elapsedTime / 3600),
                Mathf.FloorToInt(elapsedTime / 60 % 60),
                Mathf.FloorToInt(elapsedTime % 60));

            // Show Starting Timer
            if (showTimerStarted)
            {
                if(timeUntilShow > 0)
                {
                    timeUntilShow -= Time.deltaTime;
                    timeUntilShowText.text = "Show starting in aproximately " + string.Format("{00}:{1:00}",
                        Mathf.FloorToInt(timeUntilShow / 60 % 60),
                        Mathf.FloorToInt(timeUntilShow % 60));
                }
                else
                {
                    timeUntilShowText.text = "Show starting soon!";
                }

            }

            // Update nodes
            if (stateCheckCount >= stateCheckTime)
            {
                stateCheckCount = 0.0f;
                eventManager.ReadNodeStateJSON();
            }
        }

        // Check if we need to read attacks.
        attackCheckCount += Time.deltaTime;
        if (attackCheckCount >= attackCheckTime)
        {
            attackCheckCount = 0.0f;
            eventManager.SpawnAttack();
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

        // Master Key. Starts the program in its entirety with one key press
        if (Input.GetKeyDown(KeyCode.Return) && !compStarted)
        {
            startOfComp = System.DateTime.Now;
            Debug.Log(System.DateTime.Now.ToString());

            teamManager.ReadTeams();
            DataFormatter.Instance.HasStart = true;
            compStarted = true;

            //System.Diagnostics.Process.Start("notepad.exe");
        }

        if(Input.GetKeyDown(KeyCode.Space) && !readDateStarted)
        {
            startOfVisualizer = System.DateTime.Now;
            timeDelay = Mathf.Abs((int)startOfVisualizer.Subtract(startOfComp).TotalMinutes);

            DataFormatter.Instance.Delay = timeDelay;
            readDateStarted = true;
            //eventManager.ReadAttacksJSON();
            TeamViewAI.Instance.BeginComp();
            injectNotifManager.ReadInInjects();
        }

        // Create a new Infrastructure and write it to the Json
        if (Input.GetKeyDown(KeyCode.Period))
        {
            jsonWriter.GenerateData();
        }

        // Starts or stops the Time Until Show notification
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(!showTimerStarted)
            {
                showTimerStarted = true;
                timeUntilShow = 30 * 60;
                showNotifBanner.gameObject.SetActive(true);
            }
            else
            {
                showTimerStarted = false;
                showNotifBanner.gameObject.SetActive(false);
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
