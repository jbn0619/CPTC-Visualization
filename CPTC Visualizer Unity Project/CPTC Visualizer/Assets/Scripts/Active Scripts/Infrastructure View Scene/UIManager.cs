using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum View { team, main}

/// <summary>
/// Author: Justin Neft
/// Function: Keeps track of all UI elements in the scene, and modifies them as-needed.
/// </summary>
public class UIManager: Singleton<UIManager>
{
    #region Fields

    [SerializeField]
    private Canvas sceneCanvas;

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

    [Header("Team View UI")]
    [SerializeField]
    private Button nextTeam;
    [SerializeField]
    private Button leftArrow;
    [SerializeField]
    private Button rightArrow;
    [SerializeField]
    private Text teamName;
    [SerializeField]
    private Text nodeNumber;
    [SerializeField]
    private Image teamColor;
    [SerializeField]
    private Image nodeColor;
    [SerializeField]
    private InfrastructureData displayedInfra;

    private int teamShown = 0;
    private int nodeShown = 1;

    private TeamManager teamManager;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets this scene's canvas.
    /// </summary>
    public Canvas SceneCanvas
    {
        get
        {
            return sceneCanvas;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    { 
        nextTeam.onClick.AddListener(delegate { NextTeam(); });
        leftArrow.onClick.AddListener(delegate { ChangeNode(-1); });
        rightArrow.onClick.AddListener(delegate { ChangeNode(1); });
        showTimerStarted = false;
        elapsedTime = 0.0f;
        timeUntilShow = 0.0f;
        teamManager = GameManager.Instance.TeamManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ReadDateStarted)
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
                if (timeUntilShow > 0)
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
        }
    }

    public void ToggleNotifBanner()
    {
        if (!showTimerStarted)
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

    public void NextTeam()
    {
        if (teamShown >= teamManager.Teams.Count - 1)
        {
            teamShown = 0;
        }
        else
        {
            teamShown++;
        }

        teamName.text = teamManager.Teams[teamShown].TeamName;
        teamColor.color = teamManager.Teams[teamShown].TeamColor;
        nodeColor.color = teamManager.Teams[teamShown].TeamColor;
        GameManager.Instance.ChangeDisplayedInfra(teamManager.Teams[teamShown].Infra);
        Debug.Log($"{teamShown}\n" +
            $"{teamManager.Teams[teamShown].TeamColor}\n" +
            $"{teamManager.Teams[teamShown].TeamName}");
    }

    public void ChangeNode(int change)
    {
        nodeShown += change;
        nodeNumber.text = "" + change;
    }
}
