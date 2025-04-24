using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int SlotNum;
    Item SlotItemInfo;
    public Item Get_SlotItemInfo { get => SlotItemInfo; set => SlotItemInfo = value; }

    Inventory_UI InventoryUI_Ref;
    public Inventory_UI Set_InventoryUI_Ref { set => InventoryUI_Ref = value; }

    CharacterList_UI CharacterListUI_Ref;
    public CharacterList_UI Set_CharacterListUI_Ref { set => CharacterListUI_Ref = value; }

    [SerializeField] Mask GradeMask;
    [SerializeField] Image GradeColor;
    [SerializeField] Mask ItemMask;
    [SerializeField] Image ItemIcon;

    public void Init_UI(Item _item)
    {
        // _item이 null이 아니라면
        if (_item != null)
        {
            SlotItemInfo = _item;
            // UI 표시
            GradeMask.showMaskGraphic = true;
            ItemMask.showMaskGraphic = true;
            ItemIcon.sprite = _item.Get_Item_Image;

            // InventoryUI_Ref null이 아니라면
            if (InventoryUI_Ref != null)
                GradeColor.color = InventoryUI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
        }
    }

    public void ActiveF_UI()
    {
        ItemMask.showMaskGraphic = false;
        GradeMask.showMaskGraphic = false;
    }

    // 아이템 장착 
    public void On_Click_EquipBtn()
    {
        InventoryUI_Ref.EquipSlots.SetActive(false);
        SlotItemInfo.Get_isEquip = true;

        if (GameManager.Instance.Get_SelectChar != null)
        {
            GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = SlotItemInfo;
        }

        if (CharacterListUI_Ref != null)
            CharacterListUI_Ref.Refresh_EquipItem_Image();

        #region Test
        //for (int i = 0; i < GameManager.Instance.Get_SelectChar.Get_EquipItems.Length; i++)
        //{
        //    if (GameManager.Instance.Get_SelectChar.Get_EquipItems[i] == null)
        //        continue;

        //    Debug.Log(GameManager.Instance.Get_SelectChar.Get_EquipItems[i].Get_Item_Name);
        //}
        #endregion
    }
}
