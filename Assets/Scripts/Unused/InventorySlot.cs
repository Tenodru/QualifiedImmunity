using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public EquippableItem equippableItem;

    public Selection selection;
    public InfoPanel infoPanel;
    private Sprite gearSprite; //sprite of the item
    private Sprite gearIcon; //icon of the item
    private string item; //the name of the item
    private itemType type = itemType.None; //type of item

    // Start is called before the first frame update
    void Start()
    {
        if (equippableItem != null) //if there is an item in this slot, get its info
        {
            gearSprite = equippableItem.gearSprite;
            gearIcon = equippableItem.gearIcon;
            GetComponent<Image>().sprite = gearIcon;
            item = equippableItem.itemName;
            type = equippableItem.gearType;
        }
        else //set everything to empty
        {
            gearSprite = null;
            gearIcon = null;
            GetComponent<Image>().sprite = gearIcon;
            item = null;
            type = itemType.None;
        }
    }

    public void ClickSlot()
    {
        if (selection.selected)//if player is holding something
        {
            if (type != itemType.None) //if the slot has an item in it, swap the items
            {
                EquippableItem tempEquippableItem = selection.selectedEquippableItem;
                Sprite tempSelectedSprite = selection.selectedSprite;
                Sprite tempSelectedGearIcon = selection.selectedGearIcon;
                string tempSelectedItem = selection.selectedItem;
                itemType tempSelectedType = selection.selectedType;

                selection.SelectItem(equippableItem, gearSprite, gearIcon, item, type);

                equippableItem = tempEquippableItem;
                gearSprite = tempSelectedSprite;
                gearIcon = tempSelectedGearIcon;
                GetComponent<Image>().sprite = gearIcon;
                item = tempSelectedItem;
                type = tempSelectedType;
            }
            else //if the slot does not have an item in it
            {
                PlaceItemInSlot();
                selection.DeselectItem();
                DisplayItemInfo();
            }
        }
        else //if player is not holding something
        {
            if (type != itemType.None) //if the slot has an item in it, pick up the item
            {
                selection.SelectItem(equippableItem, gearSprite, gearIcon, item, type);
                ClearSlot();
                infoPanel.HideInfo();
            }
        }
    }

    public void PlaceItemInSlot()
    {
        equippableItem = selection.selectedEquippableItem;
        gearSprite = selection.selectedSprite;
        gearIcon = selection.selectedGearIcon;
        GetComponent<Image>().sprite = gearIcon;
        item = selection.selectedItem;
        type = selection.selectedType;
    }

    public void ClearSlot()
    {
        equippableItem = null;
        gearSprite = null;
        gearIcon = null;
        item = null;
        type = itemType.None;
        GetComponent<Image>().sprite = gearIcon;
    }

    public void DisplayItemInfo()
    {
        if (equippableItem != null && !selection.selected)
        {
            infoPanel.DisplayInfo(equippableItem.itemName, equippableItem.gearType, equippableItem.itemRarity, equippableItem.agilityStat, equippableItem.enduranceStat);
        }
        else
        {

        }
    }
}
