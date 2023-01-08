using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    // Highlight Info
    public Image selectItemImage;
    public TextMeshProUGUI selectItemAttributeText;
    public TextMeshProUGUI selectItemGeneralInfo;

    public void DisplayItemInfo(Item item)
    {
        selectItemImage.sprite = item.GetSprite();
        selectItemAttributeText.text = item.DisplayAttribute();
        selectItemGeneralInfo.text = item.DisplayGeneralInfo();
    }
}
