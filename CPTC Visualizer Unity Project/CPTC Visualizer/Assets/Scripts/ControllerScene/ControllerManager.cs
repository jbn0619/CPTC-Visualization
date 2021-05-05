using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SceneConfigFiles { Controller, Infrastructure}

public class ControllerManager: MonoBehaviour
{
    #region Fields
    [SerializeField]
    private FileManager fileManager;

    [SerializeField]
    private float dataPullInterval;

    [SerializeField]
    private float dataReadInterval;


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
    }

    // Update is called once per frame
    void Update()
    {

    }

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
}
