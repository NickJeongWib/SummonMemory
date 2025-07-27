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

        // �κ��丮 ����
        if (InventoryUI_Ref.Get_EquipmentSlot_List.Count < UserInfo.Equip_Inventory.Count + Gacha_Count)
        {
            Inventory_FullInfo_Panel.SetActive(true);
            return;
        }

        if (!GameManager.Instance.TestMode)
        {
            if (UserInfo.InventoryDict.ContainsKey("��� Ƽ��") == false)
            {
                Equip_GachaFail_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num)}</color>�� �����մϴ�.";
                return;
            }


            if (UserInfo.InventoryDict["��� Ƽ��"].Get_Amount < _num)
            {
                Equip_GachaFail_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num - UserInfo.InventoryDict["��� Ƽ��"].Get_Amount)}</color>�� �����մϴ�.";
                return;
            }

            UserTicket.text = $"<color=orange>{UserInfo.InventoryDict["��� Ƽ��"].Get_Amount}</color> <sprite=0> " +
                $"<color=red>{UserInfo.InventoryDict["��� Ƽ��"].Get_Amount - _num}</color>";
        }


        Equip_GachaInfo_Panel.SetActive(true);
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
        newItem.Image_Set(rand.Get_Item_Image, rand.Get_ItemImage_Path);
        // ������ ���� �ɼ� �ɷ�ġ �Ѱ��ֱ�
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
            // Ƽ�� ����, ���� �ʱ�ȭ
            UserInfo.InventoryDict["��� Ƽ��"].Get_Amount -= Gacha_Count;
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
        // ���� ���� �ʱ� �ڿ������� ��ȯ�� ���� UI�� Ȱ��ȭ
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

    // ���� �ݱ�
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
        if (UserInfo.InventoryDict.ContainsKey("��� Ƽ��"))
        {
            User_Ticket_Amount.text = $"{UserInfo.InventoryDict["��� Ƽ��"].Get_Amount}";
        }
        else
        {
            User_Ticket_Amount.text = "0";
        }
    }
    #endregion
}
