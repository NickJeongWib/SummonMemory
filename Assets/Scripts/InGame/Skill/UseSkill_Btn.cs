using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseSkill_Btn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool isSkill;

    public void OnPointerDown(PointerEventData eventData)
    {
        // 스킬이라면
        if(isSkill)
        {
            InGame_Mgr.Inst.Show_Skill_Desc(true, null, InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData, this.transform,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_Skill_Icon, true, true);
        }
        // 기본 공격이라면
        else
        {
            InGame_Mgr.Inst.Show_Skill_Desc(true, null, InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData, this.transform,
                InGame_Mgr.Inst.Get_ObjPool.Get_NormalAtk_Icon[(int)InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle], true, false);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InGame_Mgr.Inst.Show_Skill_Desc(false);
    }
}
