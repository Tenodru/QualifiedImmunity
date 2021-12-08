using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSystemHandler : MonoBehaviour
{
    public static NodeSystemHandler current;

    [SerializeField] List<MapNode> mapNodes;
    public List<MissionNode> missionNodes;
    public GameObject policeStationOutline;

    public bool nodeClicked = false;

    public void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (policeStationOutline != null)
        {
            policeStationOutline.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Returns a list of current active Map Nodes.
    /// </summary>
    public List<MapNode> GetMapNodes() 
    {
        return mapNodes;
    }

    /// <summary>
    /// Adds the specified Map Node to the Map Nodes list.
    /// </summary>
    /// <param name="newNode"></param>
    public void AddMapNode(MapNode newNode)
    {
        mapNodes.Add(newNode);
    }

    /// <summary>
    /// Called when the map is opened.
    /// </summary>
    public void OpenMap()
    {
        foreach (MissionNode node in missionNodes)
        {
            if (node.mission == MissionManager.current.activeMission)
            {
                node.node.SetActive(true);
            } else
            {
                node.node.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by a Node's EventTrigger when it detects the mouse cursor over the button.
    /// </summary>
    /// <param name="mission"></param>
    public void OnHover(GameObject outline)
    {
        outline.SetActive(true);
        MouseNotif.current.Show("lc", "Select Mission");
    }

    /// <summary>
    /// Called by a Node's EventTrigger when it detects the mouse cursor exiting the button.
    /// </summary>
    /// <param name="outline"></param>
    public void OffHover(GameObject outline)
    {
        outline.SetActive(false);
        MouseNotif.current.Hide();
    }

    /// <summary>
    /// Called by a Node when it is clicked.
    /// </summary>
    /// <param name="mission"></param>
    public void SelectMission(Mission mission)
    {
        if (!nodeClicked)
        {
            MissionInfoBoxHandler.current.ShowInfoBox();
            MissionInfoBoxHandler.current.SetMission(mission);
            nodeClicked = true;
            MapHandler.current.nodeOpen = true;
        }
        else
        {
            // Close nodes.
            MissionInfoBoxHandler.current.HideInfoBox();
            nodeClicked = false;
            MapHandler.current.nodeOpen = false;
        }
        Debug.Log("Set mission info.");
    }

    /// <summary>
    /// Called when the map is closed.
    /// </summary>
    public void CloseMap()
    {
        PlayerControlManager.current.CloseMap();
        foreach (MissionNode node in missionNodes)
        {
            node.outline.SetActive(false);
        }
        policeStationOutline.SetActive(false);
    }
}

[System.Serializable]
public class MissionNode
{
    [Tooltip("The node GameObject.")]
    public GameObject node;
    [Tooltip("The Mission associated with this node.")]
    public Mission mission;
    [Tooltip("This node's hover outline.")]
    public GameObject outline;
}
