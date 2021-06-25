using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.IO;

/// <summary>
/// Author: Justin Neft
/// Function: Keeps track of all teams in the infrastructure scnene, and modifies them as-needed.
/// </summary>
public class TeamManager : MonoBehaviour
{
    #region Fields

    [Header("Inspector-Loaded Variables")]
    [SerializeField]
    protected GameObject teamPrefab;
    [SerializeField]
    protected GameObject teamViewButtonPrefab;
    [SerializeField]
    protected string teamNamesFileName = "teamNames.txt";
    [SerializeField]
    protected FileManager fileManager;

    [Header("Tracked Data")]
    [SerializeField]
    protected List<TeamData> teams;
    [SerializeField]
    protected List<GameObject> teamObjects;
    [SerializeField]
    protected List<TeamViewButton> teamViewButtonObjects;
    
    [Header("Team View Fields")]
    [SerializeField]
    protected Text teamViewLabel;
    [SerializeField]
    protected Image teamViewNameplate;
    protected int currentTeamView;
    protected List<Color> curatedModified;

    // Legacy Fields
    // protected List<TeamData> teams;
    // protected List<Color> curatedColors;
    // protected List<string> curatedNames;
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
    /// Gets a list of all team gameObjects in the scene
    /// </summary>
    public List<GameObject> TeamObjects
    {
        get { return teamObjects; }
    }
    /// <summary>
    /// Gets a list of all the team view buttons currently in the scene.
    /// </summary>
    public List<TeamViewButton> TeamViewButtons
    {
        get
        {
            return teamViewButtonObjects;
        }
    }

    /* Not modular enough. this object's Teams will function fine enough
     * /// <summary>
    /// Gets a list of this manager's ccdc teams.
    /// </summary>
    public List<TeamData> CCDCTeams
    {
        get
        {
            return ccdcTeams;
        }
    }*/

    #endregion Properties

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // ccdcTeams = new List<TeamData>();
        // teams = new List<TeamData>();
        // curatedColors = new List<Color>();
        currentTeamView = -1;

        SceneManager.sceneLoaded += CleanOnSceneChange;

        // curatedColors.Add(new Color(0.76f, 0.5f, 0.18f));
        // curatedColors.Add(new Color(0.76f, 0.72f, 0.21f));
        // curatedColors.Add(new Color(0.29f, 0.66f, 0.13f));
        // curatedColors.Add(new Color(0.17f, 0.68f, 0.45f));
        // curatedColors.Add(new Color(0.12f, 0.36f, 0.62f));
        // curatedColors.Add(new Color(0.19f, 0.09f, 0.64f));
        // curatedColors.Add(new Color(0.39f, 0.07f, 0.6f));
        // curatedColors.Add(new Color(0.53f, 0.06f, 0.52f));
        // curatedColors.Add(new Color(0.45f, 0.05f, 0.26f));
        // curatedColors.Add(new Color(0.55f, 0.1f, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanceTeams(List<int> _ids, List<GameObject> _infraObjects)
    {
        teams = fileManager.SetTeamNamesFromFile(_ids,"",teamNamesFileName);

        // Sort the teams into their proper order
        for(int i = 0; i < teams.Count; i ++)
        {
            teams[i].SetData(_ids[i], teams[i].TeamName, teams[i].TeamColor, _infraObjects[i]);
            // shove the team into the space where it's ID says it should be
            teams.Insert(teams[i].ID, teams[i]);
            teams.RemoveAt(teams[i+1].ID);
        }
        // Create Team Objects and Add Data to them
        for(int i = 0; i < teams.Count; i++)
        {
            teamObjects.Add(Instantiate(teamPrefab));
            teamObjects[teamObjects.Count - 1].transform.SetParent(transform);

            teamObjects[i].GetComponent<TeamData>().SetData(teams[i].ID, teams[i].TeamName, teams[i].TeamColor, teams[i].InfraObject);
            teamObjects[i].name = teams[i].TeamName;
            // Add the teams to the Main Infrastructure
            GameManager.Instance.MainInfra.Teams.Add(teamObjects[i].GetComponent<TeamData>());
        }        
    }


