using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskInteract : Interactable
{
    public static DeskInteract current;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        current = this;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();

        if (!isEnabled)
        {
            return;
        }
        if (interactType == InteractType.Distance)
        {
            if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < interactDistance && !restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
            {
                highlightOutline.SetActive(false);
                MapHandler.current.OpenMap();
            }
        }
        else if (interactType == InteractType.Area)
        {
            if (interactArea.GetComponent<Collider2D>().bounds.Contains(playerPoint))
            {
                highlightOutline.SetActive(false);
                MapHandler.current.OpenMap();
            }
        }
        
    }
}
