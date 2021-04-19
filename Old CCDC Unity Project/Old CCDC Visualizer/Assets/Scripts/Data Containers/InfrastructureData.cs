using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureData: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private List<NetworkData> networks;

    [SerializeField]
    private List<NodeData> allNodes;

    [SerializeField]
    private List<int> shutDownNodes;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets a list of all networks within this infrastructure.
    /// </summary>
    public List<NetworkData> Networks
    {
        get
        {
            return networks;
        }
    }

    /// <summary>
    /// Gets a list of ALL nodes in all networks in this infrastructure.
    /// </summary>
    public List<NodeData> AllNodes
    {
        get
        {
            return allNodes;
        }
    }

    /// <summary>
    /// Gets a list of IDs of shut-down nodes.
    /// </summary>
    public List<int> ShutDownNodes
    {
        get
        {
            return shutDownNodes;
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
