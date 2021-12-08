using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Behavior handler for a single Map Node.
/// </summary>
public class MapNode : MonoBehaviour, IPointerClickHandler, IPointerDownHandler,
    IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Mission mission;

    public bool nodeClicked = false;
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MissionManager.current.activeMission == mission)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMissionInfo()
    {
        MissionInfoBoxHandler.current.SetMission(mission);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clicked: " + this.name);
        if (!nodeClicked)
        {
            for (int i = 0; i < FindObjectsOfType<MapNode>().Length; i++)
            {
                FindObjectsOfType<MapNode>()[i].GetComponent<SpriteRenderer>().color = Color.white;
                FindObjectsOfType<MapNode>()[i].nodeClicked = false;
            }
            this.GetComponent<SpriteRenderer>().color = Color.green;
            MissionInfoBoxHandler.current.ShowInfoBox();
            SetMissionInfo();
            nodeClicked = true;
            MapHandler.current.nodeOpen = true;
        }
        else
        {
            // Close nodes.
            this.GetComponent<SpriteRenderer>().color = Color.white;
            MissionInfoBoxHandler.current.HideInfoBox();
            nodeClicked = false;
            MapHandler.current.nodeOpen = false;
        } 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down: " + this.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Mouse Up: " + this.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Enter: " + this.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit: " + this.name);
    }
}
