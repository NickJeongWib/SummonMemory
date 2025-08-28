using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    // 캐릭터가 스킬을 사용할 수 있는지 알기 위해
    [SerializeField] Character_Ctrl InGame_Char;
    [SerializeField] Animator animator;

    // UI 이미지
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

    // 스킬 아이콘 UI 초기화 및 캐릭터 컨트롤러 시작과 함께 초기화
    public void Set_Character_UI(Sprite _charSprite, Sprite _skillSprite, Sprite _atkIconSprite, Material _mat, Character_Ctrl _ctrl)
    {
        // UI 값들 초기화
        InGame_Char = _ctrl;
        Characet_Icon.sprite = _charSprite;
        Skill_On_Frame.material = _mat;
        NormalAtk_On_Frame.material = _mat;
        NormalAtk_Icon.sprite = _atkIconSprite;

        // 스킬 아이콘을 못 찾았으면
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
    // TODO ## Skill_Icon.cs 버튼 누를 시 스킬 발동 부분
    // 스킬 사용시 
    public void On_Click_Skill_Btn()
    {
        // 스킬 포인트 부족
        if (InGame_Mgr.Inst.CurSP - InGame_Char.Get_SkillData.Get_SP < 0 || InGame_Mgr.Inst.InGameState == INGAME_STATE.BATTLE)
            return;

        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].CanSkill == false)
        {
            Debug.Log("버프 적용 중");
            return;
        }

        SoundManager.Inst.PlaySelectVoice(InGame_Char.Get_character.VoicePath.Get_UseSkillVoice_Path);

        InGame_Mgr.Inst.InGameState = INGAME_STATE.BATTLE;
        InGame_Mgr.Inst.UseSkill_ON += Skill_Use;

        animator.Play("SP_PopDown");

        InGame_Mgr.Inst.Skill_On_CharFace.gameObject.SetActive(true);
        InGame_Mgr.Inst.Skill_On_CharFace.UseChar_Face.sprite = InGame_Char.Get_character.Get_Illust_Img;

        // SP 감소
        for(int i = InGame_Mgr.Inst.CurSP; i > (InGame_Mgr.Inst.CurSP - InGame_Char.Get_SkillData.Get_SP); i--)
        {
            InGame_Mgr.Inst.SP_ChargeAnimator[i - 1].Play("SP_DOWN");
        }
        // 현재 잔존 sp계산
        InGame_Mgr.Inst.CurSP -= InGame_Char.Get_SkillData.Get_SP;

    }
    #endregion

    #region On_Click_NormalAtk
    // TODO ## Skill_Icon.cs 버튼 누를 시 기본 공격 발동 부분
    // 기본공격 사용시
    public void On_Click_NoramlAtk()
    {
        SoundManager.Inst.PlayUISound();
        SoundManager.Inst.PlayEffSound("Sounds/NormalAtk");

        // 중복 클릭 방지
        if (InGame_Mgr.Inst.InGameState == INGAME_STATE.BATTLE)
            return;

        InGame_Mgr.Inst.InGameState = INGAME_STATE.BATTLE;

        Transform TargetTr = null;

        // 타겟 목표 설정
        for(int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            if(0 < InGame_Mgr.Inst.CurMonsters[i].Get_CurHP)
            {
                TargetTr = InGame_Mgr.Inst.CurMonsters[i].gameObject.transform;
                break;
            }
        }

        // 기본공격 오브젝트 반환
        GameObject normalAtk = 
            InGame_Mgr.Inst.Get_ObjPool.Get_NormalAtk(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle, TargetTr);

        normalAtk.transform.position = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].transform.position +
            InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].transform.right * 0.8f;

        // 15보다 작으면 SP올려주기
        if (InGame_Mgr.Inst.CurSP < InGame_Mgr.Inst.SP_ChargeAnimator.Length)
        {
            // SP증가 애니메이션 작동
            InGame_Mgr.Inst.SP_ChargeAnimator[InGame_Mgr.Inst.CurSP].Play("SP_UP");
            // SP 올려주기
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
        // 현재 인게임 스킬포인트가 사용할 스킬포인트 이상보유하고 캐릭터가 스킬을 사용할 수 있을 때 활성화
        if (InGame_Char.Get_SkillData.Get_SP <= InGame_Mgr.Inst.CurSP && InGame_Char.CanSkill)
        {
            Skill_On_Frame.gameObject.SetActive(true);
        }
            

        // 현재 인게임 스킬포인트가 사용할 스킬포인트보다 적거나 캐릭터가 스킬을 사용할 수 없는 상태 일 때 비활성화
        if (InGame_Mgr.Inst.CurSP < InGame_Char.Get_SkillData.Get_SP || !InGame_Char.CanSkill)
        {
            Skill_On_Frame.gameObject.SetActive(false);
        }
         
    }
}
