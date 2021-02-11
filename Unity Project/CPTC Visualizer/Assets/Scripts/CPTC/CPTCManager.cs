using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPTCManager: Singleton<CPTCManager>
{
    #region Fields

    [Header("Manager GameObjects")]
    [SerializeField]
    private InfrastructureManager infraManager;
    [SerializeField]
    private TeamManager teamManager;
    [SerializeField]
    private EventManager eventManager;
    [SerializeField]
    private NotificationManager notifManager;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a reference to this scene's infrastructure manager if it exists.
    /// </summary>
    public InfrastructureManager InfraManager
    {
        get
        {
            return infraManager;
        }
    }

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
    /// Gets a reference to this scene's notification manager if it exists.
    /// </summary>
    public NotificationManager NotifManager
    {
        get
        {
            return notifManager;
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
            instance = this as CPTCManager;
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
        
    }
}
