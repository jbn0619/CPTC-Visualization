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
    private List<GameObject> allNodeObjects;
    [SerializeField]
    private List<GameObject> networkObjects;

    [SerializeField]
    private List<int> shutDownNodes;

    [Header("JSON Access Path")]
    [SerializeField]
    private string infraFilename;
    [SerializeField]
    private string nodesFilePathExtension;
    private List<Vector2> connectionsById;
    private string infraFilePathExtension;

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
    /// Gets a list of all network objects within this infrastructure.
    /// </summary>
    public List<GameObject> NetworkObjects
    {
        get
        {
            return networkObjects;
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
    /// Gets a list of all the instanced Node Objects in the scene
    /// </summary>
    public List<GameObject> AllNodeObjects
    {
        get
        {
            return allNodeObjects;
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

    public List<Vector2> ConnectionsById
    {
        get
        {
            return connectionsById;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        // set up the filepath for the access to node data
        infraFilename = "infraDraft.JSON";
        infraFilePathExtension = "\\Infrastructure\\Database\\";

        // set up the infrastructure's data values from the JSON
        this.allNodes = GameManager.Instance.FileManager.CreateInfraFromJSON(infraFilename, infraFilePathExtension).AllNodes;
        this.networks = GameManager.Instance.FileManager.CreateInfraFromJSON(infraFilename, infraFilePathExtension).Networks;

        // create gameObjects for all the nodes
        foreach (NodeData n in this.allNodes)
        {
            // TODO: Determine initial position of networks.

            // Instantiate using the InfrastructureData's tranform as a base. 
            allNodeObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));

            // set the new game object's NodeData variables to the new set of variables
            allNodeObjects[allNodeObjects.Count - 1].GetComponent<NodeData>().SetData(n.Id, n.Ip, n.IsHidden, n.Type, 
        n.State, n.Connections, n.ConnectionGOS);
        }

        // create gameObjects for all the networks
        foreach (NetworkData n in this.networks)
        {
            // TODO: Determine initial position of nodes.

            networkObjects.Add(Instantiate(GameManager.Instance.NodePrefab, this.transform.position, this.transform.rotation));

            networkObjects[networkObjects.Count - 1].GetComponent<NetworkData>().SetData(n.Id, n.Nodes, n.Connections, n.ConnectionGOS);
        }
        // draw initial raycasts between network and node connections. 
        // Will need to loop in such a way as to avoid duplicating raycasts of the same conecitons

        // set the allNodes indecies to reference the node components of the GameObjects, rather than the original list
        // this may be redundant, I'm not sure. - Ben
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i] = allNodeObjects[i].GetComponent<NodeData>();
        }
        
        connectionsById = new List<Vector2>();
        DrawAllConnections();
        // set network references to the network scripts connected to the game Objects
        for (int i = 0; i < networks.Count; i++)
        {
            networks[i] = networkObjects[i].GetComponent<NetworkData>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // to get updated states and teams that have accessed the nodes.
        GameManager.Instance.FileManager.UpdateNodes(infraFilename, infraFilePathExtension);
        
        //update node graphics
        foreach(GameObject n in this.allNodeObjects)
        {
            n.GetComponent<NodeData>().SplitSprite();
        }
        // Do we want to draw the raycasts every tick? would we be changing the positions of the nodes?
    }

    // Helper Method: Finds the Node GameObject associated with the id
    public GameObject FindNodeByID(int searchID)
    {
        foreach(GameObject obj in this.allNodeObjects)
        {
            if(obj.GetComponent<NodeData>().Id == searchID)
            {
                return obj;
            }
        }
        return null;
    }

    public string ConvertToJSON()
    {
        string cptcInfo = "";
        cptcInfo = JsonUtility.ToJson(this);
        Debug.Log(cptcInfo);
        return cptcInfo;
    }
    
    public void DrawAllConnections()
    {
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.white);
        foreach(NodeData node in this.allNodes)
        {
            foreach(int id in node.Connections)
            {
                if(!(connectionsById.Contains(new Vector2(node.Id, id)) || connectionsById.Contains(new Vector2(id, node.Id))))
                {
                    connectionsById.Add(new Vector2(id,node.Id));
                    GL.Vertex(FindNodeByID(node.Id).transform.position);
                    GL.Vertex(FindNodeByID(id).transform.position);
                }
            }
        }
        GL.End();
        Debug.Log("End of drawing");
    }

}
