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
    int bottomLinePos;
    [SerializeField]
    RectTransform contentTransform;

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
            Print("Here's some test data. This one is particularly long and will test the string's ability to wrap.");
        }
    }

    public void Print(string _input)
    {
        string nextLine = "> [" + System.DateTime.Now + "] " + _input + "\n";

        textLog.text += nextLine;
        //contentTransform.position += new Vector3(0, bottomLinePos, 0);
    }
}
