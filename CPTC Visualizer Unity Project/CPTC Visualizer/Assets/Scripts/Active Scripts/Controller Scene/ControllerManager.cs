using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneConfigFiles { Controller, Infrastructure}

public class ControllerManager: MonoBehaviour
{
    #region Fields
    /// <summary>
    /// Reference to the Filemanager Script
    /// </summary>
    [SerializeField]
    private FileManager fileManager;
    /// <summary>
    /// Reference to the DataWriter Script
    /// </summary>
    [SerializeField]
    private TestDataWriter testDataWriter;
    /// <summary>
    /// Reference to the on-screne data stream in unity
    /// </summary>
    [SerializeField]
    private DataLog dataLog;

    /// <summary>
    /// Button to tranfer Scene from Controller Scene to JSON Test Scene
    /// </summary>
    [Header("UI Objects")]
    [SerializeField]
    private Button toJSONTestButton;

    /// <summary>
    /// 
    /// </summary>
    [Header("Timer Fields")]
    [SerializeField]
    private float dataPullInterval;
    [SerializeField]
    private float dataReadInterval;

    [SerializeField]
    private float passNewDataCount;
    [SerializeField]
    private float passNewDataTime;

    [SerializeField]
    private float generateTestDataCount;
    [SerializeField]
    private float generateTestDataTime;

    /// <summary>
    /// Filename of incoming file from Laforge servers
    /// </summary>
    [Header("Incoming JSON Files")]
    [SerializeField]
    private string laforgeJSON_fileName = "cptc_finals_2020_laforge_topo.json";
    /// <summary>
    /// Incoming data from splunk querry application
    /// </summary>
    [SerializeField]
    private string splunkJSON_fileName;
    [SerializeField]
    private string splunkJSON_filePath;

    /// <summary>
    /// old data Laforge server data from the last pull
    /// </summary>
    private List<InfrastructureData> oldLaforgeInfras;
    /// <summary>
    /// old splunk data from the last pull. 
    /// </summary>
    private List<AlertData> oldSplunk;

    /// <summary>
    /// File name for laforge data read by the Infrastructure Scene
    /// </summary>
    [Header("Outgoing JSON Files")]
    [SerializeField]
    private string infraFileName = "test_controllerToInfraScene.json";
    /// <summary>
    /// File name for Splunk data read by the Infrastructure Scene
    /// </summary>
    [SerializeField]
    private string alertsFileName = "test_controllerToAlertsScene.json";
    #endregion Fields

    #region Properties

    public float DataPullInterval
    {
        get { return dataPullInterval;}
        set { dataPullInterval = value; }
    }

    public float DataReadInterval
    {
        //get { return dataReadInterval; }
        set { dataReadInterval += 5; }
    }

    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        dataReadInterval = 5;
        generateTestDataCount = 0.0f;
        generateTestDataTime = 5;
        SendInfrastructureToScene();
        SendAlertsToScene(); // This will be called from update at intervals once we are able to produce live data
        toJSONTestButton.onClick.AddListener(delegate { GoToJSONTest(); });
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to check for a config update
        generateTestDataCount += Time.deltaTime;
        if (generateTestDataCount >= generateTestDataTime)
        {
            generateTestDataCount = 0.0f;

            testDataWriter.WriteEventData();

            dataLog.Print("Test Data generated.");
        }

