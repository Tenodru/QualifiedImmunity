using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader current;

    [Header("Scene Loading References")]
    [Tooltip("The loading screen object.")]
    public GameObject loadingScreen;
    [Tooltip("The loading screen canvas group.")]
    public CanvasGroup canvasGroup;
    [Tooltip("The loading screen progress bar.")]
    public Slider progressBar;
    [Tooltip("The loading screen progress text.")]
    public TextMeshProUGUI progressText;
    [Tooltip("The loading screen title.")]
    public TextMeshProUGUI loadTitle;
    [Tooltip("The loading screen sound.")]
    public AudioClip sound;

    AsyncOperation loadingOp;
    bool isLoading = false;
    bool animTitleInvoked = false;
    int curDots = 3;
    int maxDots = 3;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreen);
        canvasGroup.alpha = 1f;
        loadingScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        {
            float progressValue= Mathf.Clamp01(loadingOp.progress * 1f);
            progressBar.value = progressValue;
            progressText.text = Mathf.Round(progressValue * 100) + "%";

            if (!animTitleInvoked)
            {
                InvokeRepeating("AnimTitle", 0f, 0.25f);
                animTitleInvoked = true;
                Debug.Log("Invoked.");
            }
        }
    }

    public void AnimTitle()
    {
        if (curDots < maxDots)
        {
            curDots++;
            string dots = new string('.', curDots);
            loadTitle.text = "LOADING" + dots;
        }
        else if (curDots == maxDots)
        {
            curDots = 0;
            loadTitle.text = "LOADING";
        }
    }

    /// <summary>
    /// Loads the specified scene.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene (string sceneName)
    {
        AudioHandler.current.PlaySound(sound);
        StartCoroutine(Timer(x => AudioHandler.current.StopMusic(), 1f));
        StartCoroutine(Timer(x => TransitionPanel.current.Animate("FadeIn"), 1f));
        StartCoroutine(Timer(x => TransitionPanel.current.Animate("FadeOut"), 2f));
        StartCoroutine(Timer(x => StartCoroutine(StartLoad(sceneName, true)), 2f));
        if (sceneName == "PoliceStation")
        {
            StartCoroutine(Timer(x => AudioHandler.current.PlayMusic(AudioHandler.current.policeStationMusic), 2.1f));
        } else
        {
            StartCoroutine(Timer(x => AudioHandler.current.PlayMusic(AudioHandler.current.missionMusic), 2.1f));
        }
    }

    /// <summary>
    /// Asynchronous scene load.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator StartLoad (string sceneName, bool wait=false)
    {
        if (wait)
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(2);
        }

        isLoading = true;
        loadingScreen.SetActive(true);
        canvasGroup.alpha = 1f;
        //StartCoroutine(FadeScreen(1, 1));

        loadingOp = SceneManager.LoadSceneAsync(sceneName);
        while (!loadingOp.isDone)
        {
            yield return null;
        }

        StartCoroutine(FadeScreen(0, 1));
        
        isLoading = false;
        animTitleInvoked = false;
    }

    /// <summary>
    /// Fades the screen alpha to the specified value over time.
    /// </summary>
    /// <param name="endVal"></param>
    /// <param name="dur"></param>
    /// <returns></returns>
    IEnumerator FadeScreen (float endVal, float dur)
    {
        float startVal = canvasGroup.alpha;
        float startValAudio = AudioHandler.current.soundSource.volume;
        float time = 0;

        while (time < dur)
        {
            if (sound != null)
            {
                AudioHandler.current.soundSource.volume = Mathf.Lerp(startValAudio, endVal, time / dur);
            }
            canvasGroup.alpha = Mathf.Lerp(startVal, endVal, time / dur);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endVal;
        loadingScreen.SetActive(false);
        AudioHandler.current.soundSource.Stop();
        isLoading = false;
        canvasGroup.alpha = 1;
        progressBar.value = 0;
        progressText.text = 0 + "%";
        AudioHandler.current.soundSource.volume = startValAudio;
        GameManager.current.AddSavedEntries();
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
