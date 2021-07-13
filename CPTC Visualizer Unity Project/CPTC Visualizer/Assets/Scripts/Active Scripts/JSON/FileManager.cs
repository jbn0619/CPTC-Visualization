using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.Scripts;
using System.Globalization;

public class FileManager: MonoBehaviour
{
    #region Fields
    string rootFilePath;
    //DirectoryInfo directoryInfo;
    [SerializeField]
    private KeyCode saveFileKey = KeyCode.Q;
    [SerializeField]
    private KeyCode readFileKey = KeyCode.E;
    /// <summary>
    /// Delay (in Seconds) to add to DateTime of new Alerts from the Splunk
    /// </summary>
    [SerializeField]
    private double delayInterval;
    #endregion Fields

    #region Properties

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        rootFilePath = "C:\\ProgramData\\CSEC Visualizer\\";
        //directoryInfo = new DirectoryInfo(rootFilePath);
        //Directory.CreateDirectory(rootFilePath + "\\Test Folder");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(saveFileKey))
        {
            //directoryInfo.CreateDirectory("\\Test Folder");

            List<string> fileData = new List<string>();
            fileData.Add("Line one. Some new data.");
            fileData.Add("Line two. Some other data.");
            fileData.Add("Line three. Here is a really long line of data that takes up almost half of my screen.");

            WriteFile("Test File", fileData, "Test Folder\\");
        }

