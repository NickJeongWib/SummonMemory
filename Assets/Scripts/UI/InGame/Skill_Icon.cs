using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    // ĳ���Ͱ� ��ų�� ����� �� �ִ��� �˱� ����
    [SerializeField] Character_Ctrl InGame_Char;
    // UI �̹���
    [SerializeField] Image Characet_Icon;
    [SerializeField] Image Skill_On_Frame;
    [SerializeField] Image Skill_Icon_Img;

    [SerializeField] Animator animator;

    private void Start()
    {
        animator = InGame_Mgr.Inst.SP_animator;
    }

    // ��ų ������ UI �ʱ�ȭ �� ĳ���� ��Ʈ�ѷ� ���۰� �Բ� �ʱ�ȭ
    public void Set_Character_UI(Sprite _charSprite, Sprite _skillSprite, Material _mat, Character_Ctrl _ctrl)
    {
        InGame_Char = _ctrl;
        Characet_Icon.sprite = _charSprite;
        Skill_On_Frame.material = _mat;

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

    void Skill_Use()
    {
        InGame_Char.Get_Skill_Prefab.SetActive(true);
    }
}
