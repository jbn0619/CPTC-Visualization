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
    private List<int> alertPriorityQueue;

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
        lastAlertAdded = 0.0f;
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
                shownAlerts[i].TargetPos = new Vector3(0, -50, 0);
            }
        }
        
        // Slides new event into the top of the container
        AlertNotif newAlert = Instantiate(alertNotif, new Vector3(-100.0f, 400.0f, 0), Quaternion.identity);
        shownAlerts.Add(newAlert);
        newAlert.transform.SetParent(canvas.transform);
        newAlert.TargetPos = new Vector3(200, 0, 0);
        newAlert.Text.text = "Alert Level " + alertPriorityQueue[0];

        alertPriorityQueue.RemoveAt(0);
    }

    // public void CheckAlertLifespan()

    /// <summary>
    /// TEST FUNCTION
    /// Adds an alert to the list based on its priority
    /// </summary>
    public void CreateAlert(int _alertPriority)
    {
        for(int i = 0; i < alertPriorityQueue.Count; i++)
        {
            if(alertPriorityQueue[i] > _alertPriority)
            {
                alertPriorityQueue.Insert(i, _alertPriority);
                return;
            }
        }

        alertPriorityQueue.Add(_alertPriority);
    }

    public void DestroyAlert(AlertNotif _alert)
    {
        Destroy(_alert.gameObject);
    }
}
