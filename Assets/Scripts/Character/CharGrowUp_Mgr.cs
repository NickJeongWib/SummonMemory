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

    [SerializeField] Text Exp_Percent;
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

    #region Exp
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
    int Remove_Exp;
    int maxLv;
    int Left_Exp;
    int Set_Level;
    int Cumulative_Exp;
    #endregion
    // ---------
    #region Open
    public void Open_Growing_Panel()
    {
        if (GameManager.Instance.Get_SelectChar.Get_Character_Lv == GameManager.Instance.Get_SelectChar.Get_Max_Lv)
        {
            Set_Active_UI(false);
        }
        else
        {
            Set_Active_UI(true);
        }

        maxLv = GameManager.Instance.Get_SelectChar.Get_Max_Lv;

        // ĳ���� ���� ���� Text_UI
        Exp_Percent.text = $"{((((float)GameManager.Instance.Get_SelectChar.Get_CurrentExp / (float)Character_List.Require_Exp[GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1]) * 100).ToString("N0"))}%";
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
        ExpSlider.value = (float)GameManager.Instance.Get_SelectChar.Get_CurrentExp / (float)Character_List.Require_Exp[GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1];
       

        // ����
        Char_Star.rectTransform.sizeDelta = new Vector3(GameManager.Instance.Get_SelectChar.Get_CharStar * 20, 20, 0);
    }
    #endregion

    #region Close
    public void On_Click_Back()
    {
        SlimeMaxCount = 0;
        selectNum = 0;
        UseCount = 0;
        Exp_Sum = 0;
        Slime_Use_Slider.value = 0;

        for (int i = 0; i < SelectMask.Length; i++)
        {
            SelectMask[i].showMaskGraphic = false;
        }
    }
    #endregion

    // ������ ���� ���� �� ������ UI
    void Open_Slime_Select()
    {
        if (GameManager.Instance.Get_SelectChar.Get_Character_Lv == GameManager.Instance.Get_SelectChar.Get_Max_Lv)
            return;

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
        if (GameManager.Instance.Get_SelectChar.Get_Character_Lv == GameManager.Instance.Get_SelectChar.Get_Max_Lv)
            return;

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

        // ���� ����� �� �ִ� �������� �ִ�ġ�� �Է��ߴٸ�
        if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
        {
            // ���Ƚ���� �ʱ�ȭ
            UseCount = Exp_Sum / Use_Exp[selectNum];
            // ���� exp �ٽ� ����ϱ� ���� 0
            Exp_Sum = 0;

            // ���� ����ŭ �ݺ�
            for (int i = 0; i < UseCount; i++)
            {
                // �ٽ� ���� ���
                Exp_Sum += Use_Exp[selectNum];
                if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
                {
                    // ��밳�� �ʱ�ȭ
                    UseCount = i + 1;
                    // �����̴� �� �ʱ�ȭ
                    Slime_Use_Slider.value = (float)UseCount / (float)SlimeMaxCount;
                    break;
                }
            }
        }
        
        // UI_Refresh
        Exp_UI_Refresh();
        Refresh_Amount_Slime();
    }

    #region Slime_Amount_Calc
    // ������ ��뷮 ��ư ����
    public void On_Click_SlimeCount(int _num)
    {
        UseCount += _num;
        Exp_Sum = Use_Exp[selectNum] * UseCount;

        if (Exp_Sum <= 0)
            Exp_Sum = 0;

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
        Remove_Exp = 0;
        Left_Exp = 0;

        //int Lv = 0;
        for (int i = GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1; i < Character_List.Cumulative_Exp.Count; i++)
        {
            // �ִ� ���� ���� ��
            if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
            {
                // �ִ� ���� ���� ��
                // ����ġ�� ������
                ExpSlider.value = 0;
                Level_Text.text = $"LV.{maxLv}" +
                    $"<sprite=0><color=orange>{maxLv}</color>";
                Set_Level = maxLv;
                Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];

                // �ִ뷹���� �ʰ� �� ���ŵǴ� ����ġ
                Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];

                Slime_Use_Btns[0].interactable = false;
                Slime_Use_Slider.interactable = false;

                ExpSlider.value = 1;
                Exp_Percent.text = $"100%";
                // Remove_Exp = 0; // Ȥ�� �ٸ� ������ �ʿ��ϸ� 0���� �ʱ�ȭ
                break;
            }
            else
            {
                Slime_Use_Btns[0].interactable = true;
                Slime_Use_Slider.interactable = true;
            }

            // ��� exp�� ������ ������ ���� ����ġ���� ������ ĳ���� ���� ����
            if (Character_List.Cumulative_Exp[i] - GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp >= Exp_Sum)
            {
                // �ִ뷹�� �޼� ��
                if (Character_List.Cumulative_Exp[maxLv - 2] <= GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
                {
                    // �ִ� ���� ���� ��
                    // ����ġ�� ������
                    ExpSlider.value = 0;
                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";
                    Set_Level = maxLv;
                    Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];

                    // �ִ뷹���� �ʰ� �� ���ŵǴ� ����ġ
                    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];

                    Slime_Use_Btns[0].interactable = false;
                    Slime_Use_Slider.interactable = false;

                    ExpSlider.value = 1;
                    Exp_Percent.text = $"100%";
                    // Remove_Exp = 0; // Ȥ�� �ٸ� ������ �ʿ��ϸ� 0���� �ʱ�ȭ
                    break;
                }

                // ���� �ִ� ���� �̸��� ���
                Left_Exp = Character_List.Cumulative_Exp[i] - (GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum);
                Cumulative_Exp = Exp_Sum;
                Debug.Log(Character_List.Require_Exp[i]);
                Debug.Log("��������ġ : " + Character_List.Cumulative_Exp[i] + "exp �� : " + Exp_Sum);

                Exp_Percent.text = $"{((((float)Left_Exp) / (float)Character_List.Require_Exp[i]) * 100).ToString("N0")}%";
                Debug.Log($"{(float)Left_Exp} / { (float)Character_List.Require_Exp[i]}");

                ExpSlider.value = (float)Left_Exp / (float)Character_List.Require_Exp[i];
               
                if (Left_Exp == Character_List.Require_Exp[i])
                {
                    ExpSlider.value = 0;
                }

                Level_Text.text = $"LV.{i + 1}" +
                    $"<sprite=0><color=orange>{maxLv}</color>";

                Set_Level = i + 1;
                break;
            }  
        }
    }

    // ���� �� 
    public void Char_Leveling()
    {
        if (Remove_Exp != 0)
        {
            GameManager.Instance.Get_SelectChar.Get_CurrentExp = 0;
        }
        else
        {
            GameManager.Instance.Get_SelectChar.Get_CurrentExp = Left_Exp;
        }

        Remove_Exp = 0;
        Left_Exp = 0;
        ExpBack.showMaskGraphic = false;

        GameManager.Instance.Get_SelectChar.Get_Character_Lv = Set_Level;
        GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp += Cumulative_Exp;
        ExpSlider.value = 0;
        Exp_Percent.text = $"0%";
        if (GameManager.Instance.Get_SelectChar.Get_Character_Lv == GameManager.Instance.Get_SelectChar.Get_Max_Lv)
            Set_Active_UI(false);

        Debug.Log("Lv : " + GameManager.Instance.Get_SelectChar.Get_Character_Lv);
        Debug.Log("CurExp : " + GameManager.Instance.Get_SelectChar.Get_CurrentExp);
        Debug.Log("Cumu : " + GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp);
    }
}
