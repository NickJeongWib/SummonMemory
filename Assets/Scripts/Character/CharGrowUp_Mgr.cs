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
        // 캐릭터 세부 사항 Text_UI
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

        // 돌파
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

        Exp_UI_Refresh();
        Refresh_Amount_Slime();
    }

    #region Slime_Amount_Calc
    // 슬라임 사용량 버튼 증감
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
        int Remove_Exp = 0;
        int Left_Exp = 0;
        //int Lv = 0;
        for (int i = 0; i < Character_List.Cumulative_Exp.Count; i++)
        {
            Debug.Log(i);
            // 사용 exp가 맞춰진 레벨의 누적 경험치보다 작으면 캐릭터 레벨 설정
            if (Character_List.Cumulative_Exp[i] >= Exp_Sum)
            {
                int maxLv = GameManager.Instance.Get_SelectChar.Get_Max_Lv;
                Debug.Log("maxLv " + maxLv);

                if (i >= maxLv)
                {
                    // 최대 레벨 도달 시
                    Debug.Log("최대 레벨 도달 : " + maxLv);

                    // 경험치는 버린다
                    ExpSlider.value = 0;

                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";

                    ExpSlider.value = 1;
                    Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }

                // 아직 최대 레벨 미만일 경우
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
                //    // 캐릭터 세부 사항 Text_UI
                //    Level_Text.text = $"LV.{i}" +
                //        $"<sprite=0><color=orange>{GameManager.Instance.Get_SelectChar.Get_Max_Lv}</color>";

                //    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[i - 1];

                //    Debug.Log("제거 경험치 : " + Remove_Exp);
                //    break;
                //}

                //Left_Exp = Character_List.Cumulative_Exp[i] - Exp_Sum;
                //ExpSlider.value = (float)Left_Exp / (float)Character_List.Require_Exp[i];

                //if (Left_Exp == Character_List.Require_Exp[i])
                //{
                //    ExpSlider.value = 0;
                //}

                //// 캐릭터 세부 사항 Text_UI
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
