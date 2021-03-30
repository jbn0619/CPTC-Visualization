using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager: MonoBehaviour
{
    #region Fields

    [Header("Camera Controls")]
    [SerializeField]
    private KeyCode up;
    [SerializeField]
    private KeyCode down;
    [SerializeField]
    private KeyCode left;
    [SerializeField]
    private KeyCode right;
    [SerializeField]
    private KeyCode faster;

    [Header("Other fields")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float moveSpeed;

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
        // Begin by seeing if we're using faster speed or not.
        float speed;
        if (Input.GetKey(faster)) speed = moveSpeed * 2;
        else speed = moveSpeed;

        // Check to see what direction the camera will move in.
        if (Input.GetKey(up)) this.gameObject.transform.position += new Vector3(0, speed, 0);

        if (Input.GetKey(down)) this.gameObject.transform.position += new Vector3(0, -1 * speed, 0);

        if (Input.GetKey(left)) this.gameObject.transform.position += new Vector3(-1 * speed, 0, 0);

        if (Input.GetKey(right)) this.gameObject.transform.position += new Vector3(speed, 0, 0);

        // See if the camera is zooming in or out.
        if (Input.mouseScrollDelta.y > 0) cam.orthographicSize -= 0.1f;
        else if (Input.mouseScrollDelta.y < 0) cam.orthographicSize += 0.1f;
    }
}
