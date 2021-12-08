using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public List<DialogueInfo> dialogueRecord;
    public List<ObjectiveInfo> objectiveRecord;

    public List<EntryInfo> savedEntries;

    public int citizenScore = 0;
    public int missionSuccesses = 0;
    public int missionFails = 0;
    public int cultistDetection = 0;


    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public void RecordDialogue(DialogueInfo dialogueInfo)
    {
        if (!dialogueRecord.Contains(dialogueInfo))
        {
            dialogueRecord.Add(dialogueInfo);
        }
    }

    public void RecordObjective(ObjectiveInfo objectiveInfo)
    {
        if (!objectiveRecord.Contains(objectiveInfo))
        {
            objectiveRecord.Add(objectiveInfo);
        }
    }

    /// <summary>
    /// Add saved entries to the notepad when called.
    /// </summary>
    public void AddSavedEntries()
    {
        foreach (EntryInfo entry in savedEntries) {
            MenuManager.current.AddEntry(entry);
        }
    }
}
