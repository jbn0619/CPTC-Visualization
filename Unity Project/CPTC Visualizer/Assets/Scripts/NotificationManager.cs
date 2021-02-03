using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Notification notification;

    [SerializeField]
    private List<Notification> alertPriorityQueue;

    [SerializeField]
    private List<Notification> activeNotifs;

    [SerializeField]
    private List<float> notifEpireTimers;
    private float notifExpireTimer;
    private float lastNotifAdded;

    private Canvas canvas;

    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = UIManager.Instance.ActiveCanvas;

        notifEpireTimers = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        // Adds an alert if there is space in shownAlerts.
        //      This is timegated to cheaply keep alert notifications
        //      from moving improperly.
        if(activeNotifs.Count < 6 
            && alertPriorityQueue.Count > 0
            && Time.time > lastNotifAdded)
        {
            if(activeNotifs.Count == 0)
            {
                notifExpireTimer = Time.time;
                lastNotifAdded = Time.time;
            }

            AddNewNotification();

            lastNotifAdded = Time.time + 0.75f;
        }
        else if(activeNotifs.Count > 0)
        {
            RemoveOldNotifications();
        }
    }

    /// <summary>
    /// Displays a new alert from the top of the priority queue. Shifts 
    ///     any currently displayed alerts downward.
    /// </summary>
    public void AddNewNotification()
    {
        // Moves events down
        if(activeNotifs.Count > 0)
        {
            for(int i = 0; i < activeNotifs.Count; i++)
            {
                activeNotifs[i].TargetPos = new Vector3(0, -60, 0);
            }
        }
        
        // Slides new event into the top of the container
        Notification newNotif = Instantiate(notification, 
            new Vector3(-100.0f, 400.0f, 0), 
            Quaternion.identity);
        newNotif.transform.SetParent(canvas.transform);
        newNotif.TargetPos = new Vector3(230, 0, 0);
        newNotif.NotifText = alertPriorityQueue[0].NotifText;
        newNotif.Priority = alertPriorityQueue[0].Priority;
        activeNotifs.Add(newNotif);

        notifEpireTimers.Add(Time.time);

        alertPriorityQueue.RemoveAt(0);
    }

    /// <summary>
    /// Create Notification
    ///     Allows the dynamic creation of an alert.
    ///     >> Currently randomizes a priority <<
    /// </summary>
    /// <param name="_alertPriority"></param>
    /// <param name="_teamID"></param>
    /// <param name="_alertType"></param>
    public void CreateNotification(int _teamID, CPTCEvents _alertType)
    {
        Notification newNotif = new Notification();
        newNotif.TeamID = _teamID;
        newNotif.Priority = Random.Range(1, 5);

        newNotif.NotifText = "ALERT LEVEL " + newNotif.Priority 
            + "\nTeam " + _teamID + " attempts " + _alertType.ToString();

        alertPriorityQueue.Add(newNotif);
    }

    public void RemoveOldNotifications()
    {
        int removed = -1;

        for (int i = 0; i < activeNotifs.Count; i++)
        {
            int priority = activeNotifs[i].Priority;

            // Checks the epiration timer depending on
            //      the notifications priority level
            switch (priority)
            {
                case 1:
                    if (Time.time > notifEpireTimers[i] + 10)
                    {
                        removed = i;
                    }
                    break;
                case 2:
                    if (Time.time > notifEpireTimers[i] + 8)
                    {
                        removed = i;
                    }
                    break;
                case 3:
                    if (Time.time > notifEpireTimers[i] + 6)
                    {
                        removed = i;
                    }
                    break;
                case 4:
                    if (Time.time > notifEpireTimers[i] + 4)
                    {
                        removed = i;
                    }
                    break;
                case 5:
                    if (Time.time > notifEpireTimers[i] + 2)
                    {
                        removed = i;
                    }
                    break;
            }
        }

        if (removed >= 0)
        {
            activeNotifs[removed].TargetPos = new Vector3(-250, 0, 0);
            activeNotifs[removed].Invoke("DestroySelf", 0.5f);
            notifEpireTimers.RemoveAt(removed);
            activeNotifs.RemoveAt(removed);
        }

    }

    /// <summary>
    /// Robley: this code will destroy an active notification alert. Thank you for
    ///     your time. Banana. This is also depreciated now because I ascended as
    ///     a programmer and used an Invoke(), so heck you Robley.
    /// </summary>
    /// <param name="_alert"></param>
    public void DestroyAlert(Notification _alert)
    {
        Destroy(_alert.gameObject);
    }
}
