using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class BuffIcon_UI : MonoBehaviour
{
    [SerializeField] BUFF_TYPE BuffType;
    [SerializeField] Character_Ctrl CharCtrl;

    [SerializeField] int Skill_Turn;
    [SerializeField] float BuffValue;

    [SerializeField] Image Skill_Icon;
    [SerializeField] Text Skill_Turn_Txt;

    // 스킬 버프 UI 초기화
    public void Set_Skill_UI(Character_Ctrl _charCtrl, Transform _parent, Sprite _sprite, int _turn, float _buffValue, BUFF_TYPE _buffType)
    {
        CharCtrl = _charCtrl;
        BuffValue = _buffValue;
        this.transform.SetParent(_parent, false);
        Skill_Icon.sprite = _sprite;
        BuffType = _buffType;

        Skill_Turn = _turn;
        Skill_Turn_Txt.text = Skill_Turn.ToString();
    }

    public void Turn_Decreased()
    {
        // 턴 감소
        Skill_Turn--;
        Skill_Turn_Txt.text = Skill_Turn.ToString();

        if (Skill_Turn <= 0)
        {
            // 현재 아이콘의 버프 타입 전달
            CharCtrl.EndBuff(BuffType, BuffValue);
            CharCtrl.CanSkill = true;
            this.gameObject.SetActive(false);
        }
    }
}
