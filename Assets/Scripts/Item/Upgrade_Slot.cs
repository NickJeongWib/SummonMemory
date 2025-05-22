using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Slot : MonoBehaviour
{
    public int SlotNum;

    [SerializeField] Mask Item_Mask;
    [SerializeField] Image Item_Image;

    public void Set_Image(Sprite _sprite)
    {
        Item_Image.sprite = _sprite;
        Item_Mask.showMaskGraphic = true;
    }

    public void Off_Image()
    {
        Item_Mask.showMaskGraphic = false;
    }
}
