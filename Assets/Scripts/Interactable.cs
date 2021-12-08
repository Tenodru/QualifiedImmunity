using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Basic class for Interactable objects.
/// </summary>
public class Interactable : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The distance from which the player can interact with an object.")]
    public float interactDistance = 5f;
    [Tooltip("The interaction key shown in the tooltip.")]
    public string interactKey;
    [Tooltip("The interaction tooltip text.")]
    public string interactText;
    public bool canBePressed;
    [Tooltip("Whether this Interactable can be interacted with.")]
    public bool isEnabled = true;
    [Tooltip("Whether this Interactable's detection system will use the center of the player or the center of the player's 'feet'.")]
    public bool useFeet = false;
    [Tooltip("How this Interactable can be interacted with.")]
    public InteractType interactType = InteractType.Distance;
    [DrawIf("interactType", InteractType.Area, ComparisonType.Equals)]
    public GameObject interactArea;

    public Vector3 playerPoint;

    [Header("References")]
    [Tooltip("The highlight outline for this object.")]
    public GameObject highlightOutline;
    [Tooltip("The interaction sound for this object.")]
    public AudioClip sound;

    [Header("Other")]
    [Tooltip("A list of Objectives. If any of these are active, this object cannot be interacted with.")]
    public List<ObjectiveInfo> restrictedObjectives;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Debug.Log("Setting outline off.");
        highlightOutline.SetActive(false);
        canBePressed = false;

        if (interactArea != null)
        {
            interactArea.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        playerPoint = PlayerController.current.feet.transform.position;
    }

    public virtual void OnMouseOver()
    {
        //Debug.Log("Mouse Enter: " + this.name);
        if (!highlightOutline || !isEnabled)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        highlightOutline.SetActive(true);
        if (!restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
        {
            MouseNotif.current.Show(interactKey, interactText);
            canBePressed = true;
        }
    }

    public virtual void OnMouseExit()
    {
        //Debug.Log("Mouse Exit: " + this.name);
        if (!highlightOutline)
        {
            return;
        }

        highlightOutline.SetActive(false);
        if (!restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
        {
            MouseNotif.current.Hide();
            canBePressed = false;
        }
    }

    public virtual void OnMouseUpAsButton()
    {
        Debug.Log("Distance from " + this.name + " and player is " + Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position));
        if (EventSystem.current.IsPointerOverGameObject() || !isEnabled)
        {
            return;
        }
        if (interactType == InteractType.Distance)
        {
            if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < interactDistance)
            {
                if (sound != null)
                    AudioHandler.current.PlaySound(sound);
            }
            else
            {
                AlertText.current.ShowAlert("Not close enough to target.", Color.red);
                return;
            }
        } else if (interactType == InteractType.Area)
        {
            if (useFeet)
            {
                playerPoint = PlayerController.current.feet.transform.position;
            }
            else
            {
                playerPoint = PlayerController.current.transform.position;
            }
            
            if (interactArea.GetComponent<Collider2D>().bounds.Contains(playerPoint))
            {
                if (sound != null)
                    AudioHandler.current.PlaySound(sound);
            }
            else
            {
                AlertText.current.ShowAlert("Not close enough to target.", Color.red);
                return;
            }
        }
        

        if (restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
        {
            AlertText.current.ShowAlert("Complete your current objective first.", Color.red);
        }
    }
}

/// <summary>
/// How an Interactable will determine whether a player can interact with it.
/// </summary>
[System.Serializable]
public enum InteractType { Distance, Area }
