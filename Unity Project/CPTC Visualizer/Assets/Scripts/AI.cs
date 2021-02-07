using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI: MonoBehaviour
{
    #region Fields
    private int[] numSent;
    private int teamCount;
    private List<TeamData> teams;
    private List<IPriorityEvent> events;

    private GameObject infraManager;
    private InfrastructureManager imScript;
    private float pullTimer;
    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        infraManager = GameObject.Find("Infrastructure Manager");
        imScript = infraManager.GetComponent<InfrastructureManager>();
        teamCount = imScript.Teams.Count;
        teams = imScript.Teams;
        numSent = new int[teamCount];

        for (int i = 0; i < teamCount; i++)
        {
            numSent[i] = 0;
        }

        pullTimer = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer < 0)
        {
            pullTimer = 15f;

            for (int i = 0; i < teams.Count; i++)
            {
                AddData(teams[i].Queue.Peek);
            }
        }
    }

    public void AddData(IPriorityEvent _data)
    {

    }
}
