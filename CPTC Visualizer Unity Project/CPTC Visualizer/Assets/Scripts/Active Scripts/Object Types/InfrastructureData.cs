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
    private List<NodeData> nodes;
    /// <summary>
    /// List of all Teams operating in the architecture
    /// </summary>
    [SerializeField]
    private List<TeamData> teams;

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
    /// Gets a list of ALL nodes in all networks in this infrastructure.
    /// </summary>
    public List<NodeData> Nodes
    {
        get
        {
            return nodes;
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
    /// Search the Infrastructure's nodes list and return a reference to the node obejct with the provided IP address
    /// </summary>
    /// <param name="_searchIP"></param>
    /// <returns></returns>
    public NodeData FindNodeByIP(string _searchIP)
    {
        foreach(NodeData node in nodes)
        {
            if(node.Ip == _searchIP)
            {
                return node;
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
        foreach(NodeData node in this.nodes)
        {
            foreach(int id in node.Connections)
            {
                if(!(connectionsById.Contains(new Vector2(node.Index, id)) || connectionsById.Contains(new Vector2(id, node.Index))))
                {
                    connectionsById.Add(new Vector2(id,node.Index));
                    GL.Vertex(nodes[node.Index].transform.position);
                    GL.Vertex(nodes[id].transform.position);
                }
            }
        }
        GL.End();
        // Debug.Log($"{name} : End of drawing");
    }

    /// <summary>
    /// Set all JSON variables within the network with data passed from the server
    /// </summary>
    /// <param name="_networks">Networks of nodes within the server</param>
    /// <param name="_nodes">all nodes within the server</param>
    public void SetData(List<NetworkData> _networks, List<NodeData> _nodes)
    {
        this.networks = _networks;
        this.nodes = _nodes;
    }
    public InfrastructureData DeepCopy()
    {
        InfrastructureData infraClone = new InfrastructureData();
        List<NetworkData> networkClones = new List<NetworkData>();
        foreach(NetworkData net in networks)
        {
            networkClones.Add(net.DeepCopy());
        }
        List<NodeData> nodeClones = new List<NodeData>();
        foreach(NodeData node in Nodes)
        {
            nodeClones.Add(node.DeepCopy());
        }
        infraClone.SetData(networkClones, nodeClones);
        return infraClone;
    }

    /// <summary>
    /// Instantiate child network and node objects, and set data lists to reference the scripts attetched to the objects
    /// </summary>
    public void InstanceChildren()
    {
        int netCount = 0;
        int nodeCount = 0;
        string debug = $"{name}.InstanceChildren\n";
        // move old information to a temp holder 
        List<NetworkData> tempNets = new List<NetworkData>();
        for(int i = 0; i < networks.Count; i++)
        {
            tempNets.Add(networks[i].DeepCopy());
        }
        // refresh internal lists of nodes and networks
        networks.Clear();
        nodes.Clear();

        // create gameObjects for all the networks
        foreach (NetworkData net in tempNets)
        {
            // Handle GameObject References
            // Create new Network Object and add it to the Infrastructure's list of Network Objects
            networks.Add(Instantiate(GameManager.Instance.NetworkPrefab, this.transform.position, this.transform.rotation).GetComponent<NetworkData>());
            // set the new object to be a child of the Infrastructure Object's
            networks[netCount].gameObject.transform.parent = transform;

            // Handle Data References
            // transfer the data from the networkData loaded from the JSON FIle to the nodeData component of the gameObject
            networks[netCount].SetData(net.NetworkName, net.Ip, net.Nodes, net.VDI);
            // set name of network object
            networks[netCount].name = net.NetworkName;

            // Handle Scaling Object Positions
            int scalar = (networks[netCount].Nodes.Count / 6);

            // Account for the networks that will have nodes placed into the center of them and shrink them a bit from the main scalar
            if(networks[netCount].Nodes.Count % 6 == 1)
            {
                networks[netCount].gameObject.transform.localScale = new Vector3(1 + (scalar - 0.4f) / 1.75f, 1 + (scalar - 0.4f) / 1.75f, 1f);
            }
            // Otherwise, treat the networks scaling like normal
            else
            {
                networks[netCount].gameObject.transform.localScale = new Vector3(1 + scalar / 1.75f, 1 + scalar / 1.75f, 1f);
            }
            
            debug += $" - Network: {networks[netCount].name} | Nodes Count: {networks[netCount].Nodes.Count}\n";

            List<NodeData> tempNodes = new List<NodeData>(networks[netCount].Nodes);
            networks[netCount].Nodes.Clear();
            // Create Instanced versions of the uninstanced NodeDatas from the network 
            foreach (NodeData node in tempNodes)
            {
                // Handle GameObject References
                // Create Base instance of the Node Object and track it using its NodeData component 
                nodes.Add(Instantiate(GameManager.Instance.NodePrefab, transform.position, transform.rotation).GetComponent<NodeData>());
                // set the new node to be a child of the correct network
                nodes[nodeCount].gameObject.transform.parent = networks[netCount].transform;
                
                // Create nodes to be in the background of the nodes, for the sake of visibility with the background
                /* Deprecated, but don't want to remove in case it is needed again.
                GameObject background = Instantiate(GameManager.Instance.NodePrefab, transform.position, transform.rotation);
                background.transform.parent = allNodeObjects[nodeCount].transform;
                background.transform.localPosition = new Vector3(0, 0, 0.01f);
                background.transform.localScale = new Vector3(7.5f,7.5f,1);
                */

                // Handle Data References
                // set the blank instanced node's variables to the values from the data passed by the JSON file
                nodes[nodeCount].SetInstanceToData(node.Ip, node.HostName, node.HostDescription, node.OS,nodeCount);
                // add the new node gameObject to the network's list of its nodes
                networks[netCount].Nodes.Add(nodes[nodeCount]);

                debug += $" -- {nodes[nodeCount].name}\n";
                nodeCount++;
            }
            
            netCount++;
        }


        int networkedNodes = 0;
        // loop through all instanced networks
        for(int i = 0; i < networks.Count; i++)
        {
            networks[i].Index = i;

            // set all nodes in this network to reference the instances nodes in the infrastructure
            for(int j = 0; j < networks[i].Nodes.Count;j++)
            {
                networks[i].AddNodeData(nodes[networkedNodes], j);
                
                networkedNodes++;
            }

            // establish connections between networks
            for(int t = 0; t < networks.Count; t++)
            {
                if (t != i && networks[i].IsAdjacentTo(networks[t]))
                {
                    networks[i].Connections.Add(t);
                }
            }
            // establish connections between nodes within networks
            // for every node in the network ...
            for (int j = 0; j < networks[i].Nodes.Count; j++)
            {
                // loop through all other nodes and add them to this node's connections list.
                for (int k = 0; k < networks[i].Nodes.Count; k++)
                {
                    // if the node isn't attempting to reference itself
                    if (networks[i].Nodes[j].Index != networks[i].Nodes[k].Index)
                    {
                        // Add the Infra's list Index of the other node to this node's connections list 
                        networks[i].Nodes[j].Connections.Add(networks[i].Nodes[k].Index);
                    }
                }
            }
        }
        // Debug.Log(debug);
    }
    
    public void PositionNetworks()
    {
        // Position each network in a circular manner based off of a center point 0,0,0
        Vector3 center = new Vector3(0,0,0);
        float degreeOffset = 2 * Mathf.PI / networks.Count;
        float currentAngle = Mathf.PI / 4;
        positioningRadius = (networks.Count - 1.5f);
        foreach(NetworkData network in networks)
        {
            network.transform.position = new Vector3(Mathf.Cos(currentAngle) * positioningRadius * network.transform.localScale.x, Mathf.Sin(currentAngle) * positioningRadius * network.transform.localScale.x, 0);
            currentAngle += degreeOffset;
        }
    }

    public void PositionNodes()
    {
        foreach(NetworkData network in networks)
        {
            List<NodeData> nodes = network.Nodes;

            float angleOffset;

            if(nodes.Count <= 6)
            {
                angleOffset = Mathf.PI * 2f / nodes.Count;
            }
            else
            {
                angleOffset = Mathf.PI * 2f / 6f;
            }

            
            // If there is the capability to have a central node, place it there first and continue with the placement
            if(nodes.Count % 6 == 1)
            {
                // Set the first node to be the center of the network
                nodes[0].transform.localPosition = new Vector3(0,0,0);

                // Set basic positions of the nodes within the network
                // Each ring is made up of up to 6 nodes, and makes a new ring when it exceeds that amount
                for(int i = 0; i < nodes.Count - 1; i++)
                {
                    // Change the radius offset based off of how many rings there are in the network
                    // Smaller networks need to take up more of the network hence higher offset
                    float radiusOffset = 1f;
                    radiusOffset -= 0.35f * (nodes.Count / 6 - 1);

                    if ((i / 6) % 2 == 1)
                    {
                        // If the node is on an even ring of the network, offset it's rotational position by 1/2 radians
                        nodes[i+1].transform.localPosition = new Vector3(Mathf.Cos(angleOffset * ((i % 6) + 0.5f)) * (i / 6 + 1) * radiusOffset, Mathf.Sin(angleOffset * ((i % 6) + 0.5f)) * (i / 6 + 1) * radiusOffset, 0);
                    }
                    else
                    {
                        nodes[i+1].transform.localPosition = new Vector3(Mathf.Cos(angleOffset * (i % 6)) * (i / 6 + 1) * radiusOffset, Mathf.Sin(angleOffset * (i % 6)) * (i / 6 + 1) * radiusOffset, 0);
                    }
                }
            }
            // If there isn't a node to place in the center first, continue with the ring placement
            else
            {
                // Set basic positions of the nodes within the network
                // Each ring is made up of up to 6 nodes, and makes a new ring when it exceeds that amount
                for(int i = 0; i < nodes.Count; i++)
                {
                    // Change the radius offset based off of how many rings there are in the network
                    // Smaller networks need to take up more of the network hence higher offset
                    float radiusOffset = 1f;
                    radiusOffset -= 0.2125f * (nodes.Count / 6);

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
