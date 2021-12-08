using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages player controls outside of player characters.
/// </summary>
public class PlayerControlManager : MonoBehaviour
{
    public static PlayerControlManager current;

    [Header("Global Player Controls")]
    public KeyCode escapeKey = KeyCode.Escape;

    // References
    GameObject player;
    GameObject playerCam;

    // Singleton instantiation.

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(current.gameObject);
        }
        else
        {
            current = this;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        playerCam = FindObjectOfType<CameraMovement>().gameObject;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            Debug.Log("Pressed ESC.");
            EscapeKeyPressed();
        }
    }

    /// <summary>
    /// Called when the ESC key is pressed.
    /// </summary>
    void EscapeKeyPressed()
    {
        if (MapHandler.current != null)
        {
            Debug.Log("Maphandler detected.");
            if (MapHandler.current.IsMapOpen() == true)
            {
                Debug.Log("Closing map.");
                CloseMap();
            }
        } 
    }

    /// <summary>
    /// Closes the Map completely.
    /// </summary>
    public void CloseMap()
    {
        if (MapHandler.current.nodeOpen)
        {
            MapHandler.current.CloseNodes();
            MissionInfoBoxHandler.current.HideInfoBox();
        }
        else
        {
            player.SetActive(true);
            playerCam.SetActive(true);
            MapHandler.current.CloseMap();
        }
        MouseNotif.current.Hide();
    }
}
