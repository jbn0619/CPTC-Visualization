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
    private List<NetworkData> networks;

    [SerializeField]
    private List<NodeData> allNodes;

    [SerializeField]
    private List<int> shutDownNodes;

    [Header("JSON Access Path")]
    [SerializeField]
    private string nodeFilename;
    [SerializeField]
    private string nodeFileExtension;

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
        // set up the filepath for the access to node data
        nodeFilename = "nodes.JSON";
        nodeFileExtension = "\\Infrastructure\\Database\\";
        // call the FileManager's List<NodeData>CreateNodesFromJSON(filename, filePathExtension) and return a list of all Nodes in the network
        // Determine initial positions of allNodes and set their transforms accordingly. 
        // draw raycasts between network and node connections
    }

    // Update is called once per frame
    void Update()
    {
        // call FileManager's List<int> UpdatedNodes(filename, filePathExtension) 
            // to get updated states and teams that have accessed the nodes.
            // This method would access this object's AllNodes and edit it with the incoming data. 
            // That way we don't need to create new nodes every update
            // It will return a list of the nodes which need to be updates

        // List<int> updatedNodes = GameManager.Instance.FileManager.UpdateNodes(nodeFilename, nodeFileExtension)

        // foreach Node in allNodes, update the sprite to represent the teams which have accessed them
    }

    /// <summary>
    /// This updates the node at the given index within the allnodes list. 
    /// </summary>
    /// <param name="_index">Index of the node within the InfrastructureData's allNodes list</param>
    /// <param name="_teamIDs">List of IDs of the teams which are currently accessing the node</param>
    /// <param name="_state">The current state of the node</param>
    // This implemented here rather than in NodeData.cs because this is where the list of all Nodes is kept, and where the index will be usefu
    // This is necessary to maintain the integrity of the allNodes list as a private variable.
    public void UpdateNodeData(int _index, List<int> _teamIDs, NodeState _state)
    {
        this.allNodes[_index].State = _state;
        this.allNodes[_index].TeamIDs = _teamIDs;
    }
}
