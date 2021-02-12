using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class CPTCTeamManager : TeamManager
{
    #region Fields

    [Header("Team View Fields")]
    [SerializeField]
    private TeamViewButton teamViewButGO;
    private List<TeamViewButton> teamViewButtons;

    #endregion Fields

    #region Properties



    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        teams = new List<TeamData>();
        teamViewButtons = new List<TeamViewButton>();
        currentTeamView = -1;

        SceneManager.sceneLoaded += CleanOnSceneChange;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Team View Methods

    /// <summary>
    /// Changes what infrastructure is currently-displayed in the scene.
    /// </summary>
    public override void ChangeTeamView(int deltaIndex)
    {
        // Check what team is currently-viewed and set it to false (hide it)
        if (currentTeamView == -1)
        {
            CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
        }
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }
        // Update the index based-on what key was hit.
        currentTeamView += deltaIndex;

        // Check what index we're at and take the appropriate actions for either wrapping-through the collection in either direction or seeing if we're in teams or infrastructure.

        // The case when we're not looking at teams, but infrastructure.
        if (currentTeamView == -1)
        {
            CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
        }
        // The case when we wrap from the bottom to the end of the teams list.
        else if (currentTeamView < -1)
        {
            currentTeamView = teams.Count - 1;
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            teams[currentTeamView].BuildTeamGraph();
        }
        // The case when we wrap from the end of teams list back to infrastructure.
        else if (currentTeamView >= teams.Count)
        {
            currentTeamView = -1;
            CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
        }
        // The case when we're somewhere within the teams list.
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
        }
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
            CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
        }
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
            teamViewLabel.text = "Main Infrastructure";
        }
        else if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            currentTeamView = teamIndex;
            teams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            teamViewLabel.text = "Team " + teamIndex;
        }
    }

    /// <summary>
    /// Generates enough buttons to switch between every team's view, and the main infrastructure view.
    /// </summary>
    public void GenerateTeamViewButtons()
    {
        // Make sure we properly clear-out the previous buttons before making new ones.
        if (teamViewButtons != null)
        {
            foreach (TeamViewButton t in teamViewButtons)
            {
                if (t != null) Destroy(t.gameObject);
            }
            teamViewButtons.Clear();
        }
        else
        {
            teamViewButtons = new List<TeamViewButton>();
        }

        // Create each button, then edit their index and text fields.
        if (UIManager.Instance.ActiveCanvas != null)
        {
            for (int i = 0; i < teams.Count + 1; i++)
            {
                TeamViewButton newButton = Instantiate(teamViewButGO, UIManager.Instance.ActiveCanvas.transform);
                if (i == teams.Count)
                {
                    newButton.NewTeamIndex = -1;
                    newButton.ButtonText.text = "Main";
                }
                else
                {
                    newButton.NewTeamIndex = i;
                    newButton.ButtonText.text = "Team " + i;
                }

                // Finally, move the button to its proper spot and add it to teamViewButtons.
                newButton.gameObject.transform.position = new Vector3(95 + (i * 100), Screen.height - 50, 0);
                teamViewButtons.Add(newButton);
            }
        }
        else
        {
            Debug.Log("ERROR: NO ACTIVE CANVAS IN SCENE!");
        }
    }

    #endregion Team View Methods

    /// <summary>
    /// Cleans-up various lists and variables for this script when switching scenes.
    /// </summary>
    public override void CleanOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        teamViewButtons.Clear();
        base.CleanOnSceneChange(scene, mode);
    }
}
