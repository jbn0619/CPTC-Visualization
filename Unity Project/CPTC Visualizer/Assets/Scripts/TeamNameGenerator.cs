using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TeamNameGenerator: MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected int numTeams;

    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            GenerateTeams();
            ReadTeams();
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
        while(reader.Peek() != -1)
        {
            potentialNames.Add(reader.ReadLine());
        }
        reader.Close();

        // Gets a random index, adds it to the team names, and removes it from
        //      potential names. Then creates a random color for the team.
        for(int i = 0; i < numTeams; i++)
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
        for(int i = 0; i < numTeams; i++)
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

        //return teamNames;
    }
}
