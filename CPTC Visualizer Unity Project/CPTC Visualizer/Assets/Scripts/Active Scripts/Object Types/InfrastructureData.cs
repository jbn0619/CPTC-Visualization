using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: unknown
///     Ben Wetzel - Summer 2021
/// Purpose: Track infiltration information for the system infrastructure of the network for penetration testing 
/// </summary>
public class InfrastructureData: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject prefabNode;

    [SerializeField]
    private List<NetworkData> networks;

    [SerializeField]
    private List<NodeData> allNodes;

    [SerializeField]
    private List<int> shutDownNodes;

    [Header("JSON Access Path")]
    [SerializeField]
    private string nodesFilename;
    [SerializeField]
    private string nodesFilePathExtension;

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
    /// Gets a list of IDs of inactive nodes.
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
        // set up the filepath for the access to node data
        nodesFilename = "nodes.JSON";
        nodesFilePathExtension = "\\Infrastructure\\Database\\";
        this.allNodes = GameManager.Instance.FileManager.CreateNodesFromJSON(nodesFilename, nodesFilePathExtension);
        
        // Determine initial positions of allNodes and set their transforms accordingly. 
        // draw initial raycasts between network and node connections
    }

    // Update is called once per frame
    void Update()
    {
        // to get updated states and teams that have accessed the nodes.
        GameManager.Instance.FileManager.UpdateNodes(nodesFilename, nodesFilePathExtension);
        
        //update node graphics
        foreach(NodeData n in this.allNodes)
        {
            n.SplitSprite();
        }
        // Do we want to draw the raycasts every tick? would we be changing the positions of the nodes?
    }

    /// <summary>
    /// This updates the node at the given index within the allnodes list. 
    /// </summary>
    /// <param name="_index">Index of the node within the InfrastructureData's allNodes list</param>
    /// <param name="_teams">List of the teams which are currently accessing the node</param>
    /// <param name="_state">The current state of the node</param>
    // This implemented here rather than in NodeData.cs because this is where the list of all Nodes is kept, and where the index will be usefu
    // This is necessary to maintain the integrity of the allNodes list as a private variable.
    public void UpdateNodeData(int _index, List<TeamData> _teams, NodeState _state)
    {
        this.allNodes[_index].State = _state;
        this.allNodes[_index].Teams = _teams;
    }

    
}
