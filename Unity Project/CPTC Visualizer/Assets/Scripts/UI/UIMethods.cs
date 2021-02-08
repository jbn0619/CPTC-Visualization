using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMethods: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private KeyCode mainMenuKey = KeyCode.Escape;
    
    #endregion Fields
    
    #region Properties
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(mainMenuKey)) LoadMainMenuScene();
    }

    #region Main Menu Button Methods

    /// <summary>
    /// Loads the CPTC Visualizer scene for the user.
    /// </summary>
    public void LoadCPTCScene()
    {
        SceneManager.LoadScene("CPTC Visualizer");
    }

    /// <summary>
    /// Loads the CCDC Visualizer scene for the user.
    /// </summary>
    public void LoadCCDCScene()
    {
        SceneManager.LoadScene("CCDC Visualizer");
    }

    /// <summary>
    /// Loads the credits scene for the user.
    /// </summary>
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    /// <summary>
    /// Loads the Data Generator Scene for the user.
    /// </summary>
    public void LoadDataGeneratorScene()
    {
        SceneManager.LoadScene("Data Generator");
    }

    #endregion Main Menu Button Methods

    #region Returning to Main Menu Methods

    /// <summary>
    /// Loads the main menu scene for the user.
    /// </summary>
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    #endregion Returning to Main Menu Methods
}
