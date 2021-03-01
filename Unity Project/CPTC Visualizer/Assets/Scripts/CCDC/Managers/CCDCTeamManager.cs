using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class CCDCTeamManager: TeamManager
{
    #region Fields

    protected List<CCDCTeamData> ccdcTeams;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of this manager's ccdc teams.
    /// </summary>
    public List<CCDCTeamData> CCDCTeams
    {
        get
        {
            return ccdcTeams;
        }
    }

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        ccdcTeams = new List<CCDCTeamData>();
        teams = new List<TeamData>();
        currentTeamView = -1;

        SceneManager.sceneLoaded += CleanOnSceneChange;
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();
    }

    #region Team View Methods

    /// <summary>
    /// Changes what infrastructure is currently-displayed in the scene.
    /// </summary>
    public override void ChangeTeamView(int deltaIndex)
    {
        SelectTeamView(currentTeamView + deltaIndex);
    }

    /// <summary>
    /// This method is called by a button to change the currently-viewed infrastructure to a team at a specific index.
    /// </summary>
    /// <param name="teamIndex">The id of the team to display.</param>
    public override void SelectTeamView(int teamIndex)
    {
        // First, disable the currently-active infrastructure.
        if (currentTeamView == -1)
        {
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
        }
        else
        {
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }

        // Wrap the team index to make sure it stays in-bounds.
        if (teamIndex < -1) teamIndex = ccdcTeams.Count - 1;
        else if (teamIndex >= ccdcTeams.Count) teamIndex = -1;

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
            teamViewLabel.text = "Main Infrastructure";
        }
        else if (teamIndex >= 0 && teamIndex < ccdcTeams.Count)
        {
            currentTeamView = teamIndex;
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            teamViewLabel.text = "Team " + teamIndex;
        }
    }

    #endregion Team View Methods
}
