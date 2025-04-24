using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using static Define;

public class Equipment_Gacha_Manager : MonoBehaviour
{
    int Gacha_Count;

    [SerializeField] GameObject Equip_GachaInfo_Panel;
    [SerializeField] Inventory_UI Inventory_UI_Ref;
    // [SerializeField] List<Equipment_Slot> EquipmentSlot_List;

    #region EquipGacha_Info_Pop
    public void EquipGachaInfo_Open(int _num)
    {
        Gacha_Count = _num;
        Equip_GachaInfo_Panel.SetActive(true);
    }

    public void EquipGachaInfo_Close()
    {
        Gacha_Count = 0;
        Equip_GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
    }
    #endregion

    #region Equip_Gacha
    // ������ �������� �Ѱ���
    Item EquipItem_Spawn()
    {
        Item rand = Item_List.Equipment_Item_List[Random.Range(0, Item_List.Equipment_Item_List.Count)];
        
        // ����Ʈ���� ���� ĳ������ ���� �ɷ�ġ�� �Ѱ��� ����Ʈ���� �̾ƾ��� ���� refŸ�� ������ �ɷ�ġ�� ������
        Item newItem = new Item(rand.Get_Item_ID, rand.Get_Item_Name, rand.Get_Item_Atk, rand.Get_Item_DEF, 
            rand.Get_Item_CRI_RATE, rand.Get_Item_CRI_DMG, rand.Get_Item_HP, rand.Get_ValueMinRange, rand.Get_ValueMaxRange,
            rand.Get_ItemType, rand.Get_EquipType);
        // �̹��� �Ѱ��ֱ�
        newItem.Image_Set(rand.Get_Item_Image);
        newItem.Spawn_Grading();

        if (newItem.Get_EquipType == EQUIP_TYPE.WEAPON)
            UserInfo.Weapon_Equipment.Add(newItem);
        else if (newItem.Get_EquipType == EQUIP_TYPE.HELMET)
            UserInfo.Helmet_Equipment.Add(newItem);
        else if (newItem.Get_EquipType == EQUIP_TYPE.UPPER)
            UserInfo.Upper_Equipment.Add(newItem);
        else if (newItem.Get_EquipType == EQUIP_TYPE.ACCESSORY)
            UserInfo.Accessory_Equipment.Add(newItem);
        else
            UserInfo.Glove_Equipment.Add(newItem);

        return newItem;
    }

    public void Equip_Gacha()
    {
        if (Inventory_UI_Ref.Get_EquipmentSlot_List.Count < UserInfo.Equip_Inventory.Count + Gacha_Count)
        {
            Debug.LogWarning("�κ��丮 ����");
            return;
        }    

        for (int i = 0; i < Gacha_Count; i++)
        {
            UserInfo.Equip_Inventory.Add(EquipItem_Spawn());
        }

        Refresh_Equipment_Slot();

        EquipGachaInfo_Close();
    }

    void Refresh_Equipment_Slot()
    {
        int count = 0;

        for (int i = 0; i < UserInfo.Equip_Inventory.Count; i++)
        {
            count = i;
            Inventory_UI_Ref.Get_EquipmentSlot_List[i].Set_Image(UserInfo.Equip_Inventory[i].Get_Item_Image, UserInfo.Equip_Inventory[i].Get_Equipment_Grade);
        }

        for (int i = count + 1; i < Inventory_UI_Ref.Get_EquipmentSlot_List.Count; i++)
        {
            Inventory_UI_Ref.Get_EquipmentSlot_List[i].Off_Image();
        }
    }
    #endregion
}
