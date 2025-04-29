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
    }

    // 아이템 장착 
    public void On_Click_EquipBtn()
    {
     
        //if (SlotItemInfo == null)
        //    return;

        //InventoryUI_Ref.EquipSlots.SetActive(false);
        //SlotItemInfo.Get_isEquip = true;

        //if (SlotItemInfo.Get_OwnCharacter != null)
        //{
        //    SlotItemInfo.Get_OwnCharacter.Refresh_Char_Equipment_State(false, SlotItemInfo.Get_EquipType);
        //    // SlotItemInfo.Get_OwnCharacter.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = null;
        //}

        //// 선택된 캐릭터가 있을 시
        //if (GameManager.Instance.Get_SelectChar != null)
        //{
        //    // 선택된 캐릭터의 현재 선택 아이템타입의 장비를 착용하고 있다면
        //    if (GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] != null)
        //    {
        //        // 장착해제
        //        GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType].Get_isEquip = false;
        //        GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(false, SlotItemInfo.Get_EquipType);
        //        // 선택된 캐릭터에서 해제
        //        // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType].Get_OwnCharacter = null;
        //    }

        //    // 장착 캐릭터 등록
        //    GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = SlotItemInfo;
        //    SlotItemInfo.Get_OwnCharacter = GameManager.Instance.Get_SelectChar;

        //    // 능력치 전달
        //    GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(true, SlotItemInfo.Get_EquipType);
        //}

        //GameManager.Instance.Get_SelectChar.TestState();

        //// 장비타입에 맞는 장비가 하나도 없을 때 다른 장비타입의 정보가 남아있는걸 방지
        //for (int i = 0; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        //{
        //    InventoryUI_Ref.Get_EquipSlot_List[i].Get_SlotItemInfo = null;
        //}

        //if (CharacterListUI_Ref != null)
        //    CharacterListUI_Ref.Refresh_EquipItem_Image();

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
    }
}
