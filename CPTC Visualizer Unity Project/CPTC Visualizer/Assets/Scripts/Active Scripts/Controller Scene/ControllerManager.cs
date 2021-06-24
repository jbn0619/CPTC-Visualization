using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneConfigFiles { Controller, Infrastructure}

public class ControllerManager: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private FileManager fileManager;
    [SerializeField]
    private TestDataWriter testDataWriter;
    [SerializeField]
    private DataLog dataLog;
    [Header("UI Objects")]
    [SerializeField]
    private Button toJSONTestButton;

    [Header("Timer Fielsds")]
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

    [Header("Incoming JSON Files")]
    [SerializeField]
    private string laforgeJSON_fileName = "cptc_finals_2020_laforge_topo.json";
    [SerializeField]
    private string splunkJSON_fileName = "test_FromSplunk.json";

    private InfrastructureData oldLaforge;
    private List<AlertData> oldSplunk;

    [Header("Outgoing JSON Files")]
    [SerializeField]
    private string infraFileName = "test_controllerToInfraScene.json";
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
        }
    }
    public void GoToJSONTest()
    {
        SendInfrastructureToScene();
        SendAlertsToScene();
        SceneManager.LoadScene(sceneName: "JSON Reader Test");
    }

    public void SendInfrastructureToScene()
    {
        InfrastructureData newLaforge = fileManager.CreateInfraFromJSON(laforgeJSON_fileName, "Infrastructure\\Database\\");
        if(oldLaforge == null || newLaforge != oldLaforge)
        {
            oldLaforge = newLaforge;
            fileManager.SaveToJSON(infraFileName, newLaforge);
        }
    }

    public void SendAlertsToScene()
    {
        List<AlertData> newSplunk = fileManager.CreateAlertsFromJSON(splunkJSON_fileName, "Alerts\\");
        if(oldSplunk == null || newSplunk.Count != oldSplunk.Count)
        {
            oldSplunk = newSplunk;
            fileManager.SaveToJSON(alertsFileName, oldSplunk);
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
