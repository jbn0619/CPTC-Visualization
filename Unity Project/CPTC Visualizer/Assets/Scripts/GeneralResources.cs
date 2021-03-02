using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompetitionType { CPTC, CCDC }

public class GeneralResources: Singleton<GeneralResources>
{
    #region Fields

    [SerializeField]
    private List<Sprite> nodeSprites;

    [Header("Materials")]
    [SerializeField]
    private Material netBase;
    [SerializeField]
    private Material netScan;

    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets a list of sprites for different node types.
    /// </summary>
    public List<Sprite> NodeSprites
    {
        get
        {
            return nodeSprites;
        }
    }

    /// <summary>
    /// Gets the material for the network when it's unmodified.
    /// </summary>
    public Material NetBase
    {
        get
        {
            return netBase;
        }
    }

    /// <summary>
    /// Gets the material for a network being scanned.
    /// </summary>
    public Material NetScan
    {
        get
        {
            return netScan;
        }
    }
    
    #endregion Properties
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
