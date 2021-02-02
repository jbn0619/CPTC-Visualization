using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertNotif: MonoBehaviour
{
    #region Fields
    private Vector3 targetPos;
    private float step;

    [SerializeField]
    private Text text;

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

    public Text Text
    {
        get
        {
            return text;
        }
    }

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        step = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
    }
}
