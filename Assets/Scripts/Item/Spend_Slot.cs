using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spend_Slot : MonoBehaviour
{
    public int SlotNum;

    Inventory_Item InventoryItem_Info;
    [SerializeField] Mask Item_Mask;
    [SerializeField] Image Item_Image;
    [SerializeField] Text Amount_Text;

    [SerializeField] Button Open_Item_Info;
    Inventory_UI Inventory_UI_Ref;
    public Inventory_UI Set_Inventory_UI { set => Inventory_UI_Ref = value; }

    private void Start()
    {
        // 버튼 호출 대기
        Open_Item_Info.onClick.AddListener(Item_Info);
    }

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

    void Item_Info()
    {
        Inventory_UI_Ref.On_Click_Open_ItemInfo(SlotNum);
    }
}
