using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A collection of a node's data, including node id, node type and what nodes this one is connected to. 
    /// </summary>
    [Serializable]
    public class Node
    {
        public int id;

        public string type;

        public List<int> connections;

        public Node(int i, NodeTypes t, List<int> c)
        {
            id = i;
            type = t.ToString();
            connections = c;
        }
    }

    /// <summary>
    /// An alert's data, namely what type of alert they are.
    /// </summary>
    [Serializable]
    public class Alert
    {
        public string type;

        public Alert(CPTCEvents t)
        {
            type = t.ToString();
        }
    }

    /// <summary>
    /// A collection of a team's data, inlcuding ID, alerts they've generated and nodes they've discovered.
    /// </summary>
    [Serializable]
    public class Team
    {
        public int teamId;

        public List<Alert> alerts;

        public List<Node> discoveredNodes;

        public Team(int i, List<Alert> a, List<Node> n)
        {
            teamId = i;
            alerts = a;
            discoveredNodes = n;
        }

        /// <summary>
        /// Converts this team object and all data within it into a json-formatted string.
        /// </summary>
        /// <returns>A large string in a JSON format.</returns>
        public string ConvertToJSON()
        {
            
            string teamInfo = "";

            teamInfo = JsonUtility.ToJson(this);

            return teamInfo;
        }
    }

    /// <summary>
    /// A collection of a network's data, including network id and nodes within it.
    /// </summary>
    [Serializable]
    public class Network
    {
        public int networkId;

        public List<Node> nodes;

        public List<int> networkConnections;

        public Network(int i, List<Node> n, List<int> nC)
        {
            networkId = i;
            nodes = n;
            networkConnections = nC;
        }
    }

    /// <summary>
    /// A collection of all network and node data compiled into one data structure.
    /// </summary>
    [Serializable]
    public class Infrastructure
    {
        public List<Network> networks;

        public Infrastructure (List<Network> n)
        {
            networks = n;
        }

        /// <summary>
        /// Converts this infrastructure object and all data within it into a json-formatted string.
        /// </summary>
        /// <returns>A large string in a JSON format.</returns>
        public string ConvertToJSON()
        {
            string infrastructureInfo = "";

            infrastructureInfo = JsonUtility.ToJson(this);

            return infrastructureInfo;
        }
    }

}
