using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Desc : MonoBehaviour
{
    [SerializeField] Text Desc_Text;
    [SerializeField] Image Skill_Icon;

    // 스킬, 설명 보이게 할 것인지, 위치, 스킬 아이콘, 위치 값, 공격 버튼인지, 기본공격인지 스킬공격인지 
    public void Show_Skill_Desc(Skill _skill, bool _isOn, Transform _tr = null, Sprite _icon = null, float _setPos = 0, bool _isUseSkill = false, bool _isSkillBtn = true)
    {
        this.gameObject.SetActive(_isOn);
    
        // 아이콘을 눌렀다면
        if(_isOn)
        {
            // 디버프, 버프 스킬 설명이라면
            if(_isUseSkill == false)
            {
                RectTransform canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(_tr.position), Camera.main, out localPos);
                (this.transform as RectTransform).localPosition = localPos + Vector2.right * _setPos;
            }
            // 스킬버튼, 기본공격 버튼 설명이라면
            else
            {
                RectTransform canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(_tr.position), Camera.main, out localPos);
                (this.transform as RectTransform).localPosition = localPos + Vector2.up * _setPos;
            }

            // 설명에 아이콘 추가
            Skill_Icon.sprite = _icon;

            // 스킬이라면
            if (_isSkillBtn)
            {
                // 설명글 출력
                Desc_Text.text = _skill.Skill_Desc_Trans(_skill.Get_Skill_Desc, _skill.Get_Damage_Ratio, _skill.Get_Buff_Ratio, _skill.Get_SP_Hill_Count, _skill.Get_Buff_Ratio, _skill.Get_DeBuff_Ratio);
            }
            // 기본 공격이라면
            else
            {
                // 설명글 출력
                Desc_Text.text = _skill.Skill_Desc_Trans(_skill.Get_NormalAtk_Desc, _skill.Get_NormalAtk_Ratio);
            }
        }
    }
}
