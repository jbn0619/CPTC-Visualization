using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: unknown
///     Ben Wetzel, David "Pat" Smith - Summer 2021
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

    [SerializeField]
    private Material mat;

    private List<Color> availableColors;
    private List<String> availableNames;
    private float positioningRadius;
    [SerializeField]
    private NodeData canvasTest;

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
        // reset list of connections made between nodes
        connectionsById = new List<Vector2>();
        
        /* Functionality moved to TeamManager
         * // Establish list of available team colors
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

        // Establish list of available team Names
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

        // set team names and colors for each team
        foreach (TeamData t in teams)
        {
            t.TeamName = GetRandomName();
            t.TeamColor = GetRandomColor();
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        // to get updated states and teams that have accessed the nodes.
        // if(live){ GameManager.Instance.FileManager.UpdateNodes();}
        // Do we want to draw the raycasts every tick? would we be changing the positions of the nodes?
    }

    


    // These mmethods are used to search through the Infrastructure's lists
    #region SearchMethods
    /// <summary>
    /// Search the Infrastructure's allNodeObjects list and return a reference to the node obejct with the provided IP address
    /// </summary>
    /// <param name="_searchIP"></param>
    /// <returns></returns>
    public GameObject FindNodeObjectByIP(string _searchIP)
    {
        foreach(GameObject obj in this.allNodeObjects)
        {
            if(obj.GetComponent<NodeData>().Ip == _searchIP)
            {
                return obj;
            }
        }
        return null;
    }
    /// <summary>
    /// Search the infrastructure's allNodeObjects list and return the node object with the correct ID
    /// </summary>
    /// <param name="_searchID">ID number of target Node</param>
    /// <returns>target Node�s Game Object</returns>
    public GameObject FindNodeObjectByID(int _searchID)
    {
        foreach(GameObject obj in this.allNodeObjects)
        {
            if(obj.GetComponent<NodeData>().Index == _searchID)
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
    /// <returns>Target NodeData from allNodes</returns>
    public NodeData FindNodeDataByID(int _searchID)
    {
        foreach (NodeData data in this.allNodes)
        {
            if (data.Index == _searchID)
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
    /// <returns>Target TeamData from Infra�s.teams</returns>
    public TeamData FindTeamByID(int _searchID)
    {
        foreach(TeamData team in this.teams)
        {
            if(team.ID == _searchID)
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
        mat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(Color.white);
        foreach(NodeData node in this.allNodes)
        {
            foreach(int id in node.Connections)
            {
                if(!(connectionsById.Contains(new Vector2(node.Index, id)) || connectionsById.Contains(new Vector2(id, node.Index))))
                {
                    connectionsById.Add(new Vector2(id,node.Index));
                    GL.Vertex(allNodeObjects[node.Index].transform.position);
                    GL.Vertex(allNodeObjects[id].transform.position);
                }
            }
        }
        GL.End();
        Debug.Log($"{name} : End of drawing");
    }

    /// <summary>
    /// Set all JSON variables within the network with data passed from the server
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
        foreach (NetworkData net in networks)
        {
            // Handle GameObject References
            // Create new Network Object and add it to the Infrastructure's list of Network Objects
            networkObjects.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation));
            // set the new object to be a child of the Infrastructure Object's
            networkObjects[netCount].transform.parent = transform;

            // Handle Data References
            // transfer the data from the networkData loaded from the JSON FIle to the nodeData component of the gameObject
            networkObjects[netCount].GetComponent<NetworkData>().SetData(net.NetworkName, net.Ip, net.Nodes, net.VDI);
            // set name of network object
            networkObjects[netCount].name = net.NetworkName;

            int scalar = (networks[netCount].Nodes.Count / 6);
            networkObjects[netCount].transform.localScale = new Vector3(1 + scalar / 9f, 1 + scalar / 9f, 1f);
            Debug.Log("Nodes Count: " + networks[netCount].Nodes.Count);

            // instantiate the nodes within this network 
            foreach (NodeData node in networks[netCount].Nodes)
            {
                // Handle GameObject References
                // Instantiate using the InfrastructureData's tranform as a base. 
                allNodeObjects.Add(Instantiate(GameManager.Instance.NodePrefab, transform.position, transform.rotation));
                // set the new node to be a child of the correct network
                allNodeObjects[nodeCount].transform.parent = networkObjects[netCount].transform;
                
                // Create nodes to be in the background of the nodes, for the sake of visibility with the background
                /* Deprecated, but don't want to remove in case it is needed again.
                GameObject background = Instantiate(GameManager.Instance.NodePrefab, transform.position, transform.rotation);
                background.transform.parent = allNodeObjects[nodeCount].transform;
                background.transform.localPosition = new Vector3(0, 0, 0.01f);
                background.transform.localScale = new Vector3(7.5f,7.5f,1);
                */

                // Handle Data References
                // set the new game object's NodeData component's variables to the values from the data passed by the JSON file
                allNodeObjects[nodeCount].GetComponent<NodeData>().SetData(node.Ip, node.HostName, node.HostDescription, node.OS);
                allNodeObjects[nodeCount].GetComponent<NodeData>().InstanceData(nodeCount);
                // add the new node gameObject to the network's list of its node objects
                networkObjects[netCount].GetComponent<NetworkData>().AddNodeObject(allNodeObjects[nodeCount]);
                
                nodeCount++;
            }
            
            netCount++;
        }


        for (int i = 0; i < allNodes.Count; i++)
        {
            // set infrastructure references to the node scripts connected to the game Objects
            allNodes[i] = allNodeObjects[i].GetComponent<NodeData>();
        }
        for(int i = 0; i < networks.Count; i++)
        {
            networkObjects[i].GetComponent<NetworkData>().Index = i;

            // set networkData's references to the network scripts connected to the game Objects
            networks[i] = networkObjects[i].GetComponent<NetworkData>();

            for(int j = 0; j < networks[i].NodeObjects.Count;j++)
            {
                // Set the internal list of NodeData for the network to be equal to the finished version of the nodeData
                networks[i].AddNodeData(networks[i].NodeObjects[j].GetComponent<NodeData>(), j);
                
                // set all nodes within this network to be adjacent to each other
                for (int k = 0; k < networks[i].Nodes.Count; k++)
                {
                    // if the node isn't attempting to reference itsels
                    if(networks[i].NodeObjects[j].GetComponent<NodeData>().Index != networks[i].NodeObjects[k].GetComponent<NodeData>().Index)
                    {
                        // Add the AllNodes Index of the other node to this node's connections list 
                        networks[i].NodeObjects[j].GetComponent<NodeData>().Connections.Add(networkObjects[i].GetComponent<NetworkData>().NodeObjects[k].GetComponent<NodeData>().Index);
                    }
                }
            }

            // establish connections between networks
            for(int t = 0; t < networks.Count; t++)
            {
                if (t != i && networks[i].IsAdjacentTo(networks[t]))
                {
                    networks[i].Connections.Add(t);
                }
            }
        }

    }
    
    public void PositionNetworks()
    {
        // Position each network in a circular manner based off of a center point 0,0,0
        Vector3 center = new Vector3(0,0,0);
        float degreeOffset = 2 * Mathf.PI / networkObjects.Count;
        float currentAngle = Mathf.PI / 4;
        positioningRadius = (networkObjects.Count + 1) * 1.25f;
        foreach(GameObject network in networkObjects)
        {
            network.transform.position = new Vector3(Mathf.Cos(currentAngle) * positioningRadius * network.transform.localScale.x, Mathf.Sin(currentAngle) * positioningRadius * network.transform.localScale.x, 0);
            currentAngle += degreeOffset;
        }
    }

    public void PositionNodes()
    {
        foreach(NetworkData network in networks)
        {
            List<GameObject> nodes = network.GetComponent<NetworkData>().NodeObjects;

            float angleOffset;

            if(nodes.Count <= 6)
            {
                angleOffset = Mathf.PI * 2f / nodes.Count;
            }
            else
            {
                angleOffset = Mathf.PI * 2f / 6f;
            }

            

            // Set basic positions of the nodes within the network
            // Each ring is made up of up to 6 nodes, and makes a new ring when it exceeds that amount
            for(int i = 0; i < nodes.Count; i++)
            {
                float radiusOffset = 0.95f;
                radiusOffset -= 0.2f * (nodes.Count / 6);
                if ((i / 6) % 2 == 1)
                {
                    // If the node is on an even ring of the network, offset it's rotational position by 1/2 radians
                    nodes[i].transform.localPosition = new Vector3(Mathf.Cos(angleOffset * ((i % 6) + 0.5f)) * (i / 6 + 1) * radiusOffset, Mathf.Sin(angleOffset * ((i % 6) + 0.5f)) * (i / 6 + 1) * radiusOffset, 0);
                }
                else
                {
                    nodes[i].transform.localPosition = new Vector3(Mathf.Cos(angleOffset * (i % 6)) * (i / 6 + 1) * radiusOffset, Mathf.Sin(angleOffset * (i % 6)) * (i / 6 + 1) * radiusOffset, 0);
                }
                
            }                     
        }
    }
    
    /*Funtionality moved to FIlemanager. Names and COlors are now being read from a file.
     * private String GetRandomName()
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
    }*/
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
