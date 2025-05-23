using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

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

    [SerializeField] GameObject Book_List;
    [SerializeField] string[] BookNames;
    [SerializeField] GameObject[] BookShine_BG;
    [SerializeField] Text[] Book_Texts;
    [SerializeField] GameObject[] Book_Store_Info;

    [Header("---Info---")]
    [SerializeField] GameObject Buying_Info;
    [SerializeField] ItemInfo_Pop ItemInfo_Pop;
    #endregion

    #region Variable
    public Sprite[] Consume_Icons;

    [SerializeField] Inventory_UI InventoryUI_Ref;
    [SerializeField] Lobby_Manager LobbyMgr_Ref;

    Store_Item StoreItem_Info;
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

            if (Book_List.activeSelf == true)
            {
                Book_List.SetActive(false);
            }
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

            if (Book_List.activeSelf == true)
            {
                Book_List.SetActive(false);
            }
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
            On_Click_Book(0);

            if (Book_List.activeSelf == false)
            {
                Book_List.SetActive(true);
            }
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

    // Currency_Store
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
        if (Dia_SelectCount != 0)
        {
            UserInfo.Dia += Dia_SelectCount;
            Dia_SelectCount = 0;
            Buying_Info.SetActive(false);
            LobbyMgr_Ref.Refresh_UI_Dia();
        }
        else
        {
            // 선택한 아이템이 있다면
            if (StoreItem_Info != null)
            {
                // 소모 아이템
                if (StoreItem_Info.Get_InvenType == INVENTORY_TYPE.SPEND)
                {
                    for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
                    {
                        if (Item_List.Spend_Item_List[i].Get_Item_Name == StoreItem_Info.Get_Item_Name)
                        {
                            UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i], StoreItem_Info.Get_Item_Ex);
                            InventoryUI_Ref.Spend_Slot_Refresh();
                            break;
                        }
                    }
                }
                // 강화 아이템
                else if (StoreItem_Info.Get_InvenType == INVENTORY_TYPE.UPGRADE)
                {
                    for (int i = 0; i < Item_List.Upgrade_Item_List.Count; i++)
                    {
                        if (Item_List.Upgrade_Item_List[i].Get_Item_Name == StoreItem_Info.Get_Item_Name)
                        {
                            UserInfo.Add_Inventory_Item(Item_List.Upgrade_Item_List[i], StoreItem_Info.Get_Item_Ex);
                            InventoryUI_Ref.Upgrade_Slot_Refresh();
                            break;
                        }
                    }
                }
            }

            Buying_Info.SetActive(false);
        }
    }

    public void On_Click_Cancel(Animator _animator)
    {
        if (Dia_SelectCount != 0)
            Dia_SelectCount = 0;

        _animator.Play("PopDown");
    }
    #endregion

    // Ticket_Store
    #region Ticket_Buy
    public void On_Click_Buy_Item(Store_Item _storeItem)
    {
        StoreItem_Info = _storeItem;
        Buying_Info.SetActive(true);
    }
    #endregion

    #region Item_Info
    public void On_ItemInfo_Pop(Store_Item _storeInfo)
    {
        ItemInfo_Pop.gameObject.SetActive(true);
        ItemInfo_Pop.Set_StoreItem(_storeInfo);
        ItemInfo_Pop.Init_ItemInfo_UI(Get_UserItem_Count(_storeInfo));
    }

    int Get_UserItem_Count(Store_Item _storeInfo)
    {
        if (_storeInfo.Get_Item_Name == "다이아")
            return UserInfo.Dia;
        if (_storeInfo.Get_Item_Name == "캐릭터 티켓")
            return UserInfo.SummonTicket;
        if (_storeInfo.Get_Item_Name == "장비 티켓")
            return UserInfo.EquipmentTicket;

        return 0;
    }
    #endregion

    #region Book_List
    public void On_Click_Book(int _num)
    {
        Book_UI_Refresh(BookNames[_num], _num);
    }

    void Book_UI_Refresh(string _bookname, int _index)
    {
        for (int i = 0; i < Book_Texts.Length; i++)
        {
            if (i == _index)
            {
                Book_Texts[i].text = $"<color=white>{_bookname}</color>";
                BookShine_BG[i].SetActive(true);
                Book_Store_Info[i].SetActive(true);
                Book_Store_Info[i].SetActive(true);
            }
            else
            {
                Book_Store_Info[i].SetActive(false);
                BookShine_BG[i].SetActive(false);
                Book_Store_Info[i].SetActive(false);
                Book_Texts[i].text = $"<color=#959595>{BookNames[i]}</color>";
            }
        }
    }
    #endregion
}
