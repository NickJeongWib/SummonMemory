using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_F : MonoBehaviour
{
    bool isEnterCo = false;

    #region Object_Off
    // 오브젝트 끄기
    public void Active_Off()
    {
        this.gameObject.SetActive(false);
    }

    // 스킬 사용 시 나오는 캐릭터 스프라이트 꺼주기
    public void Active_Face_UI_Off()
    {
        this.gameObject.SetActive(false);
    }

    // 프리펩의 부모오브젝트 비활성화
    public void Prefab_ActiveF()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
    #endregion

    #region Skill_VFX_Off
    // 스킬 이펙트꺼주기
    public void On_Skill_Effect()
    {
        InGame_Mgr.Inst.UseSkill_ON();

        InGame_Mgr.Inst.UseSkill_ON = null;
    }

    // 몬스터 스킬 이펙트 델리게이트 해제
    public void On_MonSkill_Effect()
    {
        if (InGame_Mgr.Inst.UseMonSkill_ON != null)
        {
            InGame_Mgr.Inst.UseMonSkill_ON();

            InGame_Mgr.Inst.UseMonSkill_ON = null;
        }
    }
    #endregion

    #region Turn
    // 턴 시작 상태로 바꾸기
    public void TurnStart()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_START;
        this.gameObject.SetActive(false);
    }

    public void Monster_TurnEnd()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.ENEMY_TURN_END;
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Sound
    // 스킬 보이스 실행
    public void Skill_Voice_Play()
    {
        // 스킬 사용 시 캐릭터 목소리 나오게
        SoundManager.Inst.PlaySelectVoice(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.VoicePath.Get_UseSkillVoice_Path);
    }
    #endregion

    // 로딩창 활성화
    #region Loadting
    public void Loading()
    {
        this.gameObject.SetActive(true);
    }

    public IEnumerator LoadImage()
    {
        // 이미 작동 중이면
        if (isEnterCo)
            yield break;

        // 작동 중
        isEnterCo = true;

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        // 끝나면 false
        isEnterCo = false;
    }
    #endregion
}
