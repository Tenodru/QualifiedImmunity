using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entry", menuName = "Entry")]
public class EntryInfo : ScriptableObject
{
    public string entryName;
    [TextArea(1, 5)]
    public string entryDescription;
    public Sprite entrySprite;
    public bool persist = false;
}
