using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemUpGrade : MonoBehaviour
{
    #region Var
    Item SelectItem;
    public Item Get_SelectItem { get => SelectItem; set => SelectItem = value; }

    [SerializeField] Active_F LoadingPanel;

    [Header("Ref")]
    [SerializeField] Item_Info_Panel Item_Info_Panel_Ref;
    [SerializeField] Inventory_UI InventoryUI_Ref;
    [SerializeField] Lobby_Manager LobbyManager_Ref;

    [Header("All_UI_Root")]
    [SerializeField] Text UserItemCrystal_Text;
    [SerializeField] Text[] UserPowerText;
    [SerializeField] GameObject Fail_Info_Panel;
    [SerializeField] GameObject AllUpgradeRoot;
    [SerializeField] Mask UpGradeShine_Mask;
    [SerializeField] Mask OptionReset_Mask;
    [SerializeField] Button Upgrade_Btn;
    [SerializeField] Button ResetOption_Btn;
    [SerializeField] Mask UpgradeWhite_Back;
    [SerializeField] Mask ResetWhite_Back;

    [Header("Upgrade_Root")]
    [SerializeField] GameObject UpgradeRoot;
    [SerializeField] Image[] OptionGrade_Image;
    [SerializeField] Mask[] OptionGradeMask;
    [SerializeField] int[] UpGrade_Rate;
    [SerializeField] float ItemState_Rate;

    [Header("RandomOption_Root")]
    [SerializeField] int ResetOptionCost;
    [SerializeField] int ResetOptionItem_Cost;
    [SerializeField] Text ResetOptionCostText;
    [SerializeField] Image[] Re_OptionGrade_Image;
    [SerializeField] GameObject RandomOptionRoot;
    [SerializeField] Text[] Re_OptionText;
    [SerializeField] Text Re_Option_ItemOption;
    [SerializeField] Mask[] LockBG;
    [SerializeField] Toggle[] OptionLock_Toggle;
    bool[] OpValue_Locks = new bool[3];

    [Header("ChainUpgrade_Root")]
    [SerializeField] GameObject ChainUpgrade_Panel;
    [SerializeField] int Cost;
    [SerializeField] int UpgradeItem_Cost;
    int MaxCost;
    bool isSlider;
    [SerializeField] Animator ChainAnimator;
    [SerializeField] Slider CountSlider;
    int MaxCount = 100;
    [SerializeField] int Count;
    [SerializeField] Text ChainCountText;
    [SerializeField] Button[] ChainButtons;

    [Header("---ItemOption_ResetValue_Root---")]
    [SerializeField] int ResetOptionValue_Cost;
    [SerializeField] int ResetOptionValue_Item_Cost;
    [SerializeField] GameObject ResetOptionRoot;
    [SerializeField] Text[] ResetOptionRoot_text;
    [SerializeField] Mask[] ResetOption_Mask;
    [SerializeField] Image[] ResetOption_Img;
    [SerializeField] Mask[] OPLockBG;
    [SerializeField] Toggle[] OPRELock_Toggle;
    bool[] Op_Locks = new bool[3];

    [Header("---Item_UI---")]
    [SerializeField] Sprite[] ItemOption_Grade_Sprite;
    [SerializeField] Text[] OptionText;
    [SerializeField] Image ItemImage;
    [SerializeField] Image ItemGradeBack;
    [SerializeField] Image ItemGrade;
    [SerializeField] Text ItemName;
    [SerializeField] Text ItemOption;
    [SerializeField] Image[] UpgradeImage;

    [SerializeField] Text[] UpgradeCost;
    #endregion
    // -------------------------------------------

    #region Init
    private void Start()
    {
        for (int i = 0; i < UpgradeCost.Length; i++)
        {
            UpgradeCost[i].text = $"{Cost.ToString("N0")}";
        }
        ResetOptionCostText.text = $"{ResetOptionCost.ToString("N0")}";
    }

    private void OnEnable()
    {
        // 선택된 아이템이 없으면 return
        if (SelectItem == null)
            return;

        // 선택된 아이템 강화레벨이 9이상이면, 바로 잠재능력치 설정화면
        if (SelectItem.Get_Item_Lv >= 9)
        {
            UpgradeRoot.SetActive(false);
            RandomOptionRoot.SetActive(true);
        }
        else
        {
            // 아이템이 기본옵션이 하나인 장비를 위해 
            for (int i = 0; i < UpgradeImage.Length; i++)
            {
                UpgradeImage[i].gameObject.SetActive(false);
            }

            UpgradeRoot.SetActive(true);
            RandomOptionRoot.SetActive(false);
            Refresh_Upgrade_Option();
        }

        Refresh_OptionText();
        Refresh_UserPowder();
        Refresh_UserItemCrystal();

        ItemImage.sprite = SelectItem.Get_Item_Image;
        ItemGrade.sprite = Item_Info_Panel_Ref.Get_Grade_Sprites[(int)SelectItem.Get_Equipment_Grade];
        ItemGradeBack.color = InventoryUI_Ref.Get_Colors[(int)SelectItem.Get_Equipment_Grade];
        ItemName.text = $"{SelectItem.Get_Item_Name} +{SelectItem.Get_Item_Lv}";
    }
    #endregion

    #region Item_UI_Refresh
    public void Refresh_UserPowder()
    {
        for (int i = 0; i < UserPowerText.Length; i++)
        {
            if (!UserInfo.InventoryDict.ContainsKey("재련 가루"))
            {
                UserPowerText[i].text = "0개 보유";
                return;
            }

            UserPowerText[i].text = $"{UserInfo.InventoryDict["재련 가루"].Get_Amount}개 보유";
        }
    }

    void Refresh_UserItemCrystal()
    {
        if (!UserInfo.InventoryDict.ContainsKey("재련 수정"))
        {
            UserItemCrystal_Text.text = "0개 보유";
            return;
        }

        UserItemCrystal_Text.text = $"{UserInfo.InventoryDict["재련 수정"].Get_Amount}개 보유";
    }

    void Refresh_OptionText()
    {
        int OptionLv = 0;
        for (int i = 0; i < OptionText.Length; i++)
        {
            if (OpValue_Locks[i] || Op_Locks[i])
                continue;

            OptionLv += 3;
            if (SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.NONE)
            {
                OptionText[i].text = $"<color=#606060>강화 {OptionLv} 달성 시 무작위 능력치 추가</color>";
                Re_OptionText[i].text = $"<color=#606060>강화 {OptionLv} 달성 시 무작위 능력치 추가</color>";
                ResetOptionRoot_text[i].text = $"<color=#606060>강화 {OptionLv} 달성 시 무작위 능력치 추가</color>";
                OptionGradeMask[i].showMaskGraphic = false;
                ResetOption_Mask[i].showMaskGraphic = false;
            }
            else if (SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.ATK_INT || SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.DEF_INT ||
                 SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.HP_INT)
            {
                OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{SelectItem.Get_OptionValue[i].ToString("N0")}</color>";
                Re_OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{SelectItem.Get_OptionValue[i].ToString("N0")}</color>";
                ResetOptionRoot_text[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{SelectItem.Get_OptionValue[i].ToString("N0")}</color>";
                OptionGradeMask[i].showMaskGraphic = true;
                ResetOption_Mask[i].showMaskGraphic = true;

                // 옵션 능력치에 따라 이미지 변경
                OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                Re_OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                ResetOption_Img[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
            }
            else
            {
                OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{(SelectItem.Get_OptionValue[i] * 100).ToString("N1")}%</color>";
                Re_OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{(SelectItem.Get_OptionValue[i] * 100).ToString("N1")}%</color>";
                ResetOptionRoot_text[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{(SelectItem.Get_OptionValue[i] * 100).ToString("N1")}%</color>";
                OptionGradeMask[i].showMaskGraphic = true;
                ResetOption_Mask[i].showMaskGraphic = true;

                // 옵션 능력치에 따라 이미지 변경
                OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                Re_OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                ResetOption_Img[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
            }
        }
    }
    #endregion

    #region Item_Upgrade
    int OptionNum;
    void Refresh_Upgrade_Option()
    {
        // 텍스트 초기화
        ItemOption.text = "";
        Re_Option_ItemOption.text = "";
        OptionNum = 0;

        // UI 초기화
        if (SelectItem.Get_Item_Atk != 0)
        {
            Re_Option_ItemOption.text += $"공격력                      {SelectItem.Get_Item_Atk.ToString("N0")}\n\n";
            ItemOption.text += $"공격력            {SelectItem.Get_Item_Atk.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_Atk + SelectItem.Get_Item_Atk * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_DEF != 0)
        {
            Re_Option_ItemOption.text += $"방어력                      {SelectItem.Get_Item_DEF.ToString("N0")}\n\n";
            ItemOption.text += $"방어력            {SelectItem.Get_Item_DEF.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_DEF + SelectItem.Get_Item_DEF * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_HP != 0)
        {
            Re_Option_ItemOption.text += $"체력                          {SelectItem.Get_Item_HP.ToString("N0")}\n\n";
            ItemOption.text += $"체력                {SelectItem.Get_Item_HP.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_HP + SelectItem.Get_Item_HP * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_RATE != 0)
        {
            Re_Option_ItemOption.text += $"치명확률                  {(SelectItem.Get_Item_CRI_RATE * 100).ToString("N1")}%\n\n";
            ItemOption.text += $"치명확률        {(SelectItem.Get_Item_CRI_RATE * 100).ToString("N0")}%            <color=orange>{((SelectItem.Get_Item_CRI_RATE * 100) + ((SelectItem.Get_Item_CRI_RATE * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_DMG != 0)
        {
            Re_Option_ItemOption.text += $"치명피해                  {(SelectItem.Get_Item_CRI_DMG * 100).ToString("N1")}%\n\n";
            ItemOption.text += $"치명피해        {(SelectItem.Get_Item_CRI_DMG * 100).ToString("N0")}%             <color=orange>{((SelectItem.Get_Item_CRI_DMG * 100) + ((SelectItem.Get_Item_CRI_DMG * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            OptionNum++;
        }

        NextImage_Refresh(OptionNum);
        ItemName.text = $"{SelectItem.Get_Item_Name} +{SelectItem.Get_Item_Lv}";
    }

    void NextImage_Refresh(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
           if(UpgradeImage[i].gameObject.activeSelf == false)
           {
                UpgradeImage[i].gameObject.SetActive(true);
           }
        }
    }

    // 아이템 강화 성공 시 캐릭터에 적용된 능력치를 빼준 후 10퍼 증가된 값을 더한다
    public void Refresh_Character_Ability()
    {
        if (SelectItem.Get_OwnCharacter == null)
            return;

        // 먼저 적용중인 장비의 기존 능력치를 빼준다 성공한 후 EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true)값으로 호출하여 다시 계산할 필요가있음
        SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, false);
    }

    // TODO ## ItemUpGrade 아이템 업그레이드
    public void On_Click_UpgradeBtn()
    {
        SoundManager.Inst.PlayUISound();

        // 최대 레벨 도달 시 적용x
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        if (!GameManager.Inst.TestMode)
        {
            if (UserInfo.Money < Cost || !UserInfo.InventoryDict.ContainsKey("재련 가루") || UserInfo.InventoryDict["재련 가루"].Get_Amount < UpgradeItem_Cost)
            {
                Fail_Info_Panel.SetActive(true);
                return;
            }

            // 재화 소모
            UserInfo.Money -= Cost;
            UserInfo.InventoryDict["재련 가루"].Get_Amount -= UpgradeItem_Cost;

            // 인벤토리 초기화
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Upgrade_Inventory();
            InventoryUI_Ref.Upgrade_Slot_Refresh();

            // 보유 재련 가루 표시
            Refresh_UserPowder();
        }

        LoadingPanel.Loading();

        // Debug.Log(SelectItem.Get_Item_Lv);
        int Rate = Random.Range(1, 101);
        // Debug.Log(Rate);

        // 성공 공식
        if (UpGrade_Rate[SelectItem.Get_Item_Lv] >= Rate)
        {
            Refresh_Character_Ability();
            SelectItem.UpGrade_Success(ItemState_Rate);

            // 착용중인 캐릭터가 있다면
            if (SelectItem.Get_OwnCharacter != null)
            {
                SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true);
            }
        }

        Refresh_Upgrade_Option();
        // 랜덤 능력 부여
        SelectItem.Set_UpgradeOption();
        Refresh_OptionText();


        if (SelectItem.Get_Item_Lv >= 9)
        {
            UpgradeRoot.SetActive(false);
            Item_Info_Panel_Ref.ButtonRoot_Active(true);
            RandomOptionRoot.SetActive(true);
        }

        #region Data
        // 연속 업그레이드 시 중복 호출 방지
        if (ChainUpgrade_Panel.activeSelf == false)
        {
            // TODO ## ItemUpGrade 업그레이드 데이터 저장
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);
        }
        #endregion
    }
    #endregion

    #region Item_Chain_Upgrade_Btn
    // 연속강화 창 오픈
    public void On_Click_ChainUp(GameObject _obj)
    {
        SoundManager.Inst.PlayUISound();

        _obj.SetActive(true);

        CountSlider.value = 0;
        SliderValue();
    }

    // 연속강화 창 오프
    public void On_Click_ChainBack()
    {
        SoundManager.Inst.PlayUISound();
        ChainAnimator.Play("ChainUp_Close");
    }

    // 슬라이더 값 변동마다 호출되는 함수
    public void SliderValue()
    {
        Count = Mathf.RoundToInt(MaxCount * CountSlider.value);
        ChainCountText.text = $"{Count}회";

        if (!GameManager.Inst.TestMode)
        {
            // 재화가 없다면
            if (UserInfo.Money < Count * Cost || !UserInfo.InventoryDict.ContainsKey("재련 가루") ||
            UserInfo.InventoryDict["재련 가루"].Get_Amount < UpgradeItem_Cost * Count)
            {
                // 재화 소모 증가 ui 비활성화
                CountSlider.interactable = false;

                for (int i = 0; i < ChainButtons.Length; i++)
                {
                    ChainButtons[i].interactable = false;
                }

                // 인벤토리에 재련가루가 없다면 
                if (!UserInfo.InventoryDict.ContainsKey("재련 가루"))
                {
                    Count = 0;
                }
                else // 인벤토리에 재련가루가 존재한다면
                {
                    Count = UserInfo.InventoryDict["재련 가루"].Get_Amount;
                }
               
                ChainCountText.text = $"{Count}회";
            }
            else // 재화가 인벤토리에 있다면
            {
                CountSlider.interactable = true;

                for (int i = 0; i < ChainButtons.Length; i++)
                {
                    ChainButtons[i].interactable = true;
                }
            }
        }
    }

    // 1 +,- 반복횟수 설정
    public void On_Click_Count_1(bool _isPlus)
    {
        SoundManager.Inst.PlayUISound();

        if (_isPlus)
        {
            Count++;

            if (Count >= 100)
                Count = 100;
        }
        else
        {
            Count--;
            if (Count <= 0)
                Count = 0;
        }

        SliderValue_Calculator();
    }

    // 10 +,- 반복횟수 카운드 설정
    public void On_Click_Count_10(bool _isPlus)
    {
        SoundManager.Inst.PlayUISound();

        // +면 카운터 증가
        if (_isPlus)
        {
            Count += 10;

            if (Count >= 100)
                Count = 100;
        }
        else // -면 카운터 감소
        {
            Count -= 10;
            if (Count <= 1)
                Count = 0;
        }

        SliderValue_Calculator();
    }

    // 반복횟수 최대,최소 설정
    public void On_Click_Count_MinMax(bool _isMax)
    {
        SoundManager.Inst.PlayUISound();

        if (_isMax)
        {
            Count = 100;
        }
        else
        {
            Count = 0;
        }

        SliderValue_Calculator();
    }

    void SliderValue_Calculator()
    {
        CountSlider.value = (float)Count / MaxCount;
        ChainCountText.text = $"{Count}회";
    }


    int CostSum;
    public void On_Click_ChainUpgrade()
    {
        SoundManager.Inst.PlayUISound();

        // 강화하고자 하는 아이템이 9강 이상이면 return
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        // 데이터 저장이 완료될 때까지 다른 작업하지 못하게 로딩 창 활성화
        LoadingPanel.Loading();

        // 최대 비용 = 강화 비용 * 강화 시행 횟수
        MaxCost = Cost * Count;

        // 최대 아이템 강화 비용보다 보유골드가 적으면
        if (MaxCost > UserInfo.Money)
        {
            return;
        }

        CostSum = 0;

        // 연속 강화일 때 슬라이더나 버튼으로 정한 횟수만큼 반복
        for (int i = 0; i < Count; i++)
        {
            // 사용한 총 비용 알기 위해 계산
            CostSum += Cost;
            // 아이템 강화 함수 호출!
            On_Click_UpgradeBtn();

            // 9강 도달하면 바로 break
            if (SelectItem.Get_Item_Lv >= 9)
            {
                break;
            }
        }

        // 로비Scene의 골드 수량 Text 수량 초기화해서 표시하는 함수 실행
        LobbyManager_Ref.Refresh_UI_Money();

        // 데이터 저장
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);

        ChainAnimator.Play("ChainUp_Close");
    }
    #endregion

    #region OptionValueReset
    public void On_Click_Reset_Item_Option_Value()
    {
        SoundManager.Inst.PlayUISound();

        // 옵션 다 잠궜으면
        if (OpValue_Locks[0] && OpValue_Locks[1] && OpValue_Locks[2])
            return;

        // 테스트 모드 아닐 때 재화 소모 하기 위한
        if (!GameManager.Inst.TestMode)
        {
            if (UserInfo.Money < ResetOptionValue_Cost || !UserInfo.InventoryDict.ContainsKey("재련 가루") || UserInfo.InventoryDict["재련 가루"].Get_Amount < ResetOptionValue_Item_Cost)
            {
                Fail_Info_Panel.SetActive(true);
                return;
            }

            UserInfo.Money -= ResetOptionValue_Cost;
            UserInfo.InventoryDict["재련 가루"].Get_Amount -= ResetOptionValue_Item_Cost;

            LobbyManager_Ref.Refresh_UI_Money();

            // 인벤토리 초기화
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Upgrade_Inventory();
            InventoryUI_Ref.Upgrade_Slot_Refresh();

            // 보유 재련 가루 표시
            Refresh_UserPowder();
        }

        LoadingPanel.Loading();

        // 옵션 재설정 함수
        SelectItem.Set_ResetOptionValue(OpValue_Locks);

        // 데이터 저장
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);

        // Text 초기화
        Refresh_OptionText();
    }
    #endregion

    #region OptionReset
    public void On_Click_Reset_Item_Option()
    {
        SoundManager.Inst.PlayUISound();

        // 옵션 다 잠궜으면
        if (Op_Locks[0] && Op_Locks[1] && Op_Locks[2])
            return;

        if (!GameManager.Inst.TestMode)
        {
            if (UserInfo.Money < ResetOptionCost || !UserInfo.InventoryDict.ContainsKey("재련 수정") || UserInfo.InventoryDict["재련 수정"].Get_Amount < ResetOptionItem_Cost)
            {
                Fail_Info_Panel.SetActive(true);
                return;
            }

            UserInfo.Money -= ResetOptionValue_Cost;
            UserInfo.InventoryDict["재련 수정"].Get_Amount -= ResetOptionItem_Cost;

            LobbyManager_Ref.Refresh_UI_Money();

            // 인벤토리 초기화
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Upgrade_Inventory();
            InventoryUI_Ref.Upgrade_Slot_Refresh();

            // 보유 재련 가루 표시
            Refresh_UserItemCrystal();
        }

        LoadingPanel.Loading();

        SelectItem.Set_ResetOption(Op_Locks);
        Refresh_OptionText();

        // 재료 저장
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);

        if (SelectItem.Get_OwnCharacter != null)
            SelectItem.Get_OwnCharacter.TestState();
    }
    #endregion

    #region RootClick
    public void On_Click_UpgradeRoot()
    {
        SoundManager.Inst.PlayUISound();

        for (int i = 0; i < OPRELock_Toggle.Length; i++)
        {
            OPRELock_Toggle[i].isOn = false;
        }

        AllUpgradeRoot.SetActive(true);
        ResetOptionRoot.SetActive(false);
        OptionReset_Mask.showMaskGraphic = false;
        UpGradeShine_Mask.showMaskGraphic = true;
        Upgrade_Btn.interactable = false;
        ResetOption_Btn.interactable = true;
        UpgradeWhite_Back.showMaskGraphic = true;
        ResetWhite_Back.showMaskGraphic = false;

    }

    public void On_Click_ResetOptionRoot()
    {
        SoundManager.Inst.PlayUISound();

        for (int i = 0; i < OptionLock_Toggle.Length; i++)
        {
            OptionLock_Toggle[i].isOn = false;
        }

        AllUpgradeRoot.SetActive(false);
        ResetOptionRoot.SetActive(true);
        OptionReset_Mask.showMaskGraphic = true;
        UpGradeShine_Mask.showMaskGraphic = false;
        Upgrade_Btn.interactable = true;
        ResetOption_Btn.interactable = false;
        UpgradeWhite_Back.showMaskGraphic = false;
        ResetWhite_Back.showMaskGraphic = true;
    }
    #endregion

    #region OptionValue_Lock_Toggle
    public void On_Click_Option_Lock(int _num)
    {
        SoundManager.Inst.PlayUISound();

        if (OptionLock_Toggle[_num].isOn)
        {
            Refresh_ResetOptionValueColor(_num, "<color=#80807F>");
        }
        else
        {
            Refresh_ResetOptionValueColor(_num, "<color=orange>");
        }


        LockBG[_num].showMaskGraphic = !OptionLock_Toggle[_num].isOn;
        OpValue_Locks[_num] = OptionLock_Toggle[_num].isOn;
    }

    void Refresh_ResetOptionValueColor(int _num, string _color)
    {
        if (SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.ATK_INT || SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.DEF_INT ||
             SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.HP_INT)
        {
            Re_OptionText[_num].text = $"{_color}{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[_num])} +{SelectItem.Get_OptionValue[_num].ToString("N0")}</color>";
        }
        else
        {
            Re_OptionText[_num].text = $"{_color}{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[_num])} +{(SelectItem.Get_OptionValue[_num] * 100).ToString("N1")}%</color>";
        }
    }
    #endregion

    #region Option_Lock_Toggle
    public void On_Click_OptionRe_Lock(int _num)
    {
        SoundManager.Inst.PlayUISound();

        if (OPRELock_Toggle[_num].isOn)
        {
            Refresh_ResetOptionColor(_num, "<color=#80807F>");
        }
        else
        {
            Refresh_ResetOptionColor(_num, "<color=orange>");
        }


        OPLockBG[_num].showMaskGraphic = !OPRELock_Toggle[_num].isOn;
        Op_Locks[_num] = OPRELock_Toggle[_num].isOn;
    }

    void Refresh_ResetOptionColor(int _num, string _color)
    {
        if (SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.ATK_INT || SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.DEF_INT ||
             SelectItem.Get_EquipmentOption[_num] == EQUIPMENT_OPTION.HP_INT)
        {
            ResetOptionRoot_text[_num].text = $"{_color}{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[_num])} +{SelectItem.Get_OptionValue[_num].ToString("N0")}</color>";
        }
        else
        {
            ResetOptionRoot_text[_num].text = $"{_color}{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[_num])} +{(SelectItem.Get_OptionValue[_num] * 100).ToString("N1")}%</color>";
        }
    }
    #endregion

    #region Lock_Off
    public void Lock_Off()
    {
        SoundManager.Inst.PlayUISound();

        for (int i = 0; i < OPRELock_Toggle.Length; i++)
        {
            OPRELock_Toggle[i].isOn = false;
        }

        for (int i = 0; i < OptionLock_Toggle.Length; i++)
        {
            OptionLock_Toggle[i].isOn = false;
        }
    }
    #endregion
}
