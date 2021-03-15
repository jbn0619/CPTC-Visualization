using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject notificationControls;

    [Header("Time fields")]

    [SerializeField]
    private double timeDelay;

    [SerializeField]
    private float stateCheckTime;
    private float stateCheckCount;

    [SerializeField]
    private float attackCheckTime;
    private float attackCheckCount;

    private bool readDateStarted;
    
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

    /// <summary>
    /// Change how this singleton works so that it doesn't leave this scene.
    /// </summary>
    public override void Awake()
    {
        if (instance == null)
        {
            instance = this as CCDCManager;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        readDateStarted = false;
        timeDelay = 0;
        stateCheckCount = 0.0f;
        attackCheckCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to read node states.
        stateCheckCount += Time.deltaTime;
        if (stateCheckCount >= stateCheckTime
            && readDateStarted)
        {
            stateCheckCount = 0.0f;
            eventManager.ReadNodeStateJSON();
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
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }

        // Master Key. Starts the program in its entirety with one key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            timeDelay = System.DateTime.Now.Minute;

            infraManager.ReadJson();
            teamManager.ReadTeams();
            infraManager.DisableMainView();
            CCDCDataFormatter.Instance.HasStart = true;

            //System.Diagnostics.Process.Start("notepad.exe");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CCDCDataFormatter.Instance.Delay = (System.DateTime.Now.Minute - timeDelay);
            readDateStarted = true;
            eventManager.ReadAttacksJSON();
            TeamViewAI.Instance.BeginComp();
            injectNotifManager.ReadInInjects();
        }

        // Create a new Infrastructure and write it to the Json
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    jsonWriter.GenerateData();
        //}

        // Reads in an infrastructure from the Json
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    infraManager.ReadJson();
        //}

        // Generates team names and colors
        if(Input.GetKeyDown(KeyCode.C))
        {
            teamManager.GenerateTeamNames();
            teamManager.ReadTeams();
        }

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
