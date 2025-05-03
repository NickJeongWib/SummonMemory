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

    [Header("Ref")]
    [SerializeField] Item_Info_Panel Item_Info_Panel_Ref;
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("Upgrade_Root")]
    [SerializeField] GameObject UpgradeRoot;
    [SerializeField] Image[] OptionGrade_Image;
    [SerializeField] Mask[] OptionGradeMask;
    [SerializeField] int[] UpGrade_Rate;
    [SerializeField] float ItemState_Rate;

    [Header("RandomOption_Root")]
    [SerializeField] int ResetOptionCost;
    [SerializeField] Text ResetOptionCostText;
    [SerializeField] Image[] Re_OptionGrade_Image;
    [SerializeField] GameObject RandomOptionRoot;
    [SerializeField] Text[] Re_OptionText;
    [SerializeField] Text Re_Option_ItemOption;

    [Header("ChainUpgrade_Root")]
    [SerializeField] int Cost;
    int MaxCost;
    bool isSlider;
    [SerializeField] Animator ChainAnimator;
    [SerializeField] Slider CountSlider;
    int MaxCount = 100;
    [SerializeField] int Count;
    [SerializeField] Text ChainCountText;


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

        ItemImage.sprite = SelectItem.Get_Item_Image;
        ItemGrade.sprite = Item_Info_Panel_Ref.Get_Grade_Sprites[(int)SelectItem.Get_Equipment_Grade];
        ItemGradeBack.color = Inventory_UI_Ref.Get_Colors[(int)SelectItem.Get_Equipment_Grade];
        ItemName.text = $"{SelectItem.Get_Item_Name} +{SelectItem.Get_Item_Lv}";
    }
    #endregion

    #region Item_UI_Refresh
    void Refresh_OptionText()
    {
        int OptionLv = 0;
        for (int i = 0; i < OptionText.Length; i++)
        {
            OptionLv += 3;
            if (SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.NONE)
            {
                OptionText[i].text = $"<color=#606060>강화 {OptionLv} 달성 시 무작위 능력치 추가</color>";
                Re_OptionText[i].text = $"<color=#606060>강화 {OptionLv} 달성 시 무작위 능력치 추가</color>";
                OptionGradeMask[i].showMaskGraphic = false;
            }
            else if (SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.ATK_INT || SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.DEF_INT ||
                 SelectItem.Get_EquipmentOption[i] == EQUIPMENT_OPTION.HP_INT)
            {
                OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{SelectItem.Get_OptionValue[i].ToString("N0")}</color>";
                Re_OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{SelectItem.Get_OptionValue[i].ToString("N0")}</color>";
                OptionGradeMask[i].showMaskGraphic = true;

                // 옵션 능력치에 따라 이미지 변경
                OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                Re_OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
            }
            else
            {
                OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{(SelectItem.Get_OptionValue[i] * 100).ToString("N1")}%</color>";
                Re_OptionText[i].text = $"<color=orange>{SelectItem.Get_Option_KorString(SelectItem.Get_EquipmentOption[i])} +{(SelectItem.Get_OptionValue[i] * 100).ToString("N1")}%</color>";
                OptionGradeMask[i].showMaskGraphic = true;

                // 옵션 능력치에 따라 이미지 변경
                OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
                Re_OptionGrade_Image[i].sprite = ItemOption_Grade_Sprite[(int)SelectItem.Get_EquipmentOptionGrade[i]];
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
        // 최대 레벨 도달 시 적용x
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

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
            RandomOptionRoot.SetActive(true);
        }
    }
    #endregion

    #region Item_Chain_Upgrade_Btn
    // 연속강화 창 오픈
    public void On_Click_ChainUp(GameObject _obj)
    {
        _obj.SetActive(true);

        CountSlider.value = 0;
        SliderValue();
    }

    // 연속강화 창 오프
    public void On_Click_ChainBack()
    {
        ChainAnimator.Play("ChainUp_Close");
    }

    // 슬라이더 값 변동마다 호출되는 함수
    public void SliderValue()
    {
        Count = Mathf.RoundToInt(MaxCount * CountSlider.value);
        ChainCountText.text = $"{Count}회";
    }

    // 1 +,- 반복횟수 설정
    public void On_Click_Count_1(bool _isPlus)
    {
        if (_isPlus)
        {
            Count++;

            if (Count >= 100)
                Count = 100;
        }
        else
        {
            Count--;
            if (Count <= 1)
                Count = 0;
        }

        SliderValue_Calculator();
    }

    // 10 +,- 반복횟수 카운드 설정
    public void On_Click_Count_10(bool _isPlus)
    {
        if (_isPlus)
        {
            Count += 10;

            if (Count >= 100)
                Count = 100;
        }
        else
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
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        MaxCost = Cost * Count;

        // 아이템 강화 골드보다 보유골드가 적으면
        if (MaxCost > UserInfo.Money)
        {

            return;
        }

        CostSum = 0;
        for (int i = 0; i < Count; i++)
        {
            CostSum += Cost;
            On_Click_UpgradeBtn();

            if (SelectItem.Get_Item_Lv >= 9)
            {   
                break;
            }
        }

        // Debug.Log($"{CostSum.ToString("N0")}원 사용");
        ChainAnimator.Play("ChainUp_Close");
    }



    #endregion

    #region OptionReset
    public void On_Click_Reset_Item_Option()
    {
        SelectItem.Set_ResetOption();

        Refresh_OptionText();
    }
    #endregion
}
