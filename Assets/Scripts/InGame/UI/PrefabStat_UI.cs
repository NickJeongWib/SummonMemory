using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabStat_UI : MonoBehaviour
{
    public int PosIndex;
    [SerializeField] Image CharProfile;
    [SerializeField] Text Name_Text;
    [SerializeField] Text HP_Text;
    [SerializeField] Image HP_Bar;
    [SerializeField] GameObject Death_UI;
    [SerializeField] Transform Buff_Tr;
    public Transform Get_Buff_Tr { get => Buff_Tr; }

    public void Set_UI(Sprite _icon, string _name, int _index = 0)
    {
        PosIndex = _index;

        Name_Text.text = _name;
        CharProfile.sprite = _icon;
        HP_Text.text = "100.0%";
    }

    public void Set_HP(float _value)
    {
        HP_Text.text = $"{(_value * 100.0f).ToString("N1")}%";
        HP_Bar.fillAmount = _value;

        if(_value <= 0)
        {
            Death_UI.SetActive(true);
        }
    }
}
