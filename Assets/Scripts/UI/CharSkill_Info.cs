using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSkill_Info : MonoBehaviour
{
    [SerializeField] Text Skill_Name;
    [SerializeField] Image Skill_Icon_Img;
    [SerializeField] Text Skill_Cur_Desc;
    [SerializeField] Text Skill_Next_Desc;
    [SerializeField] int Skill_Lv;

    // 스킬 정보 보여주기
    public void Show_Skill_Info(string _skillName, string _desc, Sprite _icon, int _lv)
    {
        Skill_Name.text = _skillName;

        Skill_Icon_Img.sprite = _icon;
        Skill_Cur_Desc.text = _desc;

        Skill_Lv = _lv;
    }
}
