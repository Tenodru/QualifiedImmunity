using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler current;

    [Header("Background Music")]
    public AudioSource musicSource;
    public AudioSource soundSource;

    [Header("Background Music")]
    [Tooltip("Mission Music")]
    public AudioClip missionMusic;
    [Tooltip("Police Station Music")]
    public AudioClip policeStationMusic;
    [Tooltip("Underground Music")]
    public AudioClip undergroundMusic;

    [Header("Sound Effects")]
    public AudioClip analysisError;
    public AudioClip bubblingPot;
    public AudioClip carDoor;
    public AudioClip creepyCave;
    public AudioClip cultistsDetect;
    public AudioClip exclamation;
    public AudioClip fileDeleted;
    public AudioClip fingerprintScan;
    public AudioClip folder;
    public AudioClip gunshot;
    public AudioClip keyboard;
    public AudioClip keypad;
    public AudioClip locker;
    public AudioClip openCloseHatch;
    public AudioClip policeSiren;
    public AudioClip reload;
    public AudioClip scanResult;
    public AudioClip scribble;
    public AudioClip secretDoor;
    public AudioClip shopDoorbell;
    public AudioClip siftingRubble;
    public AudioClip swoosh;


    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this.gameObject);
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
        PlayMusic(policeStationMusic);
    }

    /// <summary>
    /// Plays the specified sound effect as a oneshot.
    /// </summary>
    /// <param name="sound">The audio clip to play.</param>
    public void PlaySound(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        StartCoroutine(FadeVolume(0.1f, 1, false));
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        StartCoroutine(FadeVolume(0, 1));
    }

    /// <summary>
    /// Fades the musicSource volume to the specified value over time.
    /// </summary>
    /// <param name="endVal"></param>
    /// <param name="dur"></param>
    /// <returns></returns>
    IEnumerator FadeVolume(float endVal, float dur, bool reset = true)
    {
        float startVal = musicSource.volume;
        float time = 0;

        while (time < dur)
        {
            if (musicSource.clip != null)
            {
                musicSource.volume = Mathf.Lerp(startVal, endVal, time / dur);
            }
            time += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = endVal;
        if (reset)
        {
            musicSource.Stop();
            musicSource.volume = startVal;
        }
    }
}
