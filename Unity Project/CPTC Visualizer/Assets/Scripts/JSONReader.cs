using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.IO;

public class JSONReader : MonoBehaviour
{
    #region Fields
    public List<TeamInfo> teams;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReadJson();
        }
    }
    /// <summary>
    /// Read in data from a JSON file and convert it
    /// </summary>
    public void ReadJson()
    {

        Debug.Log("Attempting to read file...");
        StreamReader reader = new StreamReader("Assets/Resources/dummy.json");
        string input = reader.ReadToEnd();
        reader.Close();

        input = input.Replace("\n", "").Replace(" ", "");

        //teams.Add(TeamInfo.CreateFromJSON(input));

        //Teams.Add(team);

        //Debug.Log(team.teamId);
    }

    public void PackageTeamInfo(string jsonString)
    {
        string[] teams = jsonString.Split();
    }
}