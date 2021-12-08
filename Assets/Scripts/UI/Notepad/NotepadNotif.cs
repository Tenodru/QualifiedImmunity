using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadNotif : MonoBehaviour
{
    public static NotepadNotif current;
    public Text entryName;
    public Image entryImage;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public void TriggerNotif(EntryInfo entry)
    {
        entryName.text = entry.entryName;
        entryImage.sprite = entry.entrySprite;
        GetComponent<Animator>().SetTrigger("NewEntry");
    }
}
