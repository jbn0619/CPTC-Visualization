using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusableObject : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float baseDuration = 3.0f;

    #endregion Fields

    private void OnMouseDown()
    {
        Vector3 tempPosition = Camera.main.transform.position;
        tempPosition.x = this.transform.position.x;
        tempPosition.y = this.transform.position.y;
        StartCoroutine(LerpCamera(tempPosition));
    }

    IEnumerator LerpCamera(Vector3 newPos)
    {
        float timeElapsed = 0.0f;

        // Make the duration inversely proportional to the distance between the camera and the node.
        float totalDuration = baseDuration / Vector3.Distance(this.transform.position, newPos);

        while (timeElapsed < totalDuration)
        {
            // Creating a t for linear enterpolation to utilize a smooth step movement type
            float t = timeElapsed/totalDuration;
            t *= t * (3f - 2f * t);

            // Move the camera to the value determined by the t value and linear enterpolation
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, t);

            // Add to the total elapsed time
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // At the end, set the camera's position to the final position
        Camera.main.transform.position = newPos;
    }
}
