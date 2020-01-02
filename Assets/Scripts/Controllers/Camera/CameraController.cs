using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [HideInInspector] public Camera cam;

    public float moveSpeed;
    private Transform target;

    [HideInInspector] public float startSize, currentSize, zoomTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();

        startSize = cam.orthographicSize;
        target = GameManager.Player();
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!PauseManager.instance.flags.isActive && (transform.position != target.position))
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    /*
    /// <summary>
    /// Smoothly zooms the camera this script is attached to in or out
    /// depending on the target-size.
    /// </summary>
    /// <param name="targetSize"> Orthographic size of camera after zooming.  </param>
    /// <param name="time"> Time it should take to zoom. </param>
    public void ZoomCamera(float targetX, float targetY, float time)
    {
        pixCam.refResolutionX = (int)(Mathf.Lerp(pixCam.refResolutionX, targetX, time * Time.deltaTime));
        pixCam.refResolutionY = (int)(Mathf.Lerp(pixCam.refResolutionY, targetY, time * Time.deltaTime));
    }
    */

    /// <summary>
    /// Smoothly zooms the camera this script is attached to in or out
    /// depending on the target-size.
    /// </summary>
    /// <param name="targetSize"> Orthographic size of camera after zooming.  </param>
    /// <param name="time"> Time it should take to zoom. </param>
    public void ZoomCamera(float targetSize, float time)
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, time * Time.deltaTime);
    }

    public IEnumerator LerpCamera(Vector2 targetPosition, float duration)
    {
        Vector3 startPosition = cam.transform.position;

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            cam.transform.position = Vector3.Lerp(startPosition, new Vector3(targetPosition.x, targetPosition.y, startPosition.z), blend); // Blends to the corresponding opacity between start & target.

            yield return null; // Wait one frame, then repeat.
        }
    }

    public Vector2 GetPosition()
    {
        return cam.transform.position;
    }
}