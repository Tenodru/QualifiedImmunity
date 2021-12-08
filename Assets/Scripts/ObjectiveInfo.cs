using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Objective", menuName = "Objective")]
public class ObjectiveInfo : ScriptableObject
{
    public enum OnCompletionType { none, dialogueStart, nextObjective }

    public string objectiveName;
    public string objective;

    [Header("Music")]
    public AudioClip music;

    [Header("OnCompletion")]
    public OnCompletionType onCompletionType;
    [DrawIf("onCompletionType", OnCompletionType.dialogueStart, ComparisonType.Equals)]
    public DialogueInfo completionDialogue;

    [DrawIf("onCompletionType", OnCompletionType.nextObjective, ComparisonType.Equals)]
    public ObjectiveInfo nextObjective;

    // Start is called before the first frame update
    void Start()
    {

    }
}