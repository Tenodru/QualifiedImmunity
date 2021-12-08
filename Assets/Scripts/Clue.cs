using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [Tooltip("The highlight outline for this object.")]
    public GameObject highlightOutline;
    [Tooltip("Whether this Clue has multiple outlines associated with it.")]
    public bool otherOutlines = false;
    public List<GameObject> highlightOutlines;

    [Tooltip("The on-hover text to show for this clue.")]
    public string tooltipText = "Inspect";

    public Sprite offHover;
    public Sprite onHover;
    public bool observed = false;
    public DialogueInfo[] observeDialogue;
    public DialogueInfo observedDialogue;
    public List<ObjectiveInfo> objectives;

    [Tooltip("Whether this Clue's detection system will use the center of the player or the center of the player's 'feet'.")]
    public bool useFeet = false;
    [Tooltip("How this Clue can be interacted with.")]
    public InteractType interactType = InteractType.Distance;
    [DrawIf("interactType", InteractType.Area, ComparisonType.Equals)]
    public GameObject interactArea;
        
    [Tooltip("The interaction sound for this object.")]
    public AudioClip sound;


    // Start is called before the first frame update
    void Start()
    {
        if (tooltipText == "")
        {
            tooltipText = "Inspect";
        }
        if (highlightOutline != null)
        {
            highlightOutline.SetActive(false);
        }
        if (highlightOutlines != null || highlightOutlines.Count > 0)
        {
            foreach (GameObject obj in highlightOutlines)
            {
                obj.SetActive(false);
            }
        }
    }

    private void OnMouseOver()
    {
        if (!UIHoverListener.current.isUIOverride && objectives.Contains(CurrentObjective.current.currentObjective))
        {
            // If object has a highlightOutline, use that.
            // Otherwise, use the old blue color.
            if (otherOutlines)
            {
                if (highlightOutlines != null || highlightOutlines.Count > 0)
                {
                    foreach (GameObject obj in highlightOutlines)
                    {
                        MouseNotif.current.Show("LC", tooltipText);
                        obj.SetActive(true);
                    }
                }
            } else
            {
                if (highlightOutline != null)
                {
                    MouseNotif.current.Show("LC", tooltipText);
                    highlightOutline.SetActive(true);
                }
                else
                {
                    MouseNotif.current.Show("LC", tooltipText);
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    //GetComponent<SpriteRenderer>().sprite = onHover;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (otherOutlines)
        {
            if (highlightOutlines != null || highlightOutlines.Count > 0)
            {
                foreach (GameObject obj in highlightOutlines)
                {
                    MouseNotif.current.Hide();
                    obj.SetActive(false);
                }
            }
        } else
        {
            if (highlightOutline != null)
            {
                MouseNotif.current.Hide();
                highlightOutline.SetActive(false);
            }
            else
            {
                MouseNotif.current.Hide();
                GetComponent<SpriteRenderer>().color = Color.white;
                //GetComponent<SpriteRenderer>().sprite = offHover;
            }
        }
    }

    private void OnMouseUp()
    {
        if (UIHoverListener.current.isUIOverride)
        {
            
        }
        if (!UIHoverListener.current.isUIOverride && DialogueManager.current.currNPC != gameObject)
        {
            if (interactType == InteractType.Distance)
            {
                if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < 3f)
                {
                    if (!DialogueManager.current.dialogueBox.activeInHierarchy)
                    {
                        Inspect();
                    }
                }
                else
                {
                    AlertText.current.ShowAlert("Not close enough to target.", Color.red);
                    return;
                }
            }
            else if (interactType == InteractType.Area)
            {
                Vector3 playerPoint;
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
                    if (!DialogueManager.current.dialogueBox.activeInHierarchy)
                    {
                        Inspect();
                    }
                }
                else
                {
                    AlertText.current.ShowAlert("Not close enough to target.", Color.red);
                    return;
                }
            }
            /*
            if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < 1f)
            {
                if (!DialogueManager.current.dialogueBox.activeInHierarchy)
                {
                    Inspect();
                }
            }
            else
            {
                AlertText.current.ShowAlert("Not close enough to target.", Color.red);
            }*/
        }
    }

    public void Inspect()
    {
        if (objectives.Contains(CurrentObjective.current.currentObjective))
        {
            if (!observed)
            {
                GameEvents.current.InspectClue(gameObject);
                DialogueManager.current.StartDialogue(observeDialogue[objectives.IndexOf(CurrentObjective.current.currentObjective)]);
                observed = true;
            }
            else
            {
                if (observedDialogue != null)
                {
                    DialogueManager.current.StartDialogue(observedDialogue);
                }
                else
                {
                    DialogueManager.current.StartDialogue(observeDialogue[objectives.IndexOf(CurrentObjective.current.currentObjective)]);
                }
            }
        }
    }
}
