using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseNotif : MonoBehaviour
{
    public static MouseNotif current;
    public Image interactSprite;
    public Text interactKey;
    public Text interactText;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string key, string notif)
    {
        interactSprite.color = new Color(interactSprite.color.r, interactSprite.color.g, interactSprite.color.b, 1);
        interactKey.text = key;
        interactText.text = notif;
    }

    public void Hide()
    {
        interactSprite.color = new Color(interactSprite.color.r, interactSprite.color.g, interactSprite.color.b, 0);
        interactKey.text = "";
        interactText.text = "";
    }
}
