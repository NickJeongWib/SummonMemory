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

        // 캐릭터 세부 사항 Text_UI
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
       

        // 돌파
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

    // 슬라임 선택 했을 시 열리는 UI
    void Open_Slime_Select()
    {
        if (GameManager.Instance.Get_SelectChar.Get_Character_Lv == GameManager.Instance.Get_SelectChar.Get_Max_Lv)
            return;

        #region Select_Slime
        for (int i = 0; i < SelectMask.Length; i++)
        {
            if (i == 0)
            {
                if (UserInfo.InventoryDict.ContainsKey("옐로우 슬라임"))
                {
                    if (UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount;
                        break;
                    }
                }
            }
            else if (i == 1)
            {
                if (UserInfo.InventoryDict.ContainsKey("블루 슬라임"))
                {
                    if (UserInfo.InventoryDict["블루 슬라임"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["블루 슬라임"].Get_Amount;
                        break;
                    }
                }
            }
            else
            {
                if (UserInfo.InventoryDict.ContainsKey("레드 슬라임"))
                {
                    if (UserInfo.InventoryDict["레드 슬라임"].Get_Amount >= 1)
                    {
                        SelectMask[i].showMaskGraphic = true;
                        selectNum = i;
                        SlimeMaxCount = UserInfo.InventoryDict["레드 슬라임"].Get_Amount;
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
        // 인벤토리에 옐로우 슬라임이 존재할 시
        if (UserInfo.InventoryDict.ContainsKey("옐로우 슬라임"))
        {
            Low_Slime_Amout.text = $"{UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount}";
        }
        else
        {
            Low_Slime_Amout.text = $"{0}";
        }

        // 인벤토리에 블루 슬라임이 존재할 시
        if (UserInfo.InventoryDict.ContainsKey("블루 슬라임"))
        {
            Middle_Slime_Amout.text = $"{UserInfo.InventoryDict["블루 슬라임"].Get_Amount}";
        }
        else
        {
            Middle_Slime_Amout.text = $"{0}";
        }

        // 인벤토리에 레드 슬라임이 존재할 시
        if (UserInfo.InventoryDict.ContainsKey("레드 슬라임"))
        {
            High_Slime_Amout.text = $"{UserInfo.InventoryDict["레드 슬라임"].Get_Amount}";
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
        // 선택한 슬롯 제외하고 선택 시 나오는 이미지 비활성화
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
        // 옐로우 슬라임
        if (_num == 0)
        {
            // 인벤토리에 옐로우 슬라임이 존재할 시
            if (UserInfo.InventoryDict.ContainsKey("옐로우 슬라임") && UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount;
                Set_Active_UI(true);
            }
            else if (!UserInfo.InventoryDict.ContainsKey("옐로우 슬라임") || UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        else if (_num == 1)
        {
            // 인벤토리에 블루 슬라임이 존재할 시
            if (UserInfo.InventoryDict.ContainsKey("블루 슬라임") && UserInfo.InventoryDict["블루 슬라임"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["블루 슬라임"].Get_Amount;
                Set_Active_UI(true);
            }
            else if(!UserInfo.InventoryDict.ContainsKey("블루 슬라임") || UserInfo.InventoryDict["블루 슬라임"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        else if (_num == 2)
        {
            // 인벤토리에 레드 슬라임이 존재할 시
            if (UserInfo.InventoryDict.ContainsKey("레드 슬라임") && UserInfo.InventoryDict["레드 슬라임"].Get_Amount > 0)
            {
                SlimeMaxCount = UserInfo.InventoryDict["레드 슬라임"].Get_Amount;
                Set_Active_UI(true);
            }
            else if (!UserInfo.InventoryDict.ContainsKey("레드 슬라임") || UserInfo.InventoryDict["레드 슬라임"].Get_Amount <= 0)
            {
                SlimeMaxCount = 0;
                Set_Active_UI(false);
            }
        }
        #endregion

        // 슬라이더 value
        Slime_Use_Slider.value = ((float)1 / (float)SlimeMaxCount);

        Cal_Use_Slime();
        Refresh_Amount_Slime();

        Exp_UI_Refresh();
    }

    #region UI_Refresh
    void Set_Active_UI(bool _isOn)
    {
        // 보유 슬라임의 수량에 따라 비/활성화
        for (int i = 0; i < Slime_Use_Btns.Length; i++)
        {
            Slime_Use_Btns[i].interactable = _isOn;
        }

        LevelUP_Btn.interactable = _isOn;
        Slime_Use_Slider.interactable = _isOn;
    }

    public void Refresh_Slider()
    {
        // 슬라이더 변화에 따라 호출되는 함수
        UseCount = Mathf.RoundToInt(SlimeMaxCount * Slime_Use_Slider.value);
        Exp_Sum = Use_Exp[selectNum] * UseCount;

        // 만약 사용할 수 있는 슬라임의 최대치를 입력했다면
        if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
        {
            // 사용횟수를 초기화
            UseCount = Exp_Sum / Use_Exp[selectNum];
            // 사용된 exp 다시 계산하기 위해 0
            Exp_Sum = 0;

            // 사용된 값만큼 반복
            for (int i = 0; i < UseCount; i++)
            {
                // 다시 총합 계산
                Exp_Sum += Use_Exp[selectNum];
                if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
                {
                    // 사용개수 초기화
                    UseCount = i + 1;
                    // 슬라이더 값 초기화
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
    // 슬라임 사용량 버튼 증감
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
        // UI 텍스트 수정
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
        // Exp 총합, 사용갯수 초기화
        UseCount = Mathf.RoundToInt(SlimeMaxCount * Slime_Use_Slider.value);
        Exp_Sum = Use_Exp[selectNum] * UseCount;
    }
    #endregion

    void Exp_UI_Refresh()
    {
        // 현재 레벨 요구 경험치보다 많으면 뒷 배경 주황색으로 변경
        if (Character_List.Require_Exp[GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1] <= Exp_Sum)
        {
            ExpBack.showMaskGraphic = true;
        }
        else
        {
            ExpBack.showMaskGraphic = false;
        }

        // 남은 경험치
        Remove_Exp = 0;
        Left_Exp = 0;

        //int Lv = 0;
        for (int i = GameManager.Instance.Get_SelectChar.Get_Character_Lv - 1; i < Character_List.Cumulative_Exp.Count; i++)
        {
            // 최대 레벨 도달 시
            if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
            {
                // 최대 레벨 도달 시
                // 경험치는 버린다
                ExpSlider.value = 0;
                Level_Text.text = $"LV.{maxLv}" +
                    $"<sprite=0><color=orange>{maxLv}</color>";
                Set_Level = maxLv;
                Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];

                // 최대레벨을 초과 시 제거되는 경험치
                Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];

                Slime_Use_Btns[0].interactable = false;
                Slime_Use_Slider.interactable = false;

                ExpSlider.value = 1;
                Exp_Percent.text = $"100%";
                // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                break;
            }
            else
            {
                Slime_Use_Btns[0].interactable = true;
                Slime_Use_Slider.interactable = true;
            }

            // 사용 exp가 맞춰진 레벨의 누적 경험치보다 작으면 캐릭터 레벨 설정
            if (Character_List.Cumulative_Exp[i] - GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp >= Exp_Sum)
            {
                // 최대레벨 달성 시
                if (Character_List.Cumulative_Exp[maxLv - 2] <= GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
                {
                    // 최대 레벨 도달 시
                    // 경험치는 버린다
                    ExpSlider.value = 0;
                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";
                    Set_Level = maxLv;
                    Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];

                    // 최대레벨을 초과 시 제거되는 경험치
                    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];

                    Slime_Use_Btns[0].interactable = false;
                    Slime_Use_Slider.interactable = false;

                    ExpSlider.value = 1;
                    Exp_Percent.text = $"100%";
                    // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }

                // 아직 최대 레벨 미만일 경우
                Left_Exp = Character_List.Cumulative_Exp[i] - (GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum);
                Cumulative_Exp = Exp_Sum;
                Debug.Log(Character_List.Require_Exp[i]);
                Debug.Log("누적경험치 : " + Character_List.Cumulative_Exp[i] + "exp 합 : " + Exp_Sum);

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

    // 레벨 업 
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
