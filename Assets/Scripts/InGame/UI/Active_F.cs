using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_F : MonoBehaviour
{
    bool isEnterCo = false;

    public void Active_Off()
    {
        this.gameObject.SetActive(false);
    }

    public void Active_Face_UI_Off()
    {
        this.gameObject.SetActive(false);
    }

    public void On_Skill_Effect()
    {
        InGame_Mgr.Inst.UseSkill_ON();

        InGame_Mgr.Inst.UseSkill_ON = null;
    }

    public void On_MonSkill_Effect()
    {
        InGame_Mgr.Inst.UseMonSkill_ON();

        InGame_Mgr.Inst.UseMonSkill_ON = null;
    }

    public void TurnStart()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_START;
        this.gameObject.SetActive(false);
    }

    public void Prefab_ActiveF()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void Skill_Voice_Play()
    {
        // ��ų ��� �� ĳ���� ��Ҹ� ������
        SoundManager.Inst.PlaySelectVoice(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.VoicePath.Get_UseSkillVoice_Path);
    }

    public void Loading()
    {
        this.gameObject.SetActive(true);
    }

    public IEnumerator LoadImage()
    {
        // �̹� �۵� ���̸�
        if (isEnterCo)
            yield return null;

        // �۵� ��
        isEnterCo = true;

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        // ������ false
        isEnterCo = false;
    }
}
