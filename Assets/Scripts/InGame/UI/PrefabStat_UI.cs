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

    // 동적으로 생성 시 호출되는 함수
    // UI 초기화 목적으로 사용
    public void Set_UI(Sprite _icon, string _name, int _index = 0)
    {
        PosIndex = _index;

        Name_Text.text = _name;
        CharProfile.sprite = _icon;
        HP_Text.text = "100.0%";
    }

    // 캐릭터나 몬스터가 데미지 입었을 시 HP바 비율 계산하기 위한 함수
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
