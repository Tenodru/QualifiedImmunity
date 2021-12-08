using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tracks mission progress (success/failure).
/// </summary>
public class MissionManager : MonoBehaviour
{
    public static MissionManager current;

    [Tooltip("List of all game missions and their statuses.")] public List<MissionStatus> missions;
    [Tooltip("The active Mission object.")] public Mission activeMission;
    public Sprite officeClosed;

    public bool intermissionActive = false;

    bool rickmanActive = false;
    bool spriteSwapped = false;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this.gameObject);
        } else
        {
            current = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "PoliceStation")
        {
            /*
            foreach (MissionStatus missionStatus in missions)
            {
                // Disable map access if intermission is currently active.
                if (missionStatus.missionObjective == CurrentObjective.current.currentObjective && CurrentObjective.current.currentObjective != null)
                {
                    DeskInteract.current.isEnabled = false;
                    break;
                }
            }*/

            DeskInteract.current.isEnabled = !intermissionActive;

            if (missions[0].played && !missions[0].missionCompleteDialogue.complete)
            {
                activeMission = missions[0].mission;
                DialogueManager.current.startingDialogue = missions[0].missionCompleteDialogue.dialogue;
                intermissionActive = true;
            }
            if (missions[1].played && !missions[1].missionCompleteDialogue.complete)
            {
                if (!spriteSwapped)
                {
                    GameObject.FindGameObjectWithTag("MainOffice").GetComponent<SpriteRenderer>().sprite = officeClosed;
                    GameObject.FindGameObjectWithTag("Rickman").SetActive(false);
                    spriteSwapped = true;
                }
                activeMission = missions[1].mission;
                DialogueManager.current.startingDialogue = missions[1].missionCompleteDialogue.dialogue;
                intermissionActive = true;
            }
        }
    }

    /// <summary>
    /// Returns the play status of the specified Mission.
    /// </summary>
    /// <param name="mission">The Mission to seek.</param>
    /// <returns></returns>
    public bool HasMissionBeenPlayed(Mission mission)
    {
        foreach (MissionStatus missionStatus in missions)
        {
            // Mission recognized.
            if (missionStatus.mission == mission)
            {
                return missionStatus.played;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the completion status of the specified Mission.
    /// </summary>
    /// <param name="mission">The Mission to seek.</param>
    /// <returns></returns>
    public bool HasMissionBeenCompleted(Mission mission)
    {
        foreach (MissionStatus missionStatus in missions)
        {
            // Mission recognized.
            if (missionStatus.mission == mission)
            {
                return missionStatus.complete;
            }
        }
        return false;
    }

    /// <summary>
    /// Sets the play status of the specified Mission.
    /// </summary>
    /// <param name="mission">The Mission to seek.</param>
    /// <param name="status">The new play status of the Mission, false or true.</param>
    public void SetMissionPlayStatus(Mission mission, bool status)
    {
        foreach (MissionStatus missionStatus in missions)
        {
            // Mission recognized.
            if (missionStatus.mission == mission)
            {
                missionStatus.played = status;
                return;
            }
        }
    }

    /// <summary>
    /// Sets the completion status of the specified Mission.
    /// </summary>
    /// <param name="mission">The Mission to seek.</param>
    /// <param name="status">The new completion status of the Mission, false or true.</param>
    public void SetMissionCompleteStatus(Mission mission, bool status)
    {
        foreach (MissionStatus missionStatus in missions)
        {
            // Mission recognized.
            if (missionStatus.mission == mission)
            {
                missionStatus.complete = status;
                return;
            }
        }
    }

    /// <summary>
    /// Sets the completion status of the specified Mission intermission.
    /// </summary>
    /// <param name="mission">The Mission to seek.</param>
    /// <param name="status">The new completion status of the Mission intermission, false or true.</param>
    public void SetIntermissionCompleteStatus(Mission mission, bool status)
    {
        foreach (MissionStatus missionStatus in missions)
        {
            // Mission recognized.
            if (missionStatus.mission == mission)
            {
                missionStatus.missionCompleteDialogue.complete = status;
                NextMission();
                return;
            }
        }
    }

    /// <summary>
    /// Sets the next mission in the sequence, if there is one, to the active mission.
    /// </summary>
    public void NextMission()
    {
        Debug.Log("Got NextMission() call.");
        for (int i = 0; i < missions.Count-1; i++)
        {
            if (missions[i].mission == activeMission)
            {
                activeMission = missions[i + 1].mission;
                Debug.Log("Progressing mission to " + activeMission.missionName);
                return;
            }
        }
    }
}
