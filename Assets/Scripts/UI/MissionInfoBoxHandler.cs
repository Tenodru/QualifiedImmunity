using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles interactions for the Mission Info Box UI element.
/// </summary>
public class MissionInfoBoxHandler : MonoBehaviour
{
    public static MissionInfoBoxHandler current;

    [Header("References")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject missionInfoBox;
    [SerializeField] GameObject missionInfoBoxBG;
    [SerializeField] Image missionInfoImage;
    [SerializeField] TextMeshProUGUI missionInfoName;
    [SerializeField] TextMeshProUGUI missionInfoDesc;
    [SerializeField] TextMeshProUGUI missionInfoTime;

    [Header("Display Parameters")]
    [SerializeField] bool allCapsTitle = true;

    PlayerStatsHandler playerStats;
    Mission mission;
    SceneLoader sceneLoader;

    string missionSceneName;
    public static bool isMissionBoxOpen = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStatsHandler.current;
        missionInfoBox.SetActive(false);
        missionInfoBoxBG.SetActive(false);
        isMissionBoxOpen = false;
        sceneLoader = SceneLoader.current;
        canvasGroup.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the current selected mission.
    /// </summary>
    /// <param name="mission"></param>
    public void SetMission(Mission newMission)
    {
        Debug.Log("Setting current mission.");
        mission = newMission;

        SetMissionName(mission.missionName);
        SetMissionDesc(mission.missionDesc);
        //SetMissionTime(mission.timeCost);
        SetMissionScene(mission.sceneName);

        if (mission.previewImage != null)
        {
            SetMissionImage(mission.previewImage);
        }
    }

    /// <summary>
    /// Shows the info box.
    /// </summary>
    public void ShowInfoBox()
    {
        Debug.Log("Showing selected mission.");
        missionInfoBox.SetActive(true);
        missionInfoBoxBG.SetActive(true);
        isMissionBoxOpen = true;
    }

    /// <summary>
    /// Hides the info box.
    /// </summary>
    public void HideInfoBox()
    {
        Debug.Log("Hiding mission info box.");
        missionInfoBox.SetActive(false);
        missionInfoBoxBG.SetActive(false);
        isMissionBoxOpen = false;
    }

    /// <summary>
    /// Sets this mission's preview image.
    /// </summary>
    /// <param name="newImage"></param>
    public void SetMissionImage(Sprite newImage)
    {
        missionInfoImage.sprite = newImage;
    }

    /// <summary>
    /// Sets this mission's name.
    /// </summary>
    /// <param name="newName"></param>
    public void SetMissionName(string newName)
    {
        if (allCapsTitle)
            missionInfoName.text = newName.ToUpper();
        else
            missionInfoName.text = newName;
    }

    /// <summary>
    /// Sets this mission's description.
    /// </summary>
    /// <param name="newDesc"></param>
    public void SetMissionDesc(string newDesc)
    {
        missionInfoDesc.text = newDesc;
    }

    /// <summary>
    /// Sets this mission's time cost.
    /// </summary>
    /// <param name="newTime"></param>
    public void SetMissionTime(int newTime)
    {
        missionInfoTime.text = (newTime.ToString());

        if (newTime > 1)
        {
            missionInfoTime.text = (newTime.ToString());
        }
    }

    /// <summary>
    /// Sets this mission's Scene.
    /// </summary>
    /// <param name="newScene"></param>
    public void SetMissionScene(string newSceneName)
    {
        missionSceneName = newSceneName;
    }

    /// <summary>
    /// Switches the scene to the selected mission scene.
    /// </summary>
    public void PlayMission()
    {
        //playerStats.ChangeTimeBudget(-mission.timeCost);
        //SceneManager.LoadScene(missionSceneName);
        sceneLoader.LoadScene(missionSceneName);
        MissionManager.current.activeMission = mission;

        //StartCoroutine(FadeScreen(0, 1, 1));
    }

    /// <summary>
    /// Fades the screen alpha to the specified value over time.
    /// </summary>
    /// <param name="endVal"></param>
    /// <param name="dur"></param>
    /// <returns></returns>
    IEnumerator FadeScreen(float endVal, float dur, float waitTime = 0)
    {
        yield return new WaitForSeconds(waitTime);

        float startVal = canvasGroup.alpha;
        float time = 0;

        while (time < dur)
        {
            canvasGroup.alpha = Mathf.Lerp(startVal, endVal, time / dur);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endVal;
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
