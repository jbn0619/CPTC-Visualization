using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: unknown
///     Ben Wetzel - Summer 2021
/// Purpose: Track infiltration information for the system infrastructure of the network for penetration testing 
/// </summary>
[Serializable]
public class InfrastructureData: MonoBehaviour
{
    #region Fields
    [Header("Loaded from JSON")]
    [SerializeField]
    private List<NetworkData> networks;
    [SerializeField]
    private List<NodeData> allNodes;
    [SerializeField]
    private bool live; // placeholder to deterimine if using instantiated data from JSON or handbuild-infrastructure in Inspector

    [Header("GameObject References")]
    [SerializeField]
    private List<GameObject> allNodeObjects;
    [SerializeField]
    private List<GameObject> networkObjects;
    private List<int> shutDownNodes;
    [SerializeField]
    private List<Vector2> connectionsById;

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
        connectionsById = new List<Vector2>();
        
        // draw initial raycasts between network and node connections. 
        DrawAllConnections();
    }

    // Update is called once per frame
    void Update()
    {
        // to get updated states and teams that have accessed the nodes.
        // if(live){ GameManager.Instance.FileManager.UpdateNodes();}
        // Do we want to draw the raycasts every tick? would we be changing the positions of the nodes?
    }

    /// <summary>
    /// Search the infrastructure's allNodeObjects list and return the node object with the correct ID
    /// </summary>
    /// <param name="searchID"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Retrieve the data from the lists of referenced scripts. JSONUtility only returns the instance references natively.
    /// </summary>
    /// <returns>String of JSON formatted text</returns>
    public string ConvertToJSON()
    {
        string dataString = "{\"networks\": [";
        for(int i = 0; i< this.networks.Count;i++)
        {
            dataString += $"{this.networks[i].ConvertToJSON()}";
            if(i < this.networks.Count - 1) { dataString+=",";}
        }
        dataString += "], \"allNodes\":[";
        for (int i = 0; i < this.allNodes.Count; i++)
        {
            dataString += $"{JsonUtility.ToJson(this.allNodes[i])}";
            if (i < this.allNodes.Count - 1) { dataString += ","; }
        }
        dataString += "]}";
        Debug.Log(dataString);
        return dataString;
    }
    
    /// <summary>
    /// Draw connections between all nodes and networks
    /// </summary>
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

    /// <summary>
    /// Set this infrastructure's data to the new values and instantiates the requisite objects within it.
    /// </summary>
    /// <param name="_nodes">This is a list of all the nodes contained within the infrastructure</param>
    /// <param name="_networks">This is a list of all the networks contained within the infrastructure</param>
    public void SetData(List<NodeData> _nodes, List<NetworkData> _networks)
    {
        this.allNodes = _nodes;
        this.networks = _networks;

        // create gameObjects for all the networks
        foreach (NetworkData n in this.networks)
        {
            networkObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));
            networkObjects[networkObjects.Count - 1].transform.parent = this.transform;
            networkObjects[networkObjects.Count - 1].GetComponent<NetworkData>().SetData(n.Id, n.Nodes, n.Connections);
            // instantiate the nodes within this network 
            List<NodeData> networkNodes = networkObjects[networkObjects.Count - 1].GetComponent<NetworkData>().Nodes;
            foreach (NodeData o in networkNodes)
            {
                // Instantiate using the InfrastructureData's tranform as a base. 
                allNodeObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));
                networkObjects[networkObjects.Count - 1].GetComponent<NetworkData>().AddNodeObject(allNodeObjects[allNodeObjects.Count - 1]);
                allNodeObjects[allNodeObjects.Count - 1].transform.parent = networkObjects[networkObjects.Count - 1].transform;
                // set the new game object's NodeData variables to the new set of variables
                allNodeObjects[allNodeObjects.Count - 1].GetComponent<NodeData>().SetData(o.Id, o.Ip, o.IsHidden, o.Type,
                    o.State, o.Connections);
            }
        }

        // set network references to the node scripts connected to the game Objects
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i] = allNodeObjects[i].GetComponent<NodeData>();
        }
        // set network references to the network scripts connected to the game Objects
        for (int i = 0; i < networks.Count; i++)
        {
            networks[i] = networkObjects[i].GetComponent<NetworkData>();
        }
    }
}
