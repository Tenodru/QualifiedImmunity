using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportLocation;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    public List<ObjectiveInfo> restrictedObjectives;

    public AudioClip sound;
    public AudioClip music;

    public bool isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMouseOver()
    {
        if (!restrictedObjectives.Contains(CurrentObjective.current.currentObjective) && isEnabled)
        {
            MouseNotif.current.Show("LC", "Use Door");
        } 
    }

    public void OnMouseExit()
    {
        if (!restrictedObjectives.Contains(CurrentObjective.current.currentObjective) && isEnabled)
        {
            MouseNotif.current.Hide();
        }
    }

    public void OnMouseUpAsButton()
    {
        if (restrictedObjectives.Contains(CurrentObjective.current.currentObjective)
            || PlayerController.current.teleporting || DialogueManager.current.dialogueBox.activeInHierarchy || !isEnabled)
        {
            return;
        }
        if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < 2.5f)
        {
            if (sound != null)
            {
                AudioHandler.current.PlaySound(sound);
            }
            if (music != null)
            {
                AudioHandler.current.PlayMusic(music);
            }
            PlayerController.current.teleporting = true;
            TransitionPanel.current.Animate("FadeIn");
            Invoke("MovePlayer", 1f);
            Invoke("FinishTeleport", 1.2f);
        }
        else
        {
            AlertText.current.ShowAlert("Not close enough to target.", Color.red);
        }
    }

    public void MovePlayer()
    {
        PlayerController.current.transform.position = teleportLocation.position;
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    public void FinishTeleport()
    {
        TransitionPanel.current.Animate("FadeOut");
        PlayerController.current.teleporting = false;
    }
}
