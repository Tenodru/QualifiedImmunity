using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableStation : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Closes the map.
    /// </summary>
    public override void OnMouseUpAsButton()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Debug.Log("Clicked UI.");
            return;
        }
        highlightOutline.SetActive(false);
        PlayerControlManager.current.CloseMap();
    }
}
