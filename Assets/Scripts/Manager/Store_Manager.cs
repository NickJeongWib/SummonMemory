using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Manager : MonoBehaviour
{
    // �Ҹ������, ���������, ��ȯ Ƽ�� ���� ���ӿ�����Ʈ
    [Header("Spend_Store_UI")]
    [SerializeField] GameObject Spend_Store_Slot;
    [SerializeField] GameObject Spend_Btn_PopUp;
    [SerializeField] GameObject Spend_Select_Shine_BG;

    [Header("Equip_Item_UI")]
    [SerializeField] GameObject Ticket_Store_Slot;
    [SerializeField] GameObject Ticket_Btn_PopUp;
    [SerializeField] GameObject Ticket_Select_Shine_BG;

    [Header("Cook_Item_UI")]
    [SerializeField] GameObject Level_Store_Slot;
    [SerializeField] GameObject Level_Btn_PopUp;
    [SerializeField] GameObject Level_Select_Shine_BG;

    #region Spend_Store_Click
    public void On_Click_Spend_Store_Btn()
    {
        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Store_Slot.activeSelf == false && Spend_Store_Slot != null)
        {
            Spend_Store_Slot.SetActive(true);
            Ticket_Store_Slot.SetActive(false);
            Level_Store_Slot.SetActive(false);
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Btn_PopUp.activeSelf == false && Spend_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(true);
            Ticket_Btn_PopUp.SetActive(false);
            Level_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Spend_Select_Shine_BG.activeSelf == false && Spend_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(true);
            Ticket_Select_Shine_BG.SetActive(false);
            Level_Select_Shine_BG.SetActive(false);
        }
    }
    #endregion

    #region Ticket_Store_Click
    public void On_Click_Ticket_Store_Btn()
    {
        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Ticket_Store_Slot.activeSelf == false && Ticket_Store_Slot != null)
        {
            Spend_Store_Slot.SetActive(false);
            Ticket_Store_Slot.SetActive(true);
            Level_Store_Slot.SetActive(false);
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Ticket_Btn_PopUp.activeSelf == false && Ticket_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Ticket_Btn_PopUp.SetActive(true);
            Level_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Ticket_Select_Shine_BG.activeSelf == false && Ticket_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Ticket_Select_Shine_BG.SetActive(true);
            Level_Select_Shine_BG.SetActive(false);
        }
    }
    #endregion

    #region Level_Store_Click
    public void On_Click_Level_Store_Btn()
    {
        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Level_Store_Slot.activeSelf == false && Level_Store_Slot != null)
        {
            Spend_Store_Slot.SetActive(false);
            Ticket_Store_Slot.SetActive(false);
            Level_Store_Slot.SetActive(true);
        }

        // Spend_Btn_PopUp ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Level_Btn_PopUp.activeSelf == false && Level_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Ticket_Btn_PopUp.SetActive(false);
            Level_Btn_PopUp.SetActive(true);
        }

        // Spend_Select_Shine_BG ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
        if (Level_Select_Shine_BG.activeSelf == false && Level_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Ticket_Select_Shine_BG.SetActive(false);
            Level_Select_Shine_BG.SetActive(true);
        }
    }
    #endregion
}
