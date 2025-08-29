using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_StateChange : MonoBehaviour
{
    // ���
    public void StandBy()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.STANDBY;
    }

    // �� ����
    public void TurnEnd()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_END;
    }

    // �ڵ� ����
    public void AutoBattle()
    {
        // �ڵ� ������ �ƴ϶�� return
        if (InGame_Mgr.Inst.isAuto == false)
            return;

        // ��ų ��� ���� �����̰� ���� ���� SP�� ��ų�� ����� SP�̻� ���ϰ� ���� ��
        if(InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.CanSkill &&
           InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP <= InGame_Mgr.Inst.CurSP)
        { 
            InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].On_Click_Skill_Btn();
        }
        
        // ���� ĳ���Ͱ� ��ų�� ����� �� ���� �����̰ų� ��ų ���� SP�� ����ؾ��� SP���� ���ٸ� �⺻����
        if(InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.CanSkill == false ||
           InGame_Mgr.Inst.CurSP < InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP)
        {
            InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].On_Click_NoramlAtk();
        }
       
    }
}
