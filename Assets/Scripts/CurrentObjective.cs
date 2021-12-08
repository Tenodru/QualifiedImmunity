using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentObjective : MonoBehaviour
{
    public static CurrentObjective current;
    public ObjectiveInfo currentObjective;
    public Text objName;
    public Text objDesc;

    bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        if (currentObjective != null)
        {
            AssignObjective(currentObjective);
        }
    }

    public void AssignObjective(ObjectiveInfo objInfo)
    {
        currentObjective = objInfo;
        objName.text = currentObjective.objectiveName;
        objDesc.text = currentObjective.objective;
        GameEvents.current.AssignObjective(objInfo);
        GameManager.current.RecordObjective(objInfo);
        GetComponent<Animator>().SetTrigger("In");
        if (currentObjective.music != null)
        {
            AudioSource.FindObjectOfType<AudioSource>().clip = currentObjective.music;
            AudioSource.FindObjectOfType<AudioSource>().Play();
        }
    }

    public void CompleteObjective()
    {
        objDesc.text = "";
        if (currentObjective.onCompletionType == ObjectiveInfo.OnCompletionType.dialogueStart)
        {
            DialogueManager.current.StartDialogue(currentObjective.completionDialogue);
        }
        else if (currentObjective.onCompletionType == ObjectiveInfo.OnCompletionType.nextObjective)
        {
            AssignObjective(currentObjective.nextObjective);
        }
    }

    /// <summary>
    /// Toggles this component.
    /// </summary>
    public void Toggle()
    {
        active = !active;
        gameObject.SetActive(active); 
    }
}
