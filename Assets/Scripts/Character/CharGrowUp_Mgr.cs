using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharGrowUp_Mgr : MonoBehaviour
{
    // Variable
    [SerializeField] Inventory_UI InventoryUI_Ref;

    #region Text_UI 
    [Header("Text_UI")]
    [SerializeField] Text CombatPower;
    [SerializeField] TextMeshProUGUI Level_Text;
    [SerializeField] TextMeshProUGUI HP_Value_Text;
    [SerializeField] TextMeshProUGUI ATK_Value_Text;
    [SerializeField] TextMeshProUGUI DEF_Value_Text;
    [SerializeField] TextMeshProUGUI CRIR_Value_Text;
    [SerializeField] TextMeshProUGUI CRID_Value_Text;
    [SerializeField] Text Info_Char_Lv;

    [SerializeField] Text Exp_Percent;
    [SerializeField] Text Low_Slime_Amount;
    [SerializeField] Text Middle_Slime_Amount;
    [SerializeField] Text High_Slime_Amount;
    [SerializeField] Text[] Slime_Use;
    #endregion

    #region UI
    [Header("Panel_UI")]
    [SerializeField] Image Char_Star;
    [SerializeField] Mask[] SelectMask;
    [SerializeField] Button[] Slime_Use_Btns;
    [SerializeField] Button LevelUP_Btn;
    [SerializeField] Slider Slime_Use_Slider;
    [SerializeField] SpriteRenderer Char_Pixel_Img;
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
        // 이미지 변경
        Char_Pixel_Img.sprite = GameManager.Inst.Get_SelectChar.Get_Pixel_Img;

        maxLv = GameManager.Inst.Get_SelectChar.Get_Max_Lv;
        // 캐릭터 세부 사항 Text_UI
        Exp_Percent.text = $"{((((float)GameManager.Inst.Get_SelectChar.Get_CurrentExp / (float)Character_List.Require_Exp[GameManager.Inst.Get_SelectChar.Get_Character_Lv - 1]) * 100).ToString("N2"))}%";
        Level_Text.text = $"LV.{GameManager.Inst.Get_SelectChar.Get_Character_Lv}" +
            $"<sprite=0><color=orange>{GameManager.Inst.Get_SelectChar.Get_Max_Lv}</color>";

        CombatPower.text = $"{GameManager.Inst.Get_SelectChar.Calc_CombatPower()}";

        HP_Value_Text.text = $"{GameManager.Inst.Get_SelectChar.Get_BaseHP.ToString("N0")}";
        ATK_Value_Text.text = $"{GameManager.Inst.Get_SelectChar.Get_BaseAtk.ToString("N0")}";
        DEF_Value_Text.text = $"{GameManager.Inst.Get_SelectChar.Get_BaseDef.ToString("N0")}";
        CRIR_Value_Text.text = $"{(GameManager.Inst.Get_SelectChar.Get_BaseCRIR * 100).ToString("N1")}%";
        CRID_Value_Text.text = $"{(GameManager.Inst.Get_SelectChar.Get_BaseCRID * 100).ToString("N1")}%"; 

        Open_Slime_Select();
      
        if (GameManager.Inst.Get_SelectChar.Get_Character_Lv == GameManager.Inst.Get_SelectChar.Get_Max_Lv)
        {
            Set_Active_UI(false);
            Exp_Percent.text = $"100%";
            ExpSlider.value = 1;
        }
        else
        {  
            // UI Slider
            ExpSlider.value = (float)GameManager.Inst.Get_SelectChar.Get_CurrentExp / (float)Character_List.Require_Exp[GameManager.Inst.Get_SelectChar.Get_Character_Lv - 1];
            Set_Active_UI(true);
        }

        // 돌파
        Char_Star.rectTransform.sizeDelta = new Vector3(GameManager.Inst.Get_SelectChar.Get_CharStar * 20, 20, 0);

        On_Click_Slime(0);
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
        if (GameManager.Inst.Get_SelectChar.Get_Character_Lv == GameManager.Inst.Get_SelectChar.Get_Max_Lv)
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
            Low_Slime_Amount.text = $"{UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount}";
        }
        else
        {
            Low_Slime_Amount.text = $"{0}";
        }

        // 인벤토리에 블루 슬라임이 존재할 시
        if (UserInfo.InventoryDict.ContainsKey("블루 슬라임"))
        {
            Middle_Slime_Amount.text = $"{UserInfo.InventoryDict["블루 슬라임"].Get_Amount}";
        }
        else
        {
            Middle_Slime_Amount.text = $"{0}";
        }

        // 인벤토리에 레드 슬라임이 존재할 시
        if (UserInfo.InventoryDict.ContainsKey("레드 슬라임"))
        {
            High_Slime_Amount.text = $"{UserInfo.InventoryDict["레드 슬라임"].Get_Amount}";
        }
        else
        {
            High_Slime_Amount.text = $"{0}";
        }
        #endregion
    }

    public void On_Click_Slime(int _num)
    {
        if (GameManager.Inst.Get_SelectChar.Get_Character_Lv == GameManager.Inst.Get_SelectChar.Get_Max_Lv)
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

        // maxLv의 누적 경험치량보다 지금 현재 캐릭터의 누적 경험치 + 슬라임의 경험치가 더 많을 시(최대 레벨 달성하고도 경험치가 남는다면)
        if (Character_List.Cumulative_Exp[maxLv - 2] <= GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
        {
            // 사용횟수를 초기화
            UseCount = Exp_Sum / Use_Exp[selectNum];

            // 사용된 exp 다시 계산하기 위해 0
            Exp_Sum = 0;

            Slime_Use_Btns[0].interactable = false;
            Slime_Use_Slider.interactable = false;

            // 사용된 값만큼 반복
            for (int i = 0; i < UseCount; i++)
            {
                // 다시 총합 계산
                Exp_Sum += Use_Exp[selectNum];

                // 초기화 시켜 슬라임 요구치의 최대치로 설정하기 위해
                if (Character_List.Cumulative_Exp[maxLv - 2] <= GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
                {
                    // 사용개수 초기화
                    UseCount = i + 1;
                    // 슬라이더 값 초기화
                    Set_Level = 70;

                    // 최대레벨 도달했으니 EXP 경험치바는 100%로
                    ExpSlider.value = 1;
                    Exp_Percent.text = $"100%";

                    // UI 계산
                    Level_Text.text = $"LV.{maxLv}" +
                       $"<sprite=0><color=orange>{maxLv}</color>";
                    GameManager.Inst.Get_SelectChar.Calculate_State(70, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                    // Debug.Log((GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum - Character_List.Cumulative_Exp[67]) / (Character_List.Cumulative_Exp[68] - Character_List.Cumulative_Exp[67]));
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
        // Debug.Log("3 " + Exp_Sum);

        if (Exp_Sum <= 0)
            Exp_Sum = 0;

        // UI 비/활성화 
        if (Character_List.Cumulative_Exp[GameManager.Inst.Get_SelectChar.Get_Max_Lv - 2] <= Exp_Sum)
        {
            Slime_Use_Btns[0].interactable = false;
            Slime_Use_Slider.interactable = false;
        }
        else
        {
            Slime_Use_Btns[0].interactable = true;
            Slime_Use_Slider.interactable = true;
        }

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

    #region Exp_UI_Refresh
    void Exp_UI_Refresh()
    {
        // 현재 레벨 요구 경험치보다 많으면 뒷 배경 주황색으로 변경
        if (Character_List.Require_Exp[GameManager.Inst.Get_SelectChar.Get_Character_Lv - 1] <= GameManager.Inst.Get_SelectChar.Get_CurrentExp + Exp_Sum)
        {
            ExpBack.showMaskGraphic = true;
            // Show_Up_Stat(true);

        }
        else
        {
            ExpBack.showMaskGraphic = false;
            // Show_Up_Stat(false);
        }
        // Debug.Log("69. " + Character_List.Cumulative_Exp[69]);
        // Debug.Log("68. " + Character_List.Cumulative_Exp[68]);
        // 남은 경험치
        Remove_Exp = 0;
        Left_Exp = 0;

        //int Lv = 0;
        for (int i = GameManager.Inst.Get_SelectChar.Get_Character_Lv - 1; i < Character_List.Cumulative_Exp.Count; i++)
        {
            // 최대 레벨 도달 시
            if (Character_List.Cumulative_Exp[maxLv - 2] <= Exp_Sum)
            {
                if (maxLv == 70)
                {
                    Set_Level = 70;
                    ExpSlider.value = 1;
                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";
                    Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];
                    // 최대레벨을 초과 시 제거되는 경험치
                    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];
                    Slime_Use_Btns[0].interactable = false;
                    Slime_Use_Slider.interactable = false;
                    ExpSlider.value = 1;
                    Exp_Percent.text = $"100%";
                    GameManager.Inst.Get_SelectChar.Calculate_State(maxLv, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                    // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }
                else
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
                    GameManager.Inst.Get_SelectChar.Calculate_State(maxLv, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                    // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }
            }

            // 사용 exp가 맞춰진 레벨의 누적 경험치보다 작으면 캐릭터 레벨 설정
            if (Character_List.Cumulative_Exp[i] - GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp >= Exp_Sum)
            {
                // 최대레벨 달성 시
                if (Character_List.Cumulative_Exp[maxLv - 2] <= GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
                {
                    // Debug.Log("1. " + Character_List.Cumulative_Exp[69]);
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

                    GameManager.Inst.Get_SelectChar.Calculate_State(maxLv, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                    // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }

                // Debug.Log("CharExp + SUM : " + (GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum));

                if (Character_List.Cumulative_Exp[68] <= GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum)
                {
                    Set_Level = 70;
                    ExpSlider.value = 1;
                    Level_Text.text = $"LV.{maxLv}" +
                        $"<sprite=0><color=orange>{maxLv}</color>";
                    Cumulative_Exp = Character_List.Cumulative_Exp[maxLv - 2];
                    // 최대레벨을 초과 시 제거되는 경험치
                    Remove_Exp = Exp_Sum - Character_List.Cumulative_Exp[maxLv - 2];
                    Slime_Use_Btns[0].interactable = false;
                    Slime_Use_Slider.interactable = false;
                    ExpSlider.value = 1;
                    Exp_Percent.text = $"100%";
                    GameManager.Inst.Get_SelectChar.Calculate_State(maxLv, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                    // Remove_Exp = 0; // 혹시 다른 곳에서 필요하면 0으로 초기화
                    break;
                }

                
                // 남는 경험치를 계산하기 위한 코드
                if (i - 1 <= 0)
                {
                    // i - 1이 0보다 작으면 인덱스값을 이탈해서 -1까지 대입이 가능해서 0보다 작으면 0으로
                    Left_Exp = Mathf.Abs(Character_List.Cumulative_Exp[0] - (GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum));
                }
                else
                {
                    Left_Exp = Mathf.Abs(Character_List.Cumulative_Exp[i- 1] - (GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp + Exp_Sum));
                }
               
                // 누적 경험치는 사용된 슬라임의 경험치 양
                Cumulative_Exp = Exp_Sum;

                
                //Debug.Log("i : " + i + "\n사용 누적 경험치 : " + Character_List.Cumulative_Exp[i] + "\n사용 exp 합 : " + Exp_Sum + "\n현재 캐릭터 누적 경험치 : " +
                //    GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp + "\nLeftExp : " + Left_Exp + "\nLv : " + Character_List.Level[i]);

                // 남는 경험치가 현재레벨의 요구치랑 값이 똑같지 않다면
                if (Left_Exp != Character_List.Require_Exp[i])
                {
                    Exp_Percent.text = $"{((((float)Left_Exp) / (float)Character_List.Require_Exp[i]) * 100).ToString("N2")}%";
                }
                else
                {
                    // 만약 똑같다면 100%로 표기되기 때문에 빼줘서 0%로 만들어준다.
                    Exp_Percent.text = $"{Left_Exp - Character_List.Require_Exp[i]}%";
                }

                ExpSlider.value = (float)Left_Exp / (float)Character_List.Require_Exp[i];
               
                if (Left_Exp == Character_List.Require_Exp[i])
                {
                    ExpSlider.value = 0;
                }

                Level_Text.text = $"LV.{i + 1}" +
                    $"<sprite=0><color=orange>{maxLv}</color>";

                Set_Level = i + 1;
                GameManager.Inst.Get_SelectChar.Calculate_State(i + 1, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);
                break;
            }  
        }
    }
#endregion

    #region Char_Lv_Up
    // 레벨 업 
    public void Char_Leveling()
    {
        if (Remove_Exp != 0)
        {
            GameManager.Inst.Get_SelectChar.Get_CurrentExp = 0;
        }
        else
        {
            GameManager.Inst.Get_SelectChar.Get_CurrentExp = Left_Exp;
        }

        if (!GameManager.Inst.TestMode)
        {
            if (selectNum == 0)
            {
                UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount -= UseCount;
                SlimeMaxCount = UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount;
                Low_Slime_Amount.text = $"{UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount}";

                if (UserInfo.InventoryDict["옐로우 슬라임"].Get_Amount <= 0)
                {
                    Set_Active_UI(false);
                }

            }
            else if (selectNum == 1)
            {
                UserInfo.InventoryDict["블루 슬라임"].Get_Amount -= UseCount;
                SlimeMaxCount = UserInfo.InventoryDict["블루 슬라임"].Get_Amount;
                Middle_Slime_Amount.text = $"{UserInfo.InventoryDict["블루 슬라임"].Get_Amount}";

                if (UserInfo.InventoryDict["블루 슬라임"].Get_Amount <= 0)
                {
                    Set_Active_UI(false);
                }
            }
            else if (selectNum == 2)
            {
                UserInfo.InventoryDict["레드 슬라임"].Get_Amount -= UseCount;
                SlimeMaxCount = UserInfo.InventoryDict["레드 슬라임"].Get_Amount;
                High_Slime_Amount.text = $"{UserInfo.InventoryDict["레드 슬라임"].Get_Amount}";

                if (UserInfo.InventoryDict["레드 슬라임"].Get_Amount <= 0)
                {
                    Set_Active_UI(false);
                }
            }

            // 인벤토리 초기화
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Upgrade_Inventory();
            InventoryUI_Ref.Upgrade_Slot_Refresh();

        }

        Remove_Exp = 0;
        Left_Exp = 0;
        ExpBack.showMaskGraphic = false;

        // Debug.Log(Set_Level);
        GameManager.Inst.Get_SelectChar.Get_Character_Lv = Set_Level;
        GameManager.Inst.Get_SelectChar.Get_Cumulative_Exp += Cumulative_Exp;

        Slime_Use_Slider.value = 0;
        Info_Char_Lv.text = $"{GameManager.Inst.Get_SelectChar.Get_CharName} Lv.{GameManager.Inst.Get_SelectChar.Get_Character_Lv}";

        // 스탯 적용
        GameManager.Inst.Get_SelectChar.LevelUp();
        GameManager.Inst.Get_SelectChar.Calculate_State(GameManager.Inst.Get_SelectChar.Get_Character_Lv, HP_Value_Text, ATK_Value_Text, DEF_Value_Text, CRIR_Value_Text, CRID_Value_Text);

        CombatPower.text = $"{GameManager.Inst.Get_SelectChar.Calc_CombatPower()}";

        if (GameManager.Inst.Get_SelectChar.Get_Character_Lv == GameManager.Inst.Get_SelectChar.Get_Max_Lv)
        {
            Set_Active_UI(false);
            ExpSlider.value = 1;
            Exp_Percent.text = "100%";
        }
        
        
        // TODO ## CharGrowUp_Mgr 캐릭터 레벨업 시 소모 슬라임 데이터 저장 및 캐릭터 데이터 저장
        // 캐릭터가 장착 중이라면
        if (UserInfo.Equip_Characters.Contains(GameManager.Inst.Get_SelectChar))
        {
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CLEAR_EQUIP_CHAR);
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        }
        else // 캐릭터가 장착 중이 아니면
        {
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        }

        UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();

        // Debug.Log("Lv : " + GameManager.Instance.Get_SelectChar.Get_Character_Lv);
        // Debug.Log("CurExp : " + GameManager.Instance.Get_SelectChar.Get_CurrentExp);
        // Debug.Log("Cumu : " + GameManager.Instance.Get_SelectChar.Get_Cumulative_Exp);
    }
    #endregion
}
