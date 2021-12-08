using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Ending { None, BadEnd, GoodEnd, TrueEnd }

[System.Serializable]
public class DialogueSegment
{
    public string name;
    public Sprite portrait;
    public Sprite displayImage;
    [TextArea(1, 5)]
    public string dialogueText;
    public bool italics = false;
    public bool bold = false;
    public AudioClip sound;
    public Character character = Character.None;
}

[System.Serializable]
public class DialogueOptions
{
    public enum ComparisonType { None, LessThan, GreaterThan, Equal }
    public enum WorldStats { none, citizenScore, missionSuccesses, missionFails }

    public string optionName;
    public DialogueInfo nextDialogue;

    [Header("Prerequisites")]
    public WorldStats worldStat = WorldStats.none;
    public ComparisonType comparisonType = ComparisonType.None;
    public int threshold = 0;

    public DialogueOptions(string inputStr, int inputInt)
    {

    }
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueInfo : ScriptableObject
{
    public enum DialogueTransitionType { Automatic, Manual, Exit }
    public enum PanelTransitionType { None, FadeIn, FadeOut, FadeInAndOut }
    public enum DialogueType { Normal, ObjectiveRequirement, Clue, StartReview, Analysis, Accuse, IntermissionEnd, CultistDetection, Teleport, DisappearNPC, ShowNPC , Credits }

    public DialogueType dialogueType = DialogueType.Normal;

    [DrawIf("dialogueType", DialogueType.Clue, ComparisonType.Equals)]
    public EntryInfo clue;

    [DrawIf("dialogueType", DialogueType.Analysis, ComparisonType.Equals)]
    public AnalysisInfo analysis;

    [DrawIf("dialogueType", DialogueType.CultistDetection, ComparisonType.Equals)]
    public int cultistChange = 0;

    [DrawIf("dialogueType", DialogueType.Teleport, ComparisonType.Equals)]
    public TeleportLocation teleportLoc;

    [DrawIf("dialogueType", DialogueType.DisappearNPC, ComparisonType.Equals)]
    [Tooltip("The tag on the NPC you wish to disappear.")] 
    public string npcTagD;

    [DrawIf("dialogueType", DialogueType.ShowNPC, ComparisonType.Equals)]
    [Tooltip("The tag on the NPC you wish to show.")]
    public string npcTagS;

    public List<DialogueSegment> dialogueText;



    public List<DialogueOptions> options;

    public ObjectiveInfo newObjective;

    public PanelTransitionType pTransType = PanelTransitionType.None;
    public DialogueTransitionType dTransType = DialogueTransitionType.Exit;

    public bool endMission = false;
    [DrawIf("endMission", true, ComparisonType.Equals)]
    public bool conclusive = false;

    public Ending endGame = Ending.None;
}

/// <summary>
/// The character that is speaking this dialogue.
/// </summary>
public enum Character { None, Sal, Police, Aglia, NPC }
/// <summary>
/// Teleport the player here. Only used in PoliceStation and Cultist Room.
/// </summary>
public enum TeleportLocation { None, Office, ChiefRoom, CultistFail, CultistWin }
