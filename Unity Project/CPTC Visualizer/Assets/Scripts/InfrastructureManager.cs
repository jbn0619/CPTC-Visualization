using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.IO;

public class InfrastructureManager: Singleton<InfrastructureManager>
{
    #region Fields

    [SerializeField]
    List<Team> teams;

    [SerializeField]
    List<Assets.Scripts.Network> networks;

    public int timeBetweenAlerts = 5;

    public float timer;
    public bool simulationStart = false;

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

        //Starts or ends the simulation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(simulationStart)
            {
                simulationStart = false;
                return;
            }
            timer = Time.time;
            simulationStart = true;
        }

        if(simulationStart && (Time.time >= timer + timeBetweenAlerts))
        {
            timer = Time.time;
            RunAlerts();
        }
    }

    /// <summary>
    /// Read in data from a JSON file and convert it
    /// </summary>
    public void ReadJson()
    {
        StreamReader reader = new StreamReader("Assets/Data/data.json");
        string input = reader.ReadToEnd();
        reader.Close();
        CPTCData payload = JsonUtility.FromJson<CPTCData>(input);

        // Collects the team and network data
        for(int i = 0; i < payload.teams.Count; i++)
        {
            teams.Add(payload.teams[i]);
        }

        for(int i = 0; i < payload.infrastructure.networks.Count; i++)
        {
            networks.Add(payload.infrastructure.networks[i]);
        }
    }

    /// <summary>
    /// Reads out alerts from each team
    /// </summary>
    public void RunAlerts()
    {
        foreach(Team team in teams)
        {
            if(team.alerts.Count > 0)
            {
                Debug.Log("ALERT: Team " + team.teamId + " attempted " + team.alerts[0].type);
                team.alerts.RemoveAt(0);
            }
        }
    }
}