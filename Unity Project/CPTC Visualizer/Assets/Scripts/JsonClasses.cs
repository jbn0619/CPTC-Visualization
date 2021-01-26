using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class JsonClasses
    {
    }

    [System.Serializable]
    public class NetworkNode
    {
        public int id;

        public NodeTypes type;

        public List<int> connections;

        public static NetworkNode CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<NetworkNode>(jsonString);
        }
    }

    [System.Serializable]
    public class Alert
    {
        public CPTCEvents type;

        public static Alert CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Alert>(jsonString);
        }
    }

    [System.Serializable]
    public class Team
    {
        public List<Alert> alerts;

        public static List<NetworkNode> discoveredNodes;

        public Team CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Team>(jsonString);
        }
    }
}
