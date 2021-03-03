using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotificationType { Banner, Marker}

public class NotificationButton: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private NotificationType notifType;

    private int affectedNodeID;
    private int affectedTeamID;
    private CCDCAttackType attackType;
    private GameObject correspondingBanner;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets or sets what node this notification pertains-to.
    /// </summary>
    public int AffectedNodeID
    {
        get
        {
            return affectedNodeID;
        }
        set
        {
            if (value >= 0) affectedNodeID = value;
        }
    }

    /// <summary>
    /// Gets or sets what team this notification pertains-to.
    /// </summary>
    public int AffectedTeamID
    {
        get
        {
            return affectedTeamID;
        }
        set
        {
            if (value >= 0) affectedTeamID = value;
        }
    }

    /// <summary>
    /// Gets or sets what attack type this notification represents.
    /// </summary>
    public CCDCAttackType AttackType
    {
        get
        {
            return attackType;
        }
        set
        {
            attackType = value;
        }
    }

    /// <summary>
    /// Gets or sets the banner that represents this button.
    /// </summary>
    public GameObject CorrespondingBanner
    {
        get
        {
            return correspondingBanner;
        }
        set
        {
            correspondingBanner = value;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        affectedNodeID = -1;
        affectedTeamID = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Activates this marker's notification on-click.
    /// </summary>
    public void OnMarkerClick()
    {
        // Clean-up this banner's reference from the ccdcTeam, then destroy it.
        CCDCManager.Instance.TeamManager.CCDCTeams[affectedTeamID].NotifBanners.Remove(correspondingBanner);
        Destroy(correspondingBanner);

        // Play the video.
        VideoManager vidMan = CCDCManager.Instance.VideoManager;
        vidMan.PlayAttackVideo((int)attackType);

        // Remove this marker's reference from the ccdcTeam, then destroy it.
        CCDCManager.Instance.TeamManager.CCDCTeams[affectedNodeID].NotifMarkers.Remove(this);
        Destroy(this.gameObject);
    }
}
