using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.IO;

public abstract class TeamManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected CompetitionType compType;
    [SerializeField]
    protected List<TeamData> teams;

    [Header("Team View Fields")]
    [SerializeField]
    protected Text teamViewLabel;
    [SerializeField]
    protected Image teamViewNameplate;
    [SerializeField]
    protected TeamViewButton teamViewButGO;
    protected List<TeamViewButton> teamViewButtons;

    protected int currentTeamView;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of all teams currently in the competition.
    /// </summary>
    public List<TeamData> Teams
    {
        get
        {
            return teams;
        }
    }

    /// <summary>
    /// Gets a list of all the team view buttons currently in the scene.
    /// </summary>
    public List<TeamViewButton> TeamViewButtons
    {
        get
        {
            return teamViewButtons;
        }
    }

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
    public virtual void ChangeTeamView(int deltaIndex)
    {
        // Check what team is currently-viewed and set it to false (hide it)
        if (currentTeamView == -1)
        {
            switch (compType)
            {
                case CompetitionType.CCDC:
                    CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
                    break;
                case CompetitionType.CPTC:
                    CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
                    break;
            }
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
            switch (compType)
            {
                case CompetitionType.CCDC:
                    CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
                case CompetitionType.CPTC:
                    CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
            }
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
            switch (compType)
            {
                case CompetitionType.CCDC:
                    CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
                case CompetitionType.CPTC:
                    CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
            }
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
    public virtual void SelectTeamView(int teamIndex)
    {
        // First, disable the currently-active infrastructure.
        if (currentTeamView == -1)
        {
            switch (compType)
            {
                case CompetitionType.CCDC:
                    CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
                    break;
                case CompetitionType.CPTC:
                    CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            teams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            switch (compType)
            {
                case CompetitionType.CCDC:
                    CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
                case CompetitionType.CPTC:
                    CPTCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
                    break;
            }
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
    public virtual void GenerateTeamViewButtons()
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
    public virtual void CleanOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        teamViewButtons.Clear();
    }

    //// Have this return a List<string>
    //public virtual void GenerateTeamNames()
    //{
    //    List<string> potentialNames = new List<string>();
    //    List<string> teamNames = new List<string>();
    //    List<string> teamColors = new List<string>();

    //    // Generates a list of potential names from the file
    //    StreamReader reader = new StreamReader("Assets/Data/animalNames.txt");
    //    while (reader.Peek() != -1)
    //    {
    //        potentialNames.Add(reader.ReadLine());
    //    }
    //    reader.Close();

    //    // Gets a random index, adds it to the team names, and removes it from
    //    //      potential names. Then creates a random color for the team.
    //    for (int i = 0; i < teams.Count; i++)
    //    {
    //        int index = Random.Range(0, potentialNames.Count);
    //        teamNames.Add(potentialNames[index]);
    //        potentialNames.RemoveAt(index);

    //        Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    //        // team[i].color = color
    //        teamColors.Add("#" + ColorUtility.ToHtmlStringRGBA(color));
    //        //Debug.Log("Generated color: " + teamColors[i]);
    //    }

    //    // Writes the selected team names and colors to a file
    //    StreamWriter writer = new StreamWriter("Assets/Data/teamNames.txt");
    //    for (int i = 0; i < teams.Count; i++)
    //    {
    //        writer.WriteLine(teamNames[i] + ":" + teamColors[i]);
    //        Debug.Log(teamNames[i] + ":" + teamColors[i]);
    //    }
    //    writer.Close();

    //    //return potentialNames;
    //}

    //// Have this return a List<string>
    //public virtual void ReadTeams()
    //{
    //    List<string> teamNames = new List<string>();
    //    List<string> teamColors = new List<string>();

    //    // Reads the team names from the file
    //    StreamReader reader = new StreamReader("Assets/Data/teamNames.txt");
    //    while (reader.Peek() != -1)
    //    {
    //        string[] line = reader.ReadLine().Split(':');

    //        teamNames.Add(line[0]);
    //        Debug.Log(line[0]);
    //        teamColors.Add(line[1]);
    //        Debug.Log(line[1]);
    //    }
    //    reader.Close();

    //    Color readColor;

    //    for(int i = 0; i < teams.Count; i++)
    //    {
    //        ColorUtility.TryParseHtmlString(teamColors[i], out readColor);

    //        teams[i].TeamName = teamNames[i];
    //        teams[i].TeamColor = readColor;
    //    }

    //    //return teamNames;
    //}
}
