using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfraInputManager: InputManager
{
    #region Fields

    [Header("Infrastructure Inputs")]
    [SerializeField]
    private KeyCode toggleNotif = KeyCode.LeftControl;
    [SerializeField]
    private KeyCode startProgram = KeyCode.Return;
    [SerializeField]
    private KeyCode startVisualizing = KeyCode.Space;

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
        CheckInputs();
    }

    public override void CheckInputs()
    {
        CheckCameraInputs();

        // Starts or stops the Time Until Show notification
        if (Input.GetKeyDown(toggleNotif))
        {
            UIManager.Instance.ToggleNotifBanner();
        }

        // Master Key. Starts the program in its entirety with one key press
        if (Input.GetKeyDown(startProgram) && !GameManager.Instance.CompStarted)
        {
            GameManager.Instance.StartOfComp = System.DateTime.Now;
            Debug.Log(System.DateTime.Now.ToString());

            GameManager.Instance.TeamManager.ReadTeams();
            DataFormatter.Instance.HasStart = true;
            GameManager.Instance.CompStarted = true;

            //System.Diagnostics.Process.Start("notepad.exe");
        }

        // Starts visualizing
        if (Input.GetKeyDown(startVisualizing) && !GameManager.Instance.ReadDateStarted)
        {
            GameManager.Instance.StartOfVisualizer = System.DateTime.Now;
            GameManager.Instance.TimeDelay = Mathf.Abs((int)GameManager.Instance.StartOfVisualizer.Subtract(GameManager.Instance.StartOfComp).TotalMinutes);

            DataFormatter.Instance.Delay = GameManager.Instance.TimeDelay;
            GameManager.Instance.ReadDateStarted = true;
            //eventManager.ReadAttacksJSON();
            TeamViewAI.Instance.BeginComp();
        }
    }
}
