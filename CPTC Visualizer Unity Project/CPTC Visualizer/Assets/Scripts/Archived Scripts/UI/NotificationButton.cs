using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NotificationType { Banner, Marker}
/// <summary>
/// Author: Unknown
///     Ben Wetzel - Summer 2021
/// Purpose: Display prioritized alert data on the screne
/// </summary>
public class NotificationButton: MonoBehaviour
{
    #region Fields
    [Header("Tracked Fields")]
    [SerializeField]
    private NotificationType notifType;
    [SerializeField]
    private TeamData team;
    [SerializeField]
    private NodeData node;
    [SerializeField]
    private AlertData alert;

    // private int affectedNodeID;
    // private int affectedTeamID;
    private CCDCAttackType attackType;

    [Header("Object References")]
    [SerializeField]
    private SpriteRenderer buttonSprite;
    private Image bannerSprite;
    private GameObject correspondingBanner;
    
    #endregion Fields
    
    #region Properties
    /// <summary>
    /// Gets or Sets the alert this notification pertains to
    /// </summary>
    public AlertData Alert
    {
        get { return alert; }
        set { alert = value; }
    }
    /// <summary>
    /// Gets or sets what node this notification pertains-to.
    /// </summary>
    public NodeData Node
    {
        get
        {
            return node;
        }
        set
        {
            node = value;
        }
    }

    /// <summary>
    /// Gets or sets what team this notification pertains-to.
    /// </summary>
    public TeamData Team
    {
        get
        {
            return team;
        }
        set
        {
            team = value;
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
            ChangeMarkerSprite();
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
            ChangeBannerSprite();
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        //affectedNodeID = -1;
        //affectedTeamID = -1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes the sprites of this button and its corresponding banner.
    /// </summary>
    public void ChangeMarkerSprite()
    {
        Sprite newButton = GeneralResources.Instance.MarkerSprites[(int)attackType];
        buttonSprite.sprite = newButton;
        buttonSprite.transform.localScale = new Vector3(.6f, .3f, 1);
    }

    public void ChangeBannerSprite()
    {
        Sprite newBanner = GeneralResources.Instance.BannerSprites[(int)attackType];
        bannerSprite = correspondingBanner.GetComponent<Image>();
        bannerSprite.sprite = newBanner;
        bannerSprite.transform.localScale = new Vector3(50, 15, 1);
    }

    /// <summary>
    /// Activates this marker's notification on-click.
    /// </summary>
    public void OnMarkerClick()
    {
        // Clean-up this banner's reference from the ccdcTeam, then destroy it.
        team.NotifBanners.Remove(correspondingBanner);
        Destroy(correspondingBanner);

        // Play the video.
        VideoManager vidMan = GameManager.Instance.VideoManager;
        vidMan.PlayAttackVideo((int)attackType);

        // Remove this marker's reference from the ccdcTeam, then destroy it.
        team.NotifMarkers.Remove(this);
        Destroy(this.gameObject);
    }
}
