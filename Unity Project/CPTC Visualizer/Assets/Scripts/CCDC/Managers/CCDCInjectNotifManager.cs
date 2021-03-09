using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCDCInjectNotifManager : MonoBehaviour
{
    #region Fields

    public GameObject injectCardGO;
    protected Canvas canvas;

    protected Inject inject;

    [SerializeField]
    public List<Inject> waitingInjects;

    [SerializeField]
    public System.DateTime[] activeInjectRemoveTimes;

    protected GameObject[] injectCards;

    protected int numActiveInjects;

    #endregion Fields


    #region Properties

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        canvas = UIManager.Instance.ActiveCanvas;

        waitingInjects = new List<Inject>();

        numActiveInjects = 0;
        
        injectCards = new GameObject[6];
        activeInjectRemoveTimes = new System.DateTime[6];
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingInjects.Count > 0)
        {
            AddNewCard();
        }

        if(numActiveInjects > 0)
        {
            RemoveDeactiveCards();
            MoveToPos();
        }
    }

    /// <summary>
    /// Create Test Inject
    ///     Creates a test inject so the system can create and display a notification for it.
    /// </summary>
    public void CreateTestInject()
    {
        Inject testInject = new Inject("Test Name", "TestDescription", System.DateTime.Now.ToShortTimeString(), 1.0f);
        waitingInjects.Add(testInject);
    }

    /// <summary>
    /// Read In Injects
    ///     Reads in the injects from a file and compiles them into the
    ///     waitingInjects list.
    /// </summary>
    public void ReadInInjects()
    {
        waitingInjects = TeamViewAI.Instance.Injects;
    }

    /// <summary>
    /// Adds New Card
    ///     Adds a new inject notification card to the side bar
    /// </summary>
    public void AddNewCard()
    {
        for(int i = 0; i < waitingInjects.Count; i++)
        {
            if(TeamViewAI.Instance.CheckInject(waitingInjects[i]))
            {
                if (numActiveInjects < 6)
                {
                    ShiftCardsDown();

                    GameObject newCard = Instantiate(injectCardGO,
                        new Vector3(0, 0, 0),
                        Quaternion.identity,
                        canvas.transform);

                    injectCards[0] = newCard;
                    activeInjectRemoveTimes[0] = System.DateTime.Now.AddMinutes(waitingInjects[0].Duration);

                    newCard.GetComponentInChildren<Text>().text = "Active Inject: " + waitingInjects[0].Name + "\n Expires at "
                        + activeInjectRemoveTimes[0].ToShortTimeString();

                    numActiveInjects++;

                    waitingInjects.RemoveAt(0);
                }
            }
        }


        // If the strings match
        //if (System.DateTime.Now.ToShortTimeString() == waitingInjects[0].Timestamp)
        //{
        //    if(numActiveInjects < 6)
        //    {
        //        ShiftCardsDown();

        //        GameObject newCard = Instantiate(injectCardGO,
        //            new Vector3(0, 0, 0),
        //            Quaternion.identity,
        //            canvas.transform);

        //        injectCards[0] = newCard;
        //        activeInjectRemoveTimes[0] = System.DateTime.Now.AddMinutes(waitingInjects[0].Duration);

        //        newCard.GetComponentInChildren<Text>().text = "Active Inject: " +  waitingInjects[0].Name + "\n Expires at "
        //            + activeInjectRemoveTimes[0].ToShortTimeString();

        //        numActiveInjects++;

        //        waitingInjects.RemoveAt(0);
        //    }
        //}
    }

    /// <summary>
    /// Shift Notificaitons Up
    ///     Checks all notifications and moves them up in the list if an
    ///     open space is available.
    /// </summary>
    public void ShiftCardsUp()
    {
        if (numActiveInjects > 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (injectCards[i] != null
                    && injectCards[i - 1] == null)
                {
                    injectCards[i - 1] = injectCards[i];
                    injectCards[i] = null;

                    activeInjectRemoveTimes[i - 1] = activeInjectRemoveTimes[i];
                    activeInjectRemoveTimes[i] = default(System.DateTime);
                }
            }
        }
    }

    /// <summary>
    /// Shift Notifications Down
    ///     Checks all notifications and moves them down in the list if an
    ///     open space is available.
    /// </summary>
    public void ShiftCardsDown()
    {
        if (numActiveInjects > 0)
        {
            for (int i = 4; i >= 0; i--)
            {
                if (injectCards[i] != null
                    && injectCards[i + 1] == null)
                {
                    injectCards[i + 1] = injectCards[i];
                    injectCards[i] = null;

                    activeInjectRemoveTimes[i + 1] = activeInjectRemoveTimes[i];
                    activeInjectRemoveTimes[i] = default(System.DateTime);
                }
            }
        }
    }

    /// <summary>
    /// Move To Position
    ///     Iterates throguh each card and moves it to a position corresponding to its index in the array.
    /// </summary>
    public void MoveToPos()
    {
        for(int i = 0; i < injectCards.Length; i++)
        {
            if(injectCards[i] != null)
            {
                Vector3 targetPos = new Vector3(canvas.transform.position.x - 750, 800 - (i * 120), 0);

                injectCards[i].transform.position = targetPos;
            }
        }
    }

    /// <summary>
    /// Remove Deactive Inject Cards
    ///     Checks how long each inject has been active and removes any that have expired.
    /// </summary>
    public void RemoveDeactiveCards()
    {
        for(int i = 0; i < injectCards.Length; i++)
        {
            if(injectCards[i] != null)
            {
                if (System.DateTime.Now.ToShortTimeString() == activeInjectRemoveTimes[i].ToShortTimeString())
                {
                    numActiveInjects--;

                    Destroy(injectCards[i]);

                    activeInjectRemoveTimes[i] = default(System.DateTime);
                }
            }

            ShiftCardsUp();
        }
    }
}