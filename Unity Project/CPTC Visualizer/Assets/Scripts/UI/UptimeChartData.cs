using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UptimeChartData: MonoBehaviour
{
    #region Fields

    private int upTicks;
    private int downTicks;

    [SerializeField]
    private int nodeID;
    [SerializeField]
    private int teamID;

    [SerializeField]
    private Slider blueSlider;
    [SerializeField]
    private Slider redSlider;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets or sets this chart's nodeID.
    /// </summary>
    public int NodeID
    {
        get
        {
            return nodeID;
        }
        set
        {
            if (value >= 0) nodeID = value;
        }
    }

    /// <summary>
    /// Gets or sets this chart's teamID.
    /// </summary>
    public int TeamID
    {
        get
        {
            return teamID;
        }
        set
        {
            if (value >= -1) teamID = value;
        }
    }

    /// <summary>
    /// Gets or sets how-many ticks this chart's node has been on for.
    /// </summary>
    public int UpTicks
    {
        get
        {
            return upTicks;
        }
        set
        {
            if (value > upTicks) upTicks = value;
        }
    }

    /// <summary>
    /// Gets or sets how-many ticks this chart's node has been off for.
    /// </summary>
    public int DownTicks
    {
        get
        {
            return downTicks;
        }
        set
        {
            if (value > downTicks) downTicks = value;
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
        upTicks = 0;
        downTicks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateData(bool isUp)
    {
        // First, update our tick counts
        if (isUp) upTicks++;
        else downTicks++;

        // Next, recalculate ratios and change the slider values.
        int totalTicks = upTicks + downTicks;

        float upRatio = upTicks / totalTicks;
        float downRatio = downTicks / totalTicks;

        blueSlider.value = upRatio;
        redSlider.value = downRatio;
    }
}
