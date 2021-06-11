using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: David Smith
/// Function: Allows any object that has this component to be clicked on and moves the camera towards it, given the object has a collider attached as well
/// </summary>
public class FocusableObject : MonoBehaviour
{
    #region Fields
    private float baseDuration = 7.5f;
    private float conversionRate = 2f;
    private float offset = 0.25f;

    #endregion Fields

    /// <summary>
    /// When the collider is clicked on, grab the position that needs to be moved to, as well as the zoom that is needed, and then enterpolate the camera accordingly.
    /// </summary>
    private void OnMouseDown()
    {
        Vector3 tempPosition = Camera.main.transform.position;
        tempPosition.x = this.transform.position.x;
        tempPosition.y = this.transform.position.y;
        if(Camera.main.transform.position != tempPosition)
        {
            InputManager manager = GameObject.Find("InputManager").GetComponent<InputManager>();
            float zoomMin = manager.zoomMin * (conversionRate - offset);
            float zoomMax = manager.zoomMax / (conversionRate + offset);
            StartCoroutine(LerpCamera(tempPosition, zoomMin, zoomMax));
        }   
    }

    /// <summary>
    /// Handles the linear interpolation of the camera, including both the position and the zoom
    /// </summary>
    IEnumerator LerpCamera(Vector3 newPos, float zoomMin, float zoomMax)
    {
        float timeElapsed = 0.0f;

        // Make the duration inversely proportional to the distance between the camera and the node.
        float totalDuration = baseDuration / Vector3.Distance(this.transform.position, newPos);

        while (timeElapsed < totalDuration)
        {
            // Creating a t (interpolant, clamped to the range between 0,1) for linear interpolation to utilize a smooth step movement type
            float t = timeElapsed/totalDuration;
            // Formula obtained from https://gamedevbeginner.com/wp-content/uploads/Smooth-Step-930x501.jpg
            t *= t * (3f - 2f * t);

            // Handle the actual linear interpolation (Formula: a + (b - a) * t)

            // 
            /* Camera dolley effect (Move away initially for 1/8th, move to the end position for the remaining 7/8ths)
            Causing some jumpy issues and awkward movement
            if(timeElapsed < totalDuration / 8)
            {
                Vector3 camPos = Camera.main.transform.position;
                Vector3 inversePos = camPos-(newPos/1.5f);
                inversePos.z = camPos.z;
                Camera.main.transform.position = Vector3.Lerp(camPos, inversePos, t);
            }
            else
            {
                // Move the camera to the value determined by the linear interpolation
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, t);
            }
            */

            // Move the camera to the value determined by the linear interpolation
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, t);

            // At the first half, zoom out away from the object
            if(timeElapsed < totalDuration / 2f)
            {
                // Zoom camera closer to the maximum value using linear interpolation
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomMax, t);
            }
            // During the second half, zoom in towards the object
            else
            {
                // Zoom camera closer to the minimum value using linear interpolation
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomMin, t);
            }
            
            // Add to the total elapsed time
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // At the end, set the camera's position and camera zoom to the final position
        Camera.main.transform.position = newPos;
        Camera.main.orthographicSize = zoomMin;
    }
}
