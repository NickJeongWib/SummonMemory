using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemUpGrade : MonoBehaviour
{
    Item SelectItem;
    public Item Get_SelectItem { get => SelectItem; set => SelectItem = value; }

    [Header("Ref")]
    [SerializeField] Item_Info_Panel Item_Info_Panel_Ref;
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("Upgrade_Root")]
    [SerializeField] GameObject UpgradeRoot;
    [SerializeField] int[] UpGrade_Rate;
    [SerializeField] float ItemState_Rate;

    [Header("ChainUpgrade_Root")]
    int MaxCost;
    [SerializeField] int Cost;
    bool isSlider;
    [SerializeField] Animator ChainAnimator;
    [SerializeField] Slider CountSlider;
    int MaxCount = 100;
    [SerializeField] int Count;
    [SerializeField] Text ChainCountText;


    [Header("---Item_UI---")]
    [SerializeField] Image ItemImage;
    [SerializeField] Image ItemGradeBack;
    [SerializeField] Image ItemGrade;
    [SerializeField] Text ItemName;
    [SerializeField] Text ItemOption;
    [SerializeField] Image[] UpgradeImage;

    #region Init
    private void OnEnable()
    {
        // 선택된 아이템이 없으면 return
        if (SelectItem == null)
            return;

        // 선택된 아이템 강화레벨이 9이상이면, 바로 잠재능력치 설정화면
        if (SelectItem.Get_Item_Lv >= 9)
        {
            UpgradeRoot.SetActive(false);
        }
        else
        {
            // 아이템이 기본옵션이 하나인 장비를 위해 
            for (int i = 0; i < UpgradeImage.Length; i++)
            {
                UpgradeImage[i].gameObject.SetActive(false);
            }

            UpgradeRoot.SetActive(true);
            Refresh_Upgrade_Option();
        }

        ItemImage.sprite = SelectItem.Get_Item_Image;
        ItemGrade.sprite = Item_Info_Panel_Ref.Get_Grade_Sprites[(int)SelectItem.Get_Equipment_Grade];
        ItemGradeBack.color = Inventory_UI_Ref.Get_Colors[(int)SelectItem.Get_Equipment_Grade];
        ItemName.text = SelectItem.Get_Item_Name;
    }
    #endregion

    #region Item_Upgrade
    int OptionNum;
    void Refresh_Upgrade_Option()
    {
        // 텍스트 초기화
        ItemOption.text = "";
        OptionNum = 0;

        // UI 초기화
        if (SelectItem.Get_Item_Atk != 0)
        {
            ItemOption.text += $"공격력            {SelectItem.Get_Item_Atk.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_Atk + SelectItem.Get_Item_Atk * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_DEF != 0)
        {
            ItemOption.text += $"방어력            {SelectItem.Get_Item_DEF.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_DEF + SelectItem.Get_Item_DEF * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_HP != 0)
        {
            ItemOption.text += $"체력                {SelectItem.Get_Item_HP.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_HP + SelectItem.Get_Item_HP * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_RATE != 0)
        {
            ItemOption.text += $"치명확률        {(SelectItem.Get_Item_CRI_RATE * 100).ToString("N0")}%            <color=orange>{((SelectItem.Get_Item_CRI_RATE * 100) + ((SelectItem.Get_Item_CRI_RATE * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_DMG != 0)
        {
            ItemOption.text += $"치명피해        {(SelectItem.Get_Item_CRI_DMG * 100).ToString("N0")}%             <color=orange>{((SelectItem.Get_Item_CRI_DMG * 100) + ((SelectItem.Get_Item_CRI_DMG * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            OptionNum++;
        }

        NextImage_Refresh(OptionNum);
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
                Debug.Log($"{CostSum.ToString("N0")}원 사용");
                break;
            }
        }
        Debug.Log($"{CostSum.ToString("N0")}원 사용");
        ChainAnimator.Play("ChainUp_Close");
    }

    #endregion
}
