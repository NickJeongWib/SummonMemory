using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_F : MonoBehaviour
{
    bool isEnterCo = false;

    #region Object_Off
    // ������Ʈ ����
    public void Active_Off()
    {
        this.gameObject.SetActive(false);
    }

    // ��ų ��� �� ������ ĳ���� ��������Ʈ ���ֱ�
    public void Active_Face_UI_Off()
    {
        this.gameObject.SetActive(false);
    }

    // �������� �θ������Ʈ ��Ȱ��ȭ
    public void Prefab_ActiveF()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
    #endregion

    #region Skill_VFX_Off
    // ��ų ����Ʈ���ֱ�
    public void On_Skill_Effect()
    {
        InGame_Mgr.Inst.UseSkill_ON();

        InGame_Mgr.Inst.UseSkill_ON = null;
    }

    // ���� ��ų ����Ʈ ��������Ʈ ����
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
    // �� ���� ���·� �ٲٱ�
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
    // ��ų ���̽� ����
    public void Skill_Voice_Play()
    {
        // ��ų ��� �� ĳ���� ��Ҹ� ������
        SoundManager.Inst.PlaySelectVoice(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.VoicePath.Get_UseSkillVoice_Path);
    }
    #endregion

    // �ε�â Ȱ��ȭ
    #region Loadting
    public void Loading()
    {
        this.gameObject.SetActive(true);
    }

    public IEnumerator LoadImage()
    {
        // �̹� �۵� ���̸�
        if (isEnterCo)
            yield break;

        // �۵� ��
        isEnterCo = true;

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        // ������ false
        isEnterCo = false;
    }
    #endregion
}
