using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveRequirement : MonoBehaviour
{
    public enum ObjectiveType { reachArea, findClues, cultistTalk }

    public ObjectiveInfo objective;
    public ObjectiveType objectiveType;

    [DrawIf("objectiveType", ObjectiveType.findClues, ComparisonType.Equals)]
    public List<EntryInfo> clues;
    [DrawIf("objectiveType", ObjectiveType.cultistTalk, ComparisonType.Equals)]
    public int cultistsTalkedTo;
    [DrawIf("objectiveType", ObjectiveType.findClues, ComparisonType.Equals)]
    public ObjectiveInfo findCluesObjective;
    [DrawIf("objectiveType", ObjectiveType.cultistTalk, ComparisonType.Equals)]
    public DialogueInfo winCultistDialogue;
    [DrawIf("objectiveType", ObjectiveType.cultistTalk, ComparisonType.Equals)]
    public DialogueInfo failCultistDialogue;

    void Start()
    {
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (objectiveType == ObjectiveType.findClues)
        {
            GameEvents.current.onCollectClue += CollectClue;
        }
    }

    public void AddToCultistCount()
    {
        if (objectiveType != ObjectiveType.cultistTalk)
        {
            return;
        }
        cultistsTalkedTo += 1;
        if (cultistsTalkedTo >= 5 && CurrentObjective.current.currentObjective == objective)
        {
            if (GameManager.current.cultistDetection >= 5)
            {
                DialogueManager.current.StartDialogue(failCultistDialogue);
            }
            else
            {
                DialogueManager.current.StartDialogue(winCultistDialogue);
            }
        }
    }

    public void CollectClue(EntryInfo clue)
    {
        if (clues.Contains(clue))
        {
            clues.Remove(clue);
            if (clues.Count <= 0)
            {
                if (findCluesObjective != null)
                    CurrentObjective.current.AssignObjective(findCluesObjective);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && objectiveType == ObjectiveType.reachArea && objective == CurrentObjective.current.currentObjective)
        {
            CurrentObjective.current.CompleteObjective();
            gameObject.SetActive(false);
        }
    }
}
