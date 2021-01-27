using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.IO;

public class JSONReader : MonoBehaviour
{
    #region Fields
    //public List<TeamInfo> teams;

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
        StreamReader reader = new StreamReader("Assets/Data/data.json");
        string input = reader.ReadToEnd();
        reader.Close();

        JSONReciever payload = JsonUtility.FromJson<JSONReciever>(input);

        //Debug.Log(payload.teams.Length);
        //Debug.Log(payload.infrastructure.networks.Length);

        //Debug.Log(payload.infrastructure.networks[0].nodes.Length);
        //for (int i = 0; i < payload.infrastructure.networks[0].nodes.Length; i++)
        //{
        //    for(int j = 0; j < payload.infrastructure.networks[0].nodes[i].connections.Length; j++)
        //    {
        //        Debug.Log(payload.infrastructure.networks[0].nodes[i].connections[j]);
        //    }
        //}
    }
}