        if(Input.GetKeyDown(readFileKey))
        {
            List<string> fileData = new List<string>();

            fileData = ReadFile("Test File", "Test Folder\\");

            for(int i = 0; i < fileData.Count; i++)
            {
                Debug.Log(fileData[i]);
            }
        }
    }
    // Methods which directly interface with FileIO
    #region File Access Methods
    /// <summary>
    /// Reads a file with the name _fileName at _filePathExtension. Returns the fileData as an array.
    ///     Lines starting with '#' are ignored.
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_fileData"></param>
    /// <param name="_filePathExtension"></param>
    /// <returns></returns>
    public List<string> ReadFile(string _fileName, string _filePathExtension)
    {
        List<string> fileData = new List<string>();
        string filePath = rootFilePath + _filePathExtension + _fileName;

        try
        {
            // Reads the file and adds aplicable lines to fileData.
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();

                    if(line[0] != '#')
                    {
                        fileData.Add(line);
                    }
                }
            }

            Debug.Log($"{filePath} was Read Successfully!");
            return fileData;
        }
        catch(FileNotFoundException e)
        {
            Debug.Log("The file at filepath " + filePath + " could not be found!");
            Debug.Log(e);
        }

        return null;
    }
    /// <summary>
    /// Writes a file with the name _fileName and content _fileData to a file at _filePathExtension
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_fileData"></param>
    /// <param name="_filePathExtension"></param>
    public void WriteFile(string _fileName, List<string> _fileData, string _filePathExtension)
    {
        string filePath = rootFilePath + _filePathExtension + _fileName;

        // Writes out each index of the array as a line in the file.
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for(int i = 0; i < _fileData.Count; i++)
            {
                writer.WriteLine(_fileData[i]);
            }
        }

        Debug.Log("File " + _fileName + " successfully saved.");
    }

    #endregion File Access Methods

    // Methods which translate JSON strings into Game Objects
    #region JSON Translation Methods
    /// <summary>
    /// Returns the UpdateDataPacket from a JSON file.
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_filePathExtension"></param>
    /// <returns></returns>
    public UpdateDataPacket CreateDataFromJSON(string _fileName, string _filePathExtension)
    {
        string filePath = rootFilePath + _filePathExtension + _fileName;
        Debug.Log("JSON File Path: " + filePath);

        string JSONString = null;

        foreach(string line in ReadFile(_fileName, _filePathExtension))
        {
            JSONString += line;
        }

        UpdateDataPacket dataPacket = JsonUtility.FromJson<UpdateDataPacket>(JSONString);

        Debug.Log("Data packet successfully created from JSON.");

        return dataPacket;
    }

    /// <summary>
    /// Create a list of all alerts sent from the Splunk JSON
    /// </summary>
    /// <param name="_fileName">Name of the JSON file containing the data</param>
    /// <param name="_filePathExtension">name of the directory containing the file</param>
    /// <returns></returns>
    public List<AlertData> CreateAlertsFromJSON(string _fileName, string _filePathExtension)
    {
        // Log the filepath to the Debug
        string filePath = rootFilePath + _filePathExtension + _fileName;
        Debug.Log("... Loading New Splunk Alert Data ...");

        string JSONString = null;

        foreach (string line in ReadFile(_fileName, _filePathExtension))
        {
            JSONString += line.Replace("\\", "").Replace(" ", "_").Trim();
        }
        Debug.Log($"Splunk Alerts File: {filePath} found. Reading Data ...");

        // Grab the shell with the list of alerts
        SplunkAlertsShell shell = JsonUtility.FromJson<SplunkAlertsShell>(JSONString);
        List<Alert> alerts = shell.alerts;

        bool fromSplunk = false;
        // if the data is being loaded fresh from the Splunk, add the time delay to the dateTimes
        if(_fileName == "FromSplunk.JSON")
        {
            fromSplunk = true;
        }

        // build the new OverInfrastructure based on the first infrastructure in the list
        List<AlertData> returnAlerts = HolderToData(alerts, fromSplunk);

        Debug.Log($"Alerts successfully created from {filePath}");
        return returnAlerts;
    }

    /// <summary>
    /// Creates a list of all InfrastructureDatas passed by the Laforge Topology
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_filePathExtension"></param>
    public List<GameObject> CreateInfrasFromJSON(string _fileName, string _filePathExtension)
    {
        GameObject prefabInfras = GameManager.Instance.InfraPrefab;
        // Log the filepath to the Debug
        string filePath = rootFilePath + _filePathExtension + _fileName;
        Debug.Log("... Loading New Infrastructure Data ...");

        string JSONString = null;

        foreach (string line in ReadFile(_fileName, _filePathExtension))
        {
            JSONString += line.Replace('\\', ' ').Trim();
        }
        Debug.Log($"Infrastructure File: {filePath} found. Reading Data ...");

        // Grab the shell with the list of infrastructures
        LaforgeShell shell = JsonUtility.FromJson<LaforgeShell>(JSONString);
        List<Infrastructure> infras = shell.data.environment.EnvironmentToBuild[0].buildToTeam;

        // Create List of Game Objects
        List<GameObject> returnInfras = new List<GameObject>();
        // Instantiate team and add data from the file to the game Object
        for (int i = 0; i < infras.Count; i++)
        {
            returnInfras.Add(Instantiate(prefabInfras));
            InfrastructureData teamInfra = HolderToData(infras[i]);
            returnInfras[i].GetComponent<InfrastructureData>().SetData(teamInfra.Networks, teamInfra.AllNodes);
            returnInfras[i].GetComponent<InfrastructureData>().InstanceChildren();
            returnInfras[i].SetActive(false);
        }
        Debug.Log($"Infrastructures successfully created from {filePath}");
        return returnInfras;
    }

    /// <summary>
    /// Writes update data packets to a json format.
    /// </summary>
    /// <param name="fileName">The new file's name.</param>
    /// <param name="data">The data to convert to json format.</param>
    public void SaveToJSON(string fileName, List<UpdateDataPacket> data)
    {
        string filePath = "C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\" + fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\");
        }

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                foreach (UpdateDataPacket packet in data)
                {
                    sw.WriteLine(packet.ConvertToJSON());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// Writes an infrastructureData to a JSON file 
    /// </summary>
    /// <param name="_fileName">The new file's name.</param>
    /// <param name="_data">The data to convert to json format.</param>
    public void SaveToJSON(string _fileName, InfrastructureData _data)
    {
        string filePath = "C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\" + _fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\");
        }

        // translate the MonoBehavior data into a holder data structure. This allows the data to be formatted into a 
        //      JSON using JSON Utility to read the data from the holder class
        List<Infrastructure> infras = new List<Infrastructure>();
        for(int i = 0; i < _data.Teams.Count; i ++)
        {
            Infrastructure infra = DataToHolder(
                _data,
                _data.FindTeamByID(i).ID); // This organizes the teams into their numerical order
            infras.Add(infra);
        }
        List<BuildShell> builds = new List<BuildShell>();
        builds.Add(new BuildShell(infras));
        LaforgeShell shell = new LaforgeShell(new DataShell(new EnvironmentShell(builds)));

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                    sw.WriteLine(JsonUtility.ToJson(shell));
            }
            Debug.Log($"Infrastructure successfully saved to {filePath}");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void SaveToJSON(string _fileName, List<InfrastructureData> _data)
    {
        string filePath = "C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\" + _fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\");
        }

        // translate the MonoBehavior data into a holder data structure. This allows the data to be formatted into a 
        //      JSON using JSON Utility to read the data from the holder class
        List<Infrastructure> infras = new List<Infrastructure>();
        for (int i = 0; i < _data.Count; i++)
        {
            Infrastructure infra = DataToHolder(_data[i], i); 
            infras.Add(infra);
        }
        List<BuildShell> builds = new List<BuildShell>();
        builds.Add(new BuildShell(infras));
        LaforgeShell shell = new LaforgeShell(new DataShell(new EnvironmentShell(builds)));

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(JsonUtility.ToJson(shell));
            }
            Debug.Log($"Infrastructure successfully saved to {filePath}");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// Save a list of Alerts to a JSON file
    /// </summary>
    /// <param name="_fileName">name of the File to create</param>
    /// <param name="_data">list of alerts to store</param>
    public void SaveToJSON(string _fileName, List<AlertData> _data)
    {
        string filePath = "C:\\ProgramData\\CSEC Visualizer\\Alerts\\" + _fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Alerts\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Alerts\\");
        }

        // translate the MonoBehavior data into a holder data structure. This allows the data to be formatted into a 
        //      JSON using JSON Utility to read the data from the holder class
        SplunkAlertsShell shell = new SplunkAlertsShell(DataToHolder(_data));

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(JsonUtility.ToJson(shell));
            }
            Debug.Log($"Alerts successfully saved to {filePath}");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    #endregion JSON Translation Methods

    /// <summary>
    /// Return a list of sorted TeamDatas fully loaded with info and ready to be added to instanced objects
    /// </summary>
    /// <param name="_ids">List of ids representing the indexes the teams should be sorted to</param>
    /// <param name="_infraObjects">List of each team's Infrastructure object</param>
    /// <param name="_filePathExtension">File's location within the root directory</param>
    /// <param name="_fileName">name of the file</param>
    /// <returns>Return a list of sorted teams</returns>
    public List<TeamData> SetTeamNamesFromFile(List<int> _ids, List<GameObject> _infraObjects, string _filePathExtension, string _fileName)
    {
        // create list of teams to return
        List<TeamData> returnTeams = new List<TeamData>();
        for (int i = 0; i < _ids.Count; i++)
        {
            TeamData returnTeam = new TeamData();
            returnTeams.Add(returnTeam);
        }

        // create a list of strings to track names from the filereader and a list of strings to track colors from the filereader
        List<string> teamNames = new List<string>();
        List<string> teamColors = new List<string>();

        // log the filepath of the text file
        string filePath = rootFilePath + _filePathExtension + _fileName;
        Debug.Log($"Loading Team Names and Colors from : {filePath} ...");

        // Load Data from file into Local Variables
        foreach (string line in ReadFile(_fileName, _filePathExtension))
        {
            string[] lines = line.Split(':');

            teamNames.Add(lines[0]);
            teamColors.Add(lines[1]);
        }
        Debug.Log($"Names and Colors Successfully Loaded from File.");

        // bundle up the data into the return list of TeamDatas
        Color readColor;
        for (int i = 0; i < returnTeams.Count; i++)
        {
            // int index = UnityEngine.Random.Range(0, teamNames.Count); // We should randomize the text file, rather than the data we're reading it from. That way we have a consistent name bewtween scene loads
            // translate the color string into a color variable
            ColorUtility.TryParseHtmlString(teamColors[i], out readColor);
            // set all data for the team
            returnTeams[i].SetData(_ids[i], teamNames[i], readColor, _infraObjects[i]);
            // shove the team into the space where it's ID says it should be
            returnTeams.Insert(returnTeams[i].ID, returnTeams[i]);
            returnTeams.RemoveAt(i);
        }
        return returnTeams;
    }

    public void GenerateDatabase()
    {
        List<string> directories = new List<string>();


    }

    public bool IsFolderEmpty(string _filePathExtension)
    {

        return false;
    }
    #region Helper Methods
    public DateTime StringToDateTime(string _timeStamp)
    {
        DateTime dateTime;
        CultureInfo cultureInfo = new CultureInfo("en-US");
        try
        {
            dateTime = DateTime.ParseExact(_timeStamp, "F", cultureInfo);
            return dateTime;
        }
        catch (FormatException)
        {
            Console.WriteLine("Unable to parse '{0}'", _timeStamp);
        }

        dateTime = DateTime.Now;
        return dateTime;
    }

    #region Data Type Conversion
    private AlertData HolderToData(Alert _alert, bool fromSplunk)
    {
        Enum.TryParse(_alert.type, out CPTCEvents type);
        DateTime timeStamp = StringToDateTime(_alert.timeStamp);
        if (fromSplunk)
        {
            timeStamp.AddSeconds(delayInterval);
        }
        return new AlertData(type, _alert.nodeIP, _alert.teamID, timeStamp);
    }
    private Alert DataToHolder(AlertData _alert)
    {
        return new Alert(_alert.Type.ToString(),_alert.NodeIP,_alert.TeamID,_alert.TimeStamp.ToString("F"));
    }
    private TeamData HolderToData(Team _team)
    {
        TeamData team = new TeamData(); 
        team.ID =_team.id;
        return team;
    }
    private Team DataToHolder(TeamData _team)
    {
        return new Team(_team.ID, DataToHolder(_team.Alerts));
    }
    private NodeData HolderToData(ProHost _node)
    {
        NodeData node = new NodeData();
        OperatingSystems os;
        string cleanedOS = _node.ProvisionedHostToHost.OS.Replace("-","").Trim();
        Enum.TryParse(cleanedOS, out os);
        node.SetData(_node.subnet_ip, _node.ProvisionedHostToHost.hostname, _node.ProvisionedHostToHost.description, os);
        return node;
    }
    private ProHost DataToHolder(NodeData _node)
    {
        return new ProHost(_node.Ip, new HostContainer(_node.HostName, _node.HostDescription, _node.OS.ToString()), new AgentContainer());
    }
    private NetworkData HolderToData(ProNetwork _network)
    {
        NetworkData network = new NetworkData();
        network.SetData(_network.name, _network.cidr, HolderToData(_network.ProvisionedNetworkToProvisionedHost), _network.ProvisionedNetworkToNetwork.vdi_visible);
        return network;
    }
    private ProNetwork DataToHolder(NetworkData _network)
    {
        return new ProNetwork(_network.NetworkName, _network.Ip, new SubNetworkContainer(_network.VDI), DataToHolder(_network.Nodes));
    }
    private InfrastructureData HolderToData(Infrastructure _infra)
    {
        InfrastructureData infra = new InfrastructureData();
        List<NodeData> nodes = new List<NodeData>();
        foreach(ProNetwork net in _infra.TeamToProvisionedNetwork)
        {
            foreach(ProHost node in net.ProvisionedNetworkToProvisionedHost)
            {
                nodes.Add(HolderToData(node));
                nodes[nodes.Count - 1].Index = nodes.Count - 1;
            }
        }
        infra.SetData(HolderToData(_infra.TeamToProvisionedNetwork), nodes);
        return infra;
    }
    private Infrastructure DataToHolder(InfrastructureData _infra, int _teamNum)
    {
        return new Infrastructure(_teamNum, DataToHolder(_infra.Networks));
    }
    #endregion DataTypeConversion

    #region List Conversion
    private List<AlertData> HolderToData(List<Alert> _alerts, bool fromSplunk)
    {
        List<AlertData> alerts = new List<AlertData>();
        foreach(Alert alert in _alerts)
        {
            alerts.Add(HolderToData(alert, fromSplunk));
        }
        return alerts;
    }
    private List<Alert> DataToHolder(List<AlertData> _alerts)
    {
        List<Alert> alerts = new List<Alert>();
        foreach (AlertData alert in _alerts)
        {
            alerts.Add(DataToHolder(alert));
        }
        return alerts;
    }
    private List<NodeData> HolderToData(List<ProHost> _nodes)
    {
        List<NodeData> nodes = new List<NodeData>();
        foreach(ProHost node in _nodes)
        {
            nodes.Add(HolderToData(node));
        }
        return nodes;
    }
    private List<ProHost> DataToHolder(List<NodeData> _nodes)
    {
        List<ProHost> nodes = new List<ProHost>();
        foreach (NodeData node in _nodes)
        {
            nodes.Add(DataToHolder(node));
        }
        return nodes;
    }
    private List<TeamData> HolderToData(List<Team> _teams)
    {
        List<TeamData> teams = new List<TeamData>();
        foreach (Team team in _teams)
        {
            teams.Add(HolderToData(team));
        }
        return teams;
    }
    private List<Team> DataToHolder(List<TeamData> _teams)
    {
        List<Team> teams = new List<Team>();
        foreach (TeamData team in _teams)
        {
            teams.Add(DataToHolder(team));
        }
        return teams;
    }
    private List<NetworkData> HolderToData(List<ProNetwork> _networks)
    {
        List<NetworkData> networks = new List<NetworkData>();
        foreach(ProNetwork net in _networks)
        {
            networks.Add(HolderToData(net));
        }
        return networks;
    }
    private List<ProNetwork> DataToHolder(List<NetworkData> _networks)
    {
        List<ProNetwork> networks = new List<ProNetwork>();
        foreach (NetworkData netData in _networks)
        {
            networks.Add(DataToHolder(netData));
        }
        return networks;
    }
    #endregion List Conversion
    #endregion Helper Methods

    /*Redundant code because nodes will not be changing during competition event
     * /// <summary>
    /// Compares the Nodes taken from last Tick and updates the ones that have changed
    /// </summary>
    /// <param name="_fileName">name of the file with the data</param>
    /// <param name="_filePathExtension">name of the directory within the root directory</param>
    public void UpdateNodes(string _fileName, string _filePathExtension)
    {
        // get new node data
        List<NodeData> newNodes = CreateNodesFromJSON( _fileName, _filePathExtension);

        // get old node data
        List<NodeData> currentNodes = GameManager.Instance.MainInfra.AllNodes;

        int i;
        // for each node, check if it needs to be updated
        for(i = 0; i < currentNodes.Count; i++)
        {
            // If the new version of the Node Data is different than the old verion, update the old Node to the value of the new version
            if(newNodes[i].Type != currentNodes[i].Type)
            {
                GameManager.Instance.MainInfra.AllNodes[i].Type = newNodes[i].Type;
            }
            if (newNodes[i].Teams != currentNodes[i].Teams)
            {
                GameManager.Instance.MainInfra.AllNodes[i].Teams = newNodes[i].Teams;
            }
        }

        Debug.Log($" {i} / {currentNodes.Count} Nodes successfully updated. ");
    }*/
}
