using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLog: MonoBehaviour
{
    #region Fields
    [SerializeField]
    Text textLog;
    [SerializeField]
    float bottomLinePos;
    [SerializeField]
    RectTransform contentTransform;
    Vector3 logPos;

    [SerializeField]
    bool autoScroll;

    #endregion Fields
    
    #region Properties
    
    public bool AutoScroll
    {
        get { return autoScroll; }
        set { autoScroll = !autoScroll; }
    }

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        autoScroll = true;
        contentTransform.position += new Vector3(0, bottomLinePos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Print("Here's some test data. This is used to text a long string. This one is also used to test a veeeery long string.");
        }
    }

    public void Print(string _input)
    {
        string nextLine = "> [" + System.DateTime.Now + "] " + _input + "\n\n";
        Debug.Log(nextLine.Length);

        float disntanceToMove = ((nextLine.Length / 55) + 2) * (textLog.fontSize + textLog.lineSpacing + 0.2f);

        textLog.text += nextLine;
        Debug.Log("Distance to move line: " + disntanceToMove);
        float xPos = contentTransform.position.x;
        float zPos = contentTransform.position.z;

        contentTransform.position += new Vector3(0, disntanceToMove, 0);
        bottomLinePos = contentTransform.position.y;
    }
}
