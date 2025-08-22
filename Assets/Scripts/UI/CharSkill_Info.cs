using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharSkill_Info : MonoBehaviour
{
    [SerializeField] Text Skill_Name;
    [SerializeField] Image Skill_Icon_Img;
    [SerializeField] Text Skill_Cur_Desc;
    [SerializeField] Text Skill_Next_Desc;
    [SerializeField] int Skill_Lv;

    [SerializeField] Text CurLv_Text;
    [SerializeField] Text NextLv_Text;

    [SerializeField] GameObject Skill_Up_Info_Panel;
    [SerializeField] Button SkillUp_Btn;

    // 스킬 정보 보여주기
    public void Show_Skill_Info(string _skillName, string _desc, string _nextDesc, Sprite _icon, int _lv)
    {
        // 스킬 이름 UI 초기화
        Skill_Name.text = _skillName;
        // 스킬 아이콘 UI 초기화
        Skill_Icon_Img.sprite = _icon;
        // 스킬 설명 Text 초기화
        Skill_Cur_Desc.text = _desc;
        Skill_Next_Desc.text = _nextDesc;

        // 스킬 레벨 초기화
        Skill_Lv = _lv;

        // 스킬 레벨 표시 초기화
        CurLv_Text.text = $"현재 스킬 레벨 (Lv.{Skill_Lv})";

        if (Skill_Lv != 20)
        {
            SkillUp_Btn.interactable = true;
            NextLv_Text.text = $"다음 스킬 레벨 (Lv.{Skill_Lv + 1})";
        }
        else
        {
            SkillUp_Btn.interactable = false;
            NextLv_Text.text = $"스킬 레벨 (Lv.Max 도달)";
        }
    }

    public void On_Click_Skill_Up()
    {
        Skill_Up_Info_Panel.SetActive(true);
    }

    public void SkillUp_Info_Close()
    {
        Skill_Up_Info_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }

    public void On_Click_SelectSkill_Up()
    {
        if (GameManager.Inst.TestMode == false)
        {
            // 재화 소모
        }

        // 공격형 스킬이면
        if (GameManager.Inst.Get_SelectChar.SkillData.Get_SkillType == SKILL_TYPE.ATTACK)
        {
            // 상대 능력치 관련 디버프
            if (GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuffType == DEBUFF_TYPE.DEF)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0.05f, 0, 0, 0.01f);
            }
            else
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0.05f);
            }
        }
        else if (GameManager.Inst.Get_SelectChar.SkillData.Get_SkillType == SKILL_TYPE.BUFF) // 버프형 스킬이면
        {
            // 힐관련
            if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.HILL)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0, 0.01f, 0);
            }
            // SP회복 관련
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.SP_HILL)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0.2f, 0, 0);
            }
            // 방어력, 공격력 증가 관련
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.DEF ||
                GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.ATK)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0, 0.01f, 0);
            }
            // SP+공증 버프
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.ALL_BUFF)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0.2f, 0.01f, 0);
            }
        }

        SkillUp_Info_Close();
        // UI 초기화
        Skill_Info_Init();
        // 캐릭터 정보 저장 API호출
        DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.CHARLIST);
    }

    #region Character_Skill_UI
    public void Skill_Info_Init()
    {
        // 스킬 비율 변경 후 스킬 설명에 추가
        string Desc = GameManager.Inst.Get_SelectChar.SkillData.Skill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_Damage_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_SP_Hill_Count,
            GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuff_Ratio);

        string NextDesc = "";

        if (GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv < 20)
        {
            // 다음 레벨 스킬 비율 변경 후 스킬 설명에 추가
            NextDesc = GameManager.Inst.Get_SelectChar.SkillData.NextSkill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Desc,
                GameManager.Inst.Get_SelectChar.SkillData.Get_Damage_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_SP_Hill_Count,
                GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuff_Ratio);
        }
        else if (20 <= GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv)
        {
            NextDesc = "스킬 최대 레벨 달성";
        }

        // 스킬정보 초기화
        Show_Skill_Info(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Name, Desc, NextDesc,
        GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Icon, GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv);
    }
    #endregion
}
