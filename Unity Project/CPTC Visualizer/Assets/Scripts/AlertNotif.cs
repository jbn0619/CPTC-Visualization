using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertNotif: MonoBehaviour
{
    #region Fields
    private Vector3 targetPos;
    private float yPos;
    private float step;

    private int teamID;
    private int priority;

    [SerializeField]
    private string notifText;

    [SerializeField]
    private Text textBox;

    #endregion Fields

    #region Properties
    /// <summary>
    /// Get or set this button's target position.
    /// </summary>
    public Vector3 TargetPos
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = transform.position + value;
        }
    }

    public string NotifText
    {
        get
        {
            return notifText;
        }
        set
        {
            notifText = value;
        }
    }

    public int TeamID
    {
        get
        {
            return teamID;
        }

        set
        {
            teamID = value;
        }
    }

    public int Priority
    {
        get
        {
            return priority;
        }
        set
        {
            priority = value;
        }
    }
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        step = 1.0f;
        yPos = targetPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
        textBox.text = notifText;
    }
}
