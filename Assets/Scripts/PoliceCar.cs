using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceCar : MonoBehaviour
{
    private KeyCode keyCode;
    private bool ready;

    // Start is called before the first frame update
    void Start()
    {
        keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), GetComponent<InteractTrigger>().interactKey);
        ready = false;
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (GetComponent<InteractTrigger>().canBePressed)
            {
                if (ready)
                {
                    SceneManager.LoadScene("PoliceStation");
                }
                else
                {
                    AlertText.current.ShowAlert("Please finish the mission first.", Color.red);
                }
            }
        }
    }
}
