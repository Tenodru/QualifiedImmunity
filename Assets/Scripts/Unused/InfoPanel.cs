using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public Text itemName;
    public Text itemType;
    public Text stats;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //when in use, spawns top right of mouse cursor
        pos = Camera.main.WorldToViewportPoint(pos);
        //pos.x = Mathf.Clamp(pos.x, 0, 1);
        pos.y = Mathf.Clamp(pos.y, 0, .78f);
        pos.z = Camera.main.nearClipPlane;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }


    public void DisplayInfo(string _itemName, itemType _itemType, itemRarity _itemRarity, int agilityStat, int enduranceStat)
    {
        gameObject.SetActive(true);
        itemName.text = _itemName;

        if (_itemRarity == itemRarity.Common)
        {
            itemName.color = Color.gray;
        }
        else if (_itemRarity == itemRarity.Rare)
        {
            itemName.color = Color.blue;
        }
        else if (_itemRarity == itemRarity.Epic)
        {
            itemName.color = new Color(0.65f, 0, 1);
        }
        else if (_itemRarity == itemRarity.Legendary)
        {
            itemName.color = new Color(1, 0.65f, 0);
        }

        itemType.text = _itemType.ToString();
        stats.text = "+ " + agilityStat + " agility" + "\n" + "+ " + enduranceStat + " endurance";
    }

    public void HideInfo()
    {
        transform.position = new Vector2(-605.5f, -224.9f); //put info panel offscreen
        itemName.text = null;
        itemType.text = null;
        stats.text = null;
        gameObject.SetActive(false);
    }
}
