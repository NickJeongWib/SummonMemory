using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharSkill_Info : MonoBehaviour
{
    int UP_Index;

    [Header("Skill")]
    [SerializeField] Text Skill_Name;
    [SerializeField] int Skill_Lv;
    [SerializeField] Image Skill_Icon_Img;
    [SerializeField] Text Skill_Cur_Desc;
    [SerializeField] Text Skill_Next_Desc;
    [SerializeField] Text CurLv_Text;
    [SerializeField] Text NextLv_Text;

    [SerializeField] GameObject Skill_Up_Info_Panel;
    [SerializeField] Button SkillUp_Btn;

    [Header("NormalAtk")]
    [SerializeField] int NormalAtk_Lv;
    [SerializeField] Sprite[] NormalAtk_Icons;
    [SerializeField] Image NormalAtk_Icon;
    [SerializeField] Text NormalAtk_Cur_Desc;
    [SerializeField] Text NormalAtk_Next_Desc;
    [SerializeField] Text NormalAtk_CurLv_Text;
    [SerializeField] Text NormalAtk_NextLv_Text;
    [SerializeField] Button NormalAtkUp_Btn;

    // 스킬 정보 보여주기
    public void Show_Skill_Info(string _skillName, string _skillDesc, string _nextSkillDesc, Sprite _icon, int _skillLv, int _normalAtkLv,
        string _normalAtkDesc, string _nextNormalAtkDesc)
    {
        // 스킬 이름 UI 초기화
        Skill_Name.text = _skillName;
        // 스킬 아이콘 UI 초기화
        Skill_Icon_Img.sprite = _icon;
        // 스킬 설명 Text 초기화
        Skill_Cur_Desc.text = _skillDesc;
        Skill_Next_Desc.text = _nextSkillDesc;

        // 스킬 레벨 초기화
        Skill_Lv = _skillLv;

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

        // ----- 기본공격 UI 초기화 ------
        NormalAtk_Icon.sprite = NormalAtk_Icons[(int)GameManager.Inst.SelectCharacter.Get_CharElement];
        NormalAtk_Lv = _normalAtkLv;
        NormalAtk_Cur_Desc.text = _normalAtkDesc;
        NormalAtk_Next_Desc.text = _nextNormalAtkDesc;

        // 스킬 레벨 표시 초기화
        NormalAtk_CurLv_Text.text = $"현재 스킬 레벨 (Lv.{NormalAtk_Lv})";

        if (NormalAtk_Lv != 20)
        {
            NormalAtkUp_Btn.interactable = true;
            NormalAtk_NextLv_Text.text = $"다음 스킬 레벨 (Lv.{NormalAtk_Lv + 1})";
        }
        else
        {
            NormalAtkUp_Btn.interactable = false;
            NormalAtk_NextLv_Text.text = $"스킬 레벨 (Lv.Max 도달)";
        }
    }

    public void On_Click_Skill_Up(int _index)
    {
        UP_Index = _index;
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

        if (UP_Index == 0)
        {
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
        }
        else if (UP_Index == 1)
        {
            GameManager.Inst.Get_SelectChar.SkillData.Increase_NormalRatio(0.05f);
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
        // ---- 스킬 공격 설명 ----
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

        // ------- 기본공격 설명 --------
        string normalAtk_Desc = GameManager.Inst.Get_SelectChar.SkillData.Skill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Ratio);

        string NextNormal_Desc = "";

        if (GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv < 20)
        {
            // 다음 레벨 스킬 비율 변경 후 스킬 설명에 추가
            NextNormal_Desc = GameManager.Inst.Get_SelectChar.SkillData.NextSkill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Ratio);
        }
        else if (20 <= GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv)
        {
            NextNormal_Desc = "스킬 최대 레벨 달성";
        }

        // 스킬정보 초기화
        Show_Skill_Info(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Name, Desc, NextDesc,
        GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Icon, GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv,
        GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv, normalAtk_Desc, NextNormal_Desc);
    }
    #endregion
}