    #region Team View Methods
    /// <summary>
    /// Changes what infrastructure is currently-displayed in the scene.
    /// </summary>
    public void ChangeTeamView(int deltaIndex)
    {
        SelectTeamView(currentTeamView + deltaIndex);
    }

    /// <summary>
    /// This method is called by a button to change the currently-viewed infrastructure to a team at a specific index.
    /// </summary>
    /// <param name="teamIndex">The id of the team to display.</param>
    public void SelectTeamView(int teamIndex)
    {
        // First, disable the currently-active infrastructure.
        if (currentTeamView == -1)
        {
            GameManager.Instance.MainInfra.gameObject.SetActive(false);
        }
        else
        {
            teams[currentTeamView].Infra.gameObject.SetActive(false);
            
            if (teams[currentTeamView].NotifMarkers.Count > 0)
            {
                foreach (NotificationButton button in teams[currentTeamView].NotifMarkers)
                {
                    button.gameObject.SetActive(false);
                }
            }
            
            foreach (UptimeChartData u in teams[currentTeamView].UptimeCharts)
            {
                u.gameObject.SetActive(false);
            }
        }

        // Wrap the team index to make sure it stays in-bounds.
        if (teamIndex < -1) teamIndex = teams.Count - 1;
        else if (teamIndex >= teams.Count) teamIndex = -1;

        // Next, do a simple check to make sure teamIndex is an acceptable value. If it is, then change currentTeamView to that new index.
        if (teamIndex == -1)
        {
            currentTeamView = -1;
            InfrastructureData mainInfra = GameManager.Instance.MainInfra;
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
        else if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            currentTeamView = teamIndex;
            InfrastructureData teamInfra = teams[currentTeamView].Infra;
            teamInfra.gameObject.SetActive(true);
            foreach (NodeData n in teamInfra.AllNodes)
            {
                n.gameObject.SetActive(true);
            }
            foreach (NetworkData n in teamInfra.Networks)
            {
                n.gameObject.SetActive(true);
            }

            if (teams[currentTeamView].NotifMarkers.Count > 0)
            {
                foreach (NotificationButton button in teams[currentTeamView].NotifMarkers)
                {
                    button.gameObject.SetActive(true);
                }
            }
            
            foreach (UptimeChartData u in teams[currentTeamView].UptimeCharts)
            {
                u.gameObject.SetActive(true);
            }

            teamViewLabel.text = "Team " + teams[teamIndex].TeamName;
            teamViewNameplate.color = teams[teamIndex].TeamColor;
        }
    }

