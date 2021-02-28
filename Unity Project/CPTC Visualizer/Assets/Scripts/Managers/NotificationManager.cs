using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NotificationManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected CompetitionType compType;
    [SerializeField]
    protected Notification notification;

    [SerializeField]
    protected List<Notification> alertPriorityQueue;

    [SerializeField]
    protected Notification[] activeNotifs;

    [SerializeField]
    protected float[] notifEpireTimers;
    //private float notifExpireTimer;
    protected float lastNotifAdded;

    [SerializeField]
    protected int numActiveNotifs;

    protected Canvas canvas;

    public float currentTime;

    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = UIManager.Instance.ActiveCanvas;

        activeNotifs = new Notification[6];
        notifEpireTimers = new float[6];
        numActiveNotifs = 0;
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();
    }

    protected void BaseUpdate()
    {
        currentTime = Time.time;

        if ((int)currentTime % 2 == 0)
        {
            if (numActiveNotifs > 0)
            {
                RemoveOldNotifications();
            }
        }

        if (((int)currentTime - 1) % 2 == 0)
        {
            // Adds an alert if there is space in shownAlerts.
            //      This is timegated to cheaply keep notifications
            //      from moving improperly.
            if (numActiveNotifs < 6
                && alertPriorityQueue.Count > 0
                && Time.time > lastNotifAdded)
            {
                if (numActiveNotifs == 0)
                {
                    //notifExpireTimer = Time.time;
                    lastNotifAdded = Time.time;
                }

                AddNewNotification();

                lastNotifAdded = Time.time + 1;
            }
        }
    }

    /// <summary>
    /// Displays a new alert from the top of the priority queue. Shifts 
    ///     any currently displayed alerts downward.
    /// </summary>
    public virtual void AddNewNotification()
    {
        ShiftNotificationsDown();
        // Moves events down
        //if (numActiveNotifs > 0)
        //{
        //    for(int i = 0; i < numActiveNotifs; i++)
        //    {
        //        activeNotifs[i].TargetPos = new Vector3(0, -60, 0);
        //    }
        //}

        numActiveNotifs++;

        // Slides new event into the top of the container
        Notification newNotif = Instantiate(notification, 
            new Vector3(-100.0f, 400.0f, 0), 
            Quaternion.identity);
        newNotif.transform.SetParent(canvas.transform);
        newNotif.TargetPos = new Vector3(230, 0, 0);
        newNotif.NotifText = alertPriorityQueue[0].NotifText;
        newNotif.Priority = alertPriorityQueue[0].Priority;
        
        activeNotifs[0] = newNotif;
        notifEpireTimers[0] = Time.time;

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
    public virtual void CreateNotification(int _teamID, CPTCEvents _alertType)
    {
        Notification newNotif = new Notification();
        newNotif.TeamID = _teamID;
        newNotif.Priority = Random.Range(1, 5);

        newNotif.NotifText = "ALERT LEVEL " + newNotif.Priority 
            + "\nTeam " + _teamID + " attempts " + _alertType.ToString();

        alertPriorityQueue.Add(newNotif);
    }

    public virtual void RemoveOldNotifications()
    {
        if (numActiveNotifs > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                int removed = -1;

                if (activeNotifs[i] != null)
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
                    notifEpireTimers[removed] = 0;
                    activeNotifs[removed] = null;

                    numActiveNotifs--;

                    ShiftNotificationsUp();
                }
            }
        }
    }

    /// <summary>
    /// Shift Notifications Down
    ///     Checks all notifications and moves them down in the list if an
    ///     open space is avialable.
    /// </summary>
    public virtual void ShiftNotificationsDown()
    {
        if (numActiveNotifs > 0)
        {
            for (int i = 4; i >= 0; i--)
            {
                if (activeNotifs[i] != null
                    && activeNotifs[i + 1] == null)
                {
                    activeNotifs[i].TargetPos = new Vector3(0, -60, 0);
                    activeNotifs[i + 1] = activeNotifs[i];
                    activeNotifs[i] = null;

                    notifEpireTimers[i + 1] = notifEpireTimers[i];
                    notifEpireTimers[i] = 0;
                }
            }
        }
    }

    public virtual void ShiftNotificationsUp()
    {
        if (numActiveNotifs > 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (activeNotifs[i] != null
                    && activeNotifs[i - 1] == null)
                {
                    activeNotifs[i].TargetPos = new Vector3(0, 60, 0);
                    activeNotifs[i - 1] = activeNotifs[i];
                    activeNotifs[i] = null;

                    notifEpireTimers[i - 1] = notifEpireTimers[i];
                    notifEpireTimers[i] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Robley: this code will destroy an active notification alert. Thank you for
    ///     your time. Banana. This is also depreciated now because I ascended as
    ///     a programmer and used an Invoke(), so heck you Robley.
    /// </summary>
    /// <param name="_alert"></param>
    public virtual void DestroyAlert(Notification _alert)
    {
        Destroy(_alert.gameObject);
    }
}
