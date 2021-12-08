using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemRarity { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "New Equippable Item", menuName = "Equippable Item")]
public class EquippableItem : ScriptableObject
{
    public itemRarity itemRarity;
    public string itemName;
    public Sprite gearSprite;
    public Sprite gearIcon;
    public itemType gearType = itemType.None;
    public int agilityStat;
    public int enduranceStat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
