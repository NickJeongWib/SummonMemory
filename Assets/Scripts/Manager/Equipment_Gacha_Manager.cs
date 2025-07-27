using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using static Define;

public class Equipment_Gacha_Manager : MonoBehaviour
{
    int Gacha_Count;
    [SerializeField] Inventory_UI InventoryUI_Ref;
    // [SerializeField] List<Equipment_Slot> EquipmentSlot_List;

    [Header("UI_Panel")]
    [SerializeField] GameObject GachaEnter_Transition;
    [SerializeField] GameObject Inventory_FullInfo_Panel;
    [SerializeField] GameObject Equip_GachaInfo_Panel;
    [SerializeField] GameObject Equip_GachaFail_Panel;

    [Header("Text")]
    [SerializeField] Text User_Ticket_Amount;
    [SerializeField] Text FailInfoCount;
    [SerializeField] TextMeshProUGUI UserTicket;

    [Header("----Gacha_Video----")]
    [SerializeField] GameObject Gacha_Video;
    [SerializeField] VideoClip Gacha_Scenes;
    [SerializeField] VideoPlayer Videoplayer;

    private void Start()
    {
        Refresh_Equipment_Slot();
    }

    #region EquipGacha_Info_Pop
    public void EquipGachaInfo_Open(int _num)
    {
        Gacha_Count = _num;

        // 인벤토리 부족
        if (InventoryUI_Ref.Get_EquipmentSlot_List.Count < UserInfo.Equip_Inventory.Count + Gacha_Count)
        {
            Inventory_FullInfo_Panel.SetActive(true);
            return;
        }

        if (!GameManager.Instance.TestMode)
        {
            if (UserInfo.InventoryDict.ContainsKey("장비 티켓") == false)
            {
                Equip_GachaFail_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num)}</color>개 부족합니다.";
                return;
            }


            if (UserInfo.InventoryDict["장비 티켓"].Get_Amount < _num)
            {
                Equip_GachaFail_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num - UserInfo.InventoryDict["장비 티켓"].Get_Amount)}</color>개 부족합니다.";
                return;
            }

            UserTicket.text = $"<color=orange>{UserInfo.InventoryDict["장비 티켓"].Get_Amount}</color> <sprite=0> " +
                $"<color=red>{UserInfo.InventoryDict["장비 티켓"].Get_Amount - _num}</color>";
        }


        Equip_GachaInfo_Panel.SetActive(true);
    }
    #endregion

    #region Equip_Gacha
    // 아이템 랜덤으로 넘겨줌
    Item EquipItem_Spawn()
    {
        Item rand = Item_List.Equipment_Item_List[Random.Range(0, Item_List.Equipment_Item_List.Count)];
        
        // 리스트에서 뽑은 캐릭터의 기초 능력치를 넘겨줌 리스트에서 뽑아쓰면 같은 ref타입 때문에 능력치가 같아짐
        Item newItem = new Item(rand.Get_Item_ID, rand.Get_Item_Name, rand.Get_Item_Atk, rand.Get_Item_DEF, 
            rand.Get_Item_CRI_RATE, rand.Get_Item_CRI_DMG, rand.Get_Item_HP, rand.Get_ValueMinRange, rand.Get_ValueMaxRange,
            rand.Get_ItemType, rand.Get_EquipType);
        // 이미지 넘겨주기
        newItem.Image_Set(rand.Get_Item_Image, rand.Get_ItemImage_Path);
        // 아이템 랜덤 옵션 능력치 넘겨주기
        // newItem.Set_OptionList(rand.UserInfo.OptionList);

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
        // TODO ## Gacha_Manager : TestMode
        if (!GameManager.Instance.TestMode)
        {
            // 티켓 감소, 슬롯 초기화
            UserInfo.InventoryDict["장비 티켓"].Get_Amount -= Gacha_Count;
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Spend_Inventory();
            InventoryUI_Ref.Spend_Slot_Refresh();
        }

        for (int i = 0; i < Gacha_Count; i++)
        {
            UserInfo.Equip_Inventory.Add(EquipItem_Spawn());
        }

        Refresh_Equipment_Slot();
        Refresh_EquipTicket();

        Info_Close(Equip_GachaInfo_Panel);

        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        Gacha_Video_Play();
    }

    #region Gacha_Video

    void Gacha_Video_Play()
    {
        // 영상 실행 초기 자연스러운 전환을 위한 UI들 활성화
        GachaEnter_Transition.SetActive(true);
        Gacha_Video.SetActive(true);
        Videoplayer.clip = Gacha_Scenes;
        Videoplayer.Play();
    }
    #endregion

    public void Refresh_Equipment_Slot()
    {
        int count = 0;

        for (int i = 0; i < UserInfo.Equip_Inventory.Count; i++)
        {
            count = i;
            InventoryUI_Ref.Get_EquipmentSlot_List[i].Set_Image(UserInfo.Equip_Inventory[i].Get_Item_Image, UserInfo.Equip_Inventory[i].Get_Equipment_Grade);
        }

        for (int i = count + 1; i < InventoryUI_Ref.Get_EquipmentSlot_List.Count; i++)
        {
            InventoryUI_Ref.Get_EquipmentSlot_List[i].Off_Image();
        }
    }

    // 인포 닫기
    #region Error_Info_Close
    public void Info_Close(GameObject _obj)
    {
        Gacha_Count = 0;
        _obj.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
    }
    #endregion

    #endregion

    #region UI
    public void Refresh_EquipTicket()
    {
        if (UserInfo.InventoryDict.ContainsKey("장비 티켓"))
        {
            User_Ticket_Amount.text = $"{UserInfo.InventoryDict["장비 티켓"].Get_Amount}";
        }
        else
        {
            User_Ticket_Amount.text = "0";
        }
    }
    #endregion
}
