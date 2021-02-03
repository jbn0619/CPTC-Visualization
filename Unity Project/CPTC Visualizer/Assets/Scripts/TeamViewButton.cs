using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamViewButton: MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int newTeamIndex;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Text buttonText; 
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// Gets or sets the index of the team view as long as it's a lega value.
    /// </summary>
    public int NewTeamIndex
    {
        get
        {
            return newTeamIndex;
        }
        set
        {
            if (value >= -1)
            {
                newTeamIndex = value;
            }
        }
    }

    /// <summary>
    /// Gets the button component of this game object.
    /// </summary>
    public Button Button
    {
        get
        {
            return button;
        }
    }

    /// <summary>
    /// Gets the text component of this button.
    /// </summary>
    public Text ButtonText
    {
        get
        {
            return buttonText;
        }
    }
    
    #endregion Properties

    /// <summary>
    /// Sets what team view is active based-on this script's index.
    /// </summary>
    public void SetTeamView()
    {
        InfrastructureManager.Instance.SelectTeamView(newTeamIndex);
    }
}
