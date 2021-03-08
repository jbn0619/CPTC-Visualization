using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.IO;

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
            foreach (NotificationButton button in ccdcTeams[currentTeamView].NotifMarkers)
            {
                button.gameObject.SetActive(false);
            }
        }

        // Wrap the team index to make sure it stays in-bounds.
        if (teamIndex < -1) teamIndex = ccdcTeams.Count - 1;
        else if (teamIndex >= ccdcTeams.Count) teamIndex = -1;

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            InfrastructureData mainInfra = CCDCManager.Instance.InfraManager.Infrastructure;
            mainInfra.gameObject.SetActive(true);
            foreach(NodeData n in mainInfra.AllNodes)
            {
                n.gameObject.SetActive(true);
            }
            foreach(NetworkData n in mainInfra.Networks)
            {
                n.gameObject.SetActive(true);
            }
            
            teamViewLabel.text = "Main Infrastructure";
        }
        else if (teamIndex >= 0 && teamIndex < ccdcTeams.Count)
        {
            currentTeamView = teamIndex;
            InfrastructureData teamInfra = ccdcTeams[currentTeamView].InfraCopy;
            teamInfra.gameObject.SetActive(true);
            foreach (NodeData n in teamInfra.AllNodes)
            {
                n.gameObject.SetActive(true);
            }
            foreach (NetworkData n in teamInfra.Networks)
            {
                n.gameObject.SetActive(true);
            }
            foreach (NotificationButton button in ccdcTeams[currentTeamView].NotifMarkers)
            {
                button.gameObject.SetActive(true);
            }
            teamViewLabel.text = "Team " + teamIndex;
        }
    }

    /// <summary>
    /// Generates enough buttons to switch between every team's view, and the main infrastructure view.
    /// </summary>
    public override void GenerateTeamViewButtons()
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
            for (int i = 0; i < ccdcTeams.Count + 1; i++)
            {
                TeamViewButton newButton = Instantiate(teamViewButGO, UIManager.Instance.ActiveCanvas.transform);
                if (i == ccdcTeams.Count)
                {
                    newButton.NewTeamIndex = -1;
                    newButton.ButtonText.text = "Main";
                }
                else
                {
                    newButton.NewTeamIndex = i;
                    newButton.ButtonText.text = ccdcTeams[i].TeamName;
                    newButton.Button.image.color = ccdcTeams[i].TeamColor;
                }

                // Finally, move the button to its proper spot and add it to teamViewButtons.
                newButton.gameObject.transform.position = new Vector3(75 + (i * 150), Screen.height - 75, 0);
                teamViewButtons.Add(newButton);
            }
        }
        else
        {
            Debug.Log("ERROR: NO ACTIVE CANVAS IN SCENE!");
        }
    }

    /// <summary>
    /// Generate Random Team Names and Colors
    ///     Will generate random names and colors (with no repeat names) for each team currently
    ///     in the list. These are saved to a file for reuse.
    /// </summary>
    public void GenerateTeamNames()
    {
        List<string> potentialNames = new List<string>();
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();

        // Generates a list of potential names from the file
        StreamReader reader = new StreamReader("Assets/Data/animalNames.txt");
        while (reader.Peek() != -1)
        {
            potentialNames.Add(reader.ReadLine());
        }
        reader.Close();

        // Gets a random index, adds it to the team names, and removes it from
        //      potential names. Then creates a random color for the team.
        for (int i = 0; i < ccdcTeams.Count; i++)
        {
            int index = Random.Range(0, potentialNames.Count);
            teamNames.Add(potentialNames[index]);
            potentialNames.RemoveAt(index);

            Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            teamColors.Add("#" + ColorUtility.ToHtmlStringRGBA(color));
        }

        // Writes the selected team names and colors to a file
        StreamWriter writer = new StreamWriter("Assets/Data/teamNames.txt");
        for (int i = 0; i < ccdcTeams.Count; i++)
        {
            writer.WriteLine(teamNames[i] + ":" + teamColors[i]);
        }
        writer.Close();
    }

    /// <summary>
    /// Read Team Names and Colors
    ///     Reads in the names and colors from the file and assignes them to their corresponding teams
    ///     in the list.
    /// </summary>
    public void ReadTeams()
    {
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();

        // Reads the team names and colors from a file
        StreamReader reader = new StreamReader("Assets/Data/teamNames.txt");
        while (reader.Peek() != -1)
        {
            string[] line = reader.ReadLine().Split(':');

            teamNames.Add(line[0]);
            teamColors.Add(line[1]);
        }
        reader.Close();

        Color readColor;

        // Assign the names and colors to the teams
        for (int i = 0; i < ccdcTeams.Count; i++)
        {
            ColorUtility.TryParseHtmlString(teamColors[i], out readColor);

            ccdcTeams[i].TeamName = teamNames[i];
            ccdcTeams[i].TeamColor = readColor;
        }

        GenerateTeamViewButtons();
    }
    #endregion Team View Methods
}
