using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [Header("Camera Characteristics - Movement")]
    [SerializeField] float cameraSpeed;

    [Header("Camera Characteristics - Zoom")]
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float targetOrtho;
    [SerializeField] float smoothSpeed = 2.0f;
    [SerializeField] float minOrtho = 3.0f;
    [SerializeField] float maxOrtho = 8.0f;

    [Header("Object References")]
    [Tooltip("The target object the camera should follow.")]
    [SerializeField] Transform player;
    BoxCollider2D cameraBox;
    Camera cam;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        cameraBox = gameObject.GetComponent<BoxCollider2D>();
        targetOrtho = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        AspectRatioBoxChange();
        FollowPlayer();
        CameraZoom();
    }

    /// <summary>
    /// Adjusts the cameraBox size based on the camera's set orthographic size.
    /// </summary>
    void AspectRatioBoxChange()
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        cameraBox.size = new Vector2(width, height);
    }

    /// <summary>
    /// Follows a target object.
    /// </summary>
    void FollowPlayer()
    {
        Vector3 lerpPosition = Vector3.Lerp(transform.position, player.position, cameraSpeed);
        transform.position = lerpPosition;
    }

    /// <summary>
    /// Uses the mouse scroll wheel to zoom the camera in and out.
    /// </summary>
    void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Re-centers this camera.
    /// </summary>
    public void ResetCam()
    {
        transform.position = new Vector3(-44.5f, -1.4f, -10f);
    }

    /// <summary>
    /// Toggles this object.
    /// </summary>
    public void Toggle()
    {
        active = !active;
        gameObject.SetActive(active);
        gameObject.GetComponent<Camera>().enabled = active;
        ResetCam();
    }
}
