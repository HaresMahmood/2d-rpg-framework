using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [HideInInspector] public Camera cam;
    private PixelPerfectCamera pixCam;

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
        pixCam = GetComponent<PixelPerfectCamera>();

        startSize = cam.orthographicSize;
        target = GameManager.Player();
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (transform.position != target.position)
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
}