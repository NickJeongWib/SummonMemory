using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Equipment : MonoBehaviour
{
    [SerializeField] Inventory_UI InventoryUI_Ref;
    Item EquipItem;
    public Item Get_EquipItem { get => EquipItem; set => EquipItem = value; }

    [SerializeField] Image ItemIcon;
    [SerializeField] Image ItemGradeColor;
    [SerializeField] Mask ItemGradeMask;


    public void On_Click_OpenEquipSlot(int _num)
    {
        if(GameManager.Instance.Get_SelectChar.Get_EquipItems[_num] == null)
        {
            InventoryUI_Ref.EquipSlots.SetActive(true);
        }
        else
        {
            // 동일 타입의 장비를 착용하고 있다면
            InventoryUI_Ref.Item_Info.gameObject.SetActive(true);
            InventoryUI_Ref.Item_Info.Get_CurrentItem = GameManager.Instance.Get_SelectChar.Get_EquipItems[_num];
            InventoryUI_Ref.Item_Info.Open_Equip_Info(GameManager.Instance.Get_SelectChar.Get_EquipItems[_num], true);
        }

        switch (_num)
        {
            case 0:
                On_WeaponList();
                break;
            case 1:
                On_HelmetList();
                break;
            case 2:
                On_UpperList();
                break;
            case 3:
                On_AccessoryList();
                break;
            case 4:
                On_GloveList();
                break;
            default:
                break;
        }
    }

    #region WeaponUI_Refresh
    void On_WeaponList()
    {
        int num = -1;

        // 착용중인 아이템은 뒤로 정렬
        UserInfo.Weapon_Equipment.Sort((a, b) => a.Get_isEquip.CompareTo(b.Get_isEquip));

        for (int i = 0; i < UserInfo.Weapon_Equipment.Count; i++)
        {
            num = i;
            // Debug.Log(UserInfo.Weapon_Equipment[i].Get_Item_Name);
            InventoryUI_Ref.Get_EquipSlot_List[i].Init_UI(UserInfo.Weapon_Equipment[i]);
        }
        num++;

        for (int i = num; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipSlot_List[i].ActiveF_UI();
        }
    }
    #endregion

    #region Helmet_Refresh
    void On_HelmetList()
    {
        int num = -1;

        // 착용중인 아이템은 뒤로 정렬
        UserInfo.Helmet_Equipment.Sort((a, b) => a.Get_isEquip.CompareTo(b.Get_isEquip));

        for (int i = 0; i < UserInfo.Helmet_Equipment.Count; i++)
        {
            num = i;
            InventoryUI_Ref.Get_EquipSlot_List[i].Init_UI(UserInfo.Helmet_Equipment[i]);
        }
        num++;

        for (int i = num; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipSlot_List[i].ActiveF_UI();
        }
    }
    #endregion

    #region Upper_Refresh
    void On_UpperList()
    {
        int num = -1;

        // 착용중인 아이템은 뒤로 정렬
        UserInfo.Upper_Equipment.Sort((a, b) => a.Get_isEquip.CompareTo(b.Get_isEquip));

        for (int i = 0; i < UserInfo.Upper_Equipment.Count; i++)
        {
            num = i;
            InventoryUI_Ref.Get_EquipSlot_List[i].Init_UI(UserInfo.Upper_Equipment[i]);
        }
        num++;

        for (int i = num; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipSlot_List[i].ActiveF_UI();
        }
    }
    #endregion

    #region Accessory_Refresh
    void On_AccessoryList()
    {
        int num = -1;

        // 착용중인 아이템은 뒤로 정렬
        UserInfo.Accessory_Equipment.Sort((a, b) => a.Get_isEquip.CompareTo(b.Get_isEquip));

        for (int i = 0; i < UserInfo.Accessory_Equipment.Count; i++)
        {
            num = i;
            InventoryUI_Ref.Get_EquipSlot_List[i].Init_UI(UserInfo.Accessory_Equipment[i]);
        }
        num++;

        for (int i = num; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipSlot_List[i].ActiveF_UI();
        }
    }
    #endregion

    #region Glove_Refresh
    void On_GloveList()
    {
        int num = -1;

        // 착용중인 아이템은 뒤로 정렬
        UserInfo.Glove_Equipment.Sort((a, b) => a.Get_isEquip.CompareTo(b.Get_isEquip));

        for (int i = 0; i < UserInfo.Glove_Equipment.Count; i++)
        {
            num = i;
            InventoryUI_Ref.Get_EquipSlot_List[i].Init_UI(UserInfo.Glove_Equipment[i]);
        }
        num++;

        for (int i = num; i < InventoryUI_Ref.Get_EquipSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipSlot_List[i].ActiveF_UI();
        }
    }
    #endregion

    #region ItemGrade_Image
    // 장착 아이템 이미지 설정
    public void Refresh_EquipItem(Sprite _sprite, bool _bool, Item _item)
    {
        ItemIcon.sprite = _sprite;
        ItemGradeMask.showMaskGraphic = _bool;

        if (_item != null)
            ItemGradeColor.color = InventoryUI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
    }
    #endregion

    public void On_Click_ChangeEquip()
    {
        InventoryUI_Ref.Item_Info.gameObject.SetActive(false);
        InventoryUI_Ref.EquipSlots.SetActive(true);
    }
}
