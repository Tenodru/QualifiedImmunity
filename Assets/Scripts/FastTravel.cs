using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravel : MonoBehaviour
{
    //THIS SCRIPT IS NO LONGER USED
    private KeyCode keyCode;

    // Start is called before the first frame update
    void Start()
    {
        keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), GetComponent<InteractTrigger>().interactKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (GetComponent<InteractTrigger>().canBePressed)
            {
                CameraMovement.current.target = GameObject.Find("Map Sketch 1").transform;
                CameraMovement.current.playerFocus = false;
                //CameraMovement.current.zoomSize = 36;
                PlayerController.current.playerCanMove = false;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("click");
        if (!PlayerController.current.playerCanMove)
        {
            Debug.Log("click2");
            PlayerController.current.transform.position = transform.position;
            CameraMovement.current.target = PlayerController.current.transform;
            CameraMovement.current.playerFocus = true;
            //CameraMovement.current.zoomSize = 5;
            PlayerController.current.playerCanMove = true;
        }
    }
}
