using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Store_Manager : MonoBehaviour
{
    #region UI_Variable
    // �Ҹ������, ���������, ��ȯ Ƽ�� ���� ���ӿ�����Ʈ
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
    public string[] Get_BookNames { get => BookNames; }
    [SerializeField] GameObject[] BookShine_BG;
    [SerializeField] Text[] Book_Texts;
    [SerializeField] GameObject[] Book_Store_Info;

    [Header("---Info---")]
    [SerializeField] GameObject FailInfo_Panel;
    [SerializeField] GameObject Buying_Info;
    [SerializeField] ItemInfo_Pop ItemInfo_Pop;
    [SerializeField] Text[] BookCount_Texts;
    [SerializeField] GameObject Slider_Root;
    [SerializeField] Slider AmountSlider;
    [SerializeField] Text AmountText;
    #endregion

    #region Variable
    public Sprite[] Consume_Icons;
    public int Buy_Count;
    [SerializeField] Inventory_UI InventoryUI_Ref;
    [SerializeField] Lobby_Manager LobbyMgr_Ref;

    Store_Item StoreItem_Info;
    int Dia_SelectCount;
    #endregion
    ///---------------------------------------------------------

    #region Spend_Store_Click
    public void On_Click_Spend_Store_Btn()
    {
        // Spend_Inventory_Slot ��Ȱ��ȭ ���¿� ������ �� �Ǿ��ִٸ�
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

    // Currency_Store
    #region Dia_Buy
    public void On_Click_Buy_Dia(int _count, Store_Item _storeInfo)
    {
        Dia_SelectCount = _count;
        StoreItem_Info = _storeInfo;
        Buying_Info.SetActive(true);
    }
    #endregion

    #region Buying_Info
    public void SliderRoot_OnOff(bool _isOn)
    {
        Slider_Root.SetActive(_isOn);

        // �����Ϸ��� ������ ������ �Ҹ���ᰡ ���̾ƶ��
        if (StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.DIA)
        {
            // �ǳ��� ������ �� ���� ���̾ư� ���� �� �����̴� ��Ȱ��ȭ
            if (UserInfo.Dia < StoreItem_Info.Get_ConsumeCount)
            {
                AmountSlider.interactable = false;
                AmountSlider.value = 0.0f;
            }
            else
            {
                int maxQuantity = Mathf.FloorToInt(UserInfo.Dia / (float)StoreItem_Info.Get_ConsumeCount);
                maxQuantity = Mathf.Max(1, maxQuantity); // �ּ� 1���� ���� �����ϰ�
                AmountSlider.minValue = 1;
                AmountSlider.maxValue = maxQuantity;
                AmountSlider.value = 1;

                AmountSlider.interactable = true;
            }
        }
        else if (StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.R_BOOK || StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.SR_BOOK || StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.SSR_BOOK)
        {
            if (!UserInfo.InventoryDict.ContainsKey(BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]))
            {
                AmountSlider.interactable = false;
                AmountSlider.value = 0.0f;
                return;
            }
            else
            {
                int maxQuantity = Mathf.FloorToInt(UserInfo.InventoryDict[BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]].Get_Amount / (float)StoreItem_Info.Get_ConsumeCount);
                maxQuantity = Mathf.Max(1, maxQuantity); // �ּ� 1���� ���� �����ϰ�
                AmountSlider.minValue = 1;
                AmountSlider.maxValue = maxQuantity;
                AmountSlider.value = 1;

                AmountSlider.interactable = true;
            }

            // �ǳ��� ������ �� ���� ��ȯ���� ���� �� �����̴� ��Ȱ��ȭ
            if (UserInfo.InventoryDict[BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]].Get_Amount < StoreItem_Info.Get_ConsumeCount)
            {
                AmountSlider.interactable = false;
            }
            else
            {
                AmountSlider.interactable = true;
            }
        }
        
    }

    public void Amount_Slider()
    {
        AmountText.text = $"{AmountSlider.value}��";
        Buy_Count = (int)AmountSlider.value;
    }

    public void On_Click_Buy()
    {
        // TODO ## Store_Manager Diaȹ�� ���̾� ����
        if (Dia_SelectCount != 0)
        {
            if (!GameManager.Inst.TestMode)
            {
                if (UserInfo.Money - StoreItem_Info.Get_ConsumeCount < 0)
                {
                    FailInfo_Panel.SetActive(true);
                    Buying_Info.SetActive(false);
                    return;
                }

                UserInfo.Money -= StoreItem_Info.Get_ConsumeCount;
                LobbyMgr_Ref.Refresh_UI_Money();
            }
                
            UserInfo.Dia += Dia_SelectCount;
            Dia_SelectCount = 0;
            Buying_Info.SetActive(false);
            LobbyMgr_Ref.Refresh_UI_Dia();
        }
        else // ���̾� ���Ű� �ƴ϶�� ������ ����
        {
            // ������ �������� �ִٸ�
            if (StoreItem_Info != null)
            {
                // TODO ## Store_Manager ���� ��ȭ �Ҹ�
                // �׽�Ʈ������ �ƴ���
                if (!GameManager.Inst.TestMode)
                {
                    // ��ȯ���� �Һ� �ؾ��ϴ� ��ǰ�϶�
                    if (StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.R_BOOK || StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.SR_BOOK || StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.SSR_BOOK)
                    {
                        // ������������ �Ҹ� �������� ���ٸ�
                        if (UserInfo.InventoryDict.ContainsKey(BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]) == false)
                        {
                            FailInfo_Panel.SetActive(true);
                            Buying_Info.SetActive(false);
                            return;
                        }
                        // ������������ �Ҹ� �����ۿ� �´� ��ȭ�� ������ �����ϴٸ�
                        if (UserInfo.InventoryDict[BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]].Get_Amount - StoreItem_Info.Get_ConsumeCount < 0)
                        {
                            FailInfo_Panel.SetActive(true);
                            Buying_Info.SetActive(false);
                            return;
                        }

                        // �ش� ��ȯ�� ����, ���� �ʱ�ȭ
                        UserInfo.InventoryDict[BookNames[(int)StoreItem_Info.Get_ConsumeType - 2]].Get_Amount -= (StoreItem_Info.Get_ConsumeCount * Buy_Count);
                        UserInfo.Remove_Inventory_Item();
                        InventoryUI_Ref.Reset_Spend_Inventory();
                        InventoryUI_Ref.Spend_Slot_Refresh();

                        Store_Currency_Refresh();
                    }
                    // �Ҹ���ȭ�� ���̾ƶ��
                    else if (StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.DIA)
                    {
                        // ���̾ư� �����ϴٸ�
                        if(UserInfo.Dia - StoreItem_Info.Get_ConsumeCount < 0)
                        {
                            FailInfo_Panel.SetActive(true);
                            Buying_Info.SetActive(false);
                            return;
                        }

                        // ����
                        UserInfo.Dia -= (StoreItem_Info.Get_ConsumeCount * Buy_Count);
                        LobbyMgr_Ref.Refresh_UI_Dia();
                        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.DIA);
                    }
                    // �Ҹ���ȭ�� �����
                    else if (StoreItem_Info.Get_ConsumeType == CONSUME_TYPE.MONEY)
                    {
                        // ��尡 �����ϴٸ�
                        if (UserInfo.Money - StoreItem_Info.Get_ConsumeCount < 0)
                        {
                            FailInfo_Panel.SetActive(true);
                            Buying_Info.SetActive(false);
                            return;
                        }

                        // ����
                        UserInfo.Money -= StoreItem_Info.Get_ConsumeCount;
                        LobbyMgr_Ref.Refresh_UI_Money();
                        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.MONEY);
                    }
                }
                else
                {
                    // �׽�Ʈ��
                    Buy_Count = 10;
                }


                // �Ҹ� ������
                if (StoreItem_Info.Get_InvenType == INVENTORY_TYPE.SPEND)
                {
                    for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
                    {
                        // ����Ʈ�� �̹� �������� �ִٸ�
                        if (Item_List.Spend_Item_List[i].Get_Item_Name == StoreItem_Info.Get_Item_Name)
                        {
                            // �߰� ������������ ������ŭ ����
                            UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i], (StoreItem_Info.Get_Item_Ex * Buy_Count));
                            InventoryUI_Ref.Spend_Slot_Refresh();
                            break;
                        }
                    }

                    DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
                }
                // ��ȭ ������
                else if (StoreItem_Info.Get_InvenType == INVENTORY_TYPE.UPGRADE)
                {
                    for (int i = 0; i < Item_List.Upgrade_Item_List.Count; i++)
                    {
                        // ����Ʈ�� �̹� �������� �ִٸ�
                        if (Item_List.Upgrade_Item_List[i].Get_Item_Name == StoreItem_Info.Get_Item_Name)
                        {
                            // �߰� ������������ ������ŭ ����
                            UserInfo.Add_Inventory_Item(Item_List.Upgrade_Item_List[i], StoreItem_Info.Get_Item_Ex * Buy_Count);
                            InventoryUI_Ref.Upgrade_Slot_Refresh();
                            break;
                        }
                    }

                    DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
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
        if (_storeInfo.Get_Item_Name == "���̾�")
            return UserInfo.Dia;
        if (_storeInfo.Get_Item_Name == "ĳ���� Ƽ��")
            return UserInfo.SummonTicket;
        if (_storeInfo.Get_Item_Name == "��� Ƽ��")
            return UserInfo.EquipmentTicket;

        return 0;
    }

    public void Set_FailInfo_Panel(bool _isOn)
    {
        FailInfo_Panel.SetActive(_isOn);
    }
    #endregion

    #region Book_List
    public void On_Click_Book(int _num)
    {
        Book_UI_Refresh(BookNames[_num], _num);
    }

    public void Store_Currency_Refresh()
    {
        for(int i = 0; i < BookCount_Texts.Length; i++)
        {
            if(UserInfo.InventoryDict.ContainsKey(BookNames[i]))
            {
                BookCount_Texts[i].text = $"{UserInfo.InventoryDict[BookNames[i]].Get_Amount}";
            }
            else
            {
                BookCount_Texts[i].text = "0";
            }
        }
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
