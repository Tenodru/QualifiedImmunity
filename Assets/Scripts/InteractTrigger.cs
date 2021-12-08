using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    public string interactKey;
    public string interactText;
    public bool canBePressed;

    public List<ObjectiveInfo> restrictedObjectives;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact()
    {
        // Do things.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
        {
            MouseNotif.current.Show(interactKey, interactText);
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !restrictedObjectives.Contains(CurrentObjective.current.currentObjective))
        {
            MouseNotif.current.Hide();
            canBePressed = false;
        }
    }
}
