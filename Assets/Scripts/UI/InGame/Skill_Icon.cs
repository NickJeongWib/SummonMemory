using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    // ĳ���Ͱ� ��ų�� ����� �� �ִ��� �˱� ����
    [SerializeField] Character_Ctrl InGame_Char;
    [SerializeField] Animator animator;

    // UI �̹���
    [Header("Skill")]
    [SerializeField] Image Characet_Icon;
    [SerializeField] Image Skill_On_Frame;
    [SerializeField] Image NormalAtk_On_Frame;
    [SerializeField] Image Skill_Icon_Img;

    [Header("Normal_Atk")]
    [SerializeField] Image NormalAtk_Icon; 


    private void Start()
    {
        animator = InGame_Mgr.Inst.SP_animator;
    }

    // ��ų ������ UI �ʱ�ȭ �� ĳ���� ��Ʈ�ѷ� ���۰� �Բ� �ʱ�ȭ
    public void Set_Character_UI(Sprite _charSprite, Sprite _skillSprite, Sprite _atkIconSprite, Material _mat, Character_Ctrl _ctrl)
    {
        // UI ���� �ʱ�ȭ
        InGame_Char = _ctrl;
        Characet_Icon.sprite = _charSprite;
        Skill_On_Frame.material = _mat;
        NormalAtk_On_Frame.material = _mat;
        NormalAtk_Icon.sprite = _atkIconSprite;

        // ��ų �������� �� ã������
        if (InGame_Char.Get_SkillData.Get_Skill_Icon == null)
        {
            Skill_Icon_Img.sprite = Resources.Load<Sprite>(InGame_Char.Get_SkillData.Get_SkillIcon_Path);
        }
        else
        {
            Skill_Icon_Img.sprite = _skillSprite;
        }
    }

    public void Active_Off()
    {
        this.gameObject.SetActive(false);
    }

    #region On_Click_Skill
    // TODO ## Skill_Icon.cs ��ư ���� �� ��ų �ߵ� �κ�
    // ��ų ���� 
    public void On_Click_Skill_Btn()
    {
        // ��ų ����Ʈ ����
        if (InGame_Mgr.Inst.CurSP - InGame_Char.Get_SkillData.Get_SP < 0 || InGame_Mgr.Inst.InGameState == INGAME_STATE.BATTLE)
            return;

        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].CanSkill == false)
        {
            Debug.Log("���� ���� ��");
            return;
        }

        SoundManager.Inst.PlaySelectVoice(InGame_Char.Get_character.VoicePath.Get_UseSkillVoice_Path);

        InGame_Mgr.Inst.InGameState = INGAME_STATE.BATTLE;
        InGame_Mgr.Inst.UseSkill_ON += Skill_Use;

        animator.Play("SP_PopDown");

        InGame_Mgr.Inst.Skill_On_CharFace.gameObject.SetActive(true);
        InGame_Mgr.Inst.Skill_On_CharFace.UseChar_Face.sprite = InGame_Char.Get_character.Get_Illust_Img;

        // SP ����
        for(int i = InGame_Mgr.Inst.CurSP; i > (InGame_Mgr.Inst.CurSP - InGame_Char.Get_SkillData.Get_SP); i--)
        {
            InGame_Mgr.Inst.SP_ChargeAnimator[i - 1].Play("SP_DOWN");
        }
        // ���� ���� sp���
        InGame_Mgr.Inst.CurSP -= InGame_Char.Get_SkillData.Get_SP;

    }
    #endregion

    #region On_Click_NormalAtk
    // TODO ## Skill_Icon.cs ��ư ���� �� �⺻ ���� �ߵ� �κ�
    // �⺻���� ����
    public void On_Click_NoramlAtk()
    {
        SoundManager.Inst.PlayUISound();
        SoundManager.Inst.PlayEffSound("Sounds/NormalAtk");

        // �ߺ� Ŭ�� ����
        if (InGame_Mgr.Inst.InGameState == INGAME_STATE.BATTLE)
            return;

        InGame_Mgr.Inst.InGameState = INGAME_STATE.BATTLE;

        Transform TargetTr = null;

        // Ÿ�� ��ǥ ����
        for(int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            if(0 < InGame_Mgr.Inst.CurMonsters[i].Get_CurHP)
            {
                TargetTr = InGame_Mgr.Inst.CurMonsters[i].gameObject.transform;
                break;
            }
        }

        // �⺻���� ������Ʈ ��ȯ
        GameObject normalAtk = 
            InGame_Mgr.Inst.Get_ObjPool.Get_NormalAtk(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle, TargetTr);

        normalAtk.transform.position = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].transform.position +
            InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].transform.right * 0.8f;

        // 15���� ������ SP�÷��ֱ�
        if (InGame_Mgr.Inst.CurSP < InGame_Mgr.Inst.SP_ChargeAnimator.Length)
        {
            // SP���� �ִϸ��̼� �۵�
            InGame_Mgr.Inst.SP_ChargeAnimator[InGame_Mgr.Inst.CurSP].Play("SP_UP");
            // SP �÷��ֱ�
            InGame_Mgr.Inst.CurSP++;
        }

        animator.Play("SP_PopDown");
    }
    #endregion

    void Skill_Use()
    {
        InGame_Char.Get_Skill_Prefab.SetActive(true);
    }

    public void Skill_Use_Frame()
    {
        // ���� �ΰ��� ��ų����Ʈ�� ����� ��ų����Ʈ �̻����ϰ� ĳ���Ͱ� ��ų�� ����� �� ���� �� Ȱ��ȭ
        if (InGame_Char.Get_SkillData.Get_SP <= InGame_Mgr.Inst.CurSP && InGame_Char.CanSkill)
        {
            Skill_On_Frame.gameObject.SetActive(true);
        }
            

        // ���� �ΰ��� ��ų����Ʈ�� ����� ��ų����Ʈ���� ���ų� ĳ���Ͱ� ��ų�� ����� �� ���� ���� �� �� ��Ȱ��ȭ
        if (InGame_Mgr.Inst.CurSP < InGame_Char.Get_SkillData.Get_SP || !InGame_Char.CanSkill)
        {
            Skill_On_Frame.gameObject.SetActive(false);
        }
         
    }
}
