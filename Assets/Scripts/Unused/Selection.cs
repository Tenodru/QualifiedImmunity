using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum itemType { Nose, Base, Wing, Thrusters, Gun, None }

public class Selection : MonoBehaviour
{
    public bool selected = false; //is an item being held with left click?
    public EquippableItem selectedEquippableItem; 
    public Sprite selectedSprite; //the Sprite of the item held
    public Sprite selectedGearIcon; //the Icon of the item held
    public string selectedItem; //the name of the item held
    public itemType selectedType = itemType.None; //the type of item of the item held

    public Sprite blank; //used to hide selection when not used

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //when in use, spawns top right of mouse cursor
        pos = Camera.main.WorldToViewportPoint(pos);
        pos.x = Mathf.Clamp(pos.x, 0, .945f);
        pos.y = Mathf.Clamp(pos.y, .1f, 1);
        pos.z = Camera.main.nearClipPlane;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void SelectItem(EquippableItem equippableItem, Sprite gearSprite, Sprite gearIcon, string item, itemType type) //pick up an item out of a slot
    {
        selected = true;
        selectedEquippableItem = equippableItem;
        selectedSprite = gearSprite;
        selectedGearIcon = gearIcon;
        selectedItem = item;
        selectedType = type;
        GetComponent<Image>().sprite = gearIcon; //display icon on selection
        gameObject.SetActive(true); //selection icon is now active
    }

    public void DeselectItem()
    {
        selected = false;
        selectedEquippableItem = null;
        selectedSprite = null;
        selectedGearIcon = null;
        selectedItem = null;
        selectedType = itemType.None;
        GetComponent<Image>().sprite = blank; //hide selection
        transform.position = new Vector2(-472, -177); //put selection offscreen
        gameObject.SetActive(false);
    }
}
