using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Justin Neft
/// Function: The base input manager which keeps track of basic inputs that are likely to be shared between all scenes.
/// </summary>
public class InputManager: MonoBehaviour
{
    #region Fields

    [Header("Camera Inputs")]
    [SerializeField]
    private float camMoveSpeed = 0.02f;
    [SerializeField]
    private KeyCode fasterCam = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode camUp = KeyCode.W;
    [SerializeField]
    private KeyCode camDown = KeyCode.S;
    [SerializeField]
    private KeyCode camLeft = KeyCode.A;
    [SerializeField]
    private KeyCode camRight = KeyCode.D;
    [SerializeField]
    private int zoomMin = 2;
    [SerializeField]
    private int zoomMax = 10;
    [SerializeField]
    private float zoomSensitivity = 1.0f/3.0f;
    [SerializeField]
    private float orthToLocalScale = 5f;
    /*
    [Header("Controller Inputs")]
    [SerializeField]
    private KeyCode exitMenu;

    [Header("Leaderboard Inputs")]
    [SerializeField]
    private KeyCode leaderTest;

    [Header("Misc.")]
    [SerializeField]
    private KeyCode writeInfraToJSON;
    */

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

    /// <summary>
    /// Checks for user inputs every frame.
    /// </summary>
    public virtual void CheckInputs()
    {
        CheckCameraInputs();
    }

    /// <summary>
    /// Checks if the user has moved the camera at all.
    /// </summary>
    protected void CheckCameraInputs()
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

        // On scroll down, zoom out if there is the room to
        if(mainCam.orthographicSize < zoomMax-zoomSensitivity && Input.mouseScrollDelta.y < 0)
        {
            // Change the orthographic size of the camera
            mainCam.orthographicSize += 1 * zoomSensitivity;

            // Transform the camera to keep the background in a similar scale to the zoom
            // 5 is the arbitrary conversion between orthographic scale and the local scale of the camera (Can be tinkered with to find a more exact rate)
            Vector3 tempScale = mainCam.transform.localScale;
            tempScale.x += zoomSensitivity / 5;
            tempScale.y += zoomSensitivity / 5;
            mainCam.transform.localScale = tempScale;
        }

        // On scroll up, zoom in as long as it doesn't go too small
        if(mainCam.orthographicSize > zoomMin+zoomSensitivity && Input.mouseScrollDelta.y > 0)
        {
            // Change the orthographic size of the camera
            mainCam.orthographicSize -= 1 * zoomSensitivity;

            // Transform the camera to keep the background in a similar scale to the zoom
            // OrthToLocalScale has yet to be balanced definitely, best number so far was 5
            Vector3 tempScale = mainCam.transform.localScale;
            tempScale.x -= zoomSensitivity / orthToLocalScale;
            tempScale.y -= zoomSensitivity / orthToLocalScale;
            mainCam.transform.localScale = tempScale;
            
        }
    }
}
