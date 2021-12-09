using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionPanel : MonoBehaviour
{
    public static TransitionPanel current;
    public bool endingScreen = false;
    public string playOnStart = "";


    private void Start()
    {
        current = this;
        if (playOnStart != "")
        {
            GetComponent<Animator>().SetTrigger(playOnStart);
        }
        if (endingScreen)
        {
            //StartCoroutine(Timer(x => GetComponent<Animator>().SetTrigger("FadeIn"), 5f));
            StartCoroutine(Timer(x => SceneLoader.current.LoadScene("MainMenu"), 6f));
        }
    }

    public void Animate(string type)
    {
        GetComponent<Animator>().SetTrigger(type);
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
