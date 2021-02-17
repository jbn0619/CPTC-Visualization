using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UptimeChartData: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;

    [SerializeField]
    private Slider blueSlider;
    [SerializeField]
    private Slider redSlider;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets this chart's id, which will correspond to a node's id.
    /// </summary>
    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            if (value >= 0) id = value;
        }
    }

    /// <summary>
    /// Gets the blue slider for this chart.
    /// </summary>
    public Slider BlueSlider
    {
        get
        {
            return blueSlider;
        }
    }

    /// <summary>
    /// Gets the red slider for this chart.
    /// </summary>
    public Slider RedSlider
    {
        get
        {
            return redSlider;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
