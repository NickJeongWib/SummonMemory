using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_StateChange : MonoBehaviour
{
    // 대기
    public void StandBy()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.STANDBY;
    }

    // 턴 종료
    public void TurnEnd()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_END;
    }

    // 자동 전투
    public void AutoBattle()
    {
        // 자동 전투가 아니라면 return
        if (InGame_Mgr.Inst.isAuto == false)
            return;

        // 스킬 사용 가능 상태이고 현재 보유 SP가 스킬에 사용할 SP이상 지니고 있을 때
        if(InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.CanSkill &&
           InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP <= InGame_Mgr.Inst.CurSP)
        { 
            InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].On_Click_Skill_Btn();
        }
        
        // 현재 캐릭터가 스킬을 사용할 수 없는 상태이거나 스킬 보유 SP가 사용해야할 SP보다 적다면 기본공격
        if(InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.CanSkill == false ||
           InGame_Mgr.Inst.CurSP < InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP)
        {
            InGame_Mgr.Inst.Get_ObjPool.SkillIcon_List[InGame_Mgr.Inst.CurTurnCharIndex].On_Click_NoramlAtk();
        }
       
    }
}
