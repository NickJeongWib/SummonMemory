using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Slot : MonoBehaviour
{
    public int SlotNum;

    [SerializeField] Inventory_Item InventoryItem_Info;
    [SerializeField] Mask Item_Mask;
    [SerializeField] Image Item_Image;
    [SerializeField] Text Amount_Text;
   

    public void Set_Info(Inventory_Item _itemInfo)
    {
        InventoryItem_Info = _itemInfo;

        if (InventoryItem_Info != null)
        {
            Item_Image.sprite = InventoryItem_Info.Get_Item_Image;
            Amount_Text.text = $"{InventoryItem_Info.Get_Amount.ToString("N0")}";
            Item_Mask.showMaskGraphic = true;
        }
    }

    public void Off_Image()
    {
        Item_Mask.showMaskGraphic = false;
    }
}
