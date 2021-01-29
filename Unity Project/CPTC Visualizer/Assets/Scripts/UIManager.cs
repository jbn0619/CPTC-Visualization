using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager: Singleton<UIManager>
{
    #region Fields

    private List<Canvas> sceneCanvases;
    private Canvas activeCanvas;

    #endregion Fields

    /// <summary>
    /// Gets what canvas is currently-active in this scene.
    /// </summary>
    public Canvas ActiveCanvas
    {
        get
        {
            return activeCanvas;
        }
    }

    #region Properties

    /// <summary>
    /// Gets a list of all UI canvas objects in this scene.
    /// </summary>
    public List<Canvas> SceneCanvases
    {
        get
        {
            return sceneCanvases;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        sceneCanvases = new List<Canvas>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GatherNewCanvases();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Dumps any currently-loaded canvases and loads-up the new ones.
    /// </summary>
    /// <param name="scene">The new scene that was loaded.</param>
    /// <param name="mode">How this scene was loaded.</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GatherNewCanvases();
    }

    /// <summary>
    /// Gathers all new canvases in this scene, and dumps the old ones.
    /// </summary>
    void GatherNewCanvases()
    {
        sceneCanvases.Clear();

        // Check to see if there is a canvas collection in this scene.
        GameObject canvasCollector = GameObject.FindGameObjectWithTag("CanvasCollection");
        if (canvasCollector != null)
        {
            // Transfer canvas references into this script for storage/management.
            Canvas[] newCanvases = canvasCollector.GetComponentsInChildren<Canvas>();
            foreach (Canvas c in newCanvases)
            {
                sceneCanvases.Add(c);
            }

            // Disable all canvases except the first one
            for (int i = 0; i < sceneCanvases.Count; i++)
            {
                if (i > 0)
                {
                    sceneCanvases[i].gameObject.SetActive(false);
                }
                else
                {
                    activeCanvas = sceneCanvases[0];
                }
            }
        }
    }
}
