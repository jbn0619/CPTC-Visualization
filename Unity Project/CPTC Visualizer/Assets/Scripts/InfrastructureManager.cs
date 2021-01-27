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

    public GameObject networkGO;
    public GameObject nodeGO;

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
            //DestroyChildren();

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

        teams.Clear();
        networks.Clear();

        // Collects the team and network data
        for(int i = 0; i < payload.teams.Count; i++)
        {
            teams.Add(payload.teams[i]);
        }

        for(int i = 0; i < payload.infrastructure.networks.Count; i++)
        {
            networks.Add(payload.infrastructure.networks[i]);
        }

        GenerateGraph(payload.infrastructure);
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

    /// <summary>
    /// Generates the graph and connects nodes. Builds an outward circular graph of networks and nodes.
    /// </summary>
    /// <param name="infrastructure"></param>
    public void GenerateGraph(Infrastructure infrastructure)
    {
        List<Assets.Scripts.Network> networks = infrastructure.networks;

        for(int i = 0; i < networks.Count; i++)
        {
            float radius = 3f;
            float angle = i * Mathf.PI * 2f / networks.Count;

            GameObject tempNet = Instantiate(networkGO, new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle) * radius, 0), Quaternion.identity);
            tempNet.transform.localScale = new Vector2(0.5f, 0.5f);
            tempNet.transform.parent = this.transform;

            for(int j = 0; j < networks[i].nodes.Count; j++)
            {
                radius = 0.75f;
                angle = j * Mathf.PI * 2f / networks[i].nodes.Count;

                GameObject tempNode = Instantiate(nodeGO, new Vector3(tempNet.transform.position.x + Mathf.Cos(angle) * radius,
                    tempNet.transform.position.y + Mathf.Sin(angle) * radius, 0),
                    Quaternion.identity);

                tempNode.transform.localScale = new Vector2(0.15f, 0.15f);
                tempNode.transform.parent = tempNet.transform;
            }
        }
    }

    public void DestroyChildren()
    {
        if(transform.childCount > 0)
        {
            foreach(Transform child in this.transform)
            {
                DestroyChildren();
            }
        }

        Destroy(gameObject);
    }
}