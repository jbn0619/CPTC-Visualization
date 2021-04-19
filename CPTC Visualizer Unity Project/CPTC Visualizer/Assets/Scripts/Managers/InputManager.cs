using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager: MonoBehaviour
{
    #region Fields

    [Header("Controller Inputs")]
    [SerializeField]
    private KeyCode exitMenu;

    [Header("Camera Inputs")]
    [SerializeField]
    private float camMoveSpeed;
    [SerializeField]
    private KeyCode fasterCam;
    [SerializeField]
    private KeyCode camUp;
    [SerializeField]
    private KeyCode camDown;
    [SerializeField]
    private KeyCode camLeft;
    [SerializeField]
    private KeyCode camRight;

    [Header("Infrastructure Inputs")]
    [SerializeField]
    private KeyCode toggleNotif;
    [SerializeField]
    private KeyCode startProgram;
    [SerializeField]
    private KeyCode startVisualizing;

    [Header("Leaderboard Inputs")]
    [SerializeField]
    private KeyCode leaderTest;

    [Header("Misc.")]
    [SerializeField]
    private KeyCode writeInfraToJSON;

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
        // Check which scene's inputs we're looking at.
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "Controller":
                CheckControllerInputs();
                break;
            case "Infrastructure View":
                CheckInfrastructureInputs();
                break;
            case "Leaderboard View":
                CheckLeaderboardInputs();
                break;
        }
    }

    /// <summary>
    /// Checks inputs for the controller scene.
    /// </summary>
    private void CheckControllerInputs()
    {
        if (Input.GetKey(exitMenu))
        {

        }
    }

    /// <summary>
    /// Checks inputs for the infrastructure scene.
    /// </summary>
    private void CheckInfrastructureInputs()
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

        // Create a new Infrastructure and write it to the Json
        if (Input.GetKeyDown(writeInfraToJSON))
        {
            //jsonWriter.GenerateData();
        }
    }

    /// <summary>
    /// Checks inputs for the leaderboard scene.
    /// </summary>
    private void CheckLeaderboardInputs()
    {
        if (Input.GetKey(leaderTest))
        {

        }
    }

    /// <summary>
    /// Checks if the user has moved the camera at all.
    /// </summary>
    private void CheckCameraInputs()
    {
        // Grab our camera variables.
        Camera mainCam = GameManager.Instance.MainCam;
        float newMoveSpeed = camMoveSpeed;

        // Check how fast our camera will be moving based-on user inputs.
        if (Input.GetKey(fasterCam)) newMoveSpeed *= 2;

        // Move the camera in the corresponding direction.
        if (Input.GetKey(camUp))
        {
            mainCam.transform.position += new Vector3(0, newMoveSpeed, 0);
        }

        if (Input.GetKey(camDown))
        {
            mainCam.transform.position += new Vector3(0, -1 * newMoveSpeed, 0);
        }

        if (Input.GetKey(camLeft))
        {
            mainCam.transform.position += new Vector3(-1 * newMoveSpeed, 0, 0);
        }

        if (Input.GetKey(camRight))
        {
            mainCam.transform.position += new Vector3(newMoveSpeed, 0, 0);
        }
    }
}
