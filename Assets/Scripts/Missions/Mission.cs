using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An in-game Mission.
/// </summary>
[CreateAssetMenu(fileName = "Mission", menuName = "Missions/Mission", order = 1)]
[System.Serializable]
public class Mission : ScriptableObject
{
    [Header("Mission Attributes and References")]
    [Tooltip("The name for this mission. Give it something snazzy.")] public string missionName;
    //[Tooltip("The Quest for this mission.")] public QuestInfo quest;
    [Tooltip("The preview image for this mission, if applicable. By default, will use the region's preview image.")] public Sprite previewImage;
    [Tooltip("The Scene name for this mission. Be sure to spell it accurately.")] public string sceneName;
    [Tooltip("The Regions this mission can appear in.")] public List<Region> regions;
    [Tooltip("The mission description.")] [TextArea(5, 8)] public string missionDesc = "";
    [Tooltip("Whether this mission is unlocked or not.")] public bool unlocked = false;
    [Tooltip("Whether this mission is complete or not.")] public bool completed = false;
    [Tooltip("The time cost to undertake this mission.")] public int timeCost = 1;
}

/// <summary>
/// A MissionStatus object. Holds a Mission, its "played" status, and its "complete" status.
/// </summary>
[System.Serializable]
public class MissionStatus
{
    [Tooltip("A mission object.")] public Mission mission;
    [Tooltip("The mission completion objective.")] public ObjectiveInfo missionObjective;
    [Tooltip("The starting dialogue that plays in the Station when this mission is completed.")] public IntermDialogueStatus missionCompleteDialogue;
    [Tooltip("Whether this mission has been completed or not. False by default.")] public bool complete = false;
    [Tooltip("Whether this mission has been played or not. False by default.")] public bool played = false;
}

/// <summary>
/// An IntermDialogueStatus object. Holds a DialogueInfo object and its completion status.
/// </summary>
[System.Serializable]
public class IntermDialogueStatus
{
    [Tooltip("The starting dialogue that plays in the Station when this mission is completed.")] public DialogueInfo dialogue;
    [Tooltip("Whether this dialogue has been played. False by default.")] public bool complete = false;
}
