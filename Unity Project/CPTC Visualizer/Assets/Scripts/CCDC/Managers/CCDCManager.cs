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

    [SerializeField]
    private double timeDelay;
    
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
        timeDelay = 30;
    }

    // Update is called once per frame
    void Update()
    {
        // Master Key. Starts the program in its entirety with one key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Starts simulation. Is this needed?
            //jsonWriter.GenerateData();
            infraManager.ReadJson();
            //teamManager.GenerateTeamNames();
            teamManager.ReadTeams();
            eventManager.ReadAttacksJSON();
            TeamViewAI.Instance.BeginComp();
            //injectNotifManager.ReadInInjects();
        }

        // Create a new Infrastructure and write it to the Json
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jsonWriter.GenerateData();
        }

        // Reads in an infrastructure from the Json
        if(Input.GetKeyDown(KeyCode.R))
        {
            infraManager.ReadJson();
        }

        // Generates team names and colors
        if(Input.GetKeyDown(KeyCode.C))
        {
            teamManager.GenerateTeamNames();
            teamManager.ReadTeams();
        }

        // Create a test inject ot be displayed
        if(Input.GetKeyDown(KeyCode.I))
        {
            injectNotifManager.CreateTestInject();
        }

        // Reads attacks json and spawns notifications
        if(Input.GetKeyDown(KeyCode.P))
        {
            eventManager.ReadAttacksJSON();
        }
    }
}