        // Check if new data has been passed into the system
        passNewDataCount += Time.deltaTime;
        if(passNewDataCount >= passNewDataTime)
        {
            passNewDataCount = 0.0f;
            SendAlertsToScene();
            dataLog.Print("Alerts Sent to Scene.");
            SendInfrastructureToScene();
            dataLog.Print("Laforge sent to Scene.");
        }
    }

    /// <summary>
    /// Method called by Scnene transfer button when it is clicked.
    /// </summary>
    public void GoToJSONTest()
    {
        SendInfrastructureToScene();
        SendAlertsToScene();
        SceneManager.LoadScene(sceneName: "JSON Reader Test");
    }

    /// <summary>
    /// Read Infrastructure data from the Laforge JSON thorugh the FileReader and send it to Infrastructure scene
    /// </summary>
    public void SendInfrastructureToScene()
    {
        // retrieve data from new file
        List<InfrastructureData> newLaforgeInfras = fileManager.CreateInfrasFromJSON(laforgeJSON_fileName, "Infrastructure\\Database\\");
        // if the old data hasn't been pulled from the file yet, or the new data is different from the old data
        for (int i = 0; i < newLaforgeInfras.Count; i++)
        {
            // Check if updated infrastructure information needs to be loaded to Infra Scene
            if (oldLaforgeInfras == null || newLaforgeInfras[i] != oldLaforgeInfras[i])
            {
                // Save the data within the Controller Scene as data to be compared against later
                oldLaforgeInfras = newLaforgeInfras;
                // convert data to saveable format
                List<InfrastructureData> saveInfras = new List<InfrastructureData>();
                for (int j = 0; j < newLaforgeInfras.Count; j++)
                {
                    saveInfras.Add(oldLaforgeInfras[j]);
                }
                // pass the data along to the Infrastructue Scene's filepath
                fileManager.SaveToJSON(infraFileName, saveInfras);
            }
        }
    }
    /// <summary>
    /// Send Event Data from the Splunk JSON to the Infrastruction Scene
    /// </summary>
    public void SendAlertsToScene()
    {
        List<AlertData> newSplunk = fileManager.CreateAlertsFromJSON(splunkJSON_fileName, splunkJSON_filePath);
        // if the old data hasn't been pulled from the file yet, or the new data is different from the old data
        for (int i = 0; i < newSplunk.Count; i++)
        {
            if (oldSplunk == null || newSplunk[i].Type != oldSplunk[i].Type)
            {
                oldSplunk = newSplunk;
                // pass the data along to the Infrastructue Scene's filepath
                fileManager.SaveToJSON(alertsFileName, oldSplunk);
            }
        }
    }
    #region Config Files
    /// <summary>
    /// TODO: Update to a better method.
    /// 
    /// Quick and dirty Onclick workaround.
    /// </summary>
    /// <param name="sceneNum"></param>
    public void ConfigButtonOnclick(string sceneName)
    {
        switch(sceneName)
        {
            case "controller":
                {
                    WriteConfigFile(SceneConfigFiles.Controller);
                    break;
                }
            case "infrastructure":
                {
                    WriteConfigFile(SceneConfigFiles.Infrastructure);
                    break;
                }
        }
    }

    /// <summary>
    /// Writes the corresponding config file.
    /// </summary>
    /// <param name="scene"></param>
    public void WriteConfigFile(SceneConfigFiles scene)
    {
        List<string> configFileData = new List<string>();

        switch(scene)
        {
            case SceneConfigFiles.Controller:
                {
                    configFileData.Add("First line of the Controller config file.");
                    configFileData.Add("dataPullInterval: " + dataPullInterval);

                    fileManager.WriteFile("Config_Controller.txt", configFileData, null);
                    break;
                }
            case SceneConfigFiles.Infrastructure:
                {
                    configFileData.Add("First line of the Infrastructure config file.");
                    configFileData.Add("dataReadInterval: " + dataReadInterval);
                    configFileData.Add("AIViewSwapInterval: ");
                    configFileData.Add("TimeDelay: ");
                    configFileData.Add("OtherUselessInfo: ");

                    fileManager.WriteFile("Config_Infrastructure.txt", configFileData, "Infrastructure\\");
                    break;
                }
        }
    }

    /// <summary>
    /// Reads the data from the corresponding config file.
    /// </summary>
    /// <param name="scene"></param>
    public void ReadConfigFile(SceneConfigFiles scene)
    {
        switch (scene)
        {
            case SceneConfigFiles.Controller:
                {
                    

                    break;
                }
            case SceneConfigFiles.Infrastructure:
                {
                    

                    break;
                }
        }
    }
    #endregion Config Files
}
