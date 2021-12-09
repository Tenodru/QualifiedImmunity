using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeStart : MonoBehaviour
{
    public GameObject sceneManager;
    public GameObject audioHandler;
    public GameObject missionManager;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneLoader.FindObjectOfType<SceneLoader>() == null)
        {
            Instantiate(sceneManager);
            Debug.Log("spawning sceneManager");
        }
        if (AudioHandler.FindObjectOfType<AudioHandler>() == null)
        {
            Instantiate(audioHandler);
            Debug.Log("spawning audioHandler");
        }
        if (MissionManager.FindObjectOfType<MissionManager>() == null)
        {
            Instantiate(missionManager);
            Debug.Log("spawning missionManager");
        }
    }
}
