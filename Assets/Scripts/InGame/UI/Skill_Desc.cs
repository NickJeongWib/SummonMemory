using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Desc : MonoBehaviour
{
    [SerializeField] Text Desc_Text;
    [SerializeField] Image Skill_Icon;

    // ��ų, ���� ���̰� �� ������, ��ġ, ��ų ������, ��ġ ��, ���� ��ư����, �⺻�������� ��ų�������� 
    public void Show_Skill_Desc(Skill _skill, bool _isOn, Transform _tr = null, Sprite _icon = null, float _setPos = 0, bool _isUseSkill = false, bool _isSkillBtn = true)
    {
        this.gameObject.SetActive(_isOn);
    
        // �������� �����ٸ�
        if(_isOn)
        {
            // �����, ���� ��ų �����̶��
            if(_isUseSkill == false)
            {
                RectTransform canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(_tr.position), Camera.main, out localPos);
                (this.transform as RectTransform).localPosition = localPos + Vector2.right * _setPos;
            }
            // ��ų��ư, �⺻���� ��ư �����̶��
            else
            {
                RectTransform canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(_tr.position), Camera.main, out localPos);
                (this.transform as RectTransform).localPosition = localPos + Vector2.up * _setPos;
            }

            // ���� ������ �߰�
            Skill_Icon.sprite = _icon;

            // ��ų�̶��
            if (_isSkillBtn)
            {
                // ����� ���
                Desc_Text.text = _skill.Skill_Desc_Trans(_skill.Get_Skill_Desc, _skill.Get_Damage_Ratio, _skill.Get_Buff_Ratio, _skill.Get_SP_Hill_Count, _skill.Get_Buff_Ratio, _skill.Get_DeBuff_Ratio);
            }
            // �⺻ �����̶��
            else
            {
                // ����� ���
                Desc_Text.text = _skill.Skill_Desc_Trans(_skill.Get_NormalAtk_Desc, _skill.Get_NormalAtk_Ratio);
            }
        }
    }
}
