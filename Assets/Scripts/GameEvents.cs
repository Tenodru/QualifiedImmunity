using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public event Action onCloseDialogue;
    public void CloseDialogue()
    {
        if (onCloseDialogue != null)
        {
            onCloseDialogue();
        }
    }

    public event Action<ObjectiveInfo> onAssignObjective;
    public void AssignObjective(ObjectiveInfo objectiveInfo)
    {
        if (onAssignObjective != null)
        {
            onAssignObjective(objectiveInfo);
        }
    }

    public event Action<EntryInfo> onCollectClue;
    public void CollectClue(EntryInfo clue)
    {
        if (onCollectClue != null)
        {
            onCollectClue(clue);
        }
    }

    public event Action<GameObject> onInspectClue;
    public void InspectClue(GameObject obj)
    {
        if (onInspectClue != null)
        {
            onInspectClue(obj);
        }
    }
}
