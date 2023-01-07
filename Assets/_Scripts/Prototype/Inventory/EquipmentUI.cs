using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public Image headgearImage;
    public Image armorImage;
    public Image spellCardImage;
    public Image weaponImage;

    public TextMeshProUGUI playerStat;
    public TextMeshProUGUI meleeStat;
    public TextMeshProUGUI gunStat;

    public void updateUI(ItemType type, Item item)
    {
        Sprite sprite = item.prefabs.GetComponent<SpriteRenderer>().sprite;
        if (type == ItemType.MeleeWeapon || type == ItemType.Gun)
        {
            weaponImage.sprite = sprite;
        } else if (type == ItemType.SpellCard)
        {
            spellCardImage.sprite = sprite;
        } else if(type == ItemType.Headgear)
        {
            headgearImage.sprite = sprite;
        } else if (type == ItemType.Armor)
        {
            armorImage.sprite = sprite;
        }
    }
}
