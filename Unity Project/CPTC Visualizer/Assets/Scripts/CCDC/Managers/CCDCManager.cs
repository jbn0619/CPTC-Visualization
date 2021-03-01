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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //generate infrastructure
            jsonWriter.GenerateData();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            // read json
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
    }
}
