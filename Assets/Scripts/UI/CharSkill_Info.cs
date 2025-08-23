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

    // ��ų ���� �����ֱ�
    public void Show_Skill_Info(string _skillName, string _skillDesc, string _nextSkillDesc, Sprite _icon, int _skillLv, int _normalAtkLv,
        string _normalAtkDesc, string _nextNormalAtkDesc)
    {
        // ��ų �̸� UI �ʱ�ȭ
        Skill_Name.text = _skillName;
        // ��ų ������ UI �ʱ�ȭ
        Skill_Icon_Img.sprite = _icon;
        // ��ų ���� Text �ʱ�ȭ
        Skill_Cur_Desc.text = _skillDesc;
        Skill_Next_Desc.text = _nextSkillDesc;

        // ��ų ���� �ʱ�ȭ
        Skill_Lv = _skillLv;

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

        // ----- �⺻���� UI �ʱ�ȭ ------
        NormalAtk_Icon.sprite = NormalAtk_Icons[(int)GameManager.Inst.SelectCharacter.Get_CharElement];
        NormalAtk_Lv = _normalAtkLv;
        NormalAtk_Cur_Desc.text = _normalAtkDesc;
        NormalAtk_Next_Desc.text = _nextNormalAtkDesc;

        // ��ų ���� ǥ�� �ʱ�ȭ
        NormalAtk_CurLv_Text.text = $"���� ��ų ���� (Lv.{NormalAtk_Lv})";

        if (NormalAtk_Lv != 20)
        {
            NormalAtkUp_Btn.interactable = true;
            NormalAtk_NextLv_Text.text = $"���� ��ų ���� (Lv.{NormalAtk_Lv + 1})";
        }
        else
        {
            NormalAtkUp_Btn.interactable = false;
            NormalAtk_NextLv_Text.text = $"��ų ���� (Lv.Max ����)";
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
            // ��ȭ �Ҹ�
        }

        if (UP_Index == 0)
        {
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
        }
        else if (UP_Index == 1)
        {
            GameManager.Inst.Get_SelectChar.SkillData.Increase_NormalRatio(0.05f);
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
        // ---- ��ų ���� ���� ----
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

        // ------- �⺻���� ���� --------
        string normalAtk_Desc = GameManager.Inst.Get_SelectChar.SkillData.Skill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Ratio);

        string NextNormal_Desc = "";

        if (GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv < 20)
        {
            // ���� ���� ��ų ���� ���� �� ��ų ���� �߰�
            NextNormal_Desc = GameManager.Inst.Get_SelectChar.SkillData.NextSkill_Desc_Trans(GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Desc,
            GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Ratio);
        }
        else if (20 <= GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv)
        {
            NextNormal_Desc = "��ų �ִ� ���� �޼�";
        }

        // ��ų���� �ʱ�ȭ
        Show_Skill_Info(GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Name, Desc, NextDesc,
        GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Icon, GameManager.Inst.Get_SelectChar.SkillData.Get_Skill_Lv,
        GameManager.Inst.Get_SelectChar.SkillData.Get_NormalAtk_Lv, normalAtk_Desc, NextNormal_Desc);
    }
    #endregion
}
