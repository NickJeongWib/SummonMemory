using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharGrowUp_Mgr : MonoBehaviour
{
    [SerializeField] Text CombatPower;
    [SerializeField] Image Char_Star;
    [SerializeField] TextMeshProUGUI Level_Text;
    [SerializeField] TextMeshProUGUI HP_Value_Text;
    [SerializeField] TextMeshProUGUI ATK_Value_Text;
    [SerializeField] TextMeshProUGUI DEF_Value_Text;
    [SerializeField] TextMeshProUGUI CRIR_Value_Text;
    [SerializeField] TextMeshProUGUI CRID_Value_Text;


    public void Open_Growing_Panel()
    {
        Level_Text.text = $"LV.{GameManager.Instance.Get_SelectChar.Get_Character_Lv}" +
            $"<sprite=0><color=orange>{20 + (GameManager.Instance.Get_SelectChar.Get_CharStar * 10)}</color>";

        CombatPower.text = $"{GameManager.Instance.Get_SelectChar.Calc_CombatPower()}";
        HP_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseHP}";
        ATK_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseAtk}";
        DEF_Value_Text.text = $"{GameManager.Instance.Get_SelectChar.Get_BaseDef}";
        CRIR_Value_Text.text = $"{(GameManager.Instance.Get_SelectChar.Get_BaseCRIR * 100).ToString("N1")}%";
        CRID_Value_Text.text = $"{(GameManager.Instance.Get_SelectChar.Get_BaseCRID* 100).ToString("N1")}%";

        Char_Star.rectTransform.sizeDelta = new Vector3(GameManager.Instance.Get_SelectChar.Get_CharStar * 20, 20, 0);
    }
}
