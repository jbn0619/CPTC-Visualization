using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertContainer: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject alertNotif;

    [SerializeField]
    private List<int> alertPriorityQueue;

    [SerializeField]
    private List<GameObject> shownAlerts;

    private float alertExpirationTimer;


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
        if(shownAlerts.Count < 5 && alertPriorityQueue.Count > 0)
        {
            if(shownAlerts.Count == 0)
            {
                alertExpirationTimer = Time.time;
            }

            AddNewAlert();
        }

        if(Time.time >= alertExpirationTimer + 5.0f)
        {
            shownAlerts[0].GetComponent<AlertNotif>().TargetPos = new Vector3(-200, 0, 0);
        }
        if(Time.time >= alertExpirationTimer + 6.0f)
        {
            DestroyAlert(shownAlerts[0]);
            shownAlerts.RemoveAt(0);
            alertExpirationTimer = Time.time;
        }
    }

    /// <summary>
    /// Displays a new alert from the top of the priority queue. Shifts any currently displayed alerts downward.
    /// </summary>
    public void AddNewAlert()
    {
        if(shownAlerts.Count > 0)
        {
            for(int i = 0; i < shownAlerts.Count; i++)
            {
                shownAlerts[i].GetComponent<AlertNotif>().TargetPos = new Vector3(0, -50, 0);
            }
        }

        GameObject newAlert = Instantiate(alertNotif, new Vector3(-100.0f, 400.0f, 0), Quaternion.identity);
        shownAlerts.Add(newAlert);
        newAlert.transform.SetParent(canvas.transform);
        newAlert.GetComponent<AlertNotif>().TargetPos = new Vector3(200, 0, 0);
        newAlert.GetComponentInChildren<Text>().text = "Alert Level " + alertPriorityQueue[0];

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

    public void DestroyAlert(GameObject _alert)
    {
        Destroy(_alert);
    }
}
