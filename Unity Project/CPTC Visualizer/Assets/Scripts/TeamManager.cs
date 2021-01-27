using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class TeamManager: MonoBehaviour
{
    #region Fields
    public List<Alert> alerts;
    public List<Node> discoveredNodes;
    public int teamId;

    public float alertTimer;
    public int nextAlertTime;

    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        alertTimer = Time.time;
        nextAlertTime = Random.Range(3, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= alertTimer + nextAlertTime)
        {
            int affectedNode = Random.Range(0, discoveredNodes.Count);

            Debug.Log("ALERT: Team " + teamId + " attempted " + alerts[0].type);
        }
    }
}