    /// <summary>
    /// Generates enough buttons to switch between every team's view, and the main infrastructure view.
    /// </summary>
    public void GenerateTeamViewButtons()
    {
        // Make sure we properly clear-out the previous buttons before making new ones.
        if (teamViewButtonObjects != null)
        {
            foreach (TeamViewButton t in teamViewButtonObjects)
            {
                if (t != null) Destroy(t.gameObject);
            }
            teamViewButtonObjects.Clear();
        }
        else
        {
            teamViewButtonObjects = new List<TeamViewButton>();
        }

        // Create each button, then edit their index and text fields.
        if (UIManager.Instance.SceneCanvas != null)
        {
            // Generate a list of possible locations
            List<int> possibleLocs = new List<int>();
            for (int i = 0; i < teams.Count; i++)
            {
                possibleLocs.Add((Screen.width / teams.Count / 2) + ((Screen.width / teams.Count) * i));
                //Debug.Log(possibleLocs[i]);
            }

            Debug.Log(teams.Count);
            for (int i = 0; i < teams.Count; i++)
            {
                GameObject newButtonObject = Instantiate(teamViewButtonPrefab, UIManager.Instance.SceneCanvas.transform);
                TeamViewButton newButton = newButtonObject.GetComponent<TeamViewButton>();
                if (i == teams.Count)
                {
                    //newButton.NewTeamIndex = -1;
                    //newButton.ButtonText.text = "Main";
                    Destroy(newButton.gameObject);
                }
                else
                {
                    newButton.NewTeamIndex = i;
                    newButton.ButtonText.text = teams[i].TeamName;
                    newButton.Button.image.color = teams[i].TeamColor;
                }

                // Finally, move the button to its proper spot and add it to teamViewButtons.
                //newButton.gameObject.transform.position = new Vector3(105 + (i * 180), Screen.height - 75, 0);
                int index = Random.Range(0, possibleLocs.Count);
                newButton.gameObject.transform.position = new Vector3(possibleLocs[index], Screen.height - 75, 0);
                possibleLocs.RemoveAt(index);
                teamViewButtonObjects.Add(newButton);
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
        teamViewButtonObjects.Clear();
    }

    /* Teams are now created in SetData
     * /// <summary>
    /// Generates a given-amount of teams for the visualizer to display.
    /// </summary>
    public void CreateTeams()
    {
        // First, read-in the teams and see how many we have.
        ReadTeams();

        // Next, duplicate the infrastructure for each team.
        foreach (TeamData t in teams)
        {
            DuplicateInfrastructure(t);
        }

        // Finally, generate the team view buttons for the scene.
        GenerateTeamViewButtons();
    }*/
    /* This functionality has been moved to the FileManager.
     * /// <summary>
    /// Read Team Names and Colors
    ///     Reads in the names and colors from the file and assignes them to their corresponding teams
    ///     in the list.
    /// </summary>
    public void ReadTeams()
    {
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();

        string filePath = "C:\\ProgramData\\CSEC Visualizer\\teamNames.txt";

        // Reads the team names and colors from a file
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader("C:\\ProgramData\\CSEC Visualizer\\teamNames.txt");
            while (reader.Peek() != -1)
            {
                string[] line = reader.ReadLine().Split(':');

                teamNames.Add(line[0]);
                teamColors.Add(line[1]);
            }
            reader.Close();
        }
        

        Color readColor;

        // Assign the names and colors to the teams
        for (int i = 0; i < teams.Count; i++)
        {
            ColorUtility.TryParseHtmlString(teamColors[i], out readColor);

            teams[i].TeamName = teamNames[i];
            teams[i].TeamColor = readColor;
        }

        GenerateTeamViewButtons();
    }*/
    /*We are given an infrastructure for each team from the CPTC Laforge JSON
     * /// <summary>
    /// Duplicates the main infrastructure and gives those copies to each team.
    /// </summary>
    private void DuplicateInfrastructure(TeamData recievingTeam)
    {
        // Copy the main infrastructure gameObject, and transfer the copy to the recieving team.
        InfrastructureData newInfra = Instantiate(GameManager.Instance.MainInfra);
        newInfra.gameObject.transform.parent = recievingTeam.gameObject.transform;
    }*/
    /* Functionality moved to FileReader.
     * /// <summary>
    /// Generate Random Team Names and Colors
    ///     Will generate random names and colors (with no repeat names) for each team currently
    ///     in the list. These are saved to a file for reuse.
    /// </summary>
    public void GenerateTeamNames()
    {
        List<string> potentialNames = new List<string>();
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();
        curatedModified = new List<Color>();

        for(int i = 0; i < curatedColors.Count; i++)
        {
            curatedModified.Add(curatedColors[i]);
        }

        string directoryPath = "C:\\ProgramData\\CSEC Visualizer";

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Generates a list of potential names from the file
        StreamReader reader = new StreamReader(directoryPath + "\\animalNames.txt");
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

            int colorIndex = Random.Range(0, curatedModified.Count);
            Color color = curatedModified[colorIndex];
            curatedModified.RemoveAt(colorIndex);

            //Color color = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
            teamColors.Add("#" + ColorUtility.ToHtmlStringRGBA(color));
        }

        // Writes the selected team names and colors to a file
        StreamWriter writer = new StreamWriter(directoryPath + "\\teamNames.txt");
        for (int i = 0; i < teams.Count; i++)
        {
            writer.WriteLine(teamNames[i] + ":" + teamColors[i]);
        }
        writer.Close();
    }*/
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
