using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store_Manager : MonoBehaviour
{
    #region UI_Variable
    // 소모아이템, 성장아이템, 소환 티켓 상점 게임오브젝트
    [Header("Spend_Store_UI")]
    [SerializeField] GameObject Store_Spend_Panel;
    [SerializeField] ScrollRect Spend_Store_Slot;
    [SerializeField] GameObject Spend_Btn_PopUp;
    [SerializeField] GameObject Spend_Select_Shine_BG;

    [Header("Ticket_Item_UI")]
    [SerializeField] GameObject Store_Ticket_Panel;
    [SerializeField] ScrollRect Ticket_Store_Slot;
    [SerializeField] GameObject Ticket_Btn_PopUp;
    [SerializeField] GameObject Ticket_Select_Shine_BG;

    [Header("Lv_Item_UI")]
    [SerializeField] GameObject Store_Level_Panel;
    [SerializeField] ScrollRect Level_Store_Slot;
    [SerializeField] GameObject Level_Btn_PopUp;
    [SerializeField] GameObject Level_Select_Shine_BG;

    [Header("---Info---")]
    [SerializeField] GameObject Buying_Info;
    #endregion

    #region Variable
    [SerializeField] Lobby_Manager LobbyMgr_Ref;
    int Dia_SelectCount;
    #endregion
    ///---------------------------------------------------------

    #region Spend_Store_Click
    public void On_Click_Spend_Store_Btn()
    {
        // Spend_Inventory_Slot 비활성화 상태에 연결이 잘 되어있다면
        if (Store_Spend_Panel.gameObject.activeSelf == false && Store_Spend_Panel != null)
        {
            Store_Spend_Panel.gameObject.SetActive(true);
            Store_Ticket_Panel.gameObject.SetActive(false);
            Store_Level_Panel.gameObject.SetActive(false);

            Spend_Store_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp 비활성화 상태에 연결이 잘 되어있다면
        if (Spend_Btn_PopUp.activeSelf == false && Spend_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(true);
            Ticket_Btn_PopUp.SetActive(false);
            Level_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG 비활성화 상태에 연결이 잘 되어있다면
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
        // Spend_Inventory_Slot 비활성화 상태에 연결이 잘 되어있다면
        if (Store_Ticket_Panel.gameObject.activeSelf == false && Store_Ticket_Panel != null)
        {
            Store_Spend_Panel.gameObject.SetActive(false);
            Store_Ticket_Panel.gameObject.SetActive(true);
            Store_Level_Panel.gameObject.SetActive(false);

            Ticket_Store_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp 비활성화 상태에 연결이 잘 되어있다면
        if (Ticket_Btn_PopUp.activeSelf == false && Ticket_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Ticket_Btn_PopUp.SetActive(true);
            Level_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG 비활성화 상태에 연결이 잘 되어있다면
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
        // Spend_Inventory_Slot 비활성화 상태에 연결이 잘 되어있다면
        if (Store_Level_Panel.gameObject.activeSelf == false && Store_Level_Panel != null)
        {
            Store_Spend_Panel.gameObject.SetActive(false);
            Store_Ticket_Panel.gameObject.SetActive(false);
            Store_Level_Panel.gameObject.SetActive(true);

            Level_Store_Slot.verticalNormalizedPosition = 1.0f;
        }

        // Spend_Btn_PopUp 비활성화 상태에 연결이 잘 되어있다면
        if (Level_Btn_PopUp.activeSelf == false && Level_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Ticket_Btn_PopUp.SetActive(false);
            Level_Btn_PopUp.SetActive(true);
        }

        // Spend_Select_Shine_BG 비활성화 상태에 연결이 잘 되어있다면
        if (Level_Select_Shine_BG.activeSelf == false && Level_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Ticket_Select_Shine_BG.SetActive(false);
            Level_Select_Shine_BG.SetActive(true);
        }
    }
    #endregion

    #region Dia_Buy
    public void On_Click_Buy_Dia(int _count)
    {
        Dia_SelectCount = _count;
        Buying_Info.SetActive(true);
    }
    #endregion

    #region Buying_Info
    public void On_Click_Buy()
    {
        // TODO ## Store_Manager Dia획득
        UserInfo.Dia += Dia_SelectCount;
        Dia_SelectCount = 0;
        Buying_Info.SetActive(false);

        LobbyMgr_Ref.Refresh_UI_Dia();
    }

    public void On_Click_Cancel()
    {
        Dia_SelectCount = 0;
        Buying_Info.GetComponent<Animator>().Play("PopDown");

    }
    #endregion
}
