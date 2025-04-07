using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_Slot : MonoBehaviour
{
    [SerializeField] Mask Slot_Mask;
    [SerializeField] Image Item_Image;

    public void Set_Image(Sprite _sprite)
    {
        Item_Image.sprite = _sprite;
        Slot_Mask.showMaskGraphic = true;
    }

    public void Off_Image()
    {
        Slot_Mask.showMaskGraphic = false;
    }
}
