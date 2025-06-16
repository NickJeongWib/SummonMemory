
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Inventory_UI : MonoBehaviour
{
    [SerializeField] INVENTORY_TYPE Inventory_Type;

    //---- Var
    #region Panel_UI_GameObject_Var
    // �Һ�, ��� �κ��丮 ���ӿ�����Ʈ
    [SerializeField] ScrollRect Spend_Inventory_Slot;
    [SerializeField] ScrollRect Equip_Inventory_Slot;
    [SerializeField] ScrollRect Upgrade_Inventory_Slot;

    [Header("Upgrade_Item_UI")]
    [SerializeField] int UpgradeSlotCount;
    public int Get_UpgradeSlotCount { get => UpgradeSlotCount; }
    [SerializeField] GameObject Upgrade_Btn_PopUp;
    [SerializeField] GameObject Upgrade_Select_Shine_BG;
    [SerializeField] List<Upgrade_Slot> UpgradeSlot_List = new List<Upgrade_Slot>();
    public List<Upgrade_Slot> Get_UpgradeSlot_List { get => UpgradeSlot_List; }

    [Header("Spend_Item_UI")]
    [SerializeField] int SpendSlotCount;
    public int Get_SpendSlotCount { get => SpendSlotCount; }

    [SerializeField] GameObject Spend_Btn_PopUp;
    [SerializeField] GameObject Spend_Select_Shine_BG;
    [SerializeField] List<Spend_Slot> SpendSlot_List = new List<Spend_Slot>();
    public List<Spend_Slot> Get_SpendSlot_List { get => SpendSlot_List; }

    [Header("Equip_Item_UI")]
    public GameObject EquipSlots;
    [SerializeField] Color[] Grade_Colors;
    public Color[] Get_Colors { get => Grade_Colors; }
 
    [SerializeField] int EquipmentCount;
    public int Get_EquipmentCount { get => EquipmentCount; }
    [SerializeField] GameObject Equip_Btn_PopUp;
    [SerializeField] GameObject Equip_Select_Shine_BG;
    [SerializeField] List<Equipment_Slot> EquipmentSlot_List = new List<Equipment_Slot>();
    public List<Equipment_Slot> Get_EquipmentSlot_List { get => EquipmentSlot_List; }

    /// <summary>
    /// if Character Info Scene click equip ItemType Button
    /// ĳ���� ����â���� ������� ��ư�� Ŭ�� �� ��Ÿ���� �� ������ ����
    /// </summary>
    [SerializeField] List<EquipSlot> EquipSlot_List = new List<EquipSlot>();
    public List<EquipSlot> Get_EquipSlot_List { get => EquipSlot_List; }

    [Header("Item_Info_Panel")]
    public Item_Info_Panel Item_Info;
    #endregion

    //---- Function

    #region Spend_Item_Inventory_Click
    public void On_Click_Spend_Item_Btn()
    {
        Inventory_Type = INVENTORY_TYPE.SPEND;

        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Inventory_Slot.gameObject.activeSelf == false && Spend_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.gameObject.SetActive(true);
            Equip_Inventory_Slot.gameObject.SetActive(false);
            Upgrade_Inventory_Slot.gameObject.SetActive(false);

            Spend_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Btn_PopUp.activeSelf == false && Spend_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(true);
            Equip_Btn_PopUp.SetActive(false);
            Upgrade_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Select_Shine_BG.activeSelf == false && Spend_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(true);
            Equip_Select_Shine_BG.SetActive(false);
            Upgrade_Select_Shine_BG.SetActive(false);
        }
    }
    #endregion

    #region Equip_Item_Inventory_Click
    public void On_Click_Equip_Item_Btn()
    {
        Inventory_Type = INVENTORY_TYPE.EQUIPMENT;

        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Equip_Inventory_Slot.gameObject.activeSelf == false && Equip_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.gameObject.SetActive(false);
            Equip_Inventory_Slot.gameObject.SetActive(true);
            Upgrade_Inventory_Slot.gameObject.SetActive(false);

            Equip_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Equip_Btn_PopUp.activeSelf == false && Equip_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Equip_Btn_PopUp.SetActive(true);
            Upgrade_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Equip_Select_Shine_BG.activeSelf == false && Equip_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Equip_Select_Shine_BG.SetActive(true);
            Upgrade_Select_Shine_BG.SetActive(false);
        }
    }
    #endregion

    #region Upgrade_Item_Inventory_Click
    public void On_Click_Upgrade_Item_Btn()
    {
        Inventory_Type = INVENTORY_TYPE.UPGRADE;

        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Upgrade_Inventory_Slot.gameObject.activeSelf == false && Upgrade_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.gameObject.SetActive(false);
            Equip_Inventory_Slot.gameObject.SetActive(false);
            Upgrade_Inventory_Slot.gameObject.SetActive(true);

            Upgrade_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Upgrade_Btn_PopUp.activeSelf == false && Upgrade_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Equip_Btn_PopUp.SetActive(false);
            Upgrade_Btn_PopUp.SetActive(true);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Upgrade_Select_Shine_BG.activeSelf == false && Upgrade_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Equip_Select_Shine_BG.SetActive(false);
            Upgrade_Select_Shine_BG.SetActive(true);
        }
    }
    #endregion

    #region Slot_Refresh
    public void Reset_Spend_Inventory()
    {
        for (int i = 0; i < SpendSlot_List.Count; i++)
        {
            SpendSlot_List[i].Reset_Info();
        }
    }

    public void Reset_Upgrade_Inventory()
    {
        for (int i = 0; i < SpendSlot_List.Count; i++)
        {
            UpgradeSlot_List[i].Reset_Info();
        }
    }

    public void Spend_Slot_Refresh()
    {
        int index = 0;

        for (int i = 0; i < UserInfo.Spend_Inventory.Count; i++)
        {
            // �������� �������� ���� ��� 
            if (UserInfo.Spend_Inventory[i].Get_Amount <= 0)
            {
                continue;
            }

            // �������� ���� �� ���
            if (UserInfo.Spend_Inventory[i].Get_Amount >= 1)
            {
                // ������ ��� �ִ� ������ ���� ����
                SpendSlot_List[index].Set_Info(UserInfo.Spend_Inventory[i]);
                // ���� ���� ����
                index++;
            }
        }
    }

    public void Upgrade_Slot_Refresh()
    {
        int index = 0;
        for (int i = 0; i < UserInfo.Upgrade_Inventory.Count; i++)
        {
            // �������� �������� ���� ��� 
            if (UserInfo.Upgrade_Inventory[i].Get_Amount <= 0)
            {
                continue;
            }

            // �������� ���� �� ���
            if (UserInfo.Upgrade_Inventory[i].Get_Amount >= 1)
            {
                // ������ ��� �ִ� ������ ���� ����
                UpgradeSlot_List[index].Set_Info(UserInfo.Upgrade_Inventory[i]);
                // ���� ���� ����
                index++;
            }
        }
    }
    #endregion

    #region ItemInfo_Panel
    public void On_Click_Open_ItemInfo(int _slotNum)
    {
        // ��� �� ������ Ŭ�������� return
        if ((UserInfo.Equip_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.EQUIPMENT) ||
            (UserInfo.Spend_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.SPEND) ||
            (UserInfo.Upgrade_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.UPGRADE))
            return;

        // ������ ���� ǥ��
        Item_Info.ItemInfo_Refresh(Inventory_Type, _slotNum);
        Item_Info.gameObject.SetActive(true);
    }
    #endregion
}
