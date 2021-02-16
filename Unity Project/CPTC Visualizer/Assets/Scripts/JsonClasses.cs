﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// An alert's data, namely what type of alert they are.
    /// </summary>
    [Serializable]
    public class Alert
    {
        public string type;

        public List<int> affectedNodes;
        public int priority;
        public int timestamp;

        /// <summary>
        /// Constructor for the Alert class.
        /// </summary>
        /// <param name="t">This alert's type.</param>
        public Alert(CPTCEvents t, List<int> a, int _priority, int _time)
        {
            type = t.ToString();
            affectedNodes = a;
            priority = _priority;
            timestamp = _time;
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
        public List<int> discoveredNodeIds;

        /// <summary>
        /// Constructor for the Team class.
        /// </summary>
        /// <param name="i">This team's id.</param>
        /// <param name="a">The alerts generated by this team.</param>
        /// <param name="n">The IDs of nodes that this team has found.</param>
        public Team(int i, List<Alert> a, List<int> n)
        {
            teamId = i;
            alerts = a;
            discoveredNodeIds = n;
        }
    }

    /// <summary>
    /// A collection of a node's data, including node id, node type and what nodes this one is connected to. 
    /// </summary>
    [Serializable]
    public class Node
    {
        public int id;
        public string type;
        public string state;
        public List<int> connections;
        public bool isHidden;

        /// <summary>
        /// Constructor for the Node class.
        /// </summary>
        /// <param name="i">This node's id.</param>
        /// <param name="t">This node's type.</param>
        /// <param name="c">What nodes this one is connected to.</param>
        public Node(int i, NodeTypes t, NodeState s, List<int> c, bool h = false)
        {
            id = i;
            type = t.ToString();
            state = s.ToString();
            connections = c;
            isHidden = h;
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

        /// <summary>
        /// Constructor for the Network class.
        /// </summary>
        /// <param name="i">This network's id.</param>
        /// <param name="n">The nodes contained in this network.</param>
        /// <param name="nC">The other networks this one can connect to.</param>
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

        /// <summary>
        /// Constructor for the Infrastructure class.
        /// </summary>
        /// <param name="n">All the networks within this infrastructure.</param>
        public Infrastructure (List<Network> n)
        {
            networks = n;
        }
    }

    /// <summary>
    /// All of the data necessary to mimick a CPTC competition. Inlcudes a network-infrastructure, teams and events.
    /// </summary>
    [Serializable]
    public class CPTCData
    {
        public Infrastructure infrastructure;
        public List<Team> teams;

        /// <summary>
        /// Constructor for the CPTCData class.
        /// </summary>
        /// <param name="i">This data set's infrastructure. Contains networks, nodes and all their relevant connections.</param>
        /// <param name="t">This data set's teams. Contains team IDs, all their alerts and what nodes they've discovered.</param>
        public CPTCData(Infrastructure i, List<Team> t)
        {
            infrastructure = i;
            teams = t;
        }

        /// <summary>
        /// Converts this CPTCData object and all data within it into a json-formatted string.
        /// </summary>
        /// <returns>A large string in a JSON format.</returns>
        public string ConvertToJSON()
        {
            string cptcInfo = "";
            cptcInfo = JsonUtility.ToJson(this);
            return cptcInfo;
        }
    }
}
