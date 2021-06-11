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
    /// <summary>
    /// List of all Networks within the Simulation
    /// </summary>
    [Header("JSON Data Fields")]
    [SerializeField]
    private List<NetworkData> networks;
    /// <summary>
    /// List of all Nodes within the Simulation
    /// </summary>
    [SerializeField]
    private List<NodeData> allNodes;
    /// <summary>
    /// List of all Teams within the Simulation
    /// </summary>
    [SerializeField]
    private List<TeamData> teams;

    /// <summary>
    /// List of all Node Game Objects in the Unity
    /// </summary>
    [Header("GameObject References")]
    [SerializeField]
    private List<GameObject> allNodeObjects;
    /// <summary>
    /// List of all Network GameObjects in the Unity
    /// </summary>
    [SerializeField]
    private List<GameObject> networkObjects;
    /// <summary>
    /// List of all drawn connections between Node<-->Node, Node<-->Network, and Network<-->Network
    /// </summary>
    [SerializeField]
    private List<Vector2> connectionsById;

    private List<Color> availableColors;
    private List<String> availableNames;

    // Legacy Fields
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
    /// Gets a list of All teams operating within the simulation.
    /// </summary>
    public List<TeamData> Teams
    {
        get
        {
            return this.teams;
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

    /// <summary>
    /// Gets a list of vectors representing the connections between nodes
    /// </summary>
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
        
        availableColors = new List<Color>();
        availableColors.Add(new Color(135,15,133));
        availableColors.Add(new Color(43,173,115));
        availableColors.Add(new Color(99,18,153));
        availableColors.Add(new Color(48,23,163));
        availableColors.Add(new Color(31,92,158));
        availableColors.Add(new Color(140,26,26));
        availableColors.Add(new Color(74,168,33));
        availableColors.Add(new Color(194,128,46));
        availableColors.Add(new Color(115,13,66));
        availableColors.Add(new Color(194,184,54));

        availableNames = new List<String>();
        availableNames.Add("Rattlesnakes");
        availableNames.Add("Coyotes");
        availableNames.Add("Deer");
        availableNames.Add("Jackals");
        availableNames.Add("Anaconda");
        availableNames.Add("Pumas");
        availableNames.Add("Dragonflies");
        availableNames.Add("Capybaras");
        availableNames.Add("Racoons");
        availableNames.Add("Lizards");

        foreach (TeamData t in teams)
        {
            t.TeamName = GetRandomName();
            t.TeamColor = GetRandomColor();
        }

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

    private String GetRandomName()
    {
        if(availableNames.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0,availableNames.Count);
            String teamName = availableNames[randomIndex];
            availableNames.RemoveAt(randomIndex);
            return teamName;
        }
        else
        {
            return null;
        }
    }

    private Color GetRandomColor()
    {
        if(availableColors.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0,availableNames.Count);
            Color teamColor = availableColors[randomIndex];
            availableColors.RemoveAt(randomIndex);
            return teamColor;
        }
        else
        {
            return new Color(0,0,0);
        }
    }


    // These mmethods are used to search through the Infrastructure's lists using the ID of a target object
    #region SearchMethods
    /// <summary>
    /// Search the infrastructure's allNodeObjects list and return the node object with the correct ID
    /// </summary>
    /// <param name="_searchID"></param>
    /// <returns></returns>
    public GameObject FindNodeObjectByID(int _searchID)
    {
        foreach(GameObject obj in this.allNodeObjects)
        {
            if(obj.GetComponent<NodeData>().Id == _searchID)
            {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// Search the infrastructures's allNodes list for the node with the passed ID and return it
    /// </summary>
    /// <param name="_searchID">ID number of the desired node</param>
    /// <returns></returns>
    public NodeData FindNodeDataByID(int _searchID)
    {
        foreach (NodeData data in this.allNodes)
        {
            if (data.Id == _searchID)
            {
                return data;
            }
        }
        return null;
    }

    /// <summary>
    /// Search the Infrastructure's team list for the team with the passed ID number and return it.
    /// </summary>
    /// <param name="_searchID">ID number of the desired team</param>
    /// <returns></returns>
    public TeamData FindTeamByID(int _searchID)
    {
        foreach(TeamData team in this.teams)
        {
            if(team.TeamId == _searchID)
            {
                return team;
            }
        }
        return null;
    }
    #endregion SearchMethods

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
                    GL.Vertex(FindNodeObjectByID(node.Id).transform.position);
                    GL.Vertex(FindNodeObjectByID(id).transform.position);
                }
            }
        }
        GL.End();
        Debug.Log("End of drawing");
    }

    /// <summary>
    /// Set all variables within the network with data passed from the server
    /// </summary>
    /// <param name="_networks">Networks of nodes within the server</param>
    /// <param name="_nodes">all nodes within the server</param>
    /// <param name="_teams">all of the teams competing in the competition</param>
    public void SetData(List<NetworkData> _networks, List<NodeData> _nodes, List<TeamData> _teams)
    {
        this.networks = _networks;
        this.allNodes = _nodes;
        this.teams = _teams;
    }

    /// <summary>
    /// Instantiate child network and node objects, and set data lists to reference the scripts attetched to the objects
    /// </summary>
    public void InstanceChildren()
    {
        int netCount = 0;
        int nodeCount = 0;
        // create gameObjects for all the networks
        foreach (NetworkData net in this.networks)
        {
            // Handle GameObject References
            // Create new Network Object and add it to the Infrastructure's list of Network Objects
            networkObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));
            // set the new object to be a child of the Infrastructure Object's
            networkObjects[netCount].transform.parent = this.transform;

            // Handle Data References
            // transfer the data from the networkData loaded from the JSON FIle to the nodeData component of the gameObject
            networkObjects[netCount].GetComponent<NetworkData>().SetData(net.Id, net.NodeIDs, net.Connections);

            // instantiate the nodes within this network 
            foreach (int nodeId in networkObjects[netCount].GetComponent<NetworkData>().NodeIDs)
            {
                // Handle GameObject References
                // Instantiate using the InfrastructureData's tranform as a base. 
                allNodeObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));
                // set the new node to be a child of the correct network
                allNodeObjects[nodeCount].transform.parent = networkObjects[netCount].transform;

                // Handle Data References
                // grab a reference to the nodeData of the object
                NodeData nodeData = allNodeObjects[nodeCount].GetComponent<NodeData>();
                // set the new game object's NodeData component's variables to the values from the data passed by the JSON file
                allNodeObjects[nodeCount].GetComponent<NodeData>().SetData(nodeData.Id, nodeData.Ip, nodeData.IsHidden, nodeData.Type,
                    nodeData.State, nodeData.Connections, nodeData.TeamIDs);
                // add the new node gameObject to the network's list of its node objects
                networkObjects[netCount].GetComponent<NetworkData>().AddNodeObject(allNodeObjects[nodeCount]);

                // set network references to the node scripts connected to the game Objects
                allNodes[nodeCount] = allNodeObjects[nodeCount].GetComponent<NodeData>();
                nodeCount++;
            }
            nodeCount = 0;

            // set networkData's references to the network scripts connected to the game Objects
            networks[netCount] = networkObjects[netCount].GetComponent<NetworkData>();

            netCount++;
        }

        // create method of determining locations for networks and nodes based on givern parameters
        //either based on concetration of nodes, setting the networks in a pre-determined order, or some other method.

    }
    
    /* Phased out because we are using in-between classes to move data from the FileReader to the JSON files now. - BW
     * /// <summary>
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
    }*/
}
