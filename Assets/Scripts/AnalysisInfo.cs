using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnalysisOptions
{
    public EntryInfo clue;
    public DialogueInfo nextDialogue;
    public bool influenceCost = true;
}

[CreateAssetMenu(fileName = "New Analysis", menuName = "Analysis")]
public class AnalysisInfo : ScriptableObject
{
    public List<AnalysisOptions> options;
    public DialogueInfo specialGiveUp;
}
