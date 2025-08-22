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

    // ��ų ���� �����ֱ�
    public void Show_Skill_Info(string _skillName, string _desc, string _nextDesc, Sprite _icon, int _lv)
    {
        // ��ų �̸� UI �ʱ�ȭ
        Skill_Name.text = _skillName;
        // ��ų ������ UI �ʱ�ȭ
        Skill_Icon_Img.sprite = _icon;
        // ��ų ���� Text �ʱ�ȭ
        Skill_Cur_Desc.text = _desc;
        Skill_Next_Desc.text = _nextDesc;

        // ��ų ���� �ʱ�ȭ
        Skill_Lv = _lv;

        // ��ų ���� ǥ�� �ʱ�ȭ
        CurLv_Text.text = $"���� ��ų ���� (Lv.{Skill_Lv})";

        if (Skill_Lv != 20)
        {
            SkillUp_Btn.interactable = true;
            NextLv_Text.text = $"���� ��ų ���� (Lv.{Skill_Lv + 1})";
        }
        else
        {
            SkillUp_Btn.interactable = false;
            NextLv_Text.text = $"��ų ���� (Lv.Max ����)";
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
            // ��ȭ �Ҹ�
        }

        // ������ ��ų�̸�
        if (GameManager.Inst.Get_SelectChar.SkillData.Get_SkillType == SKILL_TYPE.ATTACK)
        {
            // ��� �ɷ�ġ ���� �����
            if (GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuffType == DEBUFF_TYPE.DEF)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0.05f, 0, 0, 0.01f);
            }
            else
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0.05f);
            }
        }
        else if (GameManager.Inst.Get_SelectChar.SkillData.Get_SkillType == SKILL_TYPE.BUFF) // ������ ��ų�̸�
        {
            // ������
            if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.HILL)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0, 0.01f, 0);
            }
            // SPȸ�� ����
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.SP_HILL)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0.2f, 0, 0);
            }
            // ����, ���ݷ� ���� ����
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.DEF ||
                GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.ATK)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0, 0.01f, 0);
            }
            // SP+���� ����
            else if (GameManager.Inst.Get_SelectChar.SkillData.Get_BuffType == BUFF_TYPE.ALL_BUFF)
            {
                GameManager.Inst.Get_SelectChar.SkillData.Increase_Ratio(0, 0.2f, 0.01f, 0);
            }
        }

        SkillUp_Info_Close();
        // UI �ʱ�ȭ
        Skill_Info_Init();
        // ĳ���� ���� ���� APIȣ��
        DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.CHARLIST);
    }

    #region Character_Skill_UI
    public void Skill_Info_Init()
    {
        // ��ų ���� ���� �� ��ų ���� �߰�
        string Desc = GameManager.Inst.Get_SelectChar.SkillData.Skill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_Damage_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_SP_Hill_Count,
            GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuff_Ratio);

        string NextDesc = "";

        if (GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv < 20)
        {
            // ���� ���� ��ų ���� ���� �� ��ų ���� �߰�
            NextDesc = GameManager.Inst.Get_SelectChar.SkillData.NextSkill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Desc,
                GameManager.Inst.Get_SelectChar.SkillData.Get_Damage_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_SP_Hill_Count,
                GameManager.Inst.Get_SelectChar.SkillData.Get_Buff_Ratio, GameManager.Inst.Get_SelectChar.SkillData.Get_DeBuff_Ratio);
        }
        else if (20 <= GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv)
        {
            NextDesc = "��ų �ִ� ���� �޼�";
        }

        // ��ų���� �ʱ�ȭ
        Show_Skill_Info(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Name, Desc, NextDesc,
        GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Icon, GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv);
    }
    #endregion
}
