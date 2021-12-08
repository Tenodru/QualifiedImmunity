using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hndles global map behavior.
/// </summary>
public class MapHandler : MonoBehaviour
{
    public static MapHandler current;

    [Header("References")]
    [SerializeField] GameObject operationsRoom;
    [SerializeField] GameObject map;
    [SerializeField] GameObject mapPlayer;
    [SerializeField] MapCameraController mapCamera;
    [SerializeField] Camera playerCamera;

    MenuManager menuMan;
    PlayerController playerController;

    bool mapOpen = false;
    public bool nodeOpen = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        menuMan = FindObjectOfType<MenuManager>();
        map.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Opens/shows the map and associated elements.
    /// </summary>
    public void OpenMap()
    {
        nodeOpen = false;
        mapOpen = true;
        map.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        mapPlayer.SetActive(true);
        mapCamera.Toggle();
        CurrentObjective.current.Toggle();
        playerController.gameObject.SetActive(false);
        operationsRoom.SetActive(false);
        menuMan.notepadEnabled = false;

        NodeSystemHandler.current.OpenMap();
    }

    /// <summary>
    /// Closes/hides the map and associated elements.
    /// </summary>
    public void CloseMap()
    {
        nodeOpen = false;
        mapOpen = false;
        map.SetActive(false);
        mapPlayer.SetActive(false);
        mapPlayer.GetComponent<MapPlayerController>().ResetPlayer();
        mapCamera.GetComponent<MapCameraController>().Toggle();
        CurrentObjective.current.Toggle();
        playerController.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(true);
        operationsRoom.SetActive(true);
        menuMan.notepadEnabled = true;
    }

    /// <summary>
    /// Returns whether the City Map is open or not.
    /// </summary>
    /// <returns></returns>
    public bool IsMapOpen()
    {
        return mapOpen;
    }

    /// <summary>
    /// Closes all nodes.
    /// </summary>
    public void CloseNodes()
    {
        for (int i = 0; i < FindObjectsOfType<MapNode>().Length; i++)
        {
            FindObjectsOfType<MapNode>()[i].GetComponent<SpriteRenderer>().color = Color.white;
            FindObjectsOfType<MapNode>()[i].nodeClicked = false;
        }
        nodeOpen = false;
    }
}
