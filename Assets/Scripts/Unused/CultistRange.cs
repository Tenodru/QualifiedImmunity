using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistRange : MonoBehaviour
{
    public DialogueInfo caughtDialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DialogueManager.current.dialogueBox.activeInHierarchy)
        {
            GetComponentInParent<NPC>().StartDialogue(caughtDialogue);
            gameObject.SetActive(false);
        }
    }
}
