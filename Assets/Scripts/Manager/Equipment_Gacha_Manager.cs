using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;

public class Equipment_Gacha_Manager : MonoBehaviour
{
    int Gacha_Count;

    [SerializeField] GameObject Equip_GachaInfo_Panel;
    [SerializeField] List<Equipment_Slot> EquipmentSlot_List;

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
    // 아이템 랜덤으로 넘겨줌
    Item EquipItem_Spawn()
    {
        Item rand = Item_List.Equipment_Item_List[Random.Range(0, Item_List.Equipment_Item_List.Count)];
        rand.Spawn_Grading();
        return rand;
    }

    public void Equip_Gacha()
    {
        if (EquipmentSlot_List.Count < UserInfo.Equip_Inventory.Count + Gacha_Count)
        {
            Debug.LogWarning("인벤토리 부족");
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
            EquipmentSlot_List[i].Set_Image(UserInfo.Equip_Inventory[i].Get_Item_Image);
        }

        for (int i = count + 1; i < EquipmentSlot_List.Count; i++)
        {
            EquipmentSlot_List[i].Off_Image();
        }
    }
    #endregion
}
