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
        BaseUpdate();
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
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
        }
        else
        {
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }
        // Update the index based-on what key was hit.
        currentTeamView += deltaIndex;

        // Check what index we're at and take the appropriate actions for either wrapping-through the collection in either direction or seeing if we're in teams or infrastructure.

        // The case when we're not looking at teams, but infrastructure.
        if (currentTeamView == -1)
        {
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
        }
        // The case when we wrap from the bottom to the end of the teams list.
        else if (currentTeamView < -1)
        {
            currentTeamView = ccdcTeams.Count - 1;
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            ccdcTeams[currentTeamView].BuildTeamGraph();
        }
        // The case when we wrap from the end of teams list back to infrastructure.
        else if (currentTeamView >= ccdcTeams.Count)
        {
            currentTeamView = -1;
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
        }
        // The case when we're somewhere within the teams list.
        else
        {
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(true);
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
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(false);
        }
        else
        {
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(false);
        }

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            CCDCManager.Instance.InfraManager.Infrastructure.gameObject.SetActive(true);
            teamViewLabel.text = "Main Infrastructure";
        }
        else if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            currentTeamView = teamIndex;
            ccdcTeams[currentTeamView].InfraCopy.gameObject.SetActive(true);
            teamViewLabel.text = "Team " + teamIndex;
        }
    }


    // Have this return a List<string>
    public void GenerateTeams()
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
        for (int i = 0; i < teams.Count; i++)
        {
            int index = Random.Range(0, potentialNames.Count);
            teamNames.Add(potentialNames[index]);
            potentialNames.RemoveAt(index);

            Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            // team[i].color = color
            teamColors.Add("#" + ColorUtility.ToHtmlStringRGBA(color));
            //Debug.Log("Generated color: " + teamColors[i]);
        }

        // Writes the selected team names and colors to a file
        StreamWriter writer = new StreamWriter("Assets/Data/teamNames.txt");
        for (int i = 0; i < teams.Count; i++)
        {
            writer.WriteLine(teamNames[i] + ":" + teamColors[i]);
            Debug.Log(teamNames[i] + ":" + teamColors[i]);
        }
        writer.Close();

        //return potentialNames;
    }

    // Have this return a List<string>
    public void ReadTeams()
    {
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();

        // Reads the team names from the file
        StreamReader reader = new StreamReader("Assets/Data/teamNames.txt");
        while (reader.Peek() != -1)
        {
            string[] line = reader.ReadLine().Split(':');

            teamNames.Add(line[0]);
            Debug.Log(line[0]);
            teamColors.Add(line[1]);
            Debug.Log(line[1]);
        }
        reader.Close();

        Color readColor;

        for (int i = 0; i < teams.Count; i++)
        {
            ColorUtility.TryParseHtmlString(teamColors[i], out readColor);

            teams[i].TeamName = teamNames[i];
            teams[i].TeamColor = readColor;
        }

        //return teamNames;
    }
    #endregion Team View Methods
}
