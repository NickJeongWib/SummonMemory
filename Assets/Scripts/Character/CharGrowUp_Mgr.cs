using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharGrowUp_Mgr : MonoBehaviour
{
    // Variable
    #region Text_UI 
    [Header("Text_UI")]
    [SerializeField] Text CombatPower;
    [SerializeField] TextMeshProUGUI Level_Text;
    [SerializeField] TextMeshProUGUI HP_Value_Text;
    [SerializeField] TextMeshProUGUI ATK_Value_Text;
    [SerializeField] TextMeshProUGUI DEF_Value_Text;
    [SerializeField] TextMeshProUGUI CRIR_Value_Text;
    [SerializeField] TextMeshProUGUI CRID_Value_Text;

    [SerializeField] Text Low_Slime_Amout;
    [SerializeField] Text Middle_Slime_Amout;
    [SerializeField] Text High_Slime_Amout;
    [SerializeField] Text[] Slime_Use;
    #endregion

    #region UI
    [Header("Panel_UI")]
    [SerializeField] Image Char_Star;
    [SerializeField] Mask[] SelectMask;
    [SerializeField] Button[] Slime_Use_Btns;
    [SerializeField] Button LevelUP_Btn;
    [SerializeField] Slider Slime_Use_Slider;
    #endregion

    #region
    [Header("Exp")]
    [SerializeField] Slider ExpSlider;
    [SerializeField] Mask ExpBack;
    [SerializeField] int[] Use_Exp;
    [SerializeField] int Exp_Sum;

    #endregion

    #region Value
    [SerializeField] int SlimeMaxCount;
    [SerializeField] int UseCount;
    [SerializeField] int selectNum;
    #endregion
    // ---------
    #region Open
    public void Open_Growing_Panel()
    {
        // ĳ���� ���� ���� Text_UI
        Level_Text.text = $"LV.{GameManager.Instance.Get_SelectChar.Get_Character_Lv}" +
            $"<sprite=0><color=orange>{GameManager.Instance.Get_SelectChar.Get_Max_Lv}</color>";

        CombatPower.text = $"{GameManager.Instance.Get_SelectChar.Calc_CombatPower()}";
        HP_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseHP}";
        ATK_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseAtk}";
        DEF_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseDef}";
        CRIR_Value_Text.text = $"{(GameManager.Instance.Get_SelectChar.Get_BaseCRIR * 100).ToString("N1")}%";
        CRID_Value_Text.text = $"{(GameManager.Instance.Get_SelectChar.Get_BaseCRID* 100).ToString("N1")}%";

        Open_Slime_Select();
        // UI Slider
        Slime_Use_Slider.value = (float)1 / (float)SlimeMaxCount;

        // ����
        Char_Star.rectTransform.sizeDelta = new Vector3(GameManager.Instance.Get_SelectChar.Get_CharStar * 20, 20, 0);
    }
    #endregion

    void Open_Slime_Select()
    {
        #region Select_Slime
        for (int i = 0; i < SelectMask.Length; i++)
        {
            if (i == 0)
            {
                if (UserInfo.InventoryDict.ContainsKey("���ο� ������"))
                {
                    if (UserInfo.InventoryDict["���ο� ������"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["���ο� ������"].Get_Amount;
                        break;
                    }
                }
            }
            else if (i == 1)
            {
                if (UserInfo.InventoryDict.ContainsKey("��� ������"))
                {
                    if (UserInfo.InventoryDict["��� ������"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["��� ������"].Get_Amount;
                        break;
                    }
                }
            }
            else
            {
                if (UserInfo.InventoryDict.ContainsKey("���� ������"))
                {
                    if (UserInfo.InventoryDict["���� ������"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["���� ������"].Get_Amount;
                        break;
                    }
                }
            }
        }

        // Text
        Refresh_Slime_Amount();
        #endregion
    }

    public void Refresh_Slime_Amount()
    {
        #region Slime_Text_UI
        // �κ��丮�� ���ο� �������� ������ ��
        if (UserInfo.InventoryDict.ContainsKey("���ο� ������"))
        {
            Low_Slime_Amout.text = $"{UserInfo.InventoryDict["���ο� ������"].Get_Amount}";
        }
        else
        {
            Low_Slime_Amout.text = $"{0}";
        }

        // �κ��丮�� ��� �������� ������ ��
        if (UserInfo.InventoryDict.ContainsKey("��� ������"))
        {
            Middle_Slime_Amout.text = $"{UserInfo.InventoryDict["��� ������"].Get_Amount}";
        }
        else
        {
            Middle_Slime_Amout.text = $"{0}";
        }

        // �κ��丮�� ���� �������� ������ ��
        if (UserInfo.InventoryDict.ContainsKey("���� ������"))
        {
            High_Slime_Amout.text = $"{UserInfo.InventoryDict["���� ������"].Get_Amount}";
        }
        else
        {
            High_Slime_Amout.text = $"{0}";
        }
        #endregion
    }

    public void On_Click_Slime(int _num)
    {
        #region Select_UI_Active
        // ������ ���� �����ϰ� ���� �� ������ �̹��� ��Ȱ��ȭ
        for (int i = 0; i < SelectMask.Length; i++)
        {
            if (i == _num)
            {
                SelectMask[i].showMaskGraphic = true;
                selectNum = i;
            }
            else
            {
                SelectMask[i].showMaskGraphic = false;
            }
        }
        #endregion

        #region Interative_UI
        // ���ο� ������
        if (_num == 0)
        {
            // �κ��丮�� ���ο� �������� ������ ��
            if (UserInfo.InventoryDict.ContainsKey("���ο� ������") && UserInfo.InventoryDict["���ο� ������"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["���ο� ������"].Get_Amount;
                Set_Active_UI(true);
            }
            else if (!UserInfo.InventoryDict.ContainsKey("���ο� ������") || UserInfo.InventoryDict["���ο� ������"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        else if (_num == 1)
        {
            // �κ��丮�� ��� �������� ������ ��
            if (UserInfo.InventoryDict.ContainsKey("��� ������") && UserInfo.InventoryDict["��� ������"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["��� ������"].Get_Amount;
                Set_Active_UI(true);
            }
            else if(!UserInfo.InventoryDict.ContainsKey("��� ������") || UserInfo.InventoryDict["��� ������"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        else if (_num == 2)
        {
            // �κ��丮�� ���� �������� ������ ��
            if (UserInfo.InventoryDict.ContainsKey("���� ������") && UserInfo.InventoryDict["���� ������"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["���� ������"].Get_Amount;
                Set_Active_UI(true);
            }
            else if (!UserInfo.InventoryDict.ContainsKey("���� ������") || UserInfo.InventoryDict["���� ������"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        #endregion

        // �����̴� value
        Slime_Use_Slider.value = ((float)1 / (float)SlimeMaxCount);

        Cal_Use_Slime();
        Refresh_Amount_Slime();

        Exp_UI_Refresh();
    }

    #region UI_Refresh
    void Set_Active_UI(bool _isOn)
    {
        // ���� �������� ������ ���� ��/Ȱ��ȭ
        for (int i = 0; i < Slime_Use_Btns.Length; i++)
        {
            Slime_Use_Btns[i].interactable = _isOn;
        }

        LevelUP_Btn.interactable = _isOn;
        Slime_Use_Slider.interactable = _isOn;
    }

    public void Refresh_Slider()
    {
        // �����̴� ��ȭ�� ���� ȣ��Ǵ� �Լ�
        UseCount = Mathf.RoundToInt(SlimeMaxCount * Slime_Use_Slider.value);
        Exp_Sum = Use_Exp[selectNum] * UseCount;

        Exp_UI_Refresh();
        Refresh_Amount_Slime();
    }

    #region Slime_Amount_Calc
    // ������ ��뷮 ��ư ����
    public void On_Click_SlimeCount(int _num)
    {
        UseCount += _num;
        Exp_Sum = Use_Exp[selectNum] * UseCount;

        if (UseCount >= SlimeMaxCount)
        {
            UseCount = SlimeMaxCount;
        }

        if (UseCount <= 0)
        {
            UseCount = 0;
        }

        Refresh_Amount_Slime();

        Slime_Use_Slider.value = (float)UseCount / (float)SlimeMaxCount;
    }
    #endregion

    void Refresh_Amount_Slime()
    {
        // UI �ؽ�Ʈ ����
        for (int i = 0; i < Slime_Use.Length; i++)
        {
            if (selectNum == i)
            {
                Slime_Use[i].text = $"{UseCount}";
            }
            else
            {
                Slime_Use[i].text = $"{0}";
            }
        }
    }

    void Cal_Use_Slime()
    {
        // Exp ����, ��밹�� �ʱ�ȭ
        UseCount = Mathf.RoundToInt(SlimeMaxCount * Slime_Use_Slider.value);
        Exp_Sum = Use_Exp[selectNum] * UseCount;
    }
    #endregion

    void Exp_UI_Refresh()
    {
        // ���� ���� �䱸 ����ġ���� ������ �� ��� ��Ȳ������ ����
        if (Character_List.Require_Exp[GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1] <= Exp_Sum)
        {
            ExpBack.showMaskGraphic = true;
        }
        else
        {
            ExpBack.showMaskGraphic = false;
        }

        // ���� ����ġ
        int Remove_Exp = 0;
        int Left_Exp = 0;
        //int Lv = 0;
        for (int i = 0; i < Character_List.Cumulative_Exp.Count; i++)
        {
            Debug.Log(i);
            // ��� exp�� ������ ������ ���� ����ġ���� ������ ĳ���� ���� ����
            if (Character_List.Cumulative_Exp[i] >= Exp_Sum)
            {
                int maxLv = GameManager.Instance.Get_SelectChar.Get_Max_Lv;
                Debug.Log("maxLv " + maxLv);

                if (i >= maxLv)
                {
                    // �ִ� ���� ���� ��
                    Debug.Log("�ִ� ���� ���� : " + maxLv);

                    // ����ġ�� ������
                    ExpSlider.value = 0;

                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";

                    ExpSlider.value = 1;
                    Remove_Exp = 0; // Ȥ�� �ٸ� ������ �ʿ��ϸ� 0���� �ʱ�ȭ
                    break;
                }

                // ���� �ִ� ���� �̸��� ���
                Left_Exp = Character_List.Cumulative_Exp[i] - Exp_Sum;
                ExpSlider.value = (float)Left_Exp / (float)Character_List.Require_Exp[i];

                if (Left_Exp == Character_List.Require_Exp[i])
                {
                    ExpSlider.value = 0;
                }

                Level_Text.text = $"LV.{i + 1}" +
                    $"<sprite=0><color=orange>{maxLv}</color>";

                break;
                //if (GameManager.Instance.Get_SelectChar.Get_Max_Lv == i)
                //{
                //    Debug.Log("Lv : " + i);
                //    // ĳ���� ���� ���� Text_UI
                //    Level_Text.text = $"LV.{i}" +
                //        $"<sprite=0><color=orange>{GameManager.Instance.Get_SelectChar.Get_Max_Lv}</color>";

                //    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[i - 1];

                //    Debug.Log("���� ����ġ : " + Remove_Exp);
                //    break;
                //}

                //Left_Exp = Character_List.Cumulative_Exp[i] - Exp_Sum;
                //ExpSlider.value = (float)Left_Exp / (float)Character_List.Require_Exp[i];

                //if (Left_Exp == Character_List.Require_Exp[i])
                //{
                //    ExpSlider.value = 0;
                //}

                //// ĳ���� ���� ���� Text_UI
                //Level_Text.text = $"LV.{i + 1}" +
                //    $"<sprite=0><color=orange>{GameManager.Instance.Get_SelectChar.Get_Max_Lv}</color>";
                //break;
            }

        }
    }

    public void Char_Leveling()
    {

    }
}
