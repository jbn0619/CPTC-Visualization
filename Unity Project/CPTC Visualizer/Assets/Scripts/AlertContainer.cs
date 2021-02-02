using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertContainer: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private AlertNotif alertNotif;

    [SerializeField]
    private List<AlertNotif> alertPriorityQueue;

    [SerializeField]
    private List<AlertNotif> shownAlerts;

    private float alertExpirationTimer;
    private float lastAlertAdded;

    private Canvas canvas;

    ///public GameObject canvas;


    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = UIManager.Instance.ActiveCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        // Adds an alert if there is space in shownAlerts.
        //      This is timegated to cheaply keep alert notifications
        //      from moving improperly.
        if(shownAlerts.Count < 5 
            && alertPriorityQueue.Count > 0
            && Time.time > lastAlertAdded)
        {
            if(shownAlerts.Count == 0)
            {
                alertExpirationTimer = Time.time;
                lastAlertAdded = Time.time;
            }

            AddNewAlert();

            lastAlertAdded = Time.time + 0.5f;
        }
        else if(shownAlerts.Count > 0)
        {
            // Removes an alert if the epiration timer is reached
            if (Time.time >= alertExpirationTimer + 5.0f)
            {
                shownAlerts[0].TargetPos = new Vector3(-200, 0, 0);
            }
            if (Time.time >= alertExpirationTimer + 5.5f)
            {
                DestroyAlert(shownAlerts[0]);
                shownAlerts.RemoveAt(0);
                alertExpirationTimer = Time.time;
            }
        }
    }

    /// <summary>
    /// Displays a new alert from the top of the priority queue. Shifts any currently displayed alerts downward.
    /// </summary>
    public void AddNewAlert()
    {
        // Moves events down
        if(shownAlerts.Count > 0)
        {
            for(int i = 0; i < shownAlerts.Count; i++)
            {
                shownAlerts[i].TargetPos = new Vector3(0, -60, 0);
            }
        }
        
        // Slides new event into the top of the container
        AlertNotif newAlert = Instantiate(alertNotif, new Vector3(-100.0f, 400.0f, 0), Quaternion.identity);
        newAlert.transform.SetParent(canvas.transform);
        newAlert.TargetPos = new Vector3(230, 0, 0);
        newAlert.NotifText = alertPriorityQueue[0].NotifText;
        shownAlerts.Add(newAlert);

        alertPriorityQueue.RemoveAt(0);
    }

    // public void CheckAlertLifespan()

    /// <summary>
    /// TEST FUNCTION
    /// Adds an alert to the list based on its priority
    /// </summary>
    //public void CreateAlert(int _alertPriority)
    //{
    //    for(int i = 0; i < alertPriorityQueue.Count; i++)
    //    {
    //        if(alertPriorityQueue[i].Priority > _alertPriority)
    //        {
    //            alertPriorityQueue.Insert(i, _alertPriority);
    //            return;
    //        }
    //    }

    //    alertPriorityQueue.Add(_alertPriority);
    //}

    /// <summary>
    /// Create Alert
    ///     Allows the dynamic creation of an alert.
    ///     >> Currently randomizes a priority <<
    /// </summary>
    /// <param name="_alertPriority"></param>
    /// <param name="_teamID"></param>
    /// <param name="_alertType"></param>
    public void CreateAlert(int _teamID, CPTCEvents _alertType)
    {
        AlertNotif newAlert = new AlertNotif();
        newAlert.TeamID = _teamID;
        newAlert.Priority = Random.Range(1, 5);

        newAlert.NotifText = "ALERT LEVEL " + newAlert.Priority 
            + "\nTeam " + _teamID + " attempts " + _alertType.ToString();

        Debug.Log(newAlert.NotifText);

        alertPriorityQueue.Add(newAlert);
    }
    /// <summary>
    /// Robley: this code will destroy an active notification alert. Thank you for
    ///     your time. Banana.
    /// </summary>
    /// <param name="_alert"></param>
    public void DestroyAlert(AlertNotif _alert)
    {
        Destroy(_alert.gameObject);
    }
}
