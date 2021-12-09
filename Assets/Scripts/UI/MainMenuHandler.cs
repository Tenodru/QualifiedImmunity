using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public GameObject stillMenuImage;
    public GameObject transitionPanel;
    public Button playButton;
    public AudioClip playSound;

    // Start is called before the first frame update
    void Start()
    {
        stillMenuImage.SetActive(true);
        playButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    /// <summary>
    /// Plays the game, sends the player to the Bakery Mission.
    /// </summary>
    public void Play()
    {
        PlayPlayAnim();
        //StartCoroutine(Timer(x => TransitionPanel.current.Animate("FadeIn"), 1f));
        StartCoroutine(Timer(x => SceneLoader.current.LoadScene("BakeryMission"), 0f));
    }

    /// <summary>
    /// Called when the Play button is clicked.
    /// </summary>
    public void PlayPlayAnim()
    {
        stillMenuImage.SetActive(false);
        playButton.gameObject.SetActive(false);
        videoPlayer.Play();
        AudioHandler.current.PlaySound(playSound);
    }
}
