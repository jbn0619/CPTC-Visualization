using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CCDCManager: Singleton<CCDCManager>
{
    #region Fields

    [Header("Manager GameObjects")]
    [SerializeField]
    private CCDCInfrastructureManager infraManager;
    [SerializeField]
    private JSONWriter jsonWriter;
    [SerializeField]
    private CCDCTeamManager teamManager;
    [SerializeField]
    private CCDCEventManager eventManager;
    [SerializeField]
    private CCDCInjectNotifManager injectNotifManager;
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
    /// Gets a reference to this scene's infrastructure manager if it exists.
    /// </summary>
    public CCDCInfrastructureManager InfraManager
    {
        get
        {
            return infraManager;
        }
    }

    /// <summary>
    /// Gets a reference to this scene's team manager if it exists.
    /// </summary>
    public CCDCTeamManager TeamManager
    {
        get
        {
            return teamManager;
        }
    }

    /// <summary>
    /// Gets a reference to this scene's event manager if it exists.
    /// </summary>
    public CCDCEventManager EventManager
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

            infraManager.ReadJson();
            teamManager.ReadTeams();
            infraManager.DisableMainView();
            CCDCDataFormatter.Instance.HasStart = true;
            compStarted = true;

            //System.Diagnostics.Process.Start("notepad.exe");
        }

        if(Input.GetKeyDown(KeyCode.Space) && !readDateStarted)
        {
            startOfVisualizer = System.DateTime.Now;
            timeDelay = startOfVisualizer.Subtract(startOfComp).TotalMinutes;

            CCDCDataFormatter.Instance.Delay = timeDelay;
            readDateStarted = true;
            eventManager.ReadAttacksJSON();
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
                timeUntilShow = 15 * 60;
                showNotifBanner.gameObject.SetActive(true);
            }
            else
            {
                showTimerStarted = false;
                showNotifBanner.gameObject.SetActive(false);
            }
        }

        // Reads in an infrastructure from the Json
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    infraManager.ReadJson();
        //}

        // Generates team names and colors
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    teamManager.GenerateTeamNames();
        //    teamManager.ReadTeams();
        //}

        // Create a test inject ot be displayed
        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    injectNotifManager.CreateTestInject();
        //}

        // Reads attacks json and spawns notifications
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    eventManager.ReadAttacksJSON();
        //}
    }
}
