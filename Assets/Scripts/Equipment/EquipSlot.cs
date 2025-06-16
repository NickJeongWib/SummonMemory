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

    [SerializeField] Text Item_Lv;
    [SerializeField] Mask GradeMask;
    [SerializeField] Image GradeColor;
    [SerializeField] Mask ItemMask;
    [SerializeField] Image ItemIcon;
    [SerializeField] Mask EquipMask;

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

            Item_Lv.text = $"+{_item.Get_Item_Lv}";

            if (SlotItemInfo.Get_OwnCharacter == null)
            {
                EquipMask.showMaskGraphic = false;
            }
            else
            {
                EquipMask.showMaskGraphic = true;
            }

            // InventoryUI_Ref null이 아니라면
            if (InventoryUI_Ref != null)
                GradeColor.color = InventoryUI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
        }
    }

    public void ActiveF_UI()
    {
        // 슬롯 정보 아이템 정보가 없다면 이미지 ui들을 꺼준다
        ItemMask.showMaskGraphic = false;
        GradeMask.showMaskGraphic = false;
        EquipMask.showMaskGraphic = false;
        Item_Lv.text = "";
    }

    // 아이템 장착 
    public void On_Click_EquipBtn()
    {

        #region Test
        //for (int i = 0; i < GameManager.Instance.Get_SelectChar.Get_EquipItems.Length; i++)
        //{
        //    if (GameManager.Instance.Get_SelectChar.Get_EquipItems[i] == null)
        //        continue;

        //    Debug.Log(GameManager.Instance.Get_SelectChar.Get_EquipItems[i].Get_Item_Name);
        //}
        #endregion
    }


    public void On_Click_OpenItemInfo()
    {
        if (SlotItemInfo == null)
            return;

        InventoryUI_Ref.Item_Info.Get_CurrentItem = SlotItemInfo;
        InventoryUI_Ref.Item_Info.gameObject.SetActive(true);
        InventoryUI_Ref.Item_Info.Open_Equip_Info(SlotItemInfo);
        InventoryUI_Ref.Item_Info.Get_Decomposition_Btn.SetActive(false);

        if (GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] == SlotItemInfo)
        {
            InventoryUI_Ref.Item_Info.Set_ChangeBtn(false);
        }

        if (GameManager.Instance.Get_SelectChar != SlotItemInfo.Get_OwnCharacter)
        {
            InventoryUI_Ref.Item_Info.Set_ChangeBtn(false);
            InventoryUI_Ref.Item_Info.Set_EquipBtn(true);
        }
    }
}
