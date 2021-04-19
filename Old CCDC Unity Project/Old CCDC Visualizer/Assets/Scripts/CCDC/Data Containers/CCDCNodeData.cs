using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDCNodeData: NodeData
{
    #region Fields

    private UptimeChartData uptimeChart;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets this node's uptime chart.
    /// </summary>
    public UptimeChartData UptimeChart
    {
        get
        {
            return uptimeChart;
        }
        set
        {
            uptimeChart = value;
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
