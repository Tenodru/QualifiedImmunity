using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadEntry : MonoBehaviour
{
    public EntryInfo entry;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ButtonClick()
    {
        MenuManager.current.SelectEntry(entry);
        NotepadEntry[] entries = NotepadEntry.FindObjectsOfType<NotepadEntry>();
        foreach (NotepadEntry entry in entries)
        {
            entry.GetComponent<Outline>().enabled = false;
        }
        GetComponent<Outline>().enabled = true;
    }
}
