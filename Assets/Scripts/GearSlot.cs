using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSlot : MonoBehaviour
{
    public Selection selection;
    public InfoPanel infoPanel;
    public Sprite blank;
    public Sprite[] defaultGear; //the default sprites for no gear
    public Sprite[] defaultIcons; //the default icons for no gear
    public itemType slotType; //type of slot, can't place nose item into a base slot, for example
    [Header("Display")]
    public Image[] gearPreview; //the sprites making the preview of rocket
    public SpriteRenderer[] currentGear; //the actual sprites on the rocket player controls
    [Header("Item")]
    public EquippableItem equippableItem;
    public Sprite gearSprite; //sprite of item
    public Sprite gearIcon; //icon of item
    public string item; //name of item
    public itemType type = itemType.None; //type of item,
    [Header("Alerts")]
    public Text alertText;
    public Animator alertPopUp;

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

            foreach (Image i in gearPreview)
            {
                i.sprite = gearSprite; //display the gear preview
            }

            foreach (SpriteRenderer i in currentGear)
            {
                i.sprite = gearSprite; //display the actual gear
            }
        }
        else //set default looks
        {
            gearSprite = null;
            gearIcon = null;
            GetComponent<Image>().sprite = gearIcon;
            item = null;
            type = itemType.None;

            DisplayDefaultPreview();
            DisplayDefaultIcons();
            DisplayDefaultGear();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickSlot()
    {
        if (selection.selected)//if player is holding something
        {
            if (type != itemType.None && selection.selectedType == slotType) //if the slot has an item in it, swap the items
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

                foreach (Image i in gearPreview)
                {
                    i.sprite = gearSprite; //display gear preview
                }

                foreach (SpriteRenderer i in currentGear)
                {
                    i.sprite = gearSprite; //display actual gear
                }
            }
            else //if the slot does not have an item in it
            {
                if (selection.selectedType == slotType) //if the slot is correct
                {
                    PlaceItemInSlot();
                    selection.DeselectItem();
                    DisplayItemInfo();
                }
                else //alert player
                {
                    alertText.text = "You can't equip that item in that slot.";
                    alertPopUp.SetTrigger("Alert");
                }
            }
        }
        else //if player is not holding something
        {
            if (type != itemType.None) //if the slot has an item in it
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

        foreach (Image i in gearPreview)
        {
            i.sprite = gearSprite;
        }

        foreach (SpriteRenderer i in currentGear)
        {
            i.sprite = gearSprite;
        }
    }

    public void ClearSlot()
    {
        equippableItem = null;
        gearSprite = null;
        DisplayDefaultIcons();
        gearIcon = null;
        item = null;
        type = itemType.None;
        GetComponent<Image>().sprite = gearIcon;

        DisplayDefaultPreview();
        DisplayDefaultIcons();
        DisplayDefaultGear();
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

    public void DisplayDefaultPreview()
    {
        foreach (Image i in gearPreview)
        {
            if (slotType == itemType.Nose)
            {
                i.sprite = defaultGear[0];
            }
            if (slotType == itemType.Base)
            {
                i.sprite = defaultGear[1];
            }
            if (slotType == itemType.Wing)
            {
                i.sprite = defaultGear[2];
            }
            if (slotType == itemType.Thrusters)
            {
                i.sprite = defaultGear[3];
            }
            if (slotType == itemType.Gun)
            {
                i.sprite = defaultGear[4];
            }
        }
    }

    public void DisplayDefaultGear()
    {
        foreach (SpriteRenderer i in currentGear)
        {
            if (slotType == itemType.Nose)
            {
                i.sprite = defaultGear[0];
            }
            if (slotType == itemType.Base)
            {
                i.sprite = defaultGear[1];
            }
            if (slotType == itemType.Wing)
            {
                i.sprite = defaultGear[2];
            }
            if (slotType == itemType.Thrusters)
            {
                i.sprite = defaultGear[3];
            }
            if (slotType == itemType.Gun)
            {
                i.sprite = defaultGear[4];
            }
        }
    }

    public void DisplayDefaultIcons()
    {
        if (slotType == itemType.Nose)
        {
            GetComponent<Image>().sprite = defaultIcons[0];
        }
        if (slotType == itemType.Base)
        {
            GetComponent<Image>().sprite = defaultIcons[1];
        }
        if (slotType == itemType.Wing)
        {
            GetComponent<Image>().sprite = defaultIcons[2];
        }
        if (slotType == itemType.Thrusters)
        {
            GetComponent<Image>().sprite = defaultIcons[3];
        }
        if (slotType == itemType.Gun)
        {
            GetComponent<Image>().sprite = defaultIcons[4];
        }
    }
}

