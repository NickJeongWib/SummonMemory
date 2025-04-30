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
        // ���õ� �������� ������ return
        if (SelectItem == null)
            return;

        // ���õ� ������ ��ȭ������ 9�̻��̸�, �ٷ� ����ɷ�ġ ����ȭ��
        if (SelectItem.Get_Item_Lv >= 9)
        {
            UpgradeRoot.SetActive(false);
        }
        else
        {
            // �������� �⺻�ɼ��� �ϳ��� ��� ���� 
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
        // �ؽ�Ʈ �ʱ�ȭ
        ItemOption.text = "";
        OptionNum = 0;

        // UI �ʱ�ȭ
        if (SelectItem.Get_Item_Atk != 0)
        {
            ItemOption.text += $"���ݷ�            {SelectItem.Get_Item_Atk.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_Atk + SelectItem.Get_Item_Atk * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_DEF != 0)
        {
            ItemOption.text += $"����            {SelectItem.Get_Item_DEF.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_DEF + SelectItem.Get_Item_DEF * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_HP != 0)
        {
            ItemOption.text += $"ü��                {SelectItem.Get_Item_HP.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_HP + SelectItem.Get_Item_HP * ItemState_Rate).ToString("N0")}</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_RATE != 0)
        {
            ItemOption.text += $"ġ��Ȯ��        {(SelectItem.Get_Item_CRI_RATE * 100).ToString("N0")}%            <color=orange>{((SelectItem.Get_Item_CRI_RATE * 100) + ((SelectItem.Get_Item_CRI_RATE * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            OptionNum++;
        }

        if (SelectItem.Get_Item_CRI_DMG != 0)
        {
            ItemOption.text += $"ġ������        {(SelectItem.Get_Item_CRI_DMG * 100).ToString("N0")}%             <color=orange>{((SelectItem.Get_Item_CRI_DMG * 100) + ((SelectItem.Get_Item_CRI_DMG * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
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

    // ������ ��ȭ ���� �� ĳ���Ϳ� ����� �ɷ�ġ�� ���� �� 10�� ������ ���� ���Ѵ�
    public void Refresh_Character_Ability()
    {
        if (SelectItem.Get_OwnCharacter == null)
            return;

        // ���� �������� ����� ���� �ɷ�ġ�� ���ش� ������ �� EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true)������ ȣ���Ͽ� �ٽ� ����� �ʿ䰡����
        SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, false);
    }

    // TODO ## ItemUpGrade ������ ���׷��̵�
    public void On_Click_UpgradeBtn()
    {
        // �ִ� ���� ���� �� ����x
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        // Debug.Log(SelectItem.Get_Item_Lv);
        int Rate = Random.Range(1, 101);
        // Debug.Log(Rate);

        // ���� ����
        if (UpGrade_Rate[SelectItem.Get_Item_Lv] >= Rate)
        {
            Refresh_Character_Ability();
            SelectItem.UpGrade_Success(ItemState_Rate);

            // �������� ĳ���Ͱ� �ִٸ�
            if (SelectItem.Get_OwnCharacter != null)
            {
                SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true);
            }
        }

        Refresh_Upgrade_Option();
    }
    #endregion

    #region Item_Chain_Upgrade_Btn
    // ���Ӱ�ȭ â ����
    public void On_Click_ChainUp(GameObject _obj)
    {
        _obj.SetActive(true);

        CountSlider.value = 0;
        SliderValue();
    }

    // ���Ӱ�ȭ â ����
    public void On_Click_ChainBack()
    {
        ChainAnimator.Play("ChainUp_Close");
    }

    // �����̴� �� �������� ȣ��Ǵ� �Լ�
    public void SliderValue()
    {
        Count = Mathf.RoundToInt(MaxCount * CountSlider.value);
        ChainCountText.text = $"{Count}ȸ";
    }

    // 1 +,- �ݺ�Ƚ�� ����
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

    // 10 +,- �ݺ�Ƚ�� ī��� ����
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

    // �ݺ�Ƚ�� �ִ�,�ּ� ����
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
        ChainCountText.text = $"{Count}ȸ";
    }


    int CostSum;
    public void On_Click_ChainUpgrade()
    {
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        MaxCost = Cost * Count;

        // ������ ��ȭ ��庸�� ������尡 ������
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
                Debug.Log($"{CostSum.ToString("N0")}�� ���");
                break;
            }
        }
        Debug.Log($"{CostSum.ToString("N0")}�� ���");
        ChainAnimator.Play("ChainUp_Close");
    }

    #endregion
}
