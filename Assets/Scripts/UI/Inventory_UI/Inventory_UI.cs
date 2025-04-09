
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
    [SerializeField] ScrollRect Cook_Inventory_Slot;

    [Header("Spend_Item_UI")]
    [SerializeField] GameObject Spend_Btn_PopUp;
    [SerializeField] GameObject Spend_Select_Shine_BG;

    [Header("Equip_Item_UI")]
    [SerializeField] GameObject Equip_Btn_PopUp;
    [SerializeField] GameObject Equip_Select_Shine_BG;

    [Header("Cook_Item_UI")]
    [SerializeField] GameObject Cook_Btn_PopUp;
    [SerializeField] GameObject Cook_Select_Shine_BG;

    [Header("Item_Info_Panel")]
    [SerializeField] Item_Info_Panel Item_Info;
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
            Cook_Inventory_Slot.gameObject.SetActive(false);

            Spend_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Btn_PopUp.activeSelf == false && Spend_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(true);
            Equip_Btn_PopUp.SetActive(false);
            Cook_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Select_Shine_BG.activeSelf == false && Spend_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(true);
            Equip_Select_Shine_BG.SetActive(false);
            Cook_Select_Shine_BG.SetActive(false);
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
            Cook_Inventory_Slot.gameObject.SetActive(false);

            Equip_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Equip_Btn_PopUp.activeSelf == false && Equip_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Equip_Btn_PopUp.SetActive(true);
            Cook_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Equip_Select_Shine_BG.activeSelf == false && Equip_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Equip_Select_Shine_BG.SetActive(true);
            Cook_Select_Shine_BG.SetActive(false);
        }
    }
    #endregion

    #region Cook_Item_Inventory_Click
    public void On_Click_Cook_Item_Btn()
    {
        Inventory_Type = INVENTORY_TYPE.COOK;

        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Cook_Inventory_Slot.gameObject.activeSelf == false && Cook_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.gameObject.SetActive(false);
            Equip_Inventory_Slot.gameObject.SetActive(false);
            Cook_Inventory_Slot.gameObject.SetActive(true);

            Cook_Inventory_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Cook_Btn_PopUp.activeSelf == false && Cook_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Equip_Btn_PopUp.SetActive(false);
            Cook_Btn_PopUp.SetActive(true);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Cook_Select_Shine_BG.activeSelf == false && Cook_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Equip_Select_Shine_BG.SetActive(false);
            Cook_Select_Shine_BG.SetActive(true);
        }
    }
    #endregion

    public void On_Click_Open_ItemInfo(int _slotNum)
    {
        // ��� �� ������ Ŭ�������� return
        if ((UserInfo.Equip_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.EQUIPMENT) ||
            (UserInfo.Spend_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.SPEND) ||
            (UserInfo.Cook_Inventory.Count <= _slotNum && Inventory_Type == INVENTORY_TYPE.COOK))
            return;

        Item_Info.ItemInfo_Refresh(Inventory_Type, _slotNum);
        Item_Info.gameObject.SetActive(true);
    }
}